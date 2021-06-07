using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Model;
using Repozitorijum;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaKorisnickeAktivnostiXml : ISkladisteZaKorisnickeAktivnosti
    {
        private String Lokacija { get; set; }
        private static SkladisteZaKorisnickeAktivnostiXml instance = null;

        public static SkladisteZaKorisnickeAktivnostiXml GetInstance()
        {
            if (instance == null)
            {
                return new SkladisteZaKorisnickeAktivnostiXml();
            }
            else
                return instance;
        }

        public SkladisteZaKorisnickeAktivnostiXml()
        {
            instance = this;
            Lokacija = "..\\..\\SkladistePodataka\\aktivnosti.xml";
        }
        public List<KorisnickeAktivnostiNaAplikaciji> GetAll()
        {
            List<KorisnickeAktivnostiNaAplikaciji> KorisnickeAktivnostiNaAplikacijii = new List<KorisnickeAktivnostiNaAplikaciji>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<KorisnickeAktivnostiNaAplikaciji>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                KorisnickeAktivnostiNaAplikacijii = (List<KorisnickeAktivnostiNaAplikaciji>)serializer.Deserialize(stream);
                stream.Close();
            }

            return KorisnickeAktivnostiNaAplikacijii;
        }

        public void Save(Model.KorisnickeAktivnostiNaAplikaciji korisnickeAktivnosti)
        {
            List<KorisnickeAktivnostiNaAplikaciji> KorisnickeAktivnostiNaAplikacijii = GetAll();
            KorisnickeAktivnostiNaAplikacijii.Add(korisnickeAktivnosti);

            SaveAll(KorisnickeAktivnostiNaAplikacijii);
        }

        public void SaveAll(List<KorisnickeAktivnostiNaAplikaciji> korisnickeAktivnosti)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<KorisnickeAktivnostiNaAplikaciji>));
                serializer.Serialize(stream, korisnickeAktivnosti);
            }
            finally
            {
                stream.Close();
            }
        }

        public Model.KorisnickeAktivnostiNaAplikaciji GetByJmbg(String jmbgKorisnika)
        {
            List<KorisnickeAktivnostiNaAplikaciji> KorisnickeAktivnostiNaAplikacijii = this.GetAll();
            foreach (KorisnickeAktivnostiNaAplikaciji l in KorisnickeAktivnostiNaAplikacijii)
            {
                if (l.JmbgKorisnika != null)
                {
                    if (l.JmbgKorisnika.Equals(jmbgKorisnika))
                    {
                        return l;
                    }
                }
            }
            return null;
        }


    }
}