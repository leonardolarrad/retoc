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
    class VisualizadorTorneo : Visualizador<Torneo>
    {
        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public VisualizadorTorneo(Torneo entidad)
            : base(entidad, Resources.Load(Recursos.VISUALIZADOR_TORNEO) as GameObject)
        {
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            _instancia.transform.Find("Cuadro/Nombre").GetComponent<Text>().text = entidad.Nombre;
            _instancia.transform.Find("Cuadro/Equipos").GetComponent<Text>().text = entidad.Equipos.Count.ToString();
            _instancia.transform.Find("Cuadro/Categorias").GetComponent<Text>().text = entidad.Categorias.Count.ToString();
            _instancia.transform.Find("Cuadro/Preguntas").GetComponent<Text>().text = entidad.Preguntas.Count.ToString();

        }
    }
}
