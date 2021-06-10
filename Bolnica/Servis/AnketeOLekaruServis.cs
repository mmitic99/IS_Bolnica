using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
namespace Bolnica.Servis
{
    class AnketeOLekaruServis
    {
        public ISkladisteZaAnketeOLekaru SkladisteZaAnketeOLekaru;
        public AnketeOLekaruServis()
        {
            SkladisteZaAnketeOLekaru = new SkladisteZaAnketeOLekaruXml();
        }

        public bool DaLiJeIstekloVremeZaPopunjavanjeAnkete(DateTime datum)
        {
            bool istekloVreme = true;
            if (DateTime.Now < datum.Date.AddDays(15)) istekloVreme = false;
            return istekloVreme;
        }

        public bool DaLiJeKorisnikPopunioAnketuOLekaru(PrikacenaAnketaPoslePregledaDTO anketaOLekaru)
        {
            bool popunioAnketu = false;
            AnketaLekar anketa = GetAnketaOLekaru(anketaOLekaru.JmbgLekara);
            foreach (String ID in anketa.ispunjeneAnkete)
            {
                if (ID != null && ID.Equals(anketaOLekaru.IDAnkete))
                {
                    popunioAnketu = true;
                    break;
                }
            }
            return popunioAnketu;
        }

        public AnketaLekar GetAnketaOLekaru(string jmbgLekara)
        {
            AnketaLekar anketaLekar = null;
            List<AnketaLekar> sveAnkete = SkladisteZaAnketeOLekaru.GetAll();
            foreach (AnketaLekar anketa in sveAnkete)
            {
                if (anketa.JmbgLekara.Equals(jmbgLekara))
                {
                    anketaLekar = anketa;
                    break;
                }
            }
            if (anketaLekar == null)
            {
                anketaLekar = kreirajNovuAnketuOLekaru(jmbgLekara);
            }
            return anketaLekar;
        }

        internal bool SacuvajAnketuOLekaru(PopunjenaAnketaPoslePregledaDTO popunjenaAnketa)
        {
            AnketaLekar anketaOLekaru = GetAnketaOLekaru(popunjenaAnketa.JmbgLekara);
            DodajPopunjenuAnketuOLekaru(popunjenaAnketa, anketaOLekaru);
            return SacuvajIzmenjenuAnketuOLekaru(anketaOLekaru);
        }

        private AnketaLekar kreirajNovuAnketuOLekaru(string jmbgLekara)
        {
            AnketaLekar anketaOLekaru = new AnketaLekar(jmbgLekara);
            SkladisteZaAnketeOLekaru.Save(anketaOLekaru);
            return anketaOLekaru;
        }

        private void DodajPopunjenuAnketuOLekaru(PopunjenaAnketaPoslePregledaDTO anketa, AnketaLekar anketaOLekaru)
        {
            anketaOLekaru.Komentari.Add(anketa.Komentar);
            anketaOLekaru.ProsecnaOcena = azuriranjeProsecneOcene(anketa.Ocena, anketaOLekaru.ProsecnaOcena, anketaOLekaru.BrojAnketa);
            anketaOLekaru.ispunjeneAnkete.Add(anketa.IDAnkete);
            ++anketaOLekaru.BrojAnketa;
        }

        private double azuriranjeProsecneOcene(double novaOcena, double ProsecnaOcena, int BrojAnketiranih)
        {
            return (ProsecnaOcena * BrojAnketiranih + novaOcena) / (BrojAnketiranih + 1);
        }

        private bool SacuvajIzmenjenuAnketuOLekaru(AnketaLekar anketaOLekaru)
        {
            List<AnketaLekar> ankete = SkladisteZaAnketeOLekaru.GetAll();
            for (int i = 0; i < ankete.Count; i++)
            {
                if (ankete[i].JmbgLekara.Equals(anketaOLekaru.JmbgLekara))
                {
                    ankete[i] = anketaOLekaru;
                    break;
                }
            }
            SkladisteZaAnketeOLekaruXml.GetInstance().SaveAll(ankete);
            return true;
        }
    }
}
