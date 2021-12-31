using UnityEngine;
using UnityEngine.UI;
using System;

namespace Rc.Aplicacion.Escena
{
    public class EscenaFondo : EscenaBase
    {
        private EscenaFondoBase _fondoBase;

        public EscenaFondo(GameObject original)
            : base(original)
        {
            _fondoBase = new EscenaFondoBase();
        }

        /// <summary>
        /// Cambia el tono de todos las imagenes de la escena, conservando su luminosidad y saturación.
        /// </summary>
        public void CambiarTono(Int32 tono)
        {
            CambiarTono(tono / 359f);
        }

        /// <summary>
        /// Cambia el tono de todos las imagenes de la escena, conservando su luminosidad y saturación.
        /// </summary>
        public virtual void CambiarTono(Single tono)
        {
            if (_instancia == null)
                throw new Exception("No se ha instanciado la escena.");

            _fondoBase.CambiarTono(tono);

            for(int i = 0; i < _instancia.transform.childCount; i++)
            {
                Image imagen = _instancia.transform.GetChild(i).GetComponent<Image>();

                if(imagen != null)
                {
                    Single h;
                    Single s;
                    Single v;

                    Color.RGBToHSV(imagen.color, out h, out s, out v);
                    imagen.color = Color.HSVToRGB(tono, s, v);        
                }
            }
        }

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            _fondoBase.Inicializar(ruta, mantenerPosicionGlobal);
            base.Inicializar(ruta, mantenerPosicionGlobal);            
        }

        public override void Destruir()
        {
            _fondoBase.Destruir();
            base.Destruir();
        }

    }
}