using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis.StatePattern
{
    public interface IStanjeKorisnika
    {
        bool DodajZakazivanje();
        bool DodajPomeranje();
        int DobaviBrojOtkazivanjeUProteklihMesecDana();
        bool OdblokirajKorisnika();
        bool BlokirajKorisnika();
        String DobaviPorukuZabrane();
        bool DaLiMozeDaZakaze();
        bool DaLiMozeDaPomeri();
        bool DaLiJePredZabranuZakazivanja();
        bool DaLiJePredZabranuOtkazivanja();
    }
}
