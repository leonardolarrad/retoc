using UnityEngine;
using System;
using System.Collections;

namespace Rc.Aplicacion.Escena
{
    public class EscenaBase
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------
        
        protected readonly GameObject _original;
        protected GameObject _instancia;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene el recurso original (sin instanciar).
        /// </summary>
        public GameObject Original
        {
            get { return _original; }
        }        
        
        /// <summary>
        /// Obtiene la instancia de la escnea.
        /// </summary>
        public GameObject Instancia
        {
            get { return _instancia; }
        }

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        /// <summary>
        /// Evento provocado al inciarse la escena.
        /// </summary>
        public event EventHandler IniciarEscena;

        /// <summary>
        /// Evento provocado al finalizarse la escena.
        /// </summary>
        public event EventHandler FinalizarEscena;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaBase(GameObject original)
        {
            _original = original;
        }
        
        // --------------------------------------------------  
        // Métodos
        // --------------------------------------------------
        
        /// <summary>
        /// Llamada a la función del evento <see cref="IniciarEscena"/> en caso de que tenga una función asignada.
        /// </summary>
        protected void AlIniciarEscena(EventArgs e)
        {
            if(IniciarEscena != null)
            {
                IniciarEscena(this, e);
            }
        }

        /// <summary>
        /// Llamada a la función del evento <see cref="FinalizarEscena"/> en caso de que tenga una función asignada.
        /// </summary>
        protected void AlFinalizarEscena(EventArgs e)
        {
            if(FinalizarEscena != null)
            {
                FinalizarEscena(this, e);
            }
        }

        /// <summary>
        /// Inicia la escena. 
        /// </summary>
        public virtual void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            AlIniciarEscena(EventArgs.Empty);
            Instanciar(ruta, mantenerPosicionGlobal);
        }

        /// <summary>
        /// Instancia la escena en el ruta específicado.
        /// </summary>
        protected GameObject Instanciar(string ruta, bool mantenerPosicionGlobal)
        {
            if (_original == null)
            {
                Debug.LogError("El objeto de juego a instanciar es nulo.");
                return null;
            }

            if (_instancia != null)
            {
                Debug.LogWarning("Ya existe una instancia del objeto en la escena.");
                return _instancia;
            }

            _instancia = UnityEngine.Object.Instantiate(_original);

            Transform padre = GameObject.Find(ruta).transform;

            if (padre == null)
            {
                Debug.LogError("No se ha podido encontrar el padre en la ruta: " + ruta + ".");
                return null;
            }

            _instancia.transform.SetParent(padre, mantenerPosicionGlobal);
            return _instancia;
        }

        /// <summary>
        /// Vincula los datos que se hayan pasado a la escena.
        /// </summary>
        public virtual void VincularDatos()
        {
            // ...
        }
                
        /// <summary>
        /// Termina los procesos de la escena, y luego, la añade a la lista de objectos por destruir.
        /// </summary>
        public virtual void Destruir()
        {
            if (_instancia == null)
            {
                Debug.LogWarning("La escena ha sido destruida o nunca se instanció.");
                return;
            }
            
            UnityEngine.Object.Destroy(_instancia);
            _instancia = null;

            AlFinalizarEscena(EventArgs.Empty);
        }
    }      
}