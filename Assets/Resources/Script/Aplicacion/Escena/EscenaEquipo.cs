using Rc.Aplicacion.Escena.Componentes;
using Rc.Datos.Modelo;
using UnityEngine;
using System;

namespace Rc.Aplicacion.Escena
{
    public class EscenaEquipo : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly Equipo _equipo;

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaEquipo(Equipo equipo)
          : base(Resources.Load(Recursos.ESCENA_EQUIPO) as GameObject)
        {
            _equipo = equipo;
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
                        
            _instancia.AddComponent<ComponenteEquipo>().EstablecerEscena(this);
            _instancia.GetComponent<ComponenteEquipo>().SeleccionarEquipo(_equipo);

        }

    }
}