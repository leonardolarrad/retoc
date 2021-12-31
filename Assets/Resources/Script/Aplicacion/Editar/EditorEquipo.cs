using Rc.Aplicacion.Escena;
using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Rc.Aplicacion.Editor
{
    public class EditorEquipo : EditorEntidad<Equipo>
    {
        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        public InputField CajaNombre
        {
            get { return _instancia.transform.Find("Cuadro/CajaNombre").GetComponent<InputField>(); }
        }

        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public EditorEquipo(ManejadorEditor manejadorEditor, Equipo entidad)
            : base(manejadorEditor, Resources.Load(Recursos.EDITOR_EQUIPO) as GameObject, entidad)
        {
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(string ruta, bool mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            CajaNombre.text = entidad.Nombre;

            _instancia.transform.Find("Cuadro/BotonGuardar").GetComponent<Button>().onClick.AddListener(BotonGuardar_Click);
            _instancia.transform.Find("Cuadro/BotonDeshacer").GetComponent<Button>().onClick.AddListener(BotonDeshacer_Click);
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        protected override void BotonGuardar_Click()
        {
            entidad.Nombre = CajaNombre.text;
            base.BotonGuardar_Click();
            manejadorEditor.BotonVolver_Click();
        }

        protected override void BotonDeshacer_Click()
        {
            CajaNombre.text = entidad.Nombre;
        }
    }
}
