using Rc.Aplicacion.Escena.Componentes;
using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Escena
{
    public class EscenaPrueba : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------
        
        public EscenaPrueba()
          : base(Resources.Load(Recursos.ESCENA_RONDA) as GameObject)
        {            
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public virtual void AlSolicitarContinuar(ContinuarArgsEvento e)
        {
            if (SolicitarContinuar != null)
            {
                SolicitarContinuar(this, e);
            }
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);           
        }

    }
}