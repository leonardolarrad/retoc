using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Escena.Componentes
{
    public class ComponenteRonda : ComponenteEscena<EscenaRonda>
    {
        /// <summary>
        /// Método heredado de <see cref="MonoBehaviour"/> que es llamado al iniciar el componenete.
        /// </summary>
        private void Awake()
        {
            StartCoroutine(Continuar());
        }       

        public IEnumerator Continuar()
        {
            yield return new WaitForSeconds(2f);
            _escena.AlSolicitarContinuar(ContinuarArgsEvento.Vacio);
        }

    }
}