using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Model.Skladista
{
   public class SkladisteZaTermine
   {
        private String Lokacija = "..\\..\\SkladistePodataka\\termini.xml";
        
        public List<Termin> GetAll()
      {
            List<Termin> termini = new List<Termin>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                termini = (List<Termin>)serializer.Deserialize(reader);
                reader.Close();

            }
            return termini;
        }

        public void Save(Model.Termin termin)
      {
            List<Termin> termini = GetAll();

            termini.Add(termin);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, termini);
            writer.Close();
        }
      
      public void SaveAll(List<Termin> termini)
      {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, termini);
            writer.Close();
        }
   
     
   
   }
}