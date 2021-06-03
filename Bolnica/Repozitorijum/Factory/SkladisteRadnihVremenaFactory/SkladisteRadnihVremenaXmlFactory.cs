using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Repozitorijum.Factory.SkladisteRadnihVremenaFactory
{
    public class SkladisteRadnihVremenaXmlFactory : ISkladisteRadnihVremenaFactory
    {
        public ISkladisteRadnihVremena CreateSkladisteRadnihVremena()
        {
            return new SkladisteRadnihVremenaXml();
        }
    }
}