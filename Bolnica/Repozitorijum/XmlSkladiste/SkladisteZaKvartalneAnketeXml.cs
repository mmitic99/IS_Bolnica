using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.model;
using Bolnica.Repozitorijum.ISkladista;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    class SkladisteZaKvartalneAnketeXml : ISkladisteZaKvartalneAnkete
    {
        private String Lokacija;
        public static SkladisteZaKvartalneAnketeXml instance;

        public static SkladisteZaKvartalneAnketeXml GetInstance()
        {
            if(instance==null)
            {
                return new SkladisteZaKvartalneAnketeXml();
            }
            else
            {
                return instance;
            }
        }
        public SkladisteZaKvartalneAnketeXml()
        {
            instance = this;
            Lokacija = "..\\..\\SkladistePodataka\\kvartalne ankete.xml";
        }

        public List<KvartalnaAnketa> GetAll()
        {
            List<KvartalnaAnketa> ankete = new List<KvartalnaAnketa>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<KvartalnaAnketa>));
            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                ankete = (List<KvartalnaAnketa>)serializer.Deserialize(stream);
                stream.Close();
            }

            return ankete;
        }

        public void Save(KvartalnaAnketa KvartalnaAnketa)
        {
            List<KvartalnaAnketa> ankete = GetAll();
            ankete.Add(KvartalnaAnketa);

            SaveAll(ankete);
        }

        public void SaveAll(List<KvartalnaAnketa> ankete)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<KvartalnaAnketa>));
                serializer.Serialize(stream, ankete);
            }
            finally
            {
                stream.Close();
            }
        }

    }
}
