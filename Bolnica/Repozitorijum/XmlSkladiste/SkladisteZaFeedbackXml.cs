using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.model;

namespace Bolnica.Repozitorijum.XmlSkladiste
{
    public class SkladisteZaFeedbackXml : ISkladisteZaFeedback
    {
        public SkladisteZaFeedbackXml()
        {
            Lokacija = "..\\..\\SkladistePodataka\\feedback.xml";
        }

        private string Lokacija;

        public List<Feedback> GetAll()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Feedback>));

            if (File.Exists(Lokacija))
            {
                StreamReader stream = new StreamReader(Lokacija);
                feedbacks = (List<Feedback>)serializer.Deserialize(stream);
                stream.Close();
            }

            return feedbacks;
        }

        public void Save(Feedback entitet)
        {
            List<Feedback> feedbacks = GetAll();
            feedbacks.Add(entitet);
            SaveAll(feedbacks);
        }

        public void SaveAll(List<Feedback> entitet)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Feedback>));
                serializer.Serialize(stream, entitet);
            }
            finally
            {
                stream.Close();
            }
        }
    }
}