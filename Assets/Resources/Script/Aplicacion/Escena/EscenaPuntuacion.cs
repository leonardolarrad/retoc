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
    public class EscenaPuntuacion : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly IEnumerable<KeyValuePair<Equipo, Int32>> _puntuacion;

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaPuntuacion(IEnumerable<KeyValuePair<Equipo, Int32>> puntuacion)
          : base(Resources.Load(Recursos.ESCENA_PUNTUACION) as GameObject)
        {
            _puntuacion = puntuacion;
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public virtual void AlSolicitarContinuar(ContinuarArgsEvento e)
        {
            if (SolicitarContinuar != null)
            {
                SolicitarContinuar(this, e);
            }
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            GameObject cuadroEquipo = Resources.Load(Recursos.EQUIPO_PUNTUACION) as GameObject;
            Int32 i = 0;

            foreach (KeyValuePair<Equipo, Int32> equipo in _puntuacion)
            {
                i++;

                GameObject nuevaInstancia = UnityEngine.Object.Instantiate(cuadroEquipo, _instancia.transform.Find("Columna/Filas"), mantenerPosicionGlobal);
                nuevaInstancia.name = equipo.Key.Nombre;

                nuevaInstancia.transform.Find("CuadroNo/Numero").GetComponent<Text>().text = i.ToString();
                nuevaInstancia.transform.Find("Nombre").GetComponent<Text>().text = equipo.Key.Nombre;
                nuevaInstancia.transform.Find("Puntaje").GetComponent<Text>().text = equipo.Value.ToString();

            }

            _instancia.transform.Find("Continuar").GetComponent<Button>().onClick.AddListener(() => AlSolicitarContinuar(ContinuarArgsEvento.Vacio));
        }

    }
}