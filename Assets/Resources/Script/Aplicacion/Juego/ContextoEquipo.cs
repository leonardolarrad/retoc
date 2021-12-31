using Rc.Datos.Modelo;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rc.Aplicacion.Juego
{
    [DataContract()]
    public class ContextoEquipo
    {
        [DataMember(Name = "Equipo")]
        private readonly Equipo equipo;

        [DataMember(Name = "Pregunta")]
        private readonly Pregunta pregunta;
                
        public Equipo Equipo { get { return equipo; } }
        
        public Pregunta Pregunta { get { return pregunta; } }

        [DataMember()]
        public Boolean Participacion { get; set; }

        [DataMember()]
        public Int32 Puntaje { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ContextoEquipo"/>.
        /// </summary>
        public ContextoEquipo(Equipo equipo, Pregunta pregunta)
        {
            this.equipo = equipo;
            this.pregunta = pregunta;
        }

        /// <summary>
        /// Constructor privado con fines de serialización.
        /// </summary>
        private ContextoEquipo()
        {
            // ...
        }
    }
}
