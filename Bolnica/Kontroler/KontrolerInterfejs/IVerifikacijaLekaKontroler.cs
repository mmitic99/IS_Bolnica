using Bolnica.DTOs;
using Bolnica.model;
using System.Collections.Generic;

namespace Bolnica.Kontroler
{
    public interface IVerifikacijaLekaKontroler
    {
        List<IVerifikacijaLekaDTO> GetAll();
        List<IVerifikacijaLekaDTO> GetObavestenjaByJmbg(string jmbg);
        void ObrisiVerifikacijuLeka(object verifikacija);
        void PosaljiVerifikacijuLeka(IVerifikacijaLekaDTO verifikacijaLekaDTO);
        void Save(IVerifikacijaLeka verifikacija);
        void SaveAll(List<IVerifikacijaLeka> verifikacije);
    }
}