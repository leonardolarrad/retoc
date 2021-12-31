using Rc.Aplicacion.Escena.Componentes;
using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Rc.Aplicacion.Escena
{
    public class EscenaCampeon : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly Equipo _campeon;

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaCampeon(Equipo campeon)
          : base(Resources.Load(Recursos.ESCENA_CAMPEON) as GameObject)
        {
            _campeon = campeon;
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

            _instancia.transform.Find("Campeon").GetComponent<Text>().text = _campeon.Nombre.ToUpper();
        }

    }
}