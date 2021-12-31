using Rc.Aplicacion.Escena;
using Rc.Aplicacion.Escena.Controlador;
using Rc.Datos.Modelo;
using Rc.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Aplicacion.Juego
{
    [DataContract()]
    public class Ronda : IContinuable, IResponderPregunta
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        [DataMember(Name = "Numero")]
        private readonly Int32 numero;

        [DataMember(Name = "TipoFase")]
        private readonly TipoFase tipoFase;

        [DataMember(Name = "Equipos")]
        private readonly List<ContextoEquipo> equipos;

        [DataMember(Name = "Estado")]
        private EstadoRonda estadoActual;

        [DataMember(Name = "EstadoProximo")]
        private EstadoRonda estadoProximo;

        private readonly Fase fase;
        private readonly ManejadorEscena manejadorEscena;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------
        
        public Int32 Numero { get { return numero; } }

        public TipoFase TipoFase { get { return tipoFase; } }
        
        public EstadoRonda Estado { get { return estadoActual; } }
        
        public EstadoRonda EstadoProximo { get { return estadoProximo; } }
        
        public List<ContextoEquipo> Equipos { get { return equipos; } }
      
        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public Ronda(Fase fase, Int32 numero, List<ContextoEquipo> equipos)
        {
            this.fase = fase;
            this.tipoFase = this.fase.Tipo;
            this.numero = numero;
            this.equipos = equipos;

            manejadorEscena = new ManejadorEscena();

            estadoActual = EstadoRonda.Iniciada;
            estadoProximo = EstadoRonda.Iniciada;
        }

        public Ronda(Fase fase, Ronda ronda)
        {
            this.fase = fase;
            this.tipoFase = this.fase.Tipo;
            this.numero = ronda.numero;

            List<ContextoEquipo> equipos = new List<ContextoEquipo>();

            foreach(ContextoEquipo equipo in ronda.equipos)
            {
                ContextoEquipo elemento = new ContextoEquipo(fase.ManejadorJuego.Torneo.Equipos.Find(x => x.Id == equipo.Equipo.Id), 
                    fase.ManejadorJuego.Torneo.Preguntas.Find(x => x.Id == equipo.Pregunta.Id));

                elemento.Participacion = equipo.Participacion;
                elemento.Puntaje = equipo.Puntaje;

                equipos.Add(elemento);
            }

            this.equipos = equipos;

            manejadorEscena = new ManejadorEscena();

            estadoActual = ronda.estadoActual;
            estadoProximo = ronda.estadoProximo;
        }

        /// <summary>
        /// Constructor privado con fines de serialización.
        /// </summary>
        private Ronda()
        {
            // ...
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public Boolean HanParticipadoTodos()
        {
            foreach(ContextoEquipo equipo in equipos)
            {
                if (!equipo.Participacion)
                    return false;
            }

            return true;
        }

        private ContextoEquipo ObtenerProximoEquipo()
        {
            foreach (ContextoEquipo equipo in equipos)
            {
                if (!equipo.Participacion)
                    return equipo;
            }

            return null;
        }

        public void ResponderPregunta(Respuesta respuesta, Single tiempo)
        {
            ContextoEquipo equipoActual = ObtenerProximoEquipo();

            if(equipoActual.Pregunta.RespuestaCorrecta == respuesta)
            {
                PuntuarEquipo(equipoActual);
            }

            equipoActual.Participacion = true;
            estadoActual = EstadoRonda.Respuesta;
            fase.ManejadorJuego.GuardarJuego();
        }

        public void TiempoFuera()
        {
            // ...
        }

        private void PuntuarEquipo(ContextoEquipo equipo)
        {
            equipo.Puntaje += 1;
        }

        public Puntuacion ObtenerPuntuacion()
        {
            return new Puntuacion(equipos.ToDictionary(x => x.Equipo, x => x.Puntaje));
        }

        public void SolicitarContinuar(Object remitente, ContinuarArgsEvento e)
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

        private void Continuar()
        {
            UnityEngine.Debug.Log("Ronda[" + numero + "] " + estadoProximo);
            Continuar((Int32)estadoProximo);
        }

        private void Continuar(Int32 estado)
        {
            switch(estado)
            {
                case (Int32)EstadoRonda.Iniciada:

                    estadoProximo = EstadoRonda.Equipo;
                    fase.ManejadorJuego.GuardarJuego();

                    if (numero > 0)
                    {
                        String ronda = String.Empty;

                        if (numero == fase.ManejadorJuego.Torneo.Rondas)
                        {
                            ronda = "Última";
                        }
                        else
                        {
                            ronda = Herramientas.Numero.ObtenerOrdinal(numero, false);
                        }

                        manejadorEscena.PresentarEscena(new EscenaRonda(ronda), SolicitarContinuar);
                    }
                    else
                    {
                        manejadorEscena.PresentarEscena(new EscenaRonda(numero), SolicitarContinuar);
                    }
                    Aplicacion.Fondo.CambiarColor("#A91881", "#FF1E6D");

                    break;

                case (Int32)EstadoRonda.Equipo:

                    estadoActual = EstadoRonda.Equipo;

                    if(HanParticipadoTodos())
                    {
                        estadoProximo = EstadoRonda.Finalizada;
                        Continuar();
                    }
                    else
                    {
                        estadoProximo = EstadoRonda.Categoria;
                        fase.ManejadorJuego.GuardarJuego();

                        EscenaEquipo escenaEquipo = new EscenaEquipo(ObtenerProximoEquipo().Equipo);
                        manejadorEscena.PresentarEscena(escenaEquipo, SolicitarContinuar);
                        Aplicacion.Fondo.CambiarColor("#00B27F", "#02901B");
                    }

                    break;

                case (Int32)EstadoRonda.Categoria:

                    estadoActual = EstadoRonda.Categoria;
                    estadoProximo = EstadoRonda.Pregunta;
                    fase.ManejadorJuego.GuardarJuego();

                    EscenaCategoria escenaCategoria = new EscenaCategoria(fase.ManejadorJuego.Torneo.Categorias,
                    ObtenerProximoEquipo().Pregunta.Categoria, manejadorEscena.Fondo);

                    manejadorEscena.PresentarEscena(escenaCategoria, SolicitarContinuar);
                    break;

                case (Int32)EstadoRonda.Pregunta:

                    estadoActual = EstadoRonda.Pregunta;
                    estadoProximo = EstadoRonda.Equipo;
                    fase.ManejadorJuego.GuardarJuego();

                    Pregunta pregunta = ObtenerProximoEquipo().Pregunta;
                    EscenaPregunta escenaPregunta = new EscenaPregunta(pregunta);
                    ControladorPregunta controlador = new ControladorPregunta(escenaPregunta, this);

                    manejadorEscena.PresentarEscena(escenaPregunta, SolicitarContinuar);
                    break;

                case (Int32)EstadoRonda.Finalizada:

                    estadoActual = EstadoRonda.Finalizada;
                    fase.ManejadorJuego.GuardarJuego();
                    manejadorEscena.RemoverEscenas();
                    fase.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
                    break;

                default:

                    throw new Exception("Error: Se ha intentado establecer un estado indefinido, " +
                        "y no se ha podido continuar con la ronda.");
            }
        }

        public void Reanudar()
        {
            switch(estadoActual)
            {           
                case EstadoRonda.Respuesta:

                    ContextoEquipo ultimoEquipo = equipos.FindLast(x => x.Participacion);
                    EscenaPregunta escenaPregunta = new EscenaPregunta(ultimoEquipo.Pregunta);
                    manejadorEscena.PresentarEscena(escenaPregunta, SolicitarContinuar);

                    if(ultimoEquipo.Puntaje > 0)
                    {
                        escenaPregunta.MostrarMensajeCorrecto();
                    }
                    else
                    {
                        escenaPregunta.MostrarMensajeIncorrecto();
                    }

                    escenaPregunta.MostrarAprendizaje();
                    break;

                default:

                    Continuar((Int32)estadoActual);
                    break;
            }
        }
    }
}
