using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Datos.Modelo
{
    /// <summary>
    /// Representa los tipos de pregunta que pueden existir.
    /// </summary>
    public enum TipoPregunta
    {
        Normal = 0,
        Image = 1,
        Video = 2,
    }

    /// <summary>
    /// Enumeracion de las cuatro posibles respuestas.
    /// </summary>
    public enum Respuesta
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3
    }

    /// <summary>
    /// Representa un objeto de datos pregunta en tiempo de ejecución.
    /// </summary>
    [DataContract(Name = "Pregunta", Namespace = "")]
    public class Pregunta : Objeto, IEquatable<Pregunta>, ICloneable
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------
        
        public static readonly Pregunta Vacio = new Pregunta();

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene o establece el encabezado de la pregunta.
        /// </summary>
        public String Encabezado { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de la pregunta.
        /// </summary>
        public TipoPregunta TipoPregunta { get; set; }

        /// <summary>
        /// Obtiene o establece el ruta absoluto/relativo de la multimedia.
        /// </summary>
        public String Multimedia { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta A.
        /// </summary>
        public String RespuestaA { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta B.
        /// </summary>
        public String RespuestaB { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta C.
        /// </summary>
        public String RespuestaC { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta D.
        /// </summary>
        public String RespuestaD { get; set; }

        /// <summary>
        /// Obtiene o establece la respuesta correcta.
        /// </summary>
        public Respuesta RespuestaCorrecta { get; set; }

        /// <summary>
        /// Obtiene o establece el aprendizaje de la pregunta.
        /// </summary>
        public String Aprendizaje { get; set; }

        /// <summary>
        /// Obtiene o establece la categoría de la pregunta.
        /// </summary>
        public Categoria Categoria { get; set; }

        /// <summary>
        /// Devuelve verdadero si la pregunta ya fue respondida, devuelve falso en caso contrario.
        /// </summary>
        public Boolean Respondida { get; set; }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        /// <summary>
        /// Devuelve verdadero si la pregunta está vacia, duevuelve falso en caso contrario.
        /// </summary>
        public Boolean EstaVacio()
        {
            return this.Equals(Pregunta.Vacio);
        }

        /// <summary>
        /// Devuelve verdadero si la pregunta es válida, devuelve falso en caso contrario.
        /// </summary>
        public Boolean EsValido()
        {
            return

                ((Encabezado != null) || (Encabezado != String.Empty)) &&
                ((RespuestaA != null) || (RespuestaA != String.Empty)) &&
                ((RespuestaB != null) || (RespuestaB != String.Empty)) &&
                ((RespuestaC != null) || (RespuestaC != String.Empty)) &&
                ((RespuestaD != null) || (RespuestaD != String.Empty)) &&
                //((Aprendizaje != null) || (Aprendizaje != String.Empty)) &&
                (Categoria != null);
            
        }

        // --------------------------------------------------
        // IEquatable <Pregunta>
        // --------------------------------------------------

        public Boolean Equals(Pregunta otra)
        {
            return

                this.Encabezado == otra.Encabezado &&
                this.TipoPregunta == otra.TipoPregunta &&
                this.Multimedia == otra.Multimedia &&
                this.RespuestaA == otra.RespuestaA &&
                this.RespuestaB == otra.RespuestaB &&
                this.RespuestaC == otra.RespuestaC &&
                this.RespuestaD == otra.RespuestaD &&
                this.RespuestaCorrecta == otra.RespuestaCorrecta &&
                this.Aprendizaje == otra.Aprendizaje &&
                this.Categoria == otra.Categoria &&
                this.Respondida == otra.Respondida;
        }

        // --------------------------------------------------
        // ICloneable
        // --------------------------------------------------

        public System.Object Clone()
        {
            return (Pregunta)MemberwiseClone();
        }
    }
  
}
