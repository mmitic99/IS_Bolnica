using Model.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Model.Skladista
{
    public class SkladisteZaProstorije
    {
        public String Lokacija { get; set; }

        public List<Prostorija> Prostorije
        {
            get;
            set;
        }

        public SkladisteZaProstorije()
        {
            Prostorije = new List<Prostorija>();
            Lokacija = "..\\..\\SkladistePodataka\\prostorije.xml";
        }

        public List<Prostorija> GetAll()
        {
            Prostorije = new List<Prostorija>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Prostorija>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                Prostorije = (List<Prostorija>)serializer.Deserialize(reader);
                reader.Close();
            }
            return Prostorije;
        }

        public void Save(Prostorija prostorija)
        {
            Prostorije = GetAll();

            Prostorije.Add(prostorija);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Prostorija>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, Prostorije);
            writer.Close();
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Prostorija>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, prostorije);
            writer.Close();
        }
    }
}