using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model;
using Repozitorijum;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteUpravnikXml : ISkladisteUpravnik
    {
        private SkladisteUpravnikXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\upravnici.xml";
        }
        public static SkladisteUpravnikXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteUpravnikXml();
            }
            return instance;
        }

        public List<Upravnik> GetAll()
        {
            List<Upravnik> upravnici = new List<Upravnik>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Upravnik>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                upravnici = (List<Upravnik>)serializer.Deserialize(stream);
                stream.Close();
            }

            return upravnici;
        }

        public void Save(Upravnik upravnik)
        {
            List<Upravnik> upravnici = GetAll();
            upravnici.Add(upravnik);

            SaveAll(upravnici);
        }

        public void SaveAll(List<Upravnik> upravnici)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Upravnik>));
                serializer.Serialize(stream, upravnici);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;
        private static SkladisteUpravnikXml instance = null;

    }
}