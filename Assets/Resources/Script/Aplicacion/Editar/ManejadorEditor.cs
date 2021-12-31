using Rc.Aplicacion.Escena;
using Rc.Datos.Modelo;
using Rc.Datos.Basedatos;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Editor
{
    public enum EditorIcono
    {
        Editor,
        Torneo,
        Equipo,
        Categoria,
        Pregunta
    }

    public class ManejadorEditor
    {          
        private RcBasedatos basedatos;
        private EscenaBase escenaPrincipal;
        private EscenaBase escenaActual;
        private EstadoEditor estado;
        private Torneo torneoActual;

        public ManejadorEditor(RcBasedatos basedatos)
        {
            this.basedatos = basedatos;            

            escenaPrincipal = new EscenaBase(Resources.Load(Recursos.EDITOR_PRINCIPAL) as GameObject);
            escenaPrincipal.Inicializar(Recursos.LIENZO_PREDETERMINADO);
            escenaPrincipal.Instancia.transform.Find("Menu").GetComponent<Button>().onClick.AddListener(BotonMenu_Click);
            escenaPrincipal.Instancia.transform.Find("Volver").GetComponent<Button>().onClick.AddListener(BotonVolver_Click);

            SeleccionarTorneo();
        }

        // ----------------------------------------------------------

        public void CambiarNavegador(EstadoEditor estado, String titulo, String subtitulo, 
            EditorIcono icono, EscenaBase escena, String color, Boolean volver)
        {
            this.estado = estado;
            escenaPrincipal.Instancia.transform.Find("Navegador/Titulo").GetComponent<Text>().text = titulo;
            escenaPrincipal.Instancia.transform.Find("Navegador/Subtitulo").GetComponent<Text>().text = subtitulo;

            if(color != String.Empty)
            {
                Color tmp;
                ColorUtility.TryParseHtmlString(color, out tmp);
                escenaPrincipal.Instancia.transform.Find("Navegador/Relleno").GetComponent<Image>().color = tmp;

                escenaPrincipal.Instancia.transform.Find("Volver/Relleno").GetComponent<Image>().color = tmp;
                escenaPrincipal.Instancia.transform.Find("Volver/Aura1").GetComponent<Image>().color = tmp;
                escenaPrincipal.Instancia.transform.Find("Volver/Aura2").GetComponent<Image>().color = tmp;
            }

            if(escenaActual != null)
            {
                escenaActual.Destruir();
            }
            
            escenaActual = escena;

            if(volver)
            {
                MostrarBotonVolver();
            }
            else
            {
                MostrarBotonMenu();
            }
        }

        // ----------------------------------------------------------

        public void SeleccionarTorneo()
        {
            List<Visualizador<Torneo>> visualizadores = new List<Visualizador<Torneo>>();

            foreach(Torneo torneo in basedatos.ObtenerListaTorneos())
            {
                VisualizadorTorneo visualizador = new VisualizadorTorneo(torneo);
                visualizadores.Add(visualizador);
            }

            Seleccionador<Torneo> seleccionador = new Seleccionador<Torneo>(this, visualizadores);
            seleccionador.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            CambiarNavegador(EstadoEditor.Principal, "Editor", "Selec. torneo", EditorIcono.Editor, seleccionador, "#dc0b4b", false);
        }

        public void EditarTorneo(Torneo torneo)
        {
            EditorTorneo editor = new EditorTorneo(this, torneo);
            editor.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            torneoActual = torneo;

            CambiarNavegador(EstadoEditor.ModificarTorneo, torneo.Nombre ?? "Nuevo torneo", 
                "Editar torneo", EditorIcono.Editor, editor, "#3abd7e", true);
        }

        // ----------------------------------------------------------

        public void SeleccionarEquipo(Int64 torneo)
        {
            List<Visualizador<Equipo>> visualizadores = new List<Visualizador<Equipo>>();

            foreach (Equipo equipo in basedatos.ObtenerListaEquipos("WHERE `torneo_id` = " + torneo.ToString()).OrderBy(x => x.Nombre))
            {
                VisualizadorEquipo visualizador = new VisualizadorEquipo(equipo);
                visualizadores.Add(visualizador);
            }

            Seleccionador<Equipo> seleccionador = new Seleccionador<Equipo>(this, visualizadores);
            seleccionador.Inicializar(Recursos.LIENZO_PREDETERMINADO);           

            CambiarNavegador(EstadoEditor.SelecEquipo, "Selec. equipo",
               torneoActual.Nombre, EditorIcono.Equipo, seleccionador, "#00a8f9", true);
        }

        public void EditarEquipo(Equipo equipo)
        {
            EditorEquipo editor = new EditorEquipo(this, equipo);
            editor.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            CambiarNavegador(EstadoEditor.ModificarEquipo, "Editar equipo",
                 torneoActual.Nombre, EditorIcono.Equipo, editor, "#00a8f9", true);
        }

        // ----------------------------------------------------------

        public void SeleccionarCategoria(Int64 torneo)
        {
            List<Visualizador<Categoria>> visualizadores = new List<Visualizador<Categoria>>();

            foreach (Categoria categoria in basedatos.ObtenerListaCategorias("WHERE `torneo_id` = " + torneo.ToString()).OrderBy(x => x.Nombre))
            {
                VisualizadorCategoria visualizador = new VisualizadorCategoria(categoria);
                visualizadores.Add(visualizador);
            }

            Seleccionador<Categoria> seleccionador = new Seleccionador<Categoria>(this, visualizadores);
            seleccionador.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            CambiarNavegador(EstadoEditor.SelecCategoria, "Selec. categoria",
               torneoActual.Nombre, EditorIcono.Categoria, seleccionador, "#661edc", true);
        }
        
        public void EditarCategoria(Categoria categoria)
        {
            EditorCategoria editor = new EditorCategoria(this, categoria);
            editor.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            CambiarNavegador(EstadoEditor.ModificarCategoria, "Editar categoría",
                 torneoActual.Nombre, EditorIcono.Categoria, editor, "#661edc", true);
        }

        // ----------------------------------------------------------

        public void SeleccionarPregunta(Int64 torneo)
        {
            List<Visualizador<Pregunta>> visualizadores = new List<Visualizador<Pregunta>>();

            foreach (Pregunta pregunta in basedatos.ObtenerListaPreguntas("WHERE `torneo_id` = " + torneo.ToString()).OrderBy(x => x.Categoria.Nombre))
            {
                VisualizadorPregunta visualizador = new VisualizadorPregunta(pregunta);
                visualizadores.Add(visualizador);
            }

            Seleccionador<Pregunta> seleccionador = new Seleccionador<Pregunta>(this, visualizadores);
            seleccionador.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            CambiarNavegador(EstadoEditor.SelecCategoria, "Selec. pregunta",
               torneoActual.Nombre, EditorIcono.Pregunta, seleccionador, "#ce2839", true);
        }

        public void EditarPregunta(Pregunta pregunta)
        {
            EditorPregunta editor = new EditorPregunta(this, pregunta, basedatos.ObtenerListaCategorias("WHERE `torneo_id` = " + torneoActual.Id.ToString()));
            editor.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            CambiarNavegador(EstadoEditor.ModificarPregunta, "Editar pregunta",
                 torneoActual.Nombre, EditorIcono.Pregunta, editor, "#ce2839", true);
        }

        // ----------------------------------------------------------
        
        public Objeto Modificar(Objeto entidad)
        {
            if (entidad is Torneo)
            {
                if(entidad.Id == 0)
                {
                    basedatos.AñadirTorneo((Torneo)entidad);
                    torneoActual.Id = entidad.Id;
                }
                else
                {
                    basedatos.ActualizarTorneo((Torneo)entidad);
                }

                escenaPrincipal.Instancia.transform.Find("Navegador/Titulo").GetComponent<Text>().text = ((Torneo)entidad).Nombre;
                return basedatos.ObtenerTorneo(entidad.Id);
            }
            else if (entidad is Equipo)
            {
                if (entidad.Id == 0)
                {
                    basedatos.AñadirEquipo((Equipo)entidad, torneoActual.Id);
                }
                else
                {
                    basedatos.ActualizarEquipo((Equipo)entidad);
                }
            }
            else if (entidad is Categoria)
            {
                if (entidad.Id == 0)
                {
                    basedatos.AñadirCategoria((Categoria)entidad, torneoActual.Id);
                }
                else
                {
                    basedatos.ActualizarCategoria((Categoria)entidad);
                }               
            }
            else if (entidad is Pregunta)
            {
                if (entidad.Id == 0)
                {
                    basedatos.AñadirPregunta((Pregunta)entidad, torneoActual.Id);
                }
                else
                {
                    Debug.Log("actualizar pregunta");
                    Debug.Log(basedatos.ActualizarPregunta((Pregunta)entidad));
                }
            }
            else
            {
                throw new Exception("El objeto que se intenta eliminar no se " +
                    "reconoce como un objeto tipo de la base de datos.");
            }

            return entidad;
        }

        public void Remover(Objeto entidad)
        {
            if(entidad is Torneo)
            {
                basedatos.RemoverTorneo(entidad.Id);
            }
            else if(entidad is Equipo)
            {
                basedatos.RemoverEquipo(entidad.Id);
            }
            else if(entidad is Categoria)
            {
                basedatos.RemoverCategoria(entidad.Id);
            }
            else if(entidad is Pregunta)
            {
                basedatos.RemoverPregunta(entidad.Id);
            }
            else
            {
                throw new Exception("El objeto que se intenta eliminar no se " +
                    "reconoce como un objeto tipo de la base de datos.");
            }
        }

        // Botones ------------------------------------------

        public void MostrarBotonVolver()
        {
            escenaPrincipal.Instancia.transform.Find("Menu").gameObject.SetActive(false);
            escenaPrincipal.Instancia.transform.Find("Volver").gameObject.SetActive(true);
        }

        public void MostrarBotonMenu()
        {
            escenaPrincipal.Instancia.transform.Find("Menu").gameObject.SetActive(true);
            escenaPrincipal.Instancia.transform.Find("Volver").gameObject.SetActive(false);
        }

        public void BotonMenu_Click()
        {
            Aplicacion.EjecutarMenu();
        }

        public void BotonVolver_Click()
        {
            switch (estado)
            {
                case EstadoEditor.ModificarTorneo:                    
                    SeleccionarTorneo();
                    break;

                case EstadoEditor.SelecEquipo:
                case EstadoEditor.SelecCategoria:
                case EstadoEditor.SelecPregunta:

                    EditarTorneo(torneoActual);
                    break;

                case EstadoEditor.ModificarEquipo:

                    SeleccionarEquipo(torneoActual.Id);                    
                    break;

                case EstadoEditor.ModificarCategoria:

                    SeleccionarCategoria(torneoActual.Id);
                    break;

                case EstadoEditor.ModificarPregunta:

                    SeleccionarPregunta(torneoActual.Id);
                    break;
            }
        }

        // Importar / Exportar

        public void Importar()
        {

        }

        public void Exportar()
        {

        }

    }
}
