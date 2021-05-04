using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bolnica.Repozitorijum
{
    public class SkladisteZaLekove
    {
        public String Lokacija { get; set; }
        private static SkladisteZaLekove instance = null;

        public static SkladisteZaLekove GetInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaLekove();
            }
            return instance;
        }


        public List<Lek> Lekovi
        {
            get;
            set;
        }

        public SkladisteZaLekove()
        {
            Lekovi = new List<Lek>();
            Lokacija = "..\\..\\SkladistePodataka\\lekovi.xml";
        }

        public List<Lek> GetAll()
        {
            Lekovi = new List<Lek>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lek>));

            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);

                Lekovi = (List<Lek>)serializer.Deserialize(reader);
                reader.Close();
            }
            return Lekovi;
        }

        public void Save(Lek lek)
        {
            Lekovi = GetAll();

            Lekovi.Add(lek);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Lek>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, Lekovi);
            writer.Close();
        }

        public void SaveAll(List<Lek> lekovi)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Lek>));

            StreamWriter writer = new StreamWriter(Lokacija);
            serializer.Serialize(writer, lekovi);
            writer.Close();
        }
        public Lek getById(int id)
        {
            List<Lek> lekovi = this.GetAll();
           Lek l1 = new Lek();
            foreach (Lek l in lekovi)
            {
                if (l.IdLeka == null)
                {
                    if (l.IdLeka.Equals(id))
                    {
                        l1 = l;
                        break;
                    }
                }
            }
            return l1;
        }
    }
}
