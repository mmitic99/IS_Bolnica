using System.Collections.Generic;
using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.Factory.SkladisteRadnihVremenaFactory;
using Bolnica.Repozitorijum.Factory.SkladisteZaFeedbackFactory;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Model;

namespace Bolnica.Servis
{
    public class FeedbackServis
    {
        private ISkladisteZaFeedback SkladisteZaFeedback;

        private ISkladisteZaFeedbackFactory _skladisteZaFeedbackFactory = new SkladisteZaFeedbackFactoryXml();
        public ISkladisteZaFeedbackFactory SkladisteZaFeedbackFactory
        {
            get => _skladisteZaFeedbackFactory;
            set
            {
                _skladisteZaFeedbackFactory = value;
                SkladisteZaFeedback = value.CreateSkladisteZaFeedback();
            }
        }



        public FeedbackServis()
        {
            SkladisteZaFeedback = _skladisteZaFeedbackFactory.CreateSkladisteZaFeedback();
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