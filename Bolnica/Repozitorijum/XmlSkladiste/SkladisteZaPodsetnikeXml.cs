using Bolnica.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    class SkladisteZaPodsetnikeXml : ISkladisteZaPodsetnike
    {
        private String Lokacija;

        public SkladisteZaPodsetnikeXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\podsetnici.xml";
        }

        public List<Podsetnik> GetAll()
        {
            List<Podsetnik> obavestenja = new List<Podsetnik>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Podsetnik>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                obavestenja = (List<Podsetnik>)serializer.Deserialize(stream);
                stream.Close();
            }

            return obavestenja;
        }

        public void Save(Podsetnik Podsetnik)
        {
            List<Podsetnik> podsetnici = GetAll();
            podsetnici.Add(Podsetnik);
            SaveAll(podsetnici);
            return;
        }

        public void SaveAll(List<Podsetnik> obavestenja)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Podsetnik>));
                serializer.Serialize(stream, obavestenja);
            }
            finally
            {
                stream.Close();
            }
        }

        List<Podsetnik> ISkladisteZaPodsetnike.GetPodsetniciByJmbg(string Jmbg)
        {
            List<Podsetnik> sviPodsetnici= this.GetAll();
            List<Podsetnik> odgovarajuciPodsetnici = new List<Podsetnik>();
            foreach (Podsetnik p in sviPodsetnici)
            {
                if (p.JmbgKorisnika.Equals(Jmbg))
                {
                    odgovarajuciPodsetnici.Add(p);
                }
            }
            return odgovarajuciPodsetnici;
        }
    }
}
