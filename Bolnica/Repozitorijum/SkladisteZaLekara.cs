using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Repozitorijum

{
    public class SkladisteZaLekara
    {
        private SkladisteZaLekara()
        {
            Lokacija = "..\\..\\SkladistePodataka\\lekari.xml";
        }
        public static SkladisteZaLekara GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaLekara();
            }
            return instance;
        }

        public List<Lekar> GetAll()
        {
            List<Lekar> lekari = new List<Lekar>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                lekari = (List<Lekar>)serializer.Deserialize(stream);
                stream.Close();
            }

            return lekari;
        }
        public Lekar getByJmbg(String jmbg)
        {
            List<Lekar> lekari = this.GetAll();
            Lekar lekar1 = new Lekar();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.Jmbg != null)
                {
                    if (lekar.Jmbg.Equals(jmbg))
                    {
                        lekar1 = lekar;
                        break;
                    }
                }
            }
            return lekar1;
        }
        public void Save(Lekar lekar)
        {
            List<Lekar> lekari = GetAll();
            lekari.Add(lekar);

            SaveAll(lekari);
        }


        public void SaveAll(List<Lekar> lekari)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Lekar>));
                serializer.Serialize(stream, lekari);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;
        private static SkladisteZaLekara instance = null;

    }
}