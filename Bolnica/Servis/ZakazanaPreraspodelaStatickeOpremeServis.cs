using Bolnica.model;
using Bolnica.Repozitorijum;
using Bolnica.view.UpravnikView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    public class ZakazanaPreraspodelaStatickeOpremeServis
    {
        private static ZakazanaPreraspodelaStatickeOpremeServis instance = null;
        public ISkladisteZaZakazanuPreraspodeluStatickeOpreme skladisteZaZakazanuPreraspodeluStatickeOpreme;

        public static ZakazanaPreraspodelaStatickeOpremeServis GetInstance()
        {
            if (instance == null)
            {
                instance = new ZakazanaPreraspodelaStatickeOpremeServis();
            }
            return instance;
        }

        public ZakazanaPreraspodelaStatickeOpremeServis()
        {
            skladisteZaZakazanuPreraspodeluStatickeOpreme = new SkladisteZaZakazanuPreraspodeluStatickeOpremeXml();
        }

        public void ZakaziPreraspodeluStatickeOpreme(ZakazanaPreraspodelaStatickeOpreme preraspodela)
        {
            List<ZakazanaPreraspodelaStatickeOpreme> SvePreraspodele = new List<ZakazanaPreraspodelaStatickeOpreme>();
            SvePreraspodele = skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
            SvePreraspodele.Add(preraspodela);
            skladisteZaZakazanuPreraspodeluStatickeOpreme.SaveAll(SvePreraspodele);
        }

        public void OtkaziPreraspodeluStatickeOpreme(int index) 
        {
            List<ZakazanaPreraspodelaStatickeOpreme> SvePreraspodele = new List<ZakazanaPreraspodelaStatickeOpreme>();
            SvePreraspodele = skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
            SvePreraspodele.RemoveAt(index);
            skladisteZaZakazanuPreraspodeluStatickeOpreme.SaveAll(SvePreraspodele);
        }

        public bool ProveraValidnostiPreraspodeleOpreme(String trajanje)
        {
            Regex sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(trajanje) && !trajanje.Equals(""))
                return true;
            else
            {
                MessageBox.Show("Neispravno uneto trajanje termina za preraspodelu opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public List<ZakazanaPreraspodelaStatickeOpreme> GetAll()
        {
            return skladisteZaZakazanuPreraspodeluStatickeOpreme.GetAll();
        }

        public void Save(ZakazanaPreraspodelaStatickeOpreme zakazanaPreraspodela)
        {
            skladisteZaZakazanuPreraspodeluStatickeOpreme.Save(zakazanaPreraspodela);
        }

        public void SaveAll(List<ZakazanaPreraspodelaStatickeOpreme> zakazanePreraspodele)
        {
            skladisteZaZakazanuPreraspodeluStatickeOpreme.SaveAll(zakazanePreraspodele);
        }
    }
}
