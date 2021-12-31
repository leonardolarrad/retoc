using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Rc.Datos.Basedatos
{
    /// <summary>
    /// Clase base para comandos IDbCommand.
    /// </summary>
    public abstract class ComandoSimple : IDisposable
    {
        protected SqliteCommand _comando;
        protected Dictionary<string, object> _conjunto;        

        /// <summary>
        /// Inicializa la clase ComandoSimple.
        /// </summary>
        /// <param name="comando"></param>
        /// <param name="conexion"></param>
        protected ComandoSimple(string comando, SqliteConnection conexion)
        {
            _comando = new SqliteCommand(comando, conexion);
            _conjunto = new Dictionary<string, object>();
        }

        /// <summary>
        /// Establece el valor de un campo.
        /// </summary>
        /// <param name="campo">Índice del diccionario</param>
        /// <param name="valor">Valor del campo</param>
        public void Establecer(string campo, object valor)
        {
            _conjunto[campo] = valor;
        }

        /// <summary>
        /// Llama al método Dispose() de la interfaz IDispose.
        /// </summary>
        public void Dispose()
        {
            _comando.Dispose();
        }
    } 
}