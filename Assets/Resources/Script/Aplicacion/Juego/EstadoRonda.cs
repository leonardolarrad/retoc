using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Juego
{
    public enum EstadoRonda
    {
        Ninguno = 0x00,          // Estado de juego por defecto.
        Iniciada = 0x01,         // La ronda está iniciando.
        Equipo = 0x02,           // Seleccionando equipo.
        Categoria = 0x04,        // Seleccionando categoría.
        Pregunta = 0x08,         // En cola para responder pregunta.
        Respuesta = 0x10,        // Ya existe una respuesta, pero aún no se continua el juego.
        Finalizada = 0xFF        // La ronda ha finalizado.
    }
}
