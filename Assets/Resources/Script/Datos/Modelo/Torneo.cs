using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Datos.Modelo
{
    /// <summary>
    /// Representa un objeto de dato torneo en tiempo de ejecución.
    /// </summary>
    [DataContract(Name = "Torneo", Namespace = "")]
    public class Torneo : Objeto, IEquatable<Torneo>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------
        
        public static readonly Torneo Vacio = new Torneo();

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene o establece el nombre del torneo.
        /// </summary>
        public String Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el ruta absoluto/relativo de la imagen del torneo.
        /// </summary
        public String Imagen { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de rondas del torneo. 
        /// </summary>
        public Int32 Rondas { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de rondas finales del torneo.
        /// </summary>
        public Int32 RondasFinales { get; set; }

        /// <summary>
        /// Obtiene o establece la ponderación otorgada a los equipos 
        /// por responder correctamente. 
        /// </summary>
        public Int32 Ponderacion { get; set; }

        /// <summary>
        /// Obtiene o estable la puntuación inicial que se le otorga a los equipos participantes.
        /// </summary>
        public Int32 PuntuacionInicial { get; set; }

        /// <summary>
        /// Obtiene o establece los equipos del torneo.
        /// </summary>
        public List<Equipo> Equipos { get; set; }

        /// <summary>
        /// Obtiene o establece las categorias del torneo.
        /// </summary>
        public List<Categoria> Categorias { get; set; }

        /// <summary>
        /// Obtiene o establece las preguntas del torneo.
        /// </summary>
        public List<Pregunta> Preguntas { get; set; }

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public Torneo()
        {
            Equipos = new List<Equipo>();
            Categorias = new List<Categoria>();
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        /// <summary>
        /// Devuelve verdadero si el torneo está completamente vacio, devuelve falso en caso contrario.
        /// </summary>
        public Boolean EstaVacio()
        {
            return this.Equals(Torneo.Vacio);
        }

        /// <summary>
        /// Devuelve verdadero si el torneo es válido (todos los campos obligatorios son distintos de NULL), devuelve
        /// falso en caso contrario.
        /// </summary>
        public Boolean EsValido()
        {
            Int32 equiposValidos = Equipos.FindAll(x => x.EsValido()).Count;
            Int32 categoriasValidas = Categorias.FindAll(x => x.EsValido()).Count;
            Int32 preguntasValidas = Preguntas.FindAll(x => x.EsValido()).Count;

            return
                ((Nombre != null) || (Nombre != String.Empty)) &&
                (Rondas > 1) &&  (Ponderacion > 0) &&
                (equiposValidos > 1) && (categoriasValidas > 2) &&
                (preguntasValidas >= ((equiposValidos * Rondas) + equiposValidos + 1));
        }

        // --------------------------------------------------
        // IEquatable <Tournament>
        // --------------------------------------------------

        /// <summary>
        /// Devuelve verdadero en caso de que el torneo sea igual, devuelve falso en caso contrario.
        /// </summary>
        public Boolean Equals(Torneo otro)
        {
            return
                this.Nombre == otro.Nombre &&
                this.Imagen == otro.Imagen &&
                this.Rondas == otro.Rondas &&
                this.Ponderacion == otro.Ponderacion &&
                this.PuntuacionInicial == otro.PuntuacionInicial &&
                this.Equipos == otro.Equipos &&
                this.Categorias == otro.Categorias &&
                this.Preguntas == otro.Preguntas;
        }
    }
}