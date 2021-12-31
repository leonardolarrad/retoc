using Rc.Aplicacion;
using System;

namespace Rc.Aplicacion.Escena.Controlador
{
    public class ControladorPregunta
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly IResponderPregunta destinatario;
        private readonly EscenaPregunta escena;

        // --------------------------------------------------
        // Constructor
        // --------------------------------------------------        

        public ControladorPregunta(EscenaPregunta escena, IResponderPregunta destinatario)
        {
            this.escena = escena;
            this.destinatario = destinatario;           

            // Asignar eventos al controlador.
            this.escena.ResponderPregunta += new RespuestaControladorEvento(Escena_ResponderPregunta);
            this.escena.TiempoFuera += new EventHandler(Escena_TiempoFuera);
        }

        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        private void Escena_ResponderPregunta(Object remitente, RespuestaArgsEvento e)
        {
            destinatario.ResponderPregunta(e.Respuesta, e.Tiempo);
        }

        private void Escena_TiempoFuera(Object remitente, EventArgs e)
        {
            destinatario.TiempoFuera();
        }  
    }
}