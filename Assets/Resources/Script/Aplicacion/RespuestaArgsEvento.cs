using Rc.Datos.Modelo;
using System;

namespace Rc.Aplicacion
{
    /// <summary>
    /// Argumentos de evento al responder una pregunta.
    /// </summary>
    public class RespuestaArgsEvento : EventArgs
    {
        private readonly Respuesta _respuesta;
        private readonly Int32 _tiempo;

        public RespuestaArgsEvento(Respuesta respuesta, Int32 tiempo)
        {
            _respuesta = respuesta;
            _tiempo = tiempo;
        }

        public Respuesta Respuesta
        {
            get { return _respuesta; }
        }        

        public Int32 Tiempo
        {
            get { return _tiempo; }
        }
    }
}