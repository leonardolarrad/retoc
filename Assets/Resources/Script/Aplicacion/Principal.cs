using Rc.Herramientas;
using Rc.Aplicacion.Editor;
using Rc.Aplicacion.Escena;
using Rc.Aplicacion.Juego;
using Rc.Aplicacion.Escena.Controlador;
using Rc.Datos.Modelo;
using Rc.Datos.Basedatos;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Xml;
using System.Xml.Serialization;

namespace Rc.Aplicacion
{
    /// <summary>
    /// Clase principal de la aplicación. 
    /// </summary>
    public class Principal : MonoBehaviour
    {
        /// <summary>
        /// Punto de entrada del programa.
        /// </summary>
        private void Awake()
        {
            Aplicacion.Ejecutar();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                Reanudar1();
            }
            else if(Input.GetKeyDown(KeyCode.F2))
            {
                Reanudar2();
            }
            else if(Input.GetKeyDown(KeyCode.F3))
            {
                Reanudar3();
            }
            
        }

        private void Reanudar1()
        {
            Aplicacion.ReanudarJuego("/../Tmp/" + "/Presentacion/Presentacion1.historial");
        }

        private void Reanudar2()
        {
            Aplicacion.ReanudarJuego("/../Tmp/" + "/Presentacion/Presentacion2.historial");
        }

        private void Reanudar3()
        {
            Aplicacion.ReanudarJuego("/../Tmp/" + "/Presentacion/Presentacion3.historial");
        }

        private void Editor()
        {
            String ruta = Application.dataPath + "/../RcBasedatos.db";

#if UNITY_EDITOR
            ruta = Application.dataPath + "/Resources/Database/RcBasedatos.db";
#endif
            RcBasedatos basedatos = new RcBasedatos(ruta);

            ManejadorEditor editor = new ManejadorEditor(basedatos);
        }

        private void ReanudarJuego()
        {
            ManejadorJuego manejador = new ManejadorJuego(CargarHistorial());
            manejador.Reanudar();
        }

        private HistorialJuego CargarHistorial()
        {
            HistorialJuego historial = HistorialJuego.Cargar(Application.dataPath + "/../Tmp/" + "/Primaria/Primaria.historial");

            foreach(Fase f in historial.Fases)
            {
                Debug.Log("Fase: " + f.Tipo + " | " + f.Estado + " >> " + f.EstadoProximo);
            }

            foreach(Ronda r in historial.Rondas)
            {
                Debug.Log("Ronda: " + r.Numero + " | " + r.Estado + " | " + r.EstadoProximo);
                
                foreach(ContextoEquipo e in r.Equipos)
                {
                    Debug.Log("Equipo: " + e.Equipo.Id + " | " + e.Pregunta.Id + " | " + e.Participacion + " | " + e.Puntaje);
                }
            }

            return historial;
        }

        private void Juego()
        {
            String ruta = Application.dataPath + "/../RcBasedatos.db";

#if UNITY_EDITOR
            ruta = Application.dataPath + "/Resources/Database/RcBasedatos.db";
#endif

            RcBasedatos basedatos = new RcBasedatos(ruta);

            Torneo t = basedatos.ObtenerTorneo(1);
            ManejadorJuego manejador = new ManejadorJuego(t);
            manejador.SolicitarContinuar(this, new ContinuarArgsEvento(0x00, true));
        }

        private void Basedatos()
        {
            String ruta = Application.dataPath + "/Resources/Database/rcbasedatos.db";

            RcBasedatos basedatos = new RcBasedatos(ruta);
            /*
            foreach(Torneo e in basedatos.ObtenerListaTorneos())
            {
                Debug.Log(e.Id);
                Debug.Log(e.Nombre);
                Debug.Log(e.Rondas);
                Debug.Log(e.Ponderacion);
                Debug.Log(e.PuntuacionInicial);
            }
            */

            /*
            foreach(Equipo e in basedatos.ObtenerListaEquipos("WHERE `torneo_id` = 1"))
            {
                Debug.Log(e.Id);
                Debug.Log(e.Nombre);
            }
            */

            /*
            foreach(Categoria e in basedatos.ObtenerListaCategorias())
            {
                Debug.Log(e.Id);
                Debug.Log(e.Nombre);
            }
            */

            /*
            foreach(Pregunta e in basedatos.ObtenerListaPreguntas())
            {
                Debug.Log(e.Id);
                Debug.Log(e.Encabezado);
                Debug.Log(e.TipoPregunta);
                Debug.Log(e.RespuestaA);
                Debug.Log(e.RespuestaB);
                Debug.Log(e.RespuestaC);
                Debug.Log(e.RespuestaD);
                Debug.Log(e.RespuestaCorrecta);
                Debug.Log(e.Aprendizaje);
            }
            */

            foreach (Equipo e in basedatos.ObtenerTorneo(1).Equipos.FindAll(x => x.EsValido()))
            {
                Debug.Log(e.Nombre);
            }
        }

    }
}
