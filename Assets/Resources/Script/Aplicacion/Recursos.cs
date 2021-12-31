using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion
{
    /// <summary>
    /// Clase de apoyo que contiene la ruta de los recursos usados por la aplicación.
    /// </summary>
    public static class Recursos
    {
        // --------------------------------------------------
        // Animaciones
        // --------------------------------------------------

        public static readonly String ANIMACION_PREDETERMINADA = "Vacio";

        public static readonly String ANIMACION_RONDA_NUMERO = "ronda_prueba";
        public static readonly String ANIMACION_RONDA_RONDA = "ronda_prueba";

        public static readonly String ANIMACION_PASAR_RONDA = "PasarRonda";
        public static readonly String ANIMACION_SELECCIONAR_EQUIPO = "SeleccionarEquipo";
        public static readonly String ANIMACION_SELECCIONAR_CATEGORIA = "SeleccionarCategoria";

        // --------------------------------------------------
        // Menú
        // --------------------------------------------------

        public static readonly String MENU = "Scene/Menu/Menu";

        // --------------------------------------------------
        // Escenas
        // --------------------------------------------------

        public static readonly String FONDO_PRINCIPAL = "Scene/Fondo/FondoPrincipal";

        public static readonly String ESCENA_SELEC_TORNEO = "Scene/Juego/EscenaElegirTorneo";
        public static readonly String SELEC_TORNEO = "Scene/Juego/SelecTorneo";

        public static readonly String ESCENA_FONDO_BASE = "Scene/Fondo/EscenaFondoBase";
        public static readonly String ESCENA_FONDO_01 = "Scene/Fondo/EscenaFondo01";

        public static readonly String ESCENA_TRANSICION_PREDETERMINADA = "";

        public static readonly String ESCENA_PRESENTACION = "Scene/Juego/EscenaPresentacion";
        public static readonly String ESCENA_RONDA = "Scene/Juego/EscenaRonda";
        public static readonly String ESCENA_EQUIPO = "Scene/Juego/EscenaEquipo";
        public static readonly String ESCENA_CATEGORIA = "Scene/Juego/EscenaCategoria";
        public static readonly String ESCENA_PREGUNTA = "Scene/Juego/EscenaPregunta";
        public static readonly String ESCENA_PUNTUACION = "Scene/Juego/EscenaPuntuacion";
        public static readonly String ESCENA_CAMPEON = "Scene/Juego/EscenaCampeon";

        public static readonly String EQUIPO_PUNTUACION = "Scene/Juego/EquipoPuntuacion";

        // --------------------------------------------------
        // Editor
        // --------------------------------------------------

        public static readonly String EDITOR_PRINCIPAL = "Scene/Editor/EditorPrincipal";
        public static readonly String SELECCIONADOR_PREDETERMINADO = "Scene/Editor/Seleccionador";

        public static readonly String VISUALIZADOR_TORNEO = "Scene/Editor/Torneo/VisualizadorTorneo";
        public static readonly String EDITOR_TORNEO = "Scene/Editor/Torneo/EditorTorneo";

        public static readonly String VISUALIZADOR_EQUIPO = "Scene/Editor/Equipo/VisualizadorEquipo";
        public static readonly String EDITOR_EQUIPO = "Scene/Editor/Equipo/EditorEquipo";

        public static readonly String VISUALIZADOR_CATEGORIA = "Scene/Editor/Categoria/VisualizadorCategoria";
        public static readonly String EDITOR_CATEGORIA = "Scene/Editor/Categoria/EditorCategoria";

        public static readonly String VISUALIZADOR_PREGUNTA = "Scene/Editor/Pregunta/VisualizadorPregunta";
        public static readonly String EDITOR_PREGUNTA = "Scene/Editor/Pregunta/EditorPregunta";

        // --------------------------------------------------
        // Lienzos
        // --------------------------------------------------

        public static readonly String LIENZO_PREDETERMINADO = "Lienzo";        
    }
}