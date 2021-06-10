using Bolnica.model;
using System.Collections.Generic;

namespace Bolnica.Servis
{
    public interface IVerifikacijaLekaServis
    {
        List<IVerifikacijaLeka> GetAll();
        List<IVerifikacijaLeka> GetObavestenjaByJmbg(string jmbg);
        void ObrisiVerifikacijuLeka(string idVerifikacije);
        void PosaljiVerifikacijuLeka(IVerifikacijaLeka verifikacijaLeka);
        void Save(IVerifikacijaLeka verifikacija);
        void SaveAll(List<IVerifikacijaLeka> verifikacije);
    }
}