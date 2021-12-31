using UnityEngine;

namespace Rc.Aplicacion.Escena.Componentes 
{
    public class ComponenteEscena : MonoBehaviour
    {
        protected EscenaBase _escena;

        /// <summary>
        /// Establece la escena del componente.
        /// </summary>
        public void EstablecerEscena(EscenaBase escena)
        {
            _escena = escena;
        }
        
    }

    public class ComponenteEscena<TEscena> : MonoBehaviour
        where TEscena : EscenaBase
    {
        protected TEscena _escena;

        /// <summary>
        /// Establece la escena del componente.
        /// </summary>
        public void EstablecerEscena(TEscena escena)
        {
            _escena = escena;
        }
    }
        
}