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
    public class EscenaCategoria : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly IEnumerable<Categoria> _categorias;
        private readonly Categoria _seleccion;
        private readonly EscenaFondo _fondo;

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaCategoria(IEnumerable<Categoria> categorias, Categoria seleccion, EscenaFondo fondo)
            : base(Resources.Load(Recursos.ESCENA_CATEGORIA) as GameObject)
        {
            _categorias = categorias;
            _seleccion = seleccion;
            _fondo = fondo;
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
            _instancia.AddComponent<ComponenteCategoria>();
            _instancia.GetComponent<ComponenteCategoria>().EstablecerEscena(this);
            _instancia.GetComponent<ComponenteCategoria>().SeleccionarCategoria(_categorias, _seleccion, _fondo);
        }
    }
}