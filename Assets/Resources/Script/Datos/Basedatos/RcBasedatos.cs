using Mono.Data.Sqlite;
using Rc.Datos.Modelo;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Datos.Basedatos
{
    /// <summary>
    /// Base de datos del Reto al Conocimiento. 
    /// </summary>
    public class RcBasedatos
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly String _ruta;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------

        /// <summary>
        /// Obtiene la ruta absoluta o relativa de la base de datos.
        /// </summary>
        public String Ruta
        {
            get { return _ruta; }
        }

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public RcBasedatos(String ruta)
        {
            if (!File.Exists(ruta))
                throw new FileNotFoundException(ruta);

            _ruta = ruta;
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        /// <summary>
        /// Obtiene la conexión con la base de datos.
        /// </summary>
        public SqliteConnection ObtenerConexion()
        {
            SqliteConnection conexion = new SqliteConnection("URI=file:" + _ruta);
            conexion.Open();

            // Activa las llaves ajenas. 
            using (SqliteCommand comando = new SqliteCommand("PRAGMA foreign_keys = 1", conexion))
            {
                comando.ExecuteNonQuery();
            }

            return conexion;
        }

        /// <summary>
        /// Devuelve verdadero si se puede establecer conexión con la base de datos,
        /// devuelve falso en caso contrario.
        /// </summary>
        public Boolean ProbarConexion()
        {
            SqliteConnection conexion = ObtenerConexion();

            if(conexion != null)
            {
                conexion.Close();
                return true;
            }

            return false;
        }

        #region Torneo

        /// <summary>
        /// Añade un torneo a la base de datos.
        /// </summary>
        public void AñadirTorneo(Torneo torneo)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoInsertar comando = new ComandoInsertar("INSERT INTO `TORNEO` {0}", conexion))
            {
                comando.Establecer("id", null);
                comando.Establecer("nombre", torneo.Nombre);
                comando.Establecer("imagen", torneo.Imagen);
                comando.Establecer("rondas", torneo.Rondas);
                comando.Establecer("ponderacion", torneo.Ponderacion);
                comando.Establecer("puntuacion_inicial", torneo.PuntuacionInicial);
                comando.Establecer("rondas_finales", torneo.RondasFinales);

                comando.Ejecutar();
                torneo.Id = comando.UltimoIdentificador;
            }
        }

        /// <summary>
        /// Actualiza un torneo de la base de datos.
        /// </summary>
        public Boolean ActualizarTorneo(Torneo torneo)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoActualizar comando = new ComandoActualizar("UPDATE `TORNEO` SET {0} WHERE `id` = @id", conexion))
            {
                comando.AñadirParametro("@id", torneo.Id);

                comando.Establecer("nombre", torneo.Nombre);
                comando.Establecer("imagen", torneo.Imagen);
                comando.Establecer("rondas", torneo.Rondas);
                comando.Establecer("ponderacion", torneo.Ponderacion);
                comando.Establecer("puntuacion_inicial", torneo.PuntuacionInicial);
                comando.Establecer("rondas_finales", torneo.RondasFinales);

                return comando.Ejecutar() > 0;
            }
        }

        /// <summary>
        /// Obtiene un torneo de la base de datos.
        /// </summary>
        public Torneo ObtenerTorneo(Int64 id)
        {
            Torneo torneo = null;

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `TORNEO` WHERE id = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            torneo = new Torneo()
                            {
                                Id = id,
                                Nombre = lector[1] as String,
                                Imagen = lector[2] as String,
                                Rondas = !lector.IsDBNull(3) ? lector.GetInt32(3) : 0,
                                Ponderacion = !lector.IsDBNull(4) ? lector.GetInt32(4) : 0,
                                PuntuacionInicial = !lector.IsDBNull(5) ? lector.GetInt32(5) : 0,
                                Equipos = ObtenerListaEquipos("WHERE `torneo_id` = " + id.ToString()),
                                Categorias = ObtenerListaCategorias("WHERE `torneo_id` = " + id.ToString()),
                                Preguntas = ObtenerListaPreguntas("WHERE `torneo_id` = " + id.ToString())         ,
                                RondasFinales = !lector.IsDBNull(6) ? lector.GetInt32(6) : 0
                            };
                        }
                    }
                }
            }
            return torneo;
        }

        /// <summary>
        /// Obtiene una lista de torneos de la base de datos.
        /// </summary>
        public List<Torneo> ObtenerListaTorneos(String condicion = "")
        {
            List<Torneo> torneos = new List<Torneo>();

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `TORNEO`" + condicion, conexion))
                {                    
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Torneo torneo = new Torneo()
                            {
                                Id = lector.GetInt64(0),
                                Nombre = lector[1] as String,
                                Imagen = lector[2] as String,
                                Rondas = !lector.IsDBNull(3) ? lector.GetInt32(3) : 0,
                                Ponderacion = !lector.IsDBNull(4) ? lector.GetInt32(4) : 0,
                                PuntuacionInicial = !lector.IsDBNull(5) ? lector.GetInt32(5) : 0,
                                RondasFinales = !lector.IsDBNull(6) ? lector.GetInt32(6) : 0
                            };

                            torneo.Equipos = ObtenerListaEquipos("WHERE `torneo_id` = " + torneo.Id.ToString());
                            torneo.Categorias = ObtenerListaCategorias("WHERE `torneo_id` = " + torneo.Id.ToString());
                            torneo.Preguntas = ObtenerListaPreguntas("WHERE `torneo_id` = " + torneo.Id.ToString());

                            torneos.Add(torneo);
                        }
                    }
                }
            }

            return torneos;
        }

        /// <summary>
        /// Elimina un torneo de la base de datos.
        /// </summary>
        public Boolean RemoverTorneo(Torneo torneo)
        {
            return RemoverTorneo(torneo.Id);
        }

        /// <summary>
        /// Elimina un torneo de la base de datos.
        /// </summary>
        public Boolean RemoverTorneo(Int64 id)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (SqliteCommand comando = new SqliteCommand("DELETE FROM `TORNEO` WHERE `id` = @id", conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                return comando.ExecuteNonQuery() > 0;
            }
        }

        #endregion

        #region Equipo

        /// <summary>
        /// Añade un equipo a la base de datos.
        /// </summary>
        public void AñadirEquipo(Equipo equipo, Int64 torneo)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoInsertar comando = new ComandoInsertar("INSERT INTO `EQUIPO` {0}", conexion))
            {
                comando.Establecer("id", null);
                comando.Establecer("nombre", equipo.Nombre);
                comando.Establecer("imagen", equipo.Imagen);

                comando.Establecer("torneo_id", torneo);

                comando.Ejecutar();
                equipo.Id = comando.UltimoIdentificador;
            }
        }

        /// <summary>
        /// Actualiza un equipo de la base de datos.
        /// </summary>
        public Boolean ActualizarEquipo(Equipo equipo)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoActualizar comando = new ComandoActualizar("UPDATE `EQUIPO` SET {0} WHERE `id` = @id", conexion))
            {
                comando.AñadirParametro("@id", equipo.Id);

                comando.Establecer("nombre", equipo.Nombre);
                comando.Establecer("imagen", equipo.Imagen);

                return comando.Ejecutar() > 0;
            }
        }

        /// <summary>
        /// Obtiene un equipo de la base de datos.
        /// </summary>
        public Equipo ObtenerEquipo(Int64 id)
        {
            Equipo equipo = null;

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `EQUIPO` WHERE id = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            equipo = new Equipo()
                            {
                                Id = id,
                                Nombre = lector[1] as String,
                                Imagen = lector[2] as String
                            };
                        }
                    }
                }
            }

            return equipo;
        }

        /// <summary>
        /// Obtiene una lista de equipos de la base de datos.
        /// </summary>
        public List<Equipo> ObtenerListaEquipos(String condicion = "")
        {
            List<Equipo> equipos = new List<Equipo>();

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `EQUIPO`" + condicion, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Equipo equipo = new Equipo()
                            {
                                Id = lector.GetInt64(0),
                                Nombre = lector[1] as String,
                                Imagen = lector[2] as String
                            };

                            equipos.Add(equipo);
                        }
                    }
                }
            }

            return equipos;
        }

        /// <summary>
        /// Elimina un equipo de la base de datos.
        /// </summary>
        public Boolean RemoverEquipo(Equipo equipo)
        {
            return RemoverEquipo(equipo.Id);
        }

        /// <summary>
        /// Elimina un equipo de la base de datos.
        /// </summary>
        public Boolean RemoverEquipo(Int64 id)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (SqliteCommand comando = new SqliteCommand("DELETE FROM `EQUIPO` WHERE `id` = @id", conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                return comando.ExecuteNonQuery() > 0;
            }
        }

        #endregion

        #region Categoria

        /// <summary>
        /// Añade una categoría a la base de datos.
        /// </summary>
        public void AñadirCategoria(Categoria categoria, Int64 torneo)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoInsertar comando = new ComandoInsertar("INSERT INTO `CATEGORIA` {0}", conexion))
            {
                comando.Establecer("id", null);
                comando.Establecer("nombre", categoria.Nombre);
                comando.Establecer("imagen", categoria.Imagen);
                comando.Establecer("color1", categoria.Color1);
                comando.Establecer("color2", categoria.Color2);

                comando.Establecer("torneo_id", torneo);

                comando.Ejecutar();
                categoria.Id = comando.UltimoIdentificador;
            }
        }

        /// <summary>
        /// Actualiza una categoría de la base de datos.
        /// </summary>
        public Boolean ActualizarCategoria(Categoria categoria)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoActualizar comando = new ComandoActualizar("UPDATE `CATEGORIA` SET {0} WHERE `id` = @id", conexion))
            {
                comando.AñadirParametro("@id", categoria.Id);

                comando.Establecer("nombre", categoria.Nombre);
                comando.Establecer("imagen", categoria.Imagen);
                comando.Establecer("color1", categoria.Color1);
                comando.Establecer("color2", categoria.Color2);

                return comando.Ejecutar() > 0;
            }
        }

        /// <summary>
        /// Obtiene una categoría de la base de datos.
        /// </summary>
        public Categoria ObtenerCategoria(Int64 id)
        {
            Categoria categoria = null;

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `CATEGORIA` WHERE id = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            categoria = new Categoria()
                            {
                                Id = id,
                                Nombre = lector[1] as String,
                                Imagen = lector[2] as String,
                                Color1 = lector[5] as String,
                                Color2 = lector[6] as String
                            };
                        }
                    }
                }
            }

            return categoria;
        }

        /// <summary>
        /// Obtiene una lista de categorías de la base de datos.
        /// </summary>
        public List<Categoria> ObtenerListaCategorias(String condicion = "")
        {
            List<Categoria> categorias = new List<Categoria>();

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `CATEGORIA`" + condicion, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Categoria categoria = new Categoria()
                            {
                                Id = lector.GetInt64(0),
                                Nombre = lector[1] as String,
                                Imagen = lector[2] as String,
                                Color1 = lector[5] as String,
                                Color2 = lector[6] as String
                            };

                            categorias.Add(categoria);
                        }
                    }
                }
            }

            return categorias;
        }

        /// <summary>
        /// Elimina una categoría de la base de datos.
        /// </summary>
        public Boolean RemoverCategoria(Categoria categoria)
        {
            return RemoverCategoria(categoria.Id);
        }

        /// <summary>
        /// Elimina una categoría de la base de datos.
        /// </summary>
        public Boolean RemoverCategoria(Int64 id)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (SqliteCommand comando = new SqliteCommand("DELETE FROM `CATEGORIA` WHERE `id` = @id", conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                return comando.ExecuteNonQuery() > 0;
            }
        }

        #endregion

        #region Pregunta

        /// <summary>
        /// Añade una pregunta a la base de datos.
        /// </summary>
        public void AñadirPregunta(Pregunta pregunta, Int64 torneo)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoInsertar comando = new ComandoInsertar("INSERT INTO `PREGUNTA` {0}", conexion))
            {
                comando.Establecer("id", null);
                comando.Establecer("encabezado", pregunta.Encabezado);
                comando.Establecer("tipo_pregunta", pregunta.TipoPregunta);
                comando.Establecer("multimedia", pregunta.Multimedia);
                comando.Establecer("respuesta_a", pregunta.RespuestaA);
                comando.Establecer("respuesta_b", pregunta.RespuestaB);
                comando.Establecer("respuesta_c", pregunta.RespuestaC);
                comando.Establecer("respuesta_d", pregunta.RespuestaD);
                comando.Establecer("respuesta_correcta", pregunta.RespuestaCorrecta);
                comando.Establecer("aprendizaje", pregunta.Aprendizaje);
                comando.Establecer("categoria_id", pregunta.Categoria.Id);

                comando.Establecer("torneo_id", torneo);
                comando.Establecer("categoria_id", pregunta.Categoria.Id);

                comando.Ejecutar();
                pregunta.Id = comando.UltimoIdentificador;                
            }
        }

        /// <summary>
        /// Actualiza una pregunta de la base de datos.
        /// </summary>
        public Boolean ActualizarPregunta(Pregunta pregunta)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (ComandoActualizar comando = new ComandoActualizar("UPDATE `PREGUNTA` SET {0} WHERE `id` = @id", conexion))
            {
                comando.AñadirParametro("@id", pregunta.Id);
                
                comando.Establecer("encabezado", pregunta.Encabezado);
                comando.Establecer("tipo_pregunta", pregunta.TipoPregunta);
                comando.Establecer("multimedia", pregunta.Multimedia);
                comando.Establecer("respuesta_a", pregunta.RespuestaA);
                comando.Establecer("respuesta_b", pregunta.RespuestaB);
                comando.Establecer("respuesta_c", pregunta.RespuestaC);
                comando.Establecer("respuesta_d", pregunta.RespuestaD);
                comando.Establecer("respuesta_correcta", pregunta.RespuestaCorrecta);
                comando.Establecer("aprendizaje", pregunta.Aprendizaje);
                comando.Establecer("categoria_id", pregunta.Categoria.Id);

                return comando.Ejecutar() > 0;
            }
        }

        /// <summary>
        /// Obtiene una pregunta de la base de datos.
        /// </summary>
        public Pregunta ObtenerPregunta(Int64 id)
        {
            Pregunta pregunta = null;

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `PREGUNTA` WHERE id = @id", conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            pregunta = new Pregunta()
                            {
                                Id = id,
                                Encabezado = lector[1] as String,
                                TipoPregunta = (TipoPregunta)(!lector.IsDBNull(2) ? lector.GetInt32(2) : 0),
                                Multimedia = lector[3] as String,
                                RespuestaA = lector[4] as String,
                                RespuestaB = lector[5] as String,
                                RespuestaC = lector[6] as String,
                                RespuestaD = lector[7] as String,
                                RespuestaCorrecta = (Respuesta)(!lector.IsDBNull(2) ? lector.GetInt32(8) : 0),
                                Aprendizaje = lector[9] as String,
                                Categoria = ObtenerCategoria((!lector.IsDBNull(10) ? lector.GetInt32(10) : -1))
                            };
                        }
                    }
                }
            }

            return pregunta;
        }

        /// <summary>
        /// Obtiene una lista de preguntas de la base de datos.
        /// </summary>
        public List<Pregunta> ObtenerListaPreguntas(String condicion = "")
        {
            List<Pregunta> preguntas = new List<Pregunta>();

            using (SqliteConnection conexion = ObtenerConexion())
            {
                using (SqliteCommand comando = new SqliteCommand("SELECT * FROM `PREGUNTA`" + condicion, conexion))
                {
                    using (SqliteDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Pregunta pregunta = new Pregunta()
                            {
                                Id = lector.GetInt64(0),
                                Encabezado = lector[1] as String,
                                TipoPregunta = (TipoPregunta)(!lector.IsDBNull(2) ? lector.GetInt32(2) : 0),
                                Multimedia = lector[3] as String,
                                RespuestaA = lector[4] as String,
                                RespuestaB = lector[5] as String,
                                RespuestaC = lector[6] as String,
                                RespuestaD = lector[7] as String,
                                RespuestaCorrecta = (Respuesta)(!lector.IsDBNull(8) ? lector.GetInt32(8) : 0),
                                Aprendizaje = lector[9] as String,
                                Categoria = ObtenerCategoria((!lector.IsDBNull(10) ? lector.GetInt32(10) : 0))
                            };

                            preguntas.Add(pregunta);
                        }
                    }
                }
            }

            return preguntas;
        }
        
        /// <summary>
        /// Elimina una pregunta de la base de datos.
        /// </summary>
        public Boolean RemoverPregunta(Pregunta pregunta)
        {
            return RemoverPregunta(pregunta.Id);
        }

        /// <summary>
        /// Elimina una pregunta de la base de datos.
        /// </summary>
        public Boolean RemoverPregunta(Int64 id)
        {
            using (SqliteConnection conexion = ObtenerConexion())
            using (SqliteCommand comando = new SqliteCommand("DELETE FROM `PREGUNTA` WHERE `id` = @id", conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                return comando.ExecuteNonQuery() > 0;
            }
        }

        #endregion
    }
}