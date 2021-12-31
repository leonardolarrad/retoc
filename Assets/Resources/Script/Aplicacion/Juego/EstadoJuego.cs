using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion
{
    /// <summary>
    /// Enumeración del estado del juego usada para continuar el contexto.
    /// </summary>
    public enum EstadoJuego 
    {
        Ninguno = 0x00,          // Estado de juego por defecto.
        Iniciando = 0x01,        // Aún no se ha pasado la primera ronda.
        PrimeraFase = 0x02,
        UltimaFase = 0x04,      
        Campeon = 0x80,          // Haciendo conocer quién es el campeon.
        Finalizando = 0xFF       // El juego ha terminado.
    }
}
