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
    public class Visualizador<TEntidad> : EscenaBase
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        protected TEntidad entidad;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        public TEntidad Entidad
        {
            get { return entidad; }
        }

        public Button BotonModificar
        {
            get { return _instancia.transform.Find("Modificar").GetComponent<Button>(); }
        }

        public Button BotonEliminar
        {
            get { return _instancia.transform.Find("Eliminar").GetComponent<Button>(); }
        }

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public Visualizador(TEntidad entidad, GameObject escena)
            : base(escena)
        {
            this.entidad = entidad;
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public Boolean EstaSeleccionado()
        {
            return _instancia.transform.Find("Palanca").GetComponent<Toggle>().isOn;
        }
    }
}
