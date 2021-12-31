using Rc.Datos.Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Rc.Aplicacion.Juego
{
    [DataContract(Name = "Historial", Namespace = "")]
    public class HistorialJuego
    {
        [DataMember()] public Torneo Torneo;
        [DataMember()] public DateTime Fecha;
        [DataMember()] public String Basedatos;

        [DataMember()] public EstadoJuego Estado;
        [DataMember()] public EstadoJuego EstadoProximo;

        [DataMember()] public List<PreguntaPorEquipo> PreguntasPrimeraFase;
        [DataMember()] public List<Pregunta> PreguntasUltimaFase;
        [DataMember()] public List<Pregunta> PreguntasDesempate;

        [DataMember()] public List<Fase> Fases;
        [DataMember()] public List<Ronda> Rondas;

        public HistorialJuego()
        {
            Fases = new List<Fase>();
            Rondas = new List<Ronda>();
        }

        public static void Guardar(HistorialJuego historial, String directorio)
        {
            DataContractSerializer serializador = new DataContractSerializer(typeof(HistorialJuego));

            using (XmlWriter escritor = XmlWriter.Create(Path.Combine(directorio, historial.Torneo.Nombre + ".historial"), new XmlWriterSettings() { Indent = true }))
            {
                serializador.WriteObject(escritor, historial);
            }
        }

        public static HistorialJuego Cargar(String directorio)
        {
            DataContractSerializer serializador = new DataContractSerializer(typeof(HistorialJuego));

            using (XmlReader lector = XmlReader.Create(directorio))
            {
                return (HistorialJuego)serializador.ReadObject(lector);
            }            
        }        
    }

    [DataContract(Name = "PreguntaPorEquipo", Namespace ="")]
    public class PreguntaPorEquipo
    {
        [DataMember()]
        public Equipo Equipo;

        [DataMember()]
        public Pregunta Pregunta;

        public static List<PreguntaPorEquipo> ConvertirALista(Dictionary<Equipo, List<Pregunta>> preguntas)
        {
            List<PreguntaPorEquipo> resultado = new List<PreguntaPorEquipo>();

            foreach(var kv in preguntas)
            {
                foreach(var pregunta in kv.Value)
                {
                    PreguntaPorEquipo p = new PreguntaPorEquipo()
                    {
                        Equipo = kv.Key,
                        Pregunta = pregunta
                    };
                    resultado.Add(p);
                }                
            }

            return resultado;
        }
    }
}
