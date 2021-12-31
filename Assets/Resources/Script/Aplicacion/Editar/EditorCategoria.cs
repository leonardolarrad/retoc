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
    public class EditorCategoria : EditorEntidad<Categoria>
    {
        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        public InputField CajaNombre
        {
            get { return _instancia.transform.Find("Cuadro/CajaNombre").GetComponent<InputField>(); }
        }

        public Image Icono
        {
            get { return _instancia.transform.Find("Cuadro/Icono").GetComponent<Image>();  }
        }

        public Dropdown DespegableIcono
        {
            get { return _instancia.transform.Find("Cuadro/DesplegableIcono").GetComponent<Dropdown>(); }
        }

        public Dropdown DespegableColor
        {
            get { return _instancia.transform.Find("Cuadro/DesplegableColor").GetComponent<Dropdown>(); }
        }

        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------

        public EditorCategoria(ManejadorEditor manejadorEditor, Categoria entidad)
            : base(manejadorEditor, Resources.Load(Recursos.EDITOR_CATEGORIA) as GameObject, entidad)
        {
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(string ruta, bool mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            CajaNombre.text = entidad.Nombre;

            switch(entidad.Imagen)
            {
                case "Texture/Categoria/Ajedrez":

                    DespegableIcono.value = 0;
                    break;

                case "Texture/Categoria/Biologia":
                    DespegableIcono.value = 1;
                    break;

                case "Texture/Categoria/Castellano":
                    DespegableIcono.value = 2;
                    break;

                case "Texture/Categoria/Cine":
                    DespegableIcono.value = 3;
                    break;

                case "Texture/Categoria/Computacion":
                    DespegableIcono.value = 4;
                    break;

                case "Texture/Categoria/Deporte":
                    DespegableIcono.value = 5;
                    break;

                case "Texture/Categoria/Fisica":
                    DespegableIcono.value = 6;
                    break;

                case "Texture/Categoria/Geografia":
                    DespegableIcono.value = 7;
                    break;

                case "Texture/Categoria/Historia":
                    DespegableIcono.value = 8;
                    break;

                case "Texture/Categoria/Matematica":
                    DespegableIcono.value = 9;
                    break;

                case "Texture/Categoria/Quimica":
                    DespegableIcono.value = 10;
                    break;

                case "Texture/Categoria/Religion":
                    DespegableIcono.value = 11;
                    break;

                case "Texture/Categoria/Noticia":
                    DespegableIcono.value = 12;
                    break;
            }

            switch(entidad.Color1)
            {
                case "#A91818":
                    DespegableColor.value = 0;
                    break;


                case "#A91881":
                    DespegableColor.value = 1;
                    break;

                case "#4500B6":
                    DespegableColor.value = 2;
                    break;


                case "#274AFF":
                    DespegableColor.value = 3;
                    break;

                case "#12893F":
                    DespegableColor.value = 4;
                    break;


                case "#18A979":
                    DespegableColor.value = 5;
                    break;

                case "#B29700":
                    DespegableColor.value = 6;
                    break;


                case "#B23700":
                    DespegableColor.value = 7;
                    break;

                case "#081570":
                    DespegableColor.value = 8;
                    break;


                case "#B27200":
                    DespegableColor.value = 9;
                    break;

            }

            Icono.sprite = Resources.Load<Sprite>(entidad.Imagen);

            _instancia.transform.Find("Cuadro/BotonGuardar").GetComponent<Button>().onClick.AddListener(BotonGuardar_Click);
            _instancia.transform.Find("Cuadro/BotonDeshacer").GetComponent<Button>().onClick.AddListener(BotonDeshacer_Click);
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        protected override void BotonGuardar_Click()
        {
            entidad.Nombre = CajaNombre.text;

            switch (DespegableIcono.value)
            {
                case 0:

                    entidad.Imagen = "Texture/Categoria/Ajedrez";
                    break;

                case 1:
                    entidad.Imagen = "Texture/Categoria/Biologia";
                    break;

                case 2:
                    entidad.Imagen = "Texture/Categoria/Castellano";
                    break;

                case 3:
                    entidad.Imagen = "Texture/Categoria/Cine";
                    break;

                case 4:
                    entidad.Imagen = "Texture/Categoria/Computacion";
                    break;

                case 5:
                    entidad.Imagen = "Texture/Categoria/Deporte";
                    break;

                case 6:
                    entidad.Imagen = "Texture/Categoria/Fisica";
                    break;

                case 7:
                    entidad.Imagen = "Texture/Categoria/Geografia";
                    break;

                case 8:
                    entidad.Imagen = "Texture/Categoria/Historia";
                    break;

                case 9:
                    entidad.Imagen = "Texture/Categoria/Matematica";
                    break;

                case 10:
                    entidad.Imagen = "Texture/Categoria/Quimica";
                    break;

                case 11:
                    entidad.Imagen = "Texture/Categoria/Religion";
                    break;

                case 12:
                    entidad.Imagen = "Texture/Categoria/Noticia";
                    break;
            }

            switch(DespegableColor.value)
            {
                case 0:
                    entidad.Color1 = "#A91818";
                    entidad.Color2 = "#FF1E64";
                    break;

                case 1:
                    entidad.Color1 = "#A91881";
                    entidad.Color2 = "#FF1E6D";
                    break;

                case 2:
                    entidad.Color1 = "#4500B6";
                    entidad.Color2 = "#AD0ADB";
                    break;

                case 3:
                    entidad.Color1 = "#274AFF";
                    entidad.Color2 = "#00ABFF";
                    break;

                case 4:
                    entidad.Color1 = "#12893F";
                    entidad.Color2 = "#1EFF86";
                    break;

                case 5:
                    entidad.Color1 = "#18A979";
                    entidad.Color2 = "#1ED0FF";
                    break;

                case 6:
                    entidad.Color1 = "#B29700";
                    entidad.Color2 = "#FFDD1E";
                    break;

                case 7:
                    entidad.Color1 = "#B23700";
                    entidad.Color2 = "#FF5B1E";
                    break;

                case 8:
                    entidad.Color1 = "#081570";
                    entidad.Color2 = "#1E86FF";
                    break;

                case 9:
                    entidad.Color1 = "#B27200";
                    entidad.Color2 = "#E1840F";
                    break;
            }

            base.BotonGuardar_Click();
            manejadorEditor.BotonVolver_Click();
        }

        protected override void BotonDeshacer_Click()
        {
            CajaNombre.text = entidad.Nombre;

            switch (entidad.Imagen)
            {
                case "Texture/Categoria/Ajedrez":

                    DespegableIcono.value = 0;
                    break;

                case "Texture/Categoria/Biologia":
                    DespegableIcono.value = 1;
                    break;

                case "Texture/Categoria/Castellano":
                    DespegableIcono.value = 2;
                    break;

                case "Texture/Categoria/Cine":
                    DespegableIcono.value = 3;
                    break;

                case "Texture/Categoria/Computacion":
                    DespegableIcono.value = 4;
                    break;

                case "Texture/Categoria/Deporte":
                    DespegableIcono.value = 5;
                    break;

                case "Texture/Categoria/Fisica":
                    DespegableIcono.value = 6;
                    break;

                case "Texture/Categoria/Geografia":
                    DespegableIcono.value = 7;
                    break;

                case "Texture/Categoria/Historia":
                    DespegableIcono.value = 8;
                    break;

                case "Texture/Categoria/Matematica":
                    DespegableIcono.value = 9;
                    break;

                case "Texture/Categoria/Quimica":
                    DespegableIcono.value = 10;
                    break;

                case "Texture/Categoria/Religion":
                    DespegableIcono.value = 11;
                    break;

                case "Texture/Categoria/Noticia":
                    DespegableIcono.value = 12;
                    break;
            }

            switch (entidad.Color1)
            {
                case "#A91818":
                    DespegableColor.value = 0;
                    break;


                case "#A91881":
                    DespegableColor.value = 1;
                    break;

                case "#4500B6":
                    DespegableColor.value = 2;
                    break;


                case "#274AFF":
                    DespegableColor.value = 3;
                    break;

                case "#12893F":
                    DespegableColor.value = 4;
                    break;


                case "#18A979":
                    DespegableColor.value = 5;
                    break;

                case "#B29700":
                    DespegableColor.value = 6;
                    break;


                case "#B23700":
                    DespegableColor.value = 7;
                    break;

                case "#081570":
                    DespegableColor.value = 8;
                    break;


                case "#B27200":
                    DespegableColor.value = 9;
                    break;

            }
        }
    }
}
