using Rc.Aplicacion.Escena;
using Rc.Aplicacion.Escena.Componentes;
using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion
{
    public class MenuPrincipal : EscenaBase
    {

        public MenuPrincipal()
            : base(Resources.Load<GameObject>(Recursos.MENU))
        {
        }

        public override void Inicializar(string ruta, bool mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            _instancia.transform.Find("Jugar").GetComponent<Button>().onClick.AddListener(Aplicacion.EjecutarJuego);
            _instancia.transform.Find("Editar").GetComponent<Button>().onClick.AddListener(Aplicacion.EjecutarEditor);
            _instancia.transform.Find("Salir").GetComponent<Button>().onClick.AddListener(Aplicacion.Salir);
        }

    }
}
