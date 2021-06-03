using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Repozitorijum.Factory.SkladisteZaFeedbackFactory
{
    public class SkladisteZaFeedbackFactoryXml : ISkladisteZaFeedbackFactory
    {
        public ISkladisteZaFeedback CreateSkladisteZaFeedback()
        {
            return new SkladisteZaFeedbackXml();
        }
    }
}