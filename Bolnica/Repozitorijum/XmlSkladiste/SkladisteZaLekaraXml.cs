using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.Repozitorijum.ISkladista;
using Model;

namespace Bolnica.Repozitorijum.XmlSkladiste

{
    public class SkladisteZaLekaraXml : ISkladisteZaLekara
    {
        private SkladisteZaLekaraXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\lekari.xml";
        }
        public static SkladisteZaLekaraXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaLekaraXml();
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

        public void IzmeniLekara(string jmbgLekara, Lekar lekar)
        {
            List<Lekar> lekari = GetAll();
            foreach (Lekar lekar1 in lekari)
            {
                if (lekar1.Jmbg.Equals(jmbgLekara))
                {
                    lekari.Remove(lekar1);
                    lekari.Add(lekar);
                    break;
                }
            }
            SaveAll(lekari);
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
        private static SkladisteZaLekaraXml instance = null;

    }
}