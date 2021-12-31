using Rc.Aplicacion.Escena.Componentes;
using Rc.Datos.Modelo;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Rc.Aplicacion.Escena
{
    public class EscenaPregunta : EscenaBase, ISolicitarContinuar
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------
        
        // Datos necesarios para vincular la escena.
        private readonly Pregunta _pregunta;

        // Referencias a los objetos de juego existentes en la escena.
        private GameObject _objetoPregunta;
        private GameObject _objetoTiempo;
        private GameObject _objetoAprendizaje;        
        private GameObject _objetoRespuestaCorrecta;
        private GameObject _objetoRespuestaIncorrecta;
        private GameObject _objetoTiempoFuera;

        private Button _botonRespuestaA;
        private Button _botonRespuestaB;
        private Button _botonRespuestaC;
        private Button _botonRespuestaD;
                    
        // --------------------------------------------------
        // Eventos
        // --------------------------------------------------

        /// <summary>
        /// Evento provocado al responderse la pregunta.
        /// </summary>
        public event RespuestaControladorEvento ResponderPregunta;

        /// <summary>
        /// Evento provocado al acabar el tiempo pautado para responder.
        /// </summary>
        public event EventHandler TiempoFuera;
        
        /// <summary>
        /// Evento provocado cuando la escena finaliza sus funciones, 
        /// o cuando la escena no necesita seguir instanciada.
        /// </summary>
        public event ContinuarControladorEvento SolicitarContinuar;

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public EscenaPregunta(Pregunta pregunta)
            : base(Resources.Load(Recursos.ESCENA_PREGUNTA) as GameObject)
        {
            _pregunta = pregunta;
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        private void BotonRespuesta_Presionar(Respuesta respuesta)
        {
            AlResponderPregunta(new RespuestaArgsEvento(respuesta, 0));
        }

        /// <summary>
        /// Método llamado al responder la pregunta.
        /// </summary>
        protected virtual void AlResponderPregunta(RespuestaArgsEvento e)
        {
            if (ResponderPregunta != null)
            {
                ResponderPregunta(this, e);
            }

            if(e.Respuesta == _pregunta.RespuestaCorrecta)
            {
                MostrarMensajeCorrecto();
            }
            else
            {
                MostrarMensajeIncorrecto();
            }

            UnityEngine.Object.Destroy(_instancia.GetComponent<ComponentePregunta>());
            MostrarAprendizaje();
        }

        /// <summary>
        /// Método llamado al estar en tiempo fuera.
        /// </summary>
        public virtual void EnTiempoFuera(EventArgs e)
        {
            if (TiempoFuera != null)
            {
                TiempoFuera(this, e);
            }
            
            UnityEngine.Object.Destroy(_instancia.GetComponent<ComponentePregunta>());
        }

        /// <summary>
        /// Método llamado cuando se solicita continuar.
        /// </summary>
        protected virtual void AlSolicitarContinuar(ContinuarArgsEvento e)
        {            
            if(SolicitarContinuar != null)
            {
                SolicitarContinuar(this, e);
            }
        }

        public void MostrarMensajeCorrecto()
        {
            _instancia.transform.Find("Aprendizaje/Correcto").gameObject.SetActive(true);
            Aplicacion.Fondo.CambiarColor("#12893F", "#1EFF86");

            Color color;
            ColorUtility.TryParseHtmlString("#00B067", out color);
            _instancia.transform.Find("Aprendizaje/Relleno").GetComponent<Image>().color = color;
            _instancia.transform.Find("Aprendizaje/Continuar/CuadroSeleccionado").GetComponent<Image>().color = color;
        }

        public void MostrarMensajeIncorrecto()
        {
            _instancia.transform.Find("Aprendizaje/Incorrecto").gameObject.SetActive(true);
            Aplicacion.Fondo.CambiarColor("#A91818", "#FF1E64");

            Color color;
            ColorUtility.TryParseHtmlString("#F2303C", out color);
            _instancia.transform.Find("Aprendizaje/Relleno").GetComponent<Image>().color = color;
            _instancia.transform.Find("Aprendizaje/Continuar/CuadroSeleccionado").GetComponent<Image>().color = color;
        }

        /// <summary>
        /// Muestra el aprendizaje de la pregunta.
        /// </summary>
        public void MostrarAprendizaje()
        {
            switch(_pregunta.RespuestaCorrecta)
            {
                case Respuesta.A:
                    _instancia.transform.Find("Aprendizaje/A").gameObject.SetActive(true);
                    break;

                case Respuesta.B:
                    _instancia.transform.Find("Aprendizaje/B").gameObject.SetActive(true);
                    break;

                case Respuesta.C:
                    _instancia.transform.Find("Aprendizaje/C").gameObject.SetActive(true);
                    break;

                case Respuesta.D:
                    _instancia.transform.Find("Aprendizaje/D").gameObject.SetActive(true);
                    break;
            }

            _objetoPregunta.SetActive(false);
            _objetoAprendizaje.SetActive(true);
        }

        // --------------------------------------------------
        // EscenaBase
        // --------------------------------------------------

        /// <summary>
        /// Instancia la escena y vincula sus objetos a las referencias de la clase.
        /// </summary>
        public override void Inicializar(String ruta, Boolean mantenerPosicionGlobal = false)
        {
            base.Inicializar(ruta, mantenerPosicionGlobal);
            // Asignar las referencias.   
            _objetoPregunta = _instancia.transform.Find("Pregunta").gameObject;
            _objetoAprendizaje = _instancia.transform.Find("Aprendizaje").gameObject;               
            // Añadir funciones anónimas a los botones de respuesta.
            _botonRespuestaA = _instancia.transform.Find("Pregunta/RespuestaA").GetComponent<Button>();
            _botonRespuestaA.onClick.AddListener(() => { BotonRespuesta_Presionar(Respuesta.A); });

            _botonRespuestaB = _instancia.transform.Find("Pregunta/RespuestaB").GetComponent<Button>();
            _botonRespuestaB.onClick.AddListener(() => { BotonRespuesta_Presionar(Respuesta.B); });

            _botonRespuestaC = _instancia.transform.Find("Pregunta/RespuestaC").GetComponent<Button>();
            _botonRespuestaC.onClick.AddListener(() => { BotonRespuesta_Presionar(Respuesta.C); });

            _botonRespuestaD = _instancia.transform.Find("Pregunta/RespuestaD").GetComponent<Button>();
            _botonRespuestaD.onClick.AddListener(() => { BotonRespuesta_Presionar(Respuesta.D); });            
            // Añadir el evento continuar.
            _objetoAprendizaje.transform.Find("Continuar").GetComponent<Button>().onClick.AddListener(
                () => AlSolicitarContinuar(ContinuarArgsEvento.Vacio)
            );

            // Cambiar color

            Color color;
            ColorUtility.TryParseHtmlString(_pregunta.Categoria.Color2, out color);

            _instancia.transform.Find("Pregunta/Tiempo").GetComponent<Image>().color = color;

            _instancia.transform.Find("Pregunta/RespuestaA/Aura2").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaA/Aura1").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaA/Relleno").GetComponent<Image>().color = color;

            _instancia.transform.Find("Pregunta/RespuestaB/Aura2").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaB/Aura1").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaB/Relleno").GetComponent<Image>().color = color;

            _instancia.transform.Find("Pregunta/RespuestaC/Aura2").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaC/Aura1").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaC/Relleno").GetComponent<Image>().color = color;

            _instancia.transform.Find("Pregunta/RespuestaD/Aura2").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaD/Aura1").GetComponent<Image>().color = color;
            _instancia.transform.Find("Pregunta/RespuestaD/Relleno").GetComponent<Image>().color = color;
            
            VincularDatos();

            // Iniciar el componente de la escena.
            _instancia.AddComponent<ComponentePregunta>();
            _instancia.GetComponent<ComponentePregunta>().EstablecerEscena(this);
            _instancia.GetComponent<ComponentePregunta>().IniciarTiempo(30f, 10f);
            // Ocultar objetos que no se necesiten en la primera parte de la escena.
            _objetoAprendizaje.SetActive(false);
        }
        
        /// <summary>
        /// Vincula los datos que se hayan pasado a la escena.
        /// </summary>
        public override void VincularDatos()
        {
            // Vincular el encabezado de la pregunta.
            _objetoPregunta.transform.Find("Encabezado").GetComponent<Text>().text = _pregunta.Encabezado;

            // Vincular las respuestas a los botones.    
            _botonRespuestaA.GetComponentInChildren<Text>().text = _pregunta.RespuestaA;
            _botonRespuestaB.GetComponentInChildren<Text>().text = _pregunta.RespuestaB;
            _botonRespuestaC.GetComponentInChildren<Text>().text = _pregunta.RespuestaC;
            _botonRespuestaD.GetComponentInChildren<Text>().text = _pregunta.RespuestaD;
            
            // Vincular el aprendizaje.
            _objetoAprendizaje.transform.Find("Texto").GetComponent<Text>().text = _pregunta.Aprendizaje;
        }
    }
}