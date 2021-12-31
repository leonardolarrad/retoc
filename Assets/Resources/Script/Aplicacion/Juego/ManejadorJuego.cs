using Rc.Aplicacion.Escena;
using Rc.Datos.Modelo;
using Rc.Datos.Basedatos;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rc.Aplicacion.Juego
{
    public class ManejadorJuego : IContinuable
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly Torneo torneo;
        private readonly HistorialJuego historial;
        private readonly String directorio;
        private readonly ManejadorEscena manejadorEscena;

        private EstadoJuego estadoActual;
        private EstadoJuego estadoProximo;

        private readonly List<Int64> preguntasAsignadas;
        private readonly Dictionary<Equipo, List<Pregunta>> preguntasPrimeraFase;
        private readonly List<Pregunta> preguntasUltimaFase;

        private Fase fase;        

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene el torneo referenciado por el manejador.
        /// </summary>
        public Torneo Torneo { get { return torneo; } }

        /// <summary>
        /// Obtiene el historial del juego.
        /// </summary>
        public HistorialJuego Historial { get { return historial; } }

        /// <summary>
        /// Obtiene el estado actual del manejador de juego.
        /// </summary>
        public EstadoJuego Estado { get { return estadoActual; } }
        
        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

            /// <summary>
            /// Inicializa una nueva instancia de <see cref="ManejadorJuego"/> según un <see cref="Datos.Modelo.Torneo"/> válido.
            /// </summary>
            /// <param name="torneo"></param>
        public ManejadorJuego(Torneo torneo)
        {
            this.torneo = torneo;
            manejadorEscena = new ManejadorEscena();

            preguntasAsignadas = new List<Int64>();
            preguntasPrimeraFase = new Dictionary<Equipo, List<Pregunta>>();
            preguntasUltimaFase = new List<Pregunta>();

            foreach(Equipo equipo in torneo.Equipos)
            {
                preguntasPrimeraFase.Add(equipo, new List<Pregunta>());              
            }
            
            AsignarPreguntas();

            // Copiar la base de datos para asegurar su integridad.

            directorio = Application.dataPath + "/../Tmp/" + torneo.Nombre;

            if (!Directory.Exists(Application.dataPath + "/../Tmp/" + torneo.Nombre))
            {
                Directory.CreateDirectory(Application.dataPath + "/../Tmp/" + torneo.Nombre);
            }

            String fuente = Path.Combine(Aplicacion.Opciones.DirectorioBasedatos, Aplicacion.Opciones.NombreBasedatos);
            String destino = Path.Combine(Application.dataPath + "/../Tmp/" + torneo.Nombre, Aplicacion.Opciones.NombreBasedatos);

            System.IO.File.Copy(fuente, destino, true);

            // Asignar los estados iniciales.

            estadoActual = EstadoJuego.Iniciando;
            estadoProximo = EstadoJuego.Iniciando;

            // Crear un nuevo historial.

            historial = new HistorialJuego()
            {
                Torneo = this.torneo,
                Fecha = DateTime.Now,
                Basedatos = destino,
                Estado = estadoActual,
                EstadoProximo = estadoProximo,
                PreguntasPrimeraFase = PreguntaPorEquipo.ConvertirALista(preguntasPrimeraFase),
                PreguntasUltimaFase = preguntasUltimaFase
            };
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ManejadorJuego"/> reanudando un <see cref="HistorialJuego"/> anterior.
        /// </summary>
        public ManejadorJuego(HistorialJuego historial)
        {
            this.historial = historial;
            RcBasedatos basedatos = new RcBasedatos(this.historial.Basedatos);

            if (!basedatos.ProbarConexion())
                throw new Exception("No se ha podido establecer conexión con la base de datos.");

            torneo = basedatos.ObtenerTorneo(this.historial.Torneo.Id);

            if (torneo == null)
                throw new Exception("No se ha podido encontrar el torneo. La base de datos se ha modificado o está corrupta.");

            this.historial.Torneo = torneo;
            manejadorEscena = new ManejadorEscena();
            directorio = Application.dataPath + "/../Tmp/" + torneo.Nombre;

            estadoActual = historial.Estado;
            estadoProximo = historial.EstadoProximo;

            preguntasAsignadas = new List<Int64>();
            preguntasPrimeraFase = new Dictionary<Equipo, List<Pregunta>>();
            preguntasUltimaFase = new List<Pregunta>();

            // Se recupera la lista de preguntas asignadas. De esta manera, ninguna pregunta
            // corre el riesgo de ser respondida nuevamente.

            foreach (Equipo equipo in torneo.Equipos)
            {
                preguntasPrimeraFase.Add(equipo, new List<Pregunta>());

                foreach(var preguntaPorEquipo in historial.PreguntasPrimeraFase.Where(x => x.Equipo.Id == equipo.Id))
                {
                    preguntasPrimeraFase[equipo].Add(torneo.Preguntas.Find(x => x.Id == preguntaPorEquipo.Pregunta.Id));
                    preguntasAsignadas.Add(preguntaPorEquipo.Pregunta.Id);
                }
            }

            foreach(Pregunta pregunta in historial.PreguntasUltimaFase)
            {
                preguntasUltimaFase.Add(torneo.Preguntas.Find(x => x.Id == pregunta.Id));
                preguntasAsignadas.Add(pregunta.Id);
            }            

            // Si el juego se interrumpió en medio de una fase, o en la selección del campeón,
            // entonces se reanudan las fases para poder continuar el juego.

            if(this.historial.Estado == EstadoJuego.PrimeraFase)
            {
                List<Ronda> rondas = this.historial.Rondas.Where(x => x.TipoFase == TipoFase.Primera).ToList();
                fase = new Fase(this, this.historial.Fases.Find(x => x.Tipo == TipoFase.Primera), preguntasPrimeraFase, rondas);

                this.historial.Fases[this.historial.Fases.IndexOf(this.historial.Fases.Find(x => x.Tipo == TipoFase.Primera))] = fase;
            }
            else if(this.historial.Estado >= EstadoJuego.UltimaFase)
            {
                // Se crea una instnacia de la primera ronda jugada con el motivo
                // de obtener su puntuación final.

                List<Ronda> rondasPrimeraFase = this.historial.Rondas.Where(x => x.TipoFase == TipoFase.Primera).ToList();
                Fase primeraFase = new Fase(this, this.historial.Fases.Find(x => x.Tipo == TipoFase.Primera), preguntasPrimeraFase, rondasPrimeraFase);

                // Posteriormente se inicaliza la fase final para poder reanudarla.

                List<Ronda> rondasUltimaFase = this.historial.Rondas.Where(x => x.TipoFase == TipoFase.Ultima).ToList();
                Fase ultimaFase = this.historial.Fases.Find(x => x.Tipo == TipoFase.Ultima);

                Dictionary<Equipo, List<Pregunta>> preguntasFinales = new Dictionary<Equipo, List<Pregunta>>
                {
                    { primeraFase.PuntuacionFinal.ObtenerPrimerLugar(),
                        preguntasUltimaFase.GetRange(0, torneo.RondasFinales) },

                    { primeraFase.PuntuacionFinal.ObtenerSegundoLugar(),
                        preguntasUltimaFase.GetRange(torneo.RondasFinales, torneo.RondasFinales) }
                };

                fase = new Fase(this, ultimaFase, preguntasFinales, rondasUltimaFase);
                this.historial.Fases[this.historial.Fases.IndexOf(ultimaFase)] = fase;
            }
        }
        
        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------
        
        /// <summary>
        /// Gestiona las preguntas primarias y finales para ser utilizadas durante el juego.
        /// </summary>
        private void AsignarPreguntas()
        {
            Categoria ultimaCategoria = Categoria.Vacio;

            // Asignar preguntas para la primera fase

            for (int i = 0; i < torneo.Rondas; i++)
            {
                foreach(Equipo equipo in torneo.Equipos)
                {
                    System.Random aleatorio1 = new System.Random();
                    Pregunta seleccionada = torneo.Preguntas.OrderBy(a => aleatorio1.Next()).ToList().Find(
                        x => {
                            return
                                !preguntasAsignadas.Contains(x.Id) &&
                                !x.Categoria.Equals(ultimaCategoria);
                        }
                    );

                    if (seleccionada == null)
                    {
                        seleccionada = torneo.Preguntas.OrderBy(a => aleatorio1.Next()).ToList().Find(x => !preguntasAsignadas.Contains(x.Id));

                        if (seleccionada == null)
                        {
                            throw new Exception("No se ha podido asignar las preguntas de manera correcta." +
                                " Todas las preguntas han sido respondidas o la base de datos está corrupta.");
                        }
                    }

                    preguntasPrimeraFase[equipo].Add(seleccionada);
                    preguntasAsignadas.Add(seleccionada.Id);
                    ultimaCategoria = seleccionada.Categoria;
                }
            }

            // Asignar preguntas para la última fase

            for (int i = 0; i < torneo.RondasFinales * 2; i++)
            {
                System.Random aleatorio2 = new System.Random();
                Pregunta seleccionada = torneo.Preguntas.OrderBy(a => aleatorio2.Next()).ToList().Find(x => !preguntasAsignadas.Contains(x.Id));

                if (seleccionada == null)
                {
                    throw new Exception("No se ha podido asignar las preguntas de manera correcta." +
                        " Todas las preguntas han sido respondidas o la base de datos está corrupta.");
                }

                preguntasUltimaFase.Add(seleccionada);
                preguntasAsignadas.Add(seleccionada.Id);
            }
        }

        /// <summary>
        /// Asigna una próxima pregunta de desempate y la retorna.
        /// </summary>
        public Pregunta AsignarPreguntaDesempate()
        {
            System.Random aleatorio = new System.Random();
            Pregunta seleccionada = torneo.Preguntas.OrderBy(a => aleatorio.Next()).ToList().Find(x => !preguntasAsignadas.Contains(x.Id));
            preguntasAsignadas.Add(seleccionada.Id);
            return seleccionada;
        }

        /// <summary>
        /// Solicitud para continuar el juego.
        /// </summary>
        public void SolicitarContinuar(System.Object remitente, ContinuarArgsEvento e)
        {
            if (!(remitente == manejadorEscena.EscenaActual || e.Forzar))
            {
                throw new Exception("El objeto remitente no es la escena actual o no se ha forzado la continuidad.");
            }

            if (e.Estado == 0x00)
            {
                Continuar();
            }
            else
            {
                Continuar(e.Estado);
            }
        }

        /// <summary>
        /// Continua el juego según el próximo estado asignado.
        /// </summary>
        private void Continuar()
        {
            UnityEngine.Debug.Log("Juego: " + estadoProximo);
            Continuar((Int32)estadoProximo);
        }

        /// <summary>
        /// Continua el juego según un estado válido.
        /// </summary>
        private void Continuar(Int32 estado)
        {
            switch(estado)
            {
                case (Int32)EstadoJuego.Iniciando:

                    GuardarJuego();
                    estadoProximo = EstadoJuego.PrimeraFase;
                    Continuar();
                    break;

                case (Int32)EstadoJuego.PrimeraFase:

                    estadoActual = EstadoJuego.PrimeraFase;
                    estadoProximo = EstadoJuego.UltimaFase;

                    historial.Estado = estadoActual;
                    historial.EstadoProximo = estadoProximo;
                    
                    fase = new Fase(this, TipoFase.Primera, torneo.Rondas, preguntasPrimeraFase);
                    historial.Fases.Add(fase);
                    
                    GuardarJuego();
                    fase.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
                    break;

                case (Int32)EstadoJuego.UltimaFase:

                    estadoActual = EstadoJuego.UltimaFase;
                    estadoProximo = EstadoJuego.Campeon;

                    historial.Estado = estadoActual;
                    historial.EstadoProximo = estadoProximo;

                    Dictionary<Equipo, List<Pregunta>> preguntasFinales = new Dictionary<Equipo, List<Pregunta>>();

                    preguntasFinales.Add(fase.PuntuacionFinal.ObtenerPrimerLugar(), preguntasUltimaFase.GetRange(0, torneo.RondasFinales));
                    preguntasFinales.Add(fase.PuntuacionFinal.ObtenerSegundoLugar(), preguntasUltimaFase.GetRange(torneo.RondasFinales, torneo.RondasFinales));
                                        
                    fase = new Fase(this, TipoFase.Ultima, torneo.RondasFinales, preguntasFinales);
                    historial.Fases.Add(fase);
                    GuardarJuego();
                    fase.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
                    break;

                case (Int32)EstadoJuego.Campeon:

                    estadoActual = EstadoJuego.Campeon;
                    estadoProximo = EstadoJuego.Finalizando;

                    historial.Estado = estadoActual;
                    historial.EstadoProximo = estadoProximo;

                    GuardarJuego();
                    manejadorEscena.PresentarEscena(new EscenaCampeon(fase.PuntuacionFinal.ObtenerPrimerLugar()), SolicitarContinuar);
                    Aplicacion.Fondo.CambiarColor("#A91881", "#FF1E6D");
                    break;

                case (Int32)EstadoJuego.Finalizando:

                    estadoActual = EstadoJuego.Finalizando;
                    GuardarJuego();
                    break;
            }
        }

        /// <summary>
        /// Guarda el historial del juego sobrescribiendo los datos.
        /// </summary>
        public void GuardarJuego()
        {           
            HistorialJuego.Guardar(historial, directorio);
        }

        /// <summary>
        /// Reanuda el juego mediante un historial válido.
        /// </summary>
        public void Reanudar()
        {
            switch(estadoActual)
            {
                case EstadoJuego.PrimeraFase:
                    
                    fase.Reanudar();
                    break;

                case EstadoJuego.UltimaFase:

                    fase.Reanudar();
                    break;

                default:

                    Continuar((Int32)estadoActual);
                    break;
            }
        }
    }
}
