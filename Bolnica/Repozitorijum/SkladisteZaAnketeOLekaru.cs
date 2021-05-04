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
    class SkladisteZaAnketeOLekaru
    {
        private String Lokacija;
        public static SkladisteZaAnketeOLekaru instance;
        public static SkladisteZaAnketeOLekaru GetInstance()
        {
            if (instance == null)
            {
                return new SkladisteZaAnketeOLekaru();
            }
            else
            {
                return instance;
            }
        }
        public SkladisteZaAnketeOLekaru()
        {
            instance = this;
            Lokacija = "..\\..\\SkladistePodataka\\ankete o lekarima.xml";
        }

        public List<AnketaLekar> GetAll()
        {
            List<AnketaLekar> ankete = new List<AnketaLekar>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<AnketaLekar>));
            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                ankete = (List<AnketaLekar>)serializer.Deserialize(stream);
                stream.Close();
            }

            return ankete;
        }

        public void Save(AnketaLekar AnketaLekar)
        {
            List<AnketaLekar> ankete = GetAll();
            ankete.Add(AnketaLekar);

            SaveAll(ankete);
        }

        public void SaveAll(List<AnketaLekar> ankete)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<AnketaLekar>));
                serializer.Serialize(stream, ankete);
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
