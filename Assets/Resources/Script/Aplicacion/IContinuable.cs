using System;

namespace Rc.Aplicacion
{
    public interface IContinuable
    {
        void SolicitarContinuar(Object remitente, ContinuarArgsEvento e);
    }
}
