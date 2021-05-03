using Bolnica.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bolnica.Repozitorijum
{
    public class SkladisteZaVerifikacijuLeka
    {
        public String Lokacija { get; set; }
        private static SkladisteZaVerifikacijuLeka instance = null;

        public static SkladisteZaVerifikacijuLeka GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaVerifikacijuLeka();
            }
            return instance;
        }


        public List<VerifikacijaLeka> VerifikacijeLeka
        {
            get;
            set;
        }

        public SkladisteZaVerifikacijuLeka()
        {
            VerifikacijeLeka = new List<VerifikacijaLeka>();
            Lokacija = "..\\..\\SkladistePodataka\\verifikacije lekova.xml";
        }

        public List<VerifikacijaLeka> GetAll()
        {
            VerifikacijeLeka = new List<VerifikacijaLeka>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<VerifikacijaLeka>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                VerifikacijeLeka = (List<VerifikacijaLeka>)serializer.Deserialize(reader);
                reader.Close();
            }
            return VerifikacijeLeka;
        }

        public void Save(VerifikacijaLeka verifikacija)
        {
            VerifikacijeLeka = GetAll();

            VerifikacijeLeka.Add(verifikacija);

            XmlSerializer serializer = new XmlSerializer(typeof(List<VerifikacijaLeka>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, VerifikacijeLeka);
            writer.Close();
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<VerifikacijaLeka>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, verifikacije);
            writer.Close();
        }

        public List<VerifikacijaLeka> GetObavestenjaByJmbg(String jmbg)
        {
            List<VerifikacijaLeka> sveVerifikacije = this.GetAll();
            List<VerifikacijaLeka> odgovarajuceVerifikacije = new List<VerifikacijaLeka>();
            foreach (VerifikacijaLeka vl in sveVerifikacije)
            {
                if (vl.JmbgPrimaoca.Equals(jmbg))
                {
                    odgovarajuceVerifikacije.Add(vl);
                }
            }
            return odgovarajuceVerifikacije;
        }
    }
}
