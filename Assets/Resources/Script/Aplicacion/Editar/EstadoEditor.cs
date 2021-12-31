using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Editor
{
    public enum EstadoEditor
    {
        Principal, // Predeterminado
        ModificarTorneo = 10,

        SelecEquipo = 11,
        ModificarEquipo = 21,

        SelecCategoria = 12,
        ModificarCategoria = 22,

        SelecPregunta = 13,
        ModificarPregunta = 23
    }
}
