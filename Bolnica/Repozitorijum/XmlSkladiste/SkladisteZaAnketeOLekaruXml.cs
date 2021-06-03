using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.model;
using Bolnica.Repozitorijum.ISkladista;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    class SkladisteZaAnketeOLekaruXml : ISkladisteZaAnketeOLekaru
    {
        private String Lokacija;
        public static SkladisteZaAnketeOLekaruXml instance;
        public static SkladisteZaAnketeOLekaruXml GetInstance()
        {
            if (instance == null)
            {
                return new SkladisteZaAnketeOLekaruXml();
            }
            else
            {
                return instance;
            }
        }
        public SkladisteZaAnketeOLekaruXml()
        {
            instance = this;
            Lokacija = "..\\..\\SkladistePodataka\\ankete o lekarima.xml";
        }

        public List<AnketaLekar> GetAll()
        {
            List<AnketaLekar> ankete = new List<AnketaLekar>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<AnketaLekar>));
            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                ankete = (List<AnketaLekar>)serializer.Deserialize(stream);
                stream.Close();
            }

            return ankete;
        }

        public void Save(AnketaLekar AnketaLekar)
        {
            List<AnketaLekar> ankete = GetAll();
            ankete.Add(AnketaLekar);

            SaveAll(ankete);
        }

        public void SaveAll(List<AnketaLekar> ankete)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AnketaLekar>));
                serializer.Serialize(stream, ankete);
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
