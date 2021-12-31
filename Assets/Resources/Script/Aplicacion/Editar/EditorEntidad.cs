using Rc.Aplicacion.Escena;
using Rc.Datos.Modelo;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Editor
{
    public abstract class EditorEntidad<TEntidad> : EscenaBase
        where TEntidad : Objeto
    {
        protected ManejadorEditor manejadorEditor;
        protected TEntidad entidad;

        public EditorEntidad(ManejadorEditor manejadorEditor, GameObject escena, TEntidad entidad)
            : base (escena)
        {
            this.manejadorEditor = manejadorEditor;
            this.entidad = entidad;
        }

        protected virtual void BotonGuardar_Click()
        {
            entidad = (TEntidad)manejadorEditor.Modificar(entidad);
        }

        protected abstract void BotonDeshacer_Click();
    }
}
