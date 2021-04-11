using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Repozitorijum
{
    public class SkladisteZaObavestenja
    {
        private SkladisteZaObavestenja()
        {
            Lokacija = "..\\..\\SkladistePodataka\\obavestenja.xml";
        }
        public static SkladisteZaObavestenja GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaObavestenja();
            }
            return instance;
        }

        public List<Obavestenje> GetAll()
        {
            List<Obavestenje> obavestenja = new List<Obavestenje>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenje>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                obavestenja = (List<Obavestenje>)serializer.Deserialize(stream);
                stream.Close();
            }

            return obavestenja;
        }

        public void Save(Obavestenje obavestenje)
        {
            List<Obavestenje> obavestenja = GetAll();
            obavestenja.Add(obavestenje);

            SaveAll(obavestenja);
        }

        public void SaveAll(List<Obavestenje> obavestenja)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Obavestenje>));
                serializer.Serialize(stream, obavestenja);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;
        private static SkladisteZaObavestenja instance = null;

    }
}