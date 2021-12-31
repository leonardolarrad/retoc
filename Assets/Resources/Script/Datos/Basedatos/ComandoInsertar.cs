using Mono.Data.Sqlite;
using System.Text;

namespace Rc.Datos.Basedatos
{

    public class ComandoInsertar : ComandoSimple
    {
        /// <summary>
        /// Obtiene el ultimo identificador introducido en la base de datos.
        /// </summary>
        public long UltimoIdentificador
        {
            get
            {
                _comando.CommandText = "SELECT last_insert_rowid()";
                return (long)_comando.ExecuteScalar();
            }

        }

        /// <summary>
        /// Inicializa la clase ComandoInsertar.
        /// </summary>
        /// <param name="comando"></param>
        /// <param name="conexion"></param>
        public ComandoInsertar(string comando, SqliteConnection conexion) : base(comando, conexion)
        {
        }

        /// <summary>
        /// Llama al método SqliteCommand.ExecuteNonQuery().
        /// </summary>
        public int Ejecutar()
        {
            StringBuilder campos = new StringBuilder();
            StringBuilder valores = new StringBuilder();

            foreach (var parametro in _conjunto.Keys)
            {
                campos.AppendFormat("`{0}`, ", parametro);
                valores.AppendFormat("@{0}, ", parametro);
            }

            _comando.CommandText = string.Format(_comando.CommandText, "(" + campos.ToString().Trim(' ', ',') + ") VALUES (" + valores.ToString().Trim(' ', ',') + ")");

            foreach (var parametro in _conjunto)
            {
                _comando.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }

            return _comando.ExecuteNonQuery();
        }
    }
}