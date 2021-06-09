using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis.StatePattern
{
    interface IStanjeKorisnika
    {
        bool DodajZakazivanje(String JmbgKorisnika);
        bool DodajPomeranje(String JmbgKorisnika);
        bool DaLiMozeDaZakaze(String JmbgKorisnika);
        bool DaLiMozeDaPomeri(String JmbgKorisnika);
        int DobaviBrojOtkazivanjeUProteklihMesecDana(String JmbgKorisnika);
        bool OdblokirajKorisnika(String JmbgKorisnika);
        bool BlokirajKorisnika(String JmbgKorisnika);
        String DobaviPorukuZabrane(String JmbgKorisnik);
    }
}
