using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Datos.Modelo
{
    /// <summary>
    /// Representa un objeto de datos de categoria en tiempo de ejecución.
    /// </summary>
    [DataContract(Name = "Categoria", Namespace = "")]
    public class Categoria : Objeto, IEquatable<Categoria>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------
        
        public static readonly Categoria Vacio = new Categoria();

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene o establece el nombre de la categoría.
        /// </summary>
        public String Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el ruta absoluto/relativo a la imagen de la categoría.
        /// </summary>
        public String Imagen { get; set; }

        /// <summary>
        /// Obtiene o establece el color de la categoría. 
        /// </summary>
        public Int32 Color { get; set; }

        public String Color1 { get; set; }

        public String Color2 { get; set; }

        /// <summary>
        /// Obtiene o establece las preguntas adjuntas a la categoría. 
        /// </summary>
        public List<Pregunta> Preguntas { get; set; }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        /// <summary>
        /// Devuelve verdadero si el torneo está completamente vacio, devuelve falso en caso contrario.
        /// </summary>
        public Boolean EstaVacio()
        {
            return this.Equals(Equipo.Vacio);
        }

        /// <summary>
        /// Devuelve verdadero si el torneo es válido (todos los campos obligatorios son distintos de NULL), devuelve
        /// falso en caso contrario.
        /// </summary>
        public Boolean EsValido()
        {
            return true;
        }

        // --------------------------------------------------
        // IEquatable <Categoria>
        // --------------------------------------------------

        public Boolean Equals(Categoria otra)
        {
            return
                this.Nombre == otra.Nombre &&
                this.Imagen == otra.Imagen &&
                this.Color == otra.Color &&
                this.Preguntas == otra.Preguntas;
        }

    }
}
