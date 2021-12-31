using System;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Datos.Modelo
{
    /// <summary>
    /// Representa un objeto de datoss de equipo en tiempo de ejecución. 
    /// </summary>
    [DataContract(Name = "Equipo", Namespace = "")]
    public class Equipo : Objeto, IEquatable<Equipo>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------
        
        public static readonly Equipo Vacio = new Equipo();

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene o establece el nombre del equipo.
        /// </summary>
        public String Nombre { get; set; }

        /// <summary>
        /// Obtiene o establece el ruta absoluto/relativo de la imagen del equipo.
        /// </summary>
        public String Imagen { get; set; }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        /// <summary>
        /// Devuelve verdadero si la categoría está completamente vacia, devuelve falso en caso contrario.
        /// </summary>
        public Boolean EstaVacio()
        {
            return this.Equals(Equipo.Vacio);
        }

        /// <summary>
        /// Devuelve verdadero si la categoría es válida, devuelve falso en caso contrario.
        /// </summary>
        public Boolean EsValido()
        {
            return true;
        }

        // --------------------------------------------------
        // IEquatable <Equipo>
        // --------------------------------------------------

        public Boolean Equals(Equipo otro)
        {
            return
                this.Nombre == otro.Nombre &&
                this.Imagen == otro.Imagen;
        }
    }
}