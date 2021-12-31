using UnityEngine;
using UnityEngine.UI;
using System;

namespace Rc.Aplicacion.Escena
{
    public class EscenaFondoBase : EscenaBase
    {
        public EscenaFondoBase()
            : base(Resources.Load(Recursos.ESCENA_FONDO_BASE) as GameObject)
        {
        }
        
        /// <summary>
        /// Cambia el degrado de dos colores del fondo base.
        /// </summary>
        public void CambiarTono(Color32 color1, Color32 color2)
        {
            _instancia.GetComponent<Image>().material.SetColor("_Color", color1);
            _instancia.GetComponent<Image>().material.SetColor("_Color2", color2);
        }

        /// <summary>
        /// Cambia el tono de todos las imagenes de la escena, conservando su luminosidad y saturación.
        /// </summary>
        public void CambiarTono(Single tono)
        {
            Single h;
            Single s;
            Single v;

            Color.RGBToHSV(_instancia.GetComponent<Image>().material.GetColor("_Color"), out h, out s, out v);
            _instancia.GetComponent<Image>().material.SetColor("_Color", Color.HSVToRGB(tono, s, v));

            Color.RGBToHSV(_instancia.GetComponent<Image>().material.GetColor("_Color2"), out h, out s, out v);
            _instancia.GetComponent<Image>().material.SetColor("_Color2", Color.HSVToRGB(tono, s, v));
        }
    }
}