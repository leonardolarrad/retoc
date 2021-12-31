using Rc.Datos.Modelo;
using System;
namespace Rc.Aplicacion
{
    public interface IResponderPregunta
    {
        void ResponderPregunta(Respuesta respuesta, Single tiempo);
        void TiempoFuera();
    }
}
