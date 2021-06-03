using Bolnica.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bolnica.Repozitorijum.ISkladista;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteBolnickihLecenjaXml : ISkladisteBolnickihLecenja
    {
        private SkladisteBolnickihLecenjaXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\bolnicka lecenja.xml";
        }
        public static SkladisteBolnickihLecenjaXml GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteBolnickihLecenjaXml();
            }
            return instance;
        }

        public List<BolnickoLecenje> GetAll()
        {
            List<BolnickoLecenje> bolnickaLecenja = new List<BolnickoLecenje>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<BolnickoLecenje>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                bolnickaLecenja = (List<BolnickoLecenje>)serializer.Deserialize(stream);
                stream.Close();
            }

            return bolnickaLecenja;
        }

        public void Save(BolnickoLecenje bolnickoLecenje)
        {
            List<BolnickoLecenje> bolnickaLecenja = GetAll();
            bolnickaLecenja.Add(bolnickoLecenje);

            SaveAll(bolnickaLecenja);
        }

        public void SaveAll(List<BolnickoLecenje> bolnickaLecenja)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<BolnickoLecenje>));
                serializer.Serialize(stream, bolnickaLecenja);
            }
            finally
            {
                stream.Close();
            }
        }

        public void RemoveById(string jmbgPacijenta)
        {
            List<BolnickoLecenje> svaLecenja = this.GetAll();
            foreach(BolnickoLecenje bolnickoLecenje in svaLecenja)
            {
                if (bolnickoLecenje.jmbgPacijenta.Equals(jmbgPacijenta))
                {
                    svaLecenja.Remove(bolnickoLecenje);
                    break;
                }
                this.SaveAll(svaLecenja);
            }
        }

        private String Lokacija;
        private static SkladisteBolnickihLecenjaXml instance = null;
    }
}
