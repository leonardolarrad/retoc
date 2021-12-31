using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Juego
{
    public enum EstadoFase
    {
        Ninguno = 0x00,          // Estado de juego por defecto.
        Iniciando = 0x01,        // La ronda está iniciando.
        Ronda = 0x02,
        Puntuacion = 0x04,
        Empate = 0x08,
        PuntuacionEmpate = 0x10,
        Finalizando = 0xFF       // La ronda ha finalizado.

    }
}
