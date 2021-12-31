using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rc.Herramientas
{
    public class Numero
    {
        /// <summary>
        /// Enumeración de los números cardinales
        /// </summary>
        private enum NumeroOrdinal
        {
            Primero = 1,
            Segundo = 2,
            Tercero = 3,
            Cuarto = 4,
            Quinto = 5,
            Sexto = 6,
            Séptimo = 7,
            Octavo = 8,
            Noveno = 9,
            Décimo = 10,
            Vigésimo = 20,
            Trigésimo = 30,
            Cudragésimo = 40,
            Quincuágesimo = 50,
            Sexagésimo = 60,
            Septuagésimo = 70,
            Octagésimo = 80,
            Nonagésimo = 90,
            Centésimo = 100
        }

        /// <summary>
        /// Obtiene un número cardinal en forma de cadena.
        /// </summary>
        public static String ObtenerOrdinal(Int32 numero, Boolean masculino)
        {
            String ordinal = String.Empty;
            Char genero = masculino ? 'o' : 'a';

            if(numero < 1)
            {
                return String.Empty;
            }

            if(numero >= 1 && numero <= 10)
            {
                ordinal = Enum.GetName(typeof(NumeroOrdinal), numero);
                ordinal = ordinal.Remove(ordinal.Length - 1, 1) + genero;
                return ordinal;
            }

            if(numero > 10 && numero <= 99)
            {
                ordinal = Enum.GetName(typeof(NumeroOrdinal), ((int)(numero / 10)) * 10);
                ordinal = ordinal.Remove(ordinal.Length - 1, 1) + genero;

                String ordinalPrimario = Enum.GetName(typeof(NumeroOrdinal), numero % 10);
                ordinalPrimario = ordinalPrimario.Remove(ordinalPrimario.Length - 1, 1) + genero;

                ordinal = ordinal + " " + ordinalPrimario;
                return ordinal;
            }

            if(numero == 100)
            {
                ordinal = "Centésimo";
                ordinal = ordinal.Remove(ordinal.Length - 1, 1) + genero;
                return ordinal;
            }
    
            return Convert.ToString(numero);
        }
    }
}
