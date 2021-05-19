using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteSekretaraXml : ISkladisteSekretara
    {
        private SkladisteSekretaraXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\sekretari.xml";
        }
        public static SkladisteSekretaraXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteSekretaraXml();
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
        private static SkladisteSekretaraXml instance = null;
    }
}