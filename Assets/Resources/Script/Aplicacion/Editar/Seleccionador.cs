using Rc.Aplicacion.Escena;
using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Aplicacion.Editor
{    
    public class Seleccionador<TEntidad> : EscenaBase        
        where TEntidad : Objeto
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private ManejadorEditor manejadorEditor;
        private List<Visualizador<TEntidad>> visualizadores;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public Seleccionador(ManejadorEditor manejadorEditor, List<Visualizador<TEntidad>> visualizadores)
            : base(Resources.Load(Recursos.SELECCIONADOR_PREDETERMINADO) as GameObject)
        {
            this.manejadorEditor = manejadorEditor;
            this.visualizadores = visualizadores;
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);

            _instancia.transform.Find("BotonCrearNuevo").GetComponent<Button>().onClick.AddListener(Añadir);
            _instancia.transform.Find("BotonEliminarSelec").GetComponent<Button>().onClick.AddListener(RemoverSelec);

            if (typeof(TEntidad) == typeof(Torneo))
            {
                _instancia.transform.Find("BotonCrearNuevo/Texto").GetComponent<Text>().text = "Crear torneo";               
            }
            else if (typeof(TEntidad) == typeof(Equipo))
            {
                _instancia.transform.Find("BotonCrearNuevo/Texto").GetComponent<Text>().text = "Crear equipo";

                Color azul;
                ColorUtility.TryParseHtmlString("#00a8f9", out azul);

                _instancia.transform.Find("BotonCrearNuevo/Aura1").GetComponent<Image>().color = azul;
                _instancia.transform.Find("BotonCrearNuevo/Aura2").GetComponent<Image>().color = azul;
                _instancia.transform.Find("BotonCrearNuevo/Relleno").GetComponent<Image>().color = azul;

                _instancia.transform.Find("BotonEliminarSelec/Aura1").GetComponent<Image>().color = azul;
                _instancia.transform.Find("BotonEliminarSelec/Aura2").GetComponent<Image>().color = azul;
                _instancia.transform.Find("BotonEliminarSelec/Relleno").GetComponent<Image>().color = azul;

            }
            else if (typeof(TEntidad) == typeof(Categoria))
            {
                _instancia.transform.Find("BotonCrearNuevo/Texto").GetComponent<Text>().text = "Crear categoría";

                Color morado;
                ColorUtility.TryParseHtmlString("#661edc", out morado); 

                _instancia.transform.Find("BotonCrearNuevo/Aura1").GetComponent<Image>().color = morado;
                _instancia.transform.Find("BotonCrearNuevo/Aura2").GetComponent<Image>().color = morado;
                _instancia.transform.Find("BotonCrearNuevo/Relleno").GetComponent<Image>().color = morado;

                _instancia.transform.Find("BotonEliminarSelec/Aura1").GetComponent<Image>().color = morado;
                _instancia.transform.Find("BotonEliminarSelec/Aura2").GetComponent<Image>().color = morado;
                _instancia.transform.Find("BotonEliminarSelec/Relleno").GetComponent<Image>().color = morado;
            }
            else if (typeof(TEntidad) == typeof(Pregunta))
            {
                _instancia.transform.Find("BotonCrearNuevo/Texto").GetComponent<Text>().text = "Crear pregunta";

                Color rojo;
                ColorUtility.TryParseHtmlString("#ce2839", out rojo);

                _instancia.transform.Find("BotonCrearNuevo/Aura1").GetComponent<Image>().color = rojo;
                _instancia.transform.Find("BotonCrearNuevo/Aura2").GetComponent<Image>().color = rojo;
                _instancia.transform.Find("BotonCrearNuevo/Relleno").GetComponent<Image>().color = rojo;

                _instancia.transform.Find("BotonEliminarSelec/Aura1").GetComponent<Image>().color = rojo;
                _instancia.transform.Find("BotonEliminarSelec/Aura2").GetComponent<Image>().color = rojo;
                _instancia.transform.Find("BotonEliminarSelec/Relleno").GetComponent<Image>().color = rojo;
            }

            foreach (Visualizador<TEntidad> visualizador in visualizadores)
            {
                visualizador.Inicializar(ruta);
                visualizador.Instancia.transform.SetParent(_instancia.transform.Find("Columna/Filas").transform);

                visualizador.BotonModificar.onClick.AddListener(() => Modificar(visualizador.Entidad));
                visualizador.BotonEliminar.onClick.AddListener(() => Remover(visualizador.Entidad));
            }            
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public void Añadir()
        {
            if (typeof(TEntidad) == typeof(Torneo))
            {
                manejadorEditor.EditarTorneo(new Torneo());
            }
            else if (typeof(TEntidad) == typeof(Equipo))
            {
                manejadorEditor.EditarEquipo(new Equipo());
            }
            else if (typeof(TEntidad) == typeof(Categoria))
            {
                manejadorEditor.EditarCategoria(new Categoria());
            }
            else if (typeof(TEntidad) == typeof(Pregunta))
            {
                manejadorEditor.EditarPregunta(new Pregunta());
            }
        }

        public void Modificar(TEntidad entidad)
        {
            if (typeof(TEntidad) == typeof(Torneo))
            {
                manejadorEditor.EditarTorneo((Torneo)((Objeto)entidad));
            }
            else if (typeof(TEntidad) == typeof(Equipo))
            {
                manejadorEditor.EditarEquipo((Equipo)((Objeto)entidad));
            }
            else if (typeof(TEntidad) == typeof(Categoria))
            {
                manejadorEditor.EditarCategoria((Categoria)((Objeto)entidad));
            }
            else if (typeof(TEntidad) == typeof(Pregunta))
            {
                manejadorEditor.EditarPregunta((Pregunta)((Objeto)entidad));
            }

            manejadorEditor.Modificar(entidad);
        }

        public void Remover(TEntidad entidad)
        {
            manejadorEditor.Remover(entidad);
            Visualizador<TEntidad> visualizador = visualizadores.Find(x => x.Entidad == entidad);
            visualizador.Destruir();
            visualizadores.Remove(visualizador);
        }

        public void RemoverSelec()
        {
            foreach(TEntidad entidad in visualizadores.Where(x => x.EstaSeleccionado()).Select(x => x.Entidad).ToList())
            {
                Remover(entidad);
            }
        }

    }
}
