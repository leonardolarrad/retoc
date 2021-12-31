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
    public class VisualizadorEquipo : Visualizador<Equipo>
    {
        public VisualizadorEquipo(Equipo entidad)
            : base(entidad, Resources.Load(Recursos.VISUALIZADOR_EQUIPO) as GameObject)
        {
        }

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);
            _instancia.transform.Find("Cuadro/Nombre").GetComponent<Text>().text = entidad.Nombre;
        }
    }
}
