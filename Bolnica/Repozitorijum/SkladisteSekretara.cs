using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Repozitorijum
{
    public class SkladisteSekretara
    {
        private SkladisteSekretara()
        {
            Lokacija = "..\\..\\SkladistePodataka\\sekretari.xml";
        }
        public static SkladisteSekretara GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteSekretara();
            }
            return instance;
        }

        public List<Sekretar> GetAll()
        {
            List<Sekretar> sekretari = new List<Sekretar>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Sekretar>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                sekretari = (List<Sekretar>)serializer.Deserialize(stream);
                stream.Close();
            }

            return sekretari;
        }

        public void Save(Model.Sekretar sekretar)
        {
            List<Sekretar> sekretari = GetAll();
            sekretari.Add(sekretar);

            SaveAll(sekretari);
        }

        public void SaveAll(List<Sekretar> sekretari)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Sekretar>));
                serializer.Serialize(stream, sekretari);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;
        private static SkladisteSekretara instance = null;
    }
}