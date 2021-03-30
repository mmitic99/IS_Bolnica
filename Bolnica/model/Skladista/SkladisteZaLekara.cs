using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Model.Skladista
{
   public class SkladisteZaLekara
    {
        private String Lokacija = "..\\..\\..\\Skladiste\\lekari.xml";
        
        public List<Lekar> GetAll()
        {
            List<Lekar> lekari = new List<Lekar>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                lekari = (List<Lekar>)serializer.Deserialize(reader);
                reader.Close();

            }
            return lekari;
        }

        public void Save(Model.Lekar lekar)
        {
            List<Lekar> lekari = GetAll();

            lekari.Add(lekar);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, lekari);
            writer.Close();

        }

        public void SaveAll(List<Lekar> lekari)
      {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, lekari);
            writer.Close();
        }
   
      
   
   }
}