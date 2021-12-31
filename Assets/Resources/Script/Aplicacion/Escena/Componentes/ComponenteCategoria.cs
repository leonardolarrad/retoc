using UnityEngine;
using UnityEngine.UI;
using Rc.Datos.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rc.Aplicacion.Escena.Componentes
{
    public class ComponenteCategoria : ComponenteEscena<EscenaCategoria>
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private IEnumerable<Categoria> _categorias;
        private IEnumerator<Categoria> _enumerador;
        private Categoria _seleccion;
        private EscenaFondo _fondo;
        private Animator _controlador;
        private Text _texto;

        private Image[] _imagen;
        private String[] _nombreCategoria;

        private Categoria[] _categoriaTemporal;

        private Int32 _indice;
        private const Int32 _vueltas = 30;
        private const Single _velocidadMaxima = 5f;
        private const Single _velocidadMinima = 0.4f;
        private Single _velocidad;

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public void SeleccionarCategoria(IEnumerable<Categoria> categorias, Categoria seleccion, EscenaFondo fondo)
        {
            _categorias = categorias;
            _seleccion = seleccion;
            _fondo = fondo;
            _controlador = GetComponent<Animator>();
            _indice = 0;

            _nombreCategoria = new String[3];
            _categoriaTemporal = new Categoria[3];

            _imagen = new Image[4]
            {
                this.gameObject.transform.Find("Categoria0").GetComponent<Image>(),
                this.gameObject.transform.Find("Categoria1").GetComponent<Image>(), 
                this.gameObject.transform.Find("Categoria2").GetComponent<Image>(),
                this.gameObject.transform.Find("Categoria3").GetComponent<Image>()
            };

            _texto = this.gameObject.transform.Find("Texto").GetComponent<Text>();          
            AnimationClip animacion = _controlador.runtimeAnimatorController.animationClips[0];

            if (animacion.events.Length == 0)
            {
                AnimationEvent evento1 = new AnimationEvent
                {
                    time = 0.25f,
                    functionName = "IntervaloCategoria"
                };

                AnimationEvent evento2 = new AnimationEvent
                {
                    time = 0.5f,
                    functionName = "SiguienteCategoria"
                };

                animacion.AddEvent(evento1);
                animacion.AddEvent(evento2);               
            }

            DesordenarCategorias();
            SiguienteCategoria();
            _controlador.Play(Recursos.ANIMACION_SELECCIONAR_CATEGORIA);
        }

        /// <summary>
        /// Desordena la lista de categorias e introduce la categoría seleccionada acomodando su posición.
        /// </summary>
        private Categoria[] DesordenarCategorias()
        {
            System.Random aleatorio = new System.Random();
            _enumerador = _categorias.OrderBy(i => aleatorio.Next()).ToList().GetEnumerator();

            if (_categorias.Count() < 4)
                return null;                    
                
            Categoria[] resultado = new Categoria[_vueltas + 1];

            for(int i = 0; i < resultado.Length; i++)
            {
                if (i == (resultado.Length - 2))
                {
                    resultado[i] = _seleccion;
                }
                else if((i == (resultado.Length - 1)) || (i == (resultado.Length - 3)) || (i == (resultado.Length - 4)))
                {
                    do
                    {
                        resultado[i] = ObtenerProximaCategoria();
                    }
                    while (resultado[i].Equals(_seleccion));
                }
                else
                {
                    resultado[i] = ObtenerProximaCategoria();
                }
            }
            _enumerador = ((IEnumerable<Categoria>)resultado).GetEnumerator();
            return resultado;
        }

        /// <summary>
        /// Obtiene la próxima categoría del enumerador.
        /// </summary>
        private Categoria ObtenerProximaCategoria()
        {
            if (!_enumerador.MoveNext())
            {
                _enumerador.Reset();
                _enumerador.MoveNext();
            }

            return _enumerador.Current;
        }

        /// <summary>
        /// Pasa a la siguiente categoría. Método llamado cuando la animación finaliza.
        /// </summary>
        private void SiguienteCategoria()
        {
            if (_indice >= _vueltas)
            {
                StartCoroutine(Continuar());
                return;
            }
            
            _indice++;
           
            switch (_indice)
            {
                case 1:

                    Categoria primeraCategoria = ObtenerProximaCategoria();
                    _imagen[0].sprite = Resources.Load<Sprite>(primeraCategoria.Imagen);
                    _nombreCategoria[0] = primeraCategoria.Nombre;
                    _categoriaTemporal[0] = primeraCategoria;

                    _imagen[2].gameObject.SetActive(false);
                    _imagen[3].gameObject.SetActive(false);
                    break;

                case 2:
                    _imagen[2].gameObject.SetActive(true);
                    break;

                case 3:
                    _imagen[3].gameObject.SetActive(true);
                    break;
            }

            Categoria proximaCategoria = ObtenerProximaCategoria();

            _imagen[3].sprite = _imagen[2].sprite;
            _imagen[2].sprite = _imagen[1].sprite;
            _imagen[1].sprite = _imagen[0].sprite;
            _imagen[0].sprite = Resources.Load<Sprite>(proximaCategoria.Imagen);

            _categoriaTemporal[1] = _categoriaTemporal[0];
            _categoriaTemporal[0] = proximaCategoria;

            _velocidad = ((((_vueltas - _indice) * 100 / (Single)_vueltas) * (_velocidadMaxima - _velocidadMinima)) / 100f) + _velocidadMinima;
            _controlador.speed = _velocidad;
        }
        
        /// <summary>
        /// Método llamado en la mitad de la animación principal.
        /// </summary>
        private void IntervaloCategoria()
        {
            if (_categoriaTemporal[1] != null)
            {
                //_fondo.CambiarTono(_categoriaTemporal[1].Color);
                Aplicacion.Fondo.CambiarColor(_categoriaTemporal[1].Color1, _categoriaTemporal[1].Color2);
                _texto.text = _categoriaTemporal[1].Nombre.ToUpper();
            }
        }

        /// <summary>
        /// Detiene la escena unos segundos y luego continua con el contexto del juego.
        /// </summary>
        private IEnumerator Continuar()
        {
            _controlador.enabled = false;            
            yield return new WaitForSeconds(1f);
            _escena.AlSolicitarContinuar(ContinuarArgsEvento.Vacio);
        }
    }
}