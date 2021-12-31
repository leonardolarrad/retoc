using Rc.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rc.Aplicacion.Juego
{
    public class Puntuacion
    {
        // --------------------------------------------------
        // Campos
        // --------------------------------------------------

        private readonly Dictionary<Equipo, Int32> coleccion;

        // --------------------------------------------------
        // Propiedades
        // --------------------------------------------------
        
        public Dictionary<Equipo, Int32> Coleccion
        {
            get { return coleccion; }
        }

        public IEnumerable<Equipo> Equipos
        {
            get { return coleccion.Keys; }
        }

        // --------------------------------------------------
        // Constructores
        // --------------------------------------------------

        public Puntuacion(IEnumerable<Equipo> equipos)
        {
            coleccion = equipos.ToDictionary(x => x, x => 0);
        }

        public Puntuacion(IEnumerable<KeyValuePair<Equipo, Int32>> coleccion)
        {
            this.coleccion = coleccion.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        // --------------------------------------------------
        // Métodos
        // --------------------------------------------------

        public Boolean HayEmpate()
        {
            return HayEmpate(ObtenerPrimerLugar());
        }

        public Boolean HayEmpate(Equipo equipoEmpatado, Int32 minimo = 2, Equipo excluido = null)
        {
            Int32 numeroEmpatados = 0;

            foreach (KeyValuePair<Equipo, Int32> equipo in coleccion)
            {
                if (equipo.Value == coleccion[equipoEmpatado] && (excluido != null ? equipo.Key != excluido : true))
                    numeroEmpatados++;
            }

            return numeroEmpatados >= minimo;
        }

        public IEnumerable<Equipo> ObtenerEquiposEmpatados()
        {
            return ObtenerEquiposEmpatados(ObtenerPrimerLugar());
        }

        public IEnumerable<Equipo> ObtenerEquiposEmpatados(Equipo equipoEmpatado, Int32 minimo = 2, Equipo excluido = null)
        {            
            if (!HayEmpate(equipoEmpatado, minimo))
                yield break;
            
            foreach (KeyValuePair<Equipo, Int32> equipo in coleccion)
            {
                if (equipo.Value == coleccion[equipoEmpatado]  && (excluido != null ? equipo.Key != excluido : true))
                    yield return equipo.Key;
            }
        }

        public Equipo ObtenerPrimerLugar()
        {
            if (!(coleccion.Count() > 0))
                throw new IndexOutOfRangeException("La colección no posee el indice del equipo requerido.");

            return coleccion.OrderByDescending(x => x.Value).ToArray()[0].Key;
        }

        public Equipo ObtenerSegundoLugar()
        {
            if (!(coleccion.Count() > 1))
                throw new IndexOutOfRangeException("La colección no posee el indice del equipo requerido.");

            return coleccion.OrderByDescending(x => x.Value).ToArray()[1].Key;
        }

    }
}
