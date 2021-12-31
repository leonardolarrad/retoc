using System;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Datos.Modelo 
{
    /// <summary>
    /// Clase base para entidades de bases de datos.
    /// </summary>
    [DataContract(Name = "Object", Namespace = "")]
    public class Objeto
    {
        /// <summary>
        /// Obtiene o establece el id del objeto.
        /// </summary>
        [DataMember(Name = "Id")]
        public Int64 Id { get; set; }
    }
}
