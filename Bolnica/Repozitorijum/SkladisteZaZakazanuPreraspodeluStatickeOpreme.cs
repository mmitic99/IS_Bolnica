using Bolnica.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Bolnica.Repozitorijum
{
    public class SkladisteZaZakazanuPreraspodeluStatickeOpreme
    {
        public String Lokacija { get; set; }
        private static SkladisteZaZakazanuPreraspodeluStatickeOpreme instance = null;

        public static SkladisteZaZakazanuPreraspodeluStatickeOpreme GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaZakazanuPreraspodeluStatickeOpreme();
            }
            return instance;
        }


        public List<ZakazanaPreraspodelaStatickeOpreme> ZakazanePreraspodele
        {
            get;
            set;
        }

        public SkladisteZaZakazanuPreraspodeluStatickeOpreme()
        {
            ZakazanePreraspodele = new List<ZakazanaPreraspodelaStatickeOpreme>();
            Lokacija = "..\\..\\SkladistePodataka\\zakazane preraspodele staticke opreme.xml";
        }

        public List<ZakazanaPreraspodelaStatickeOpreme> GetAll()
        {
            ZakazanePreraspodele = new List<ZakazanaPreraspodelaStatickeOpreme>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZakazanaPreraspodelaStatickeOpreme>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                ZakazanePreraspodele = (List<ZakazanaPreraspodelaStatickeOpreme>)serializer.Deserialize(reader);
                reader.Close();
            }
            return ZakazanePreraspodele;
        }

        public void Save(ZakazanaPreraspodelaStatickeOpreme zakazanaPreraspodela)
        {
            ZakazanePreraspodele = GetAll();

            ZakazanePreraspodele.Add(zakazanaPreraspodela);

            XmlSerializer serializer = new XmlSerializer(typeof(List<ZakazanaPreraspodelaStatickeOpreme>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, ZakazanePreraspodele);
            writer.Close();
        }

        public void SaveAll(List<ZakazanaPreraspodelaStatickeOpreme> zakazanePreraspodele)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZakazanaPreraspodelaStatickeOpreme>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, zakazanePreraspodele);
            writer.Close();
        }
    }
}
