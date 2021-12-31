using UnityEngine;
using UnityEngine.UI;
using System;

namespace Rc.Aplicacion.Escena
{
    public class FondoPrincipal : EscenaBase
    {
        public FondoPrincipal()
            : base(Resources.Load<GameObject>(Recursos.FONDO_PRINCIPAL))
        {
        }

        public void CambiarColor(String color1, String color2)
        {
            Color color1Parseado;
            ColorUtility.TryParseHtmlString(color1, out color1Parseado);

            Color color2Parseado;
            ColorUtility.TryParseHtmlString(color2, out color2Parseado);

            _instancia.transform.Find("Degradado").GetComponent<Image>().material.SetColor("_Color", color1Parseado);
            _instancia.transform.Find("Degradado").GetComponent<Image>().material.SetColor("_Color2", color2Parseado);

            color1Parseado.a = 15 / 255f;
            _instancia.transform.Find("Rayas").GetComponent<Image>().color = color1Parseado;

        }
    }
}
