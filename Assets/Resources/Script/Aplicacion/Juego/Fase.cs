using Rc.Aplicacion.Escena;
using Rc.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rc.Aplicacion.Juego
{
    [DataContract()]
    public class Fase : IContinuable
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        [DataMember(Name = "TipoFase")]
        private TipoFase tipoFase;

        [DataMember(Name = "Estado")]
        private EstadoFase estadoActual;

        [DataMember(Name = "EstadoProximo")]
        private EstadoFase estadoProximo;

        private Ronda ronda;

        [DataMember(Name = "CantidadRondas")]
        private readonly Int32 cantidadRondas;

        [DataMember(Name = "NumeroRonda")]
        private Int32 numeroRonda;

        [DataMember(Name = "NumeroRondaDesempate")]
        private Int32 numeroRondaDesempate;

        private readonly Dictionary<Equipo, List<Pregunta>> preguntas;
        private readonly Puntuacion puntuacion;
        private Puntuacion puntuacionDesempate;

        private readonly ManejadorJuego manejadorJuego;
        private readonly ManejadorEscena manejadorEscena;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------
        
        public ManejadorJuego ManejadorJuego { get { return manejadorJuego; } }
        
        public TipoFase Tipo { get { return tipoFase; } }
        
        public EstadoFase Estado { get { return estadoActual; } }
        
        public EstadoFase EstadoProximo { get { return estadoProximo; } }
        
        public Int32 CantidadRondas { get { return cantidadRondas; } }

        public IEnumerable<Equipo> Equipos { get { return preguntas.Keys; } }
        
        public Puntuacion Puntuacion { get { return puntuacion; } }

        public Puntuacion PuntuacionFinal
        {
            get
            {
                if (puntuacionDesempate != null)
                {              
                    if (puntuacionDesempate.Coleccion.Select(x => x.Key).Contains(puntuacion.ObtenerPrimerLugar()))
                    {
                        return new Puntuacion(new Dictionary<Equipo, Int32>()
                        {
                            { puntuacionDesempate.ObtenerPrimerLugar(),

                                puntuacion.Coleccion[puntuacionDesempate.ObtenerPrimerLugar()] + 
                                puntuacionDesempate.Coleccion[puntuacionDesempate.ObtenerPrimerLugar()]},

                            { puntuacionDesempate.ObtenerSegundoLugar(),

                                puntuacion.Coleccion[puntuacionDesempate.ObtenerSegundoLugar()] + 
                                puntuacionDesempate.Coleccion[puntuacionDesempate.ObtenerSegundoLugar()]}
                        });
                    }
                    else
                    {
                        return new Puntuacion(new Dictionary<Equipo, Int32>()
                        {
                            { puntuacion.ObtenerPrimerLugar(),
                                puntuacion.Coleccion[puntuacion.ObtenerPrimerLugar()]},

                            { puntuacionDesempate.ObtenerPrimerLugar(),

                                puntuacion.Coleccion[puntuacionDesempate.ObtenerPrimerLugar()] +
                                puntuacionDesempate.Coleccion[puntuacionDesempate.ObtenerPrimerLugar()]}
                        });
                    }
                }
                else
                {
                    return puntuacion;
                }
            }
        }

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public Fase(ManejadorJuego manejadorJuego, TipoFase tipoFase, Int32 cantidadRondas, Dictionary<Equipo, List<Pregunta>> preguntas)
        {
            this.manejadorJuego = manejadorJuego;
            manejadorEscena = new ManejadorEscena();

            this.tipoFase = tipoFase;
            this.cantidadRondas = cantidadRondas;
            numeroRonda = 0;
            numeroRondaDesempate = 0;

            this.preguntas = preguntas;
            puntuacion = new Puntuacion(Equipos);

            estadoActual = EstadoFase.Iniciando;
            estadoProximo = EstadoFase.Iniciando;
        }

        public Fase(ManejadorJuego manejadorJuego, Fase fase, Dictionary<Equipo, List<Pregunta>> preguntas, List<Ronda> rondas)
        {
            this.manejadorJuego = manejadorJuego;
            manejadorEscena = new ManejadorEscena();

            tipoFase = fase.tipoFase;
            cantidadRondas = fase.cantidadRondas;
            numeroRonda = fase.numeroRonda;
            numeroRondaDesempate = fase.numeroRondaDesempate;

            this.preguntas = preguntas;
            puntuacion = new Puntuacion(Equipos);

            estadoActual = fase.estadoActual;
            estadoProximo = fase.estadoProximo;

            // Obtener la puntuación de las rondas anteriores finalizadas.

            foreach(Ronda ronda in rondas.Where(x => x.Numero > 0 && x.TipoFase == tipoFase && (x.Estado == EstadoRonda.Finalizada || x.HanParticipadoTodos())))
            {
                Ronda rondaReanudada = new Ronda(this, ronda);

                foreach (KeyValuePair<Equipo, Int32> puntuacionRonda in rondaReanudada.ObtenerPuntuacion().Coleccion)
                {
                    puntuacion.Coleccion[puntuacionRonda.Key] += puntuacionRonda.Value * manejadorJuego.Torneo.Ponderacion;
                }
            }

            // Removemos las preguntas que ya han sido respondidas en 
            // las otras rondas.

            foreach(Ronda ronda in rondas.Where(x => x.Numero > 0 && x.TipoFase == tipoFase))
            {
                Ronda rondaReanudada = new Ronda(this, ronda);

                foreach (ContextoEquipo contextoEquipo in rondaReanudada.Equipos)
                {
                    if(contextoEquipo.Participacion)
                    {
                        this.preguntas[contextoEquipo.Equipo].RemoveAt(this.preguntas[contextoEquipo.Equipo].IndexOf(contextoEquipo.Pregunta));
                    }
                }
            }

            // Reanudamos la última ronda de la fase. Si todas las rondas están finalizadas,
            // entonces calculamos la puntuación final de la fase.

            Ronda tmp;
            
            if((tmp = rondas.Find(x => x.Estado != EstadoRonda.Finalizada)) != null)
            {
                ronda = new Ronda(this, tmp);
                manejadorJuego.Historial.Rondas[manejadorJuego.Historial.Rondas.IndexOf(tmp)] = ronda;

                if (ronda.Numero < 0)
                {
                    puntuacionDesempate = new Puntuacion(ronda.Equipos.Select(x => x.Equipo));
                }
            }
            else if ((tmp = rondas.FindLast(x => x.Numero < 0)) != null)
            {
                Ronda rondaDesempateFinal = new Ronda(this, tmp);
                puntuacionDesempate = rondaDesempateFinal.ObtenerPuntuacion();
            }            
        }

        /// <summary>
        /// Constructor privado para fines de serialización.
        /// </summary>
        private Fase()
        {
            // ...
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        private Boolean EsUltimaRonda()
        {
            return numeroRonda == cantidadRondas;
        }

        private void PasarRonda()
        {
            numeroRonda++;
            List<ContextoEquipo> equipos = new List<ContextoEquipo>();

            for (int i = 0; i < preguntas.Count(); i++)
            {
                ContextoEquipo equipo = new ContextoEquipo(
                    preguntas.ToList()[i].Key,
                    preguntas.ToList()[i].Value.First()
                );

                equipos.Add(equipo);
                preguntas.ToList()[i].Value.Remove(preguntas.ToList()[i].Value.First());
            }

            ronda = new Ronda(this, numeroRonda, equipos);
            manejadorJuego.Historial.Rondas.Add(ronda);
            manejadorJuego.GuardarJuego();
            ronda.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
        }

        private void PasarRondaDesempate()
        {
            Int32 condicionEmpate = tipoFase == TipoFase.Ultima ? 2 : 3;

            if (puntuacionDesempate == null)
            {
                if (Puntuacion.HayEmpate(Puntuacion.ObtenerPrimerLugar(), condicionEmpate))
                {                    
                    puntuacionDesempate = new Puntuacion(
                        Puntuacion.ObtenerEquiposEmpatados(Puntuacion.ObtenerPrimerLugar(), condicionEmpate)
                    );

                    UnityEngine.Debug.Log(puntuacionDesempate.Coleccion.Count());
                }
                else
                {
                    puntuacionDesempate = new Puntuacion(
                        Puntuacion.ObtenerEquiposEmpatados(Puntuacion.ObtenerSegundoLugar(), 2, Puntuacion.ObtenerPrimerLugar())
                    );
                }
            }
            else
            {
                if (!puntuacionDesempate.Equipos.Contains(Puntuacion.ObtenerPrimerLugar()))
                    condicionEmpate = 2;

                if (puntuacionDesempate.HayEmpate(puntuacionDesempate.ObtenerPrimerLugar(), condicionEmpate))
                {
                    puntuacionDesempate = new Puntuacion(
                        puntuacionDesempate.ObtenerEquiposEmpatados(puntuacionDesempate.ObtenerPrimerLugar(), condicionEmpate)
                    );
                }
                else
                {
                    puntuacionDesempate = new Puntuacion(
                        puntuacionDesempate.ObtenerEquiposEmpatados(puntuacionDesempate.ObtenerSegundoLugar(), 2, puntuacionDesempate.ObtenerPrimerLugar())
                    );
                }
            }

            List<ContextoEquipo> contextoEquiposEmpatados = new List<ContextoEquipo>();

            foreach (Equipo equipo in puntuacionDesempate.Equipos)
            {
                ContextoEquipo contextoEquipo = new ContextoEquipo(equipo, manejadorJuego.AsignarPreguntaDesempate());
                contextoEquiposEmpatados.Add(contextoEquipo);
            }

            numeroRondaDesempate++;
            ronda = new Ronda(this, numeroRondaDesempate * -1, contextoEquiposEmpatados);
            manejadorJuego.Historial.Rondas.Add(ronda);
            manejadorJuego.GuardarJuego();
            ronda.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
        }

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
                Continuar((EstadoFase)e.Estado);
            }
        }

        private void Continuar()
        {
            UnityEngine.Debug.Log("Fase[" + tipoFase + "] " + estadoProximo);
            Continuar(estadoProximo);
        }

        private void Continuar(EstadoFase estado)
        {
            switch (estado)
            {
                case EstadoFase.Iniciando:

                    estadoProximo = EstadoFase.Ronda;
                    Continuar();
                    break;

                case EstadoFase.Ronda:

                    estadoActual = EstadoFase.Ronda;
                    estadoProximo = EstadoFase.Puntuacion;
                    manejadorEscena.RemoverEscenas();
                    PasarRonda();
                    break;

                case EstadoFase.Puntuacion:

                    estadoActual = EstadoFase.Puntuacion;

                    foreach (KeyValuePair<Equipo, Int32> puntuacionRonda in ronda.ObtenerPuntuacion().Coleccion)
                    {
                        puntuacion.Coleccion[puntuacionRonda.Key] += puntuacionRonda.Value * manejadorJuego.Torneo.Ponderacion;
                    }

                    if (EsUltimaRonda())
                    {
                        Int32 condicionEmpate1 = tipoFase == TipoFase.Ultima ? 2 : 3;

                        if (Puntuacion.HayEmpate(Puntuacion.ObtenerPrimerLugar(), condicionEmpate1)
                            || Puntuacion.HayEmpate(Puntuacion.ObtenerSegundoLugar(), 2, Puntuacion.ObtenerPrimerLugar()))
                        {
                            estadoProximo = EstadoFase.Empate;
                        }
                        else
                        {
                            estadoProximo = EstadoFase.Finalizando;
                        }
                    }
                    else
                    {
                        estadoProximo = EstadoFase.Ronda;
                    }

                    manejadorJuego.GuardarJuego();
                    manejadorEscena.PresentarEscena(new EscenaPuntuacion(puntuacion.Coleccion.OrderByDescending(x => x.Value)), SolicitarContinuar);
                    Aplicacion.Fondo.CambiarColor("#FF902B", "#FFF02B");
                    break;

                case EstadoFase.Empate:

                    estadoActual = EstadoFase.Empate;
                    estadoProximo = EstadoFase.PuntuacionEmpate;
                    manejadorEscena.RemoverEscenas();
                    PasarRondaDesempate();
                    break;

                case EstadoFase.PuntuacionEmpate:

                    estadoActual = EstadoFase.PuntuacionEmpate;

                    foreach (KeyValuePair<Equipo, Int32> puntuacionRonda in ronda.ObtenerPuntuacion().Coleccion)
                    {
                        puntuacionDesempate.Coleccion[puntuacionRonda.Key] += puntuacionRonda.Value;
                        UnityEngine.Debug.Log(puntuacionDesempate.Coleccion[puntuacionRonda.Key]);
                    }

                    Int32 condicionEmpate2 = tipoFase == TipoFase.Ultima ? 2 : 3;

                    if (!puntuacionDesempate.Equipos.Contains(Puntuacion.ObtenerPrimerLugar())) // Key
                        condicionEmpate2 = 2;

                    UnityEngine.Debug.Log(puntuacionDesempate.HayEmpate(puntuacionDesempate.ObtenerPrimerLugar(), condicionEmpate2));
                    UnityEngine.Debug.Log(puntuacionDesempate.HayEmpate(puntuacionDesempate.ObtenerSegundoLugar(), 2, puntuacionDesempate.ObtenerPrimerLugar()));

                    if (puntuacionDesempate.HayEmpate(puntuacionDesempate.ObtenerPrimerLugar(), condicionEmpate2)
                            || puntuacionDesempate.HayEmpate(puntuacionDesempate.ObtenerSegundoLugar(), 2, puntuacionDesempate.ObtenerPrimerLugar()))
                    {
                        estadoProximo = EstadoFase.Empate;
                    }
                    else
                    {
                        estadoProximo = EstadoFase.Finalizando;
                    }
                    manejadorEscena.PresentarEscena(new EscenaPuntuacion(puntuacionDesempate.Coleccion.OrderByDescending(x => x.Value)), SolicitarContinuar);
                    Aplicacion.Fondo.CambiarColor("#FF902B", "#FFF02B");
                    break;

                case EstadoFase.Finalizando:

                    estadoActual = EstadoFase.Finalizando;                  

                    manejadorJuego.GuardarJuego();
                    manejadorEscena.RemoverEscenas();
                    manejadorJuego.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
                    break;
            }
        }    
        
        public void Reanudar()
        {
            switch(estadoActual)
            {
                case EstadoFase.Iniciando:

                    Continuar();
                    break;

                case EstadoFase.Ronda:
                    
                    ronda.Reanudar();
                    break;

                case EstadoFase.Puntuacion:
                    manejadorEscena.PresentarEscena(new EscenaPuntuacion(puntuacion.Coleccion.OrderByDescending(x => x.Value)), SolicitarContinuar);
                    break;

                case EstadoFase.Empate:

                    ronda.Reanudar();
                    break;
            }
        }
    }
}
