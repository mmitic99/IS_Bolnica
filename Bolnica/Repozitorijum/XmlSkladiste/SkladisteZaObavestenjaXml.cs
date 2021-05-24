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
            bool sacuvaj = true;
            foreach(Obavestenje o in obavestenja)
            {
                if(o.Sadrzaj.Equals(obavestenje.Sadrzaj) && o.Naslov.Equals(obavestenje.Naslov) && o.JmbgKorisnika.Equals(obavestenje.JmbgKorisnika) && obavestenje.Podsetnik)
                {
                    sacuvaj = false;
                    break;
                }
            }
            if(sacuvaj)
            {
                obavestenja.Add(obavestenje);

            }
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
                if(o.JmbgKorisnika.Equals(korisnickoIme) && !o.Podsetnik)
                {
                    odgovarajucaObavestanje.Add(o);
                }
            }
            return odgovarajucaObavestanje;
        }

        public List<Obavestenje> GetPodsetniciByJmbg(String jmbg)
        {
            List<Obavestenje> svaObavestenja = this.GetAll();
            List<Obavestenje> svaObavestenjaFiltrirano = this.GetAll();
            List<Obavestenje> odgovarajucaObavestanje = new List<Obavestenje>();
            bool remove = false;
            foreach (Obavestenje o in svaObavestenja)
            {
                if (o.VremeObavestenja < DateTime.Today && o.Podsetnik)
                {
                    svaObavestenjaFiltrirano.Remove(o);
                }
                else if (o.JmbgKorisnika.Equals(jmbg) && o.Podsetnik)
                {
                    odgovarajucaObavestanje.Add(o);
                }
            }
            
            this.SaveAll(svaObavestenjaFiltrirano);
            return odgovarajucaObavestanje;
        }

    }
}