using Rc.Aplicacion.Escena;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Juego
{
    public class ManejadorEscena
    {
        private EscenaBase escena;
        private EscenaFondo fondo;

        public EscenaBase EscenaActual
        {
            get { return escena; }
        }

        public EscenaFondo Fondo
        {
            get { return fondo; }
        }

        public void PresentarEscena(EscenaBase escena, ContinuarControladorEvento funcion)
        {
            if (this.escena != null && this.escena.Instancia != null)
            {
                this.escena.Destruir();
            }

            this.escena = escena;
            ISolicitarContinuar escenaContinuable = (ISolicitarContinuar)this.escena;

            if (escenaContinuable != null)
            {
                // Si la escena puede ser continuable, entonces
                // se establece el evento para solicitar continuar.
                escenaContinuable.SolicitarContinuar += new ContinuarControladorEvento(funcion);
            }

            this.escena.Inicializar(Recursos.LIENZO_PREDETERMINADO, false);
        }

        public void RemoverEscenas()
        {
            if (escena != null && escena.Instancia != null)
            {
                escena.Destruir();
            }

            if (fondo != null && fondo.Instancia != null)
            {
                fondo.Destruir();
            }
        }
    }
}
