using UnityEngine;
using System;
using System.Collections.Generic;

namespace Rc.Aplicacion.Escena.Componentes
{
    /// <summary>
    /// Clase de apoyo para realizar animaciones en la aplicación.
    /// </summary>
    public class ComponenteAnimacion
    {
        /// <summary>
        /// Devuelve verdadero si el controlador se encuentra reproduciendo la animación específicada,
        /// devuelve falso en caso contrario.
        /// </summary>
        public static Boolean EstaReproduciendo(Animator controlador, String animacion)
        {
            if (controlador.GetCurrentAnimatorStateInfo(0).IsName(animacion) &&
                controlador.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                return true;
            }            

            return false;
        }

        public static Boolean EstaReproduciendo(Animator controlador)
        {
            if(controlador.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Devuelve verdadero si uno de los controladores se encuentra reproduciendo una animación,
        /// devuelve falso en caso contrario.
        /// </summary>
        public static Boolean EstaReproduciendo(List<Animator> controladores)
        {
            foreach (Animator controlador in controladores)
            {
                if (controlador.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}