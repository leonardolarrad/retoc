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
    public class EscenaRonda : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private String _ronda;
        private Boolean desempate;

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaRonda(Int32 ronda)
           : this(Convert.ToString(ronda))
        {
            desempate = true;
        }

        public EscenaRonda(String ronda)
          : base(Resources.Load(Recursos.ESCENA_RONDA) as GameObject)
        {
            _ronda = ronda;
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
            _instancia.transform.Find("Numero").GetComponent<Text>().text = _ronda;


            if(desempate)
            {
                _instancia.transform.Find("Numero").GetComponent<Text>().text = "Ronda de";
                _instancia.transform.Find("Ronda").GetComponent<Text>().text = "Desempate";
            }

            _instancia.AddComponent<ComponenteRonda>();
            _instancia.GetComponent<ComponenteRonda>().EstablecerEscena(this);
        }
    }
}