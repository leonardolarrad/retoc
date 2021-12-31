using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Escena.Componentes
{
    public class ComponentePregunta : ComponenteEscena<EscenaPregunta>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private Boolean _estado;

        private Single _tiempo;
        private Single _tiempoTranscurrido;

        private Single _retardo;
        private Single _retardoTranscurrido;

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        /// <summary>
        /// Inicializa el temporizador con un tiempo definido luego de un retardo.
        /// </summary>
        public void IniciarTiempo(Single tiempo, Single retardo)
        {
            _tiempo = tiempo;
            _tiempoTranscurrido = _tiempo;

            _retardo = retardo;
            _retardoTranscurrido = _retardo;

            _estado = true;
        }
        
        /// <summary>
        /// Pausa el temporizador.
        /// </summary>
        public void PausarTiempo()
        {
            _estado = false;
        }

        /// <summary>
        /// Continua el temporizador.
        /// </summary>
        public void ContinuarTiempo()
        {
            _estado = true;
        }

        /// <summary>
        /// Método heredado de <see cref="MonoBehaviour"/> que es llamado una vez por cuadro.
        /// </summary>
        private void LateUpdate()
        {
            if (!_estado)
                return;

            if(_retardoTranscurrido > 0)
            {
                _retardoTranscurrido -= Time.deltaTime;
                return;
            }

            if(_tiempoTranscurrido > 0)
            {
                _tiempoTranscurrido -= Time.deltaTime;
                _escena.Instancia.transform.Find("Pregunta/Tiempo").GetComponent<Image>().fillAmount = _tiempoTranscurrido / _tiempo;
            }
            else
            {
                _escena.EnTiempoFuera(EventArgs.Empty);
            }
        }
    }
}