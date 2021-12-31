using Rc.Herramientas;
using Rc.Aplicacion.Editor;
using Rc.Aplicacion.Escena;
using Rc.Aplicacion.Juego;
using Rc.Datos.Modelo;
using Rc.Datos.Basedatos;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rc.Aplicacion
{
    public class Aplicacion
    {
        public static OpcionesAplicacion Opciones = new OpcionesAplicacion();

        private static ManejadorJuego juego;
        private static ManejadorEditor editor;
        private static MenuPrincipal menu;

        public static FondoPrincipal Fondo;

        public static void Ejecutar()
        {
            Fondo = new FondoPrincipal();
            Fondo.Inicializar(Recursos.LIENZO_PREDETERMINADO);

            EjecutarMenu();            
        }

        public static void EjecutarMenu()
        {
            LimpiarInterfaz();         

            menu = new MenuPrincipal();
            menu.Inicializar(Recursos.LIENZO_PREDETERMINADO);
            Fondo.CambiarColor("#1CD0FF", "#1EFFA2");
        }

        public static void EjecutarJuego()
        {
            menu.Destruir();
            String ruta = Application.dataPath + "/../RcBasedatos.db";

#if UNITY_EDITOR
            ruta = Application.dataPath + "/Resources/Database/RcBasedatos.db";
#endif
            RcBasedatos basedatos = new RcBasedatos(ruta);
            EscenaSelecTorneo selecTorneo = new EscenaSelecTorneo(basedatos.ObtenerListaTorneos().Where(x => x.EsValido()));
            selecTorneo.Inicializar(Recursos.LIENZO_PREDETERMINADO);
        }

        public static void EjecutarTorneo(Int64 id)
        {
            LimpiarInterfaz();
            String ruta = Application.dataPath + "/../RcBasedatos.db";
#if UNITY_EDITOR
            ruta = Application.dataPath + "/Resources/Database/RcBasedatos.db";
#endif
            RcBasedatos basedatos = new RcBasedatos(ruta);

            Torneo torneo = basedatos.ObtenerTorneo(id);
            ManejadorJuego manejador = new ManejadorJuego(torneo);
            manejador.SolicitarContinuar(null, new ContinuarArgsEvento(0x00, true));
        }

        public static void ReanudarJuego(String ruta)
        {
            LimpiarInterfaz();

            HistorialJuego historial = HistorialJuego.Cargar(Application.dataPath + ruta);
            ManejadorJuego manejador = new ManejadorJuego(historial);
            manejador.Reanudar();
        }

        public static void EjecutarEditor()
        {
            menu.Destruir();
            String ruta = Application.dataPath + "/../RcBasedatos.db";

#if UNITY_EDITOR
            ruta = Application.dataPath + "/Resources/Database/RcBasedatos.db";
#endif
            RcBasedatos basedatos = new RcBasedatos(ruta);
            ManejadorEditor editor = new ManejadorEditor(basedatos);
            Fondo.CambiarColor("#1CD0FF", "#1EFFA2");
        }
        

        public static void Salir()
        {
            Application.Quit();
        }

        private static void LimpiarInterfaz()
        {
            foreach(Transform hijo in  GameObject.Find(Recursos.LIENZO_PREDETERMINADO).transform)
            {
                if(hijo != Fondo.Instancia.transform)
                {
                    GameObject.Destroy(hijo.gameObject);
                }
            }
        }
    }
}
