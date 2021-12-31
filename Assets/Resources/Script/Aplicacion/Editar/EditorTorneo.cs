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
    public class EditorTorneo : EditorEntidad<Torneo>
    {
        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        public InputField CajaNombre
        {
            get { return _instancia.transform.Find("Cuadro/CajaNombre").GetComponent<InputField>(); }
        }

        public InputField CajaPonderacion
        {
            get { return _instancia.transform.Find("Cuadro/CajaPonderacion").GetComponent<InputField>(); }
        }

        public InputField CajaRondas
        {
            get { return _instancia.transform.Find("Cuadro/CajaRondas").GetComponent<InputField>(); }
        }

        public InputField CajaRondasFinales
        {
            get { return _instancia.transform.Find("Cuadro/CajaRondasFinales").GetComponent<InputField>(); }
        }

        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public EditorTorneo(ManejadorEditor manejadorEditor, Torneo entidad)
            : base(manejadorEditor, Resources.Load(Recursos.EDITOR_TORNEO) as GameObject, entidad)
        {
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(string ruta, bool mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            CajaNombre.text = entidad.Nombre;
            CajaPonderacion.text = entidad.Ponderacion.ToString();
            CajaRondas.text = entidad.Rondas.ToString();
            CajaRondasFinales.text = entidad.RondasFinales.ToString();

            _instancia.transform.Find("Cuadro/BotonGuardar").GetComponent<Button>().onClick.AddListener(BotonGuardar_Click);
            _instancia.transform.Find("Cuadro/BotonDeshacer").GetComponent<Button>().onClick.AddListener(BotonDeshacer_Click);

            _instancia.transform.Find("Equipos").GetComponent<Button>().onClick.AddListener(BotonEquipos_Click);
            _instancia.transform.Find("Categorias").GetComponent<Button>().onClick.AddListener(BotonCategorias_Click);
            _instancia.transform.Find("Preguntas").GetComponent<Button>().onClick.AddListener(BotonPreguntas_Click);
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        protected override void BotonGuardar_Click()
        {
            entidad.Nombre = CajaNombre.text;
            entidad.Ponderacion = Int32.Parse(CajaPonderacion.text);
            entidad.Rondas = Int32.Parse(CajaRondas.text);
            entidad.RondasFinales =  Int32.Parse(CajaRondasFinales.text);

            base.BotonGuardar_Click();
        }

        protected override void BotonDeshacer_Click()
        {
            CajaNombre.text = entidad.Nombre;
            CajaPonderacion.text = entidad.Ponderacion.ToString();
            CajaRondas.text = entidad.Rondas.ToString();
            CajaRondasFinales.text = entidad.RondasFinales.ToString();
        }

        private void BotonEquipos_Click()
        {
            if(entidad.Id != 0)
            {
                manejadorEditor.SeleccionarEquipo(entidad.Id);
            }
            else
            {
                Debug.Log("Se debe crear el torneo primero");
            }
        }

        private void BotonCategorias_Click()
        {
            if (entidad.Id != 0)
            {
                manejadorEditor.SeleccionarCategoria(entidad.Id);
            }
            else
            {
                Debug.Log("Se debe crear el torneo primero");
            }
        }

        private void BotonPreguntas_Click()
        {
            if(entidad.Id == 0)
            {
                Debug.Log("Se debe crear el torneo primero");
                return;
            }

            if(entidad.Categorias.Count < 1)
            {
                Debug.Log("Debe existir al menos una categoría");
                return;
            }

            if (entidad.Id != 0)
            {
                manejadorEditor.SeleccionarPregunta(entidad.Id);
            }
            else
            {
                
            }
        }
    }
}
