using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model;
using Repozitorijum;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaObavestenjaXml : ISkladisteZaObavestenja
    {
        private String Lokacija;
        private static SkladisteZaObavestenjaXml instance = null;

        public SkladisteZaObavestenjaXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\obavestenja.xml";
        }
        public static SkladisteZaObavestenjaXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaObavestenjaXml();
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
            return;
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

        public List<Obavestenje> GetObavestenjaByJmbg(String korisnickoIme)
        {
            List<Obavestenje> svaObavestenja = this.GetAll();
            List<Obavestenje> odgovarajucaObavestanje = new List<Obavestenje>();
            foreach(Obavestenje o in svaObavestenja)
            {
                if(o.JmbgKorisnika.Equals(korisnickoIme))
                {
                    odgovarajucaObavestanje.Add(o);
                }
            }
            return odgovarajucaObavestanje;
        }
    }
}