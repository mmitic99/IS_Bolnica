using System.Collections.Generic;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Model;

namespace Bolnica.Servis
{
    public class FeedbackServis
    {
        private ISkladisteZaFeedback SkladisteZaFeedback;

        public FeedbackServis()
        {
            SkladisteZaFeedback = new SkladisteZaFeedbackXml();
        }
        public List<Feedback> GetAll()
        {
            return SkladisteZaFeedback.GetAll();
        }

        public bool Save(Feedback feedback)
        {
            SkladisteZaFeedback.Save(feedback);
            return true;
        }

        public void SaveAll(List<Feedback> feedbacks)
        {
            SkladisteZaFeedback.SaveAll(feedbacks);
        }
    }
}