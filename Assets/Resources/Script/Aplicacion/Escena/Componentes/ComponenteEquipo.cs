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
    public class ComponenteEquipo : ComponenteEscena<EscenaEquipo>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public void SeleccionarEquipo(Equipo equipo)
        {
            transform.Find("Equipo").GetComponent<Text>().text = equipo.Nombre;

            System.Random aleatorio = new System.Random();
            transform.Find(aleatorio.Next(1, 4).ToString()).gameObject.SetActive(true);
            
            StartCoroutine(Continuar());
        }

        public IEnumerator Continuar()
        {
            yield return new WaitForSeconds(2.5f);
            _escena.AlSolicitarContinuar(ContinuarArgsEvento.Vacio);
        }
    }
}