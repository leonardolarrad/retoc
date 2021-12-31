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
    public class EscenaSelecTorneo : EscenaBase
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly IEnumerable<Torneo> torneos;
        
        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaSelecTorneo(IEnumerable<Torneo> torneos)
          : base(Resources.Load(Recursos.ESCENA_SELEC_TORNEO) as GameObject)
        {
            this.torneos = torneos;
        }        

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);
            GameObject cuadroTorneo = Resources.Load(Recursos.SELEC_TORNEO) as GameObject;

            foreach(Torneo torneo in torneos)
            {
                GameObject nuevaInstancia = UnityEngine.Object.Instantiate(cuadroTorneo, _instancia.transform.Find("Columna/Filas"), mantenerPosicionGlobal);
                nuevaInstancia.name = torneo.Nombre;

                nuevaInstancia.transform.Find("Nombre").GetComponent<Text>().text = torneo.Nombre;
                nuevaInstancia.transform.Find("Jugar").GetComponent<Button>().onClick.AddListener(() => Aplicacion.EjecutarTorneo(torneo.Id));
            }

            _instancia.transform.Find("Menu").GetComponent<Button>().onClick.AddListener(() => Aplicacion.EjecutarMenu());
        }

    }
}