using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.Repozitorijum.ISkladista;
using Model;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaProstorijeXml : ISkladisteZaProstorije
    {
        public String Lokacija { get; set; }
        private static SkladisteZaProstorijeXml instance = null;

        public static SkladisteZaProstorijeXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaProstorijeXml();
            }
            return instance;
        }


        public List<Prostorija> Prostorije
        {
            get;
            set;
        }

        public SkladisteZaProstorijeXml()
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
        public Prostorija GetById(int id)
        {
            List<Prostorija> prostorije = this.GetAll();
            Prostorija p1 = new Prostorija();
            foreach (Prostorija p in prostorije)
            {
                if (p.IdProstorije != null)
                {
                    if (p.IdProstorije.Equals(id))
                    {
                        p1 = p;
                        break;
                    }
                }
            }
            return p1;
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