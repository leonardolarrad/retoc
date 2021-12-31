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
    public class EditorPregunta : EditorEntidad<Pregunta>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private List<Categoria> categorias;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        public InputField Encabezado
        {
            get { return _instancia.transform.Find("Cuadro/CajaEncabezado").GetComponent<InputField>(); }
        }

        public InputField RespuestaA
        {
            get { return _instancia.transform.Find("Cuadro/CajaRespuestaA").GetComponent<InputField>(); }
        }

        public InputField RespuestaB
        {
            get { return _instancia.transform.Find("Cuadro/CajaRespuestaB").GetComponent<InputField>(); }
        }

        public InputField RespuestaC
        {
            get { return _instancia.transform.Find("Cuadro/CajaRespuestaC").GetComponent<InputField>(); }
        }

        public InputField RespuestaD
        {
            get { return _instancia.transform.Find("Cuadro/CajaRespuestaD").GetComponent<InputField>(); }
        }
        
        public Dropdown RespuestaCorrecta
        {
            get { return _instancia.transform.Find("Cuadro/DesplegableRespuestaCorrecta").GetComponent<Dropdown>(); }
        }

        public Dropdown Categoria
        {
            get { return _instancia.transform.Find("Cuadro/DesplegableCategoria").GetComponent<Dropdown>(); }
        }

        public InputField Aprendizaje
        {
            get { return _instancia.transform.Find("Cuadro/CajaAprendizaje").GetComponent<InputField>(); }
        }

        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public EditorPregunta(ManejadorEditor manejadorEditor, Pregunta entidad, List<Categoria> categorias)
            : base(manejadorEditor, Resources.Load(Recursos.EDITOR_PREGUNTA) as GameObject, entidad)
        {
            this.categorias = categorias;
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(string ruta, bool mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            Encabezado.text = entidad.Encabezado;

            RespuestaA.text = entidad.RespuestaA;
            RespuestaB.text = entidad.RespuestaB;
            RespuestaC.text = entidad.RespuestaC;
            RespuestaD.text = entidad.RespuestaD;

            RespuestaCorrecta.value = (Int32)entidad.RespuestaCorrecta;

            Categoria.AddOptions(categorias.OrderBy(x => x.Nombre).Select(x => x.Nombre).ToList());
            Categoria.value = !entidad.EstaVacio() ? categorias.OrderBy(x => x.Nombre).ToList().IndexOf(categorias.Find(x => x.Id == entidad.Categoria.Id)) : 0;

            Aprendizaje.text = entidad.Aprendizaje;

            _instancia.transform.Find("Cuadro/BotonGuardar").GetComponent<Button>().onClick.AddListener(BotonGuardar_Click);
            _instancia.transform.Find("Cuadro/BotonDeshacer").GetComponent<Button>().onClick.AddListener(BotonDeshacer_Click);
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        protected override void BotonGuardar_Click()
        {
            entidad.Encabezado = Encabezado.text;

            entidad.RespuestaA = RespuestaA.text;
            entidad.RespuestaB = RespuestaB.text;
            entidad.RespuestaC = RespuestaC.text;
            entidad.RespuestaD = RespuestaD.text;

            entidad.RespuestaCorrecta = (Respuesta)RespuestaCorrecta.value;
            entidad.Categoria = categorias.OrderBy(x => x.Nombre).ToList()[Categoria.value];

            entidad.Aprendizaje = Aprendizaje.text;

            base.BotonGuardar_Click();
            manejadorEditor.BotonVolver_Click();
        }

        protected override void BotonDeshacer_Click()
        {
            Encabezado.text = entidad.Encabezado;

            RespuestaA.text = entidad.RespuestaA;
            RespuestaB.text = entidad.RespuestaB;
            RespuestaC.text = entidad.RespuestaC;
            RespuestaD.text = entidad.RespuestaD;

            RespuestaCorrecta.value = (Int32)entidad.RespuestaCorrecta;
            Categoria.value = categorias.OrderBy(x => x.Nombre).ToList().IndexOf(categorias.Find(x => x.Id == entidad.Categoria.Id));

            Aprendizaje.text = entidad.Aprendizaje;
        }
    }
}
