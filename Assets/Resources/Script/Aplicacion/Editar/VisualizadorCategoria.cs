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
    class VisualizadorCategoria : Visualizador<Categoria>
    {
        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public VisualizadorCategoria(Categoria entidad)
            : base(entidad, Resources.Load(Recursos.VISUALIZADOR_CATEGORIA) as GameObject)
        {
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);
            _instancia.transform.Find("Cuadro/Nombre").GetComponent<Text>().text = entidad.Nombre;
            _instancia.transform.Find("Icono").GetComponent<Image>().sprite = Resources.Load<Sprite>(entidad.Imagen);
        }
    }
}
