using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.model;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    class SkladisteRadnihVremenaXml : ISkladisteRadnihVremena
    {
        public SkladisteRadnihVremenaXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\radnaVremena.xml";
        }

        public List<RadnoVreme> GetAll()
        {
            List<RadnoVreme> radnaVremena = new List<RadnoVreme>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<RadnoVreme>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                radnaVremena = (List<RadnoVreme>)serializer.Deserialize(stream);
                stream.Close();
            }

            return radnaVremena;
        }

        public void Save(RadnoVreme radnoVreme)
        {
            List<RadnoVreme> radnaVremena = GetAll();
            radnoVreme.GenerisiIdRadnogVremena();
            radnaVremena.Add(radnoVreme);

            SaveAll(radnaVremena);
        }

        public void SaveAll(List<RadnoVreme> radnaVremena)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<RadnoVreme>));
                serializer.Serialize(stream, radnaVremena);
            }
            finally
            {
                stream.Close();
            }
        }

        private String Lokacija;

    }
}
