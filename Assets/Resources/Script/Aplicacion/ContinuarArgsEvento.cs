using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion
{
    /// <summary>
    /// Clase de argumentos del evento continuar. Usada para pasar argumentos al delegado <see cref="ContinuarControladorEvento"/>.
    /// </summary>
    public class ContinuarArgsEvento : EventArgs
    {
        private Int32 _estado;
        private Boolean _forzar;

        /// <summary>
        /// Clase vacia.
        /// </summary>
        public static readonly ContinuarArgsEvento Vacio = new ContinuarArgsEvento();

        /// <summary>
        /// Inicializa los argumentos del evento continuar.
        /// </summary>
        public ContinuarArgsEvento()
        {
            _estado = 0x00000000;
            _forzar = false;
        }

        /// <summary>
        /// Inicializa los argumentos del evento continuar.
        /// </summary>
        public ContinuarArgsEvento(Int32 estado, Boolean forzar = false)
        {
            _estado = estado;
            _forzar = forzar;
        }

        /// <summary>
        /// Obtiene el estado del juego en caso de que la continuación sea irregular.
        /// </summary>
        public Int32 Estado
        {
            get { return _estado; }
        }

        /// <summary>
        /// Obtiene verdadero si se desea forzar la continuación, devuelve falso en caso contrario.
        /// </summary>
        public Boolean Forzar
        {
            get { return _forzar; }
        }
    }
}
