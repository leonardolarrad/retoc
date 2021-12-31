using Mono.Data.Sqlite;
using System.Text;

namespace Rc.Datos.Basedatos
{
    public class ComandoActualizar : ComandoSimple
    {
        /// <summary>
        /// Inicializa la clase ComandoActualizar.
        /// </summary>
        /// <param name="comando"></param>
        /// <param name="conexion"></param>
        public ComandoActualizar(string comando, SqliteConnection conexion) : base(comando, conexion)
        {
        }

        /// <summary>
        /// Añade un parametro al comando.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="valor"></param>
        public void AñadirParametro(string nombre, object valor)
        {
            _comando.Parameters.AddWithValue(nombre, valor);
        }

        /// <summary>
        /// Llama al método SqliteCommand.ExecuteNonQuery()
        /// </summary>
        public int Ejecutar()
        {
            StringBuilder constructorCadenas = new StringBuilder();

            foreach (var parametro in _conjunto.Keys)
            {
                constructorCadenas.AppendFormat("`{0}` = @{0}, ", parametro);
            }

            _comando.CommandText = string.Format(_comando.CommandText, constructorCadenas.ToString().Trim(' ', ','));

            foreach (var parametro in _conjunto)
            {
                _comando.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }

            return _comando.ExecuteNonQuery();
        }
    }

}