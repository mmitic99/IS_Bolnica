using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model;
using Repozitorijum;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaSpecijalizacijuXml : ISkladisteZaSpecijalizaciju
    {
        public SkladisteZaSpecijalizacijuXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\specijalizacije.xml";
        }

        public List<Specijalizacija> GetAll()
        {
            List<Specijalizacija> specijalizacije = new List<Specijalizacija>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Specijalizacija>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                specijalizacije = (List<Specijalizacija>)serializer.Deserialize(stream);
                stream.Close();
            }

            return specijalizacije;
        }

        public void Save(Model.Specijalizacija specijalizacija)
        {
            List<Specijalizacija> specijalizacije = GetAll();
            specijalizacije.Add(specijalizacija);

            SaveAll(specijalizacije);
        }

        public void SaveAll(List<Specijalizacija> specijalizacije)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Specijalizacija>));
                serializer.Serialize(stream, specijalizacije);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;

    }
}