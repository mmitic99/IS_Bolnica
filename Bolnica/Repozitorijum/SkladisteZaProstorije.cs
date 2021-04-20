using Model;
using Model.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Repozitorijum
{
    public class SkladisteZaProstorije
    {
        public String Lokacija { get; set; }
        private static SkladisteZaProstorije instance = null;

        public static SkladisteZaProstorije GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaProstorije();
            }
            return instance;
        }


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
        public Prostorija getById(int id)
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