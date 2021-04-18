using Bolnica.view.UpravnikView;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace Servis
{
    public class ProstorijeServis
    {

        private static ProstorijeServis instance = null;
        public int IdProstorijeGenerator = 0;
        public static ProstorijeServis GetInstance()
        {
            if (instance == null)
            {
                instance = new ProstorijeServis();
            }
            return instance;
        }

        public ProstorijeServis()
        {
            skladisteZaProstorije = new SkladisteZaProstorije();
        }

        public void RenovirajProstoriju()
        {
            // TODO: implement
        }

        public void ZavrsiRenoviranje()
        {
            // TODO: implement
        }

        public void IzmeniKolicinuLeka(int idLeka, double kolicina, int izProstorije, int uProstoriju)
        {
            // TODO: implement
        }

        public void IzmeniKolicinuPotrosneOpreme(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    foreach (PotrosnaOprema oprema in soba.Potrosna_)
                    {
                        if (oprema.TipOpreme_.Equals(tipOpreme))
                            oprema.KolicinaOpreme_ += kolicina;
                    }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public void IzmeniKolicinuStacionarneOpreme(int idProstorije, String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.IdProstorije_ == idProstorije)
                {
                    foreach (StacionarnaOprema oprema in soba.Staticka_)
                    {
                        if (oprema.TipStacionarneOpreme_.Equals(tipOpreme))
                            oprema.Kolicina_ += kolicina;
                    }
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public int DobaviProstoriju(DateTime pocetakTermina, DateTime krajTermina, Model.Enum.VrstaProstorije vrstaProstorije)
        {
            // TODO: implement
            return 0;
        }

        public List<Termin> PrikaziTermine(int idProstorije)
        {
            // TODO: implement
            return null;
        }

        public bool DaLiJeSLobodnaProstorija(int iDProstorije, DateTime pocetakTermina, DateTime krajTermina)
        {
            // TODO: implement
            return false;
        }

        public List<Prostorija> GetAll()
        {
            return SkladisteZaProstorije.GetInstance().GetAll();
        }

        public void Save(Prostorija prostorija)
        {
            SkladisteZaProstorije.GetInstance().Save(prostorija);
        }

        public void SaveAll(List<Prostorija> prostorije)
        {
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public void DodajStacionarnuOpremu(int idProstorije, String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.IdProstorije_ == idProstorije)
                {
                    if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        soba.Staticka_.Add(new StacionarnaOprema(tipOpreme, kolicina));
                    }
                    else if (true){}
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public void DodajPotrosnuOpremu(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    soba.Potrosna_.Add(new PotrosnaOprema(tipOpreme, kolicina));
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public void IzbrisiStacionarnuOpremu(int idProstorije, int index)
        {
            List<StacionarnaOprema> potrosna = new List<StacionarnaOprema>();
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.IdProstorije_ == idProstorije)
                {
                    soba.Staticka_.RemoveAt(index);
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public void IzbrisiPotrosnuOpremu(int index)
        {
            List<PotrosnaOprema> potrosna = new List<PotrosnaOprema>();
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    soba.Potrosna_.RemoveAt(index);
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
        }

        public Prostorija GetMagacin()
        {
            Prostorija p = new Prostorija();
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            foreach (Prostorija soba in prostorije)
            {
                if (soba.VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    return soba;
            }
            return p;
        }

        public void DodajProstoriju(Prostorija p)
        {
            Prostorija prostorija = p;
            int indexPoslednjeProstorije = UpravnikWindow.GetInstance().ListaProstorija.Count - 1;
            IdProstorijeGenerator = UpravnikWindow.GetInstance().ListaProstorija[indexPoslednjeProstorije].IdProstorije_;
            prostorija.IdProstorije_ = ++IdProstorijeGenerator;
            UpravnikWindow.GetInstance().ListaProstorija.Add(prostorija);
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            UpravnikWindow.GetInstance().TabelaProstorija.Items.Refresh();
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.Items.Refresh();
            OcistiTextPolja();
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            int StariIdProstorije = UpravnikWindow.GetInstance().ListaProstorija[index].IdProstorije_;
            UpravnikWindow.GetInstance().ListaProstorija[index] = p;
            UpravnikWindow.GetInstance().ListaProstorija[index].IdProstorije_ = StariIdProstorije;
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            UpravnikWindow.GetInstance().TabelaProstorija.Items.Refresh();
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.Items.Refresh();
            OcistiTextPolja();
        }

        public void IzbrisiProstoriju(int index)
        {
            UpravnikWindow.GetInstance().ListaProstorija.RemoveAt(index);
            UpravnikWindow.GetInstance().TabelaProstorija.Items.Refresh();
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.Items.Refresh();
        }

        public bool ProveriValidnostProstorije(String BrojProstorije, String Sprat, int IndexSelektovaneVrsteProstorije, String Kvadratura)
        {
            bool checkBrojProstorije = false;
            bool checkSprat = false;
            bool checkVrsta = false;
            bool checkKvadratura = false;

            Regex sablon = new Regex(@"^[0-9a-zA-Z]+$");
            if (sablon.IsMatch(BrojProstorije))
            {

                foreach (Prostorija Soba in UpravnikWindow.GetInstance().ListaProstorija)
                {
                    if (BrojProstorije.Equals(Soba.BrojSobe_))
                    {
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    else
                    {
                        checkBrojProstorije = true;
                    }
                }
            }
            else 
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9]$");
            if (sablon.IsMatch(Sprat))
                checkSprat = true;
            else
            {
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kvadratura))
                checkKvadratura = true;
            else 
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (IndexSelektovaneVrsteProstorije != -1)
                checkVrsta = true;
            else
            {
                MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (checkKvadratura == true && checkSprat == true && checkBrojProstorije == true && checkVrsta == true)
                return true;
            else
                return false;
        }

        public bool ProveriValidnostIzmeneProstorije(String BrojProstorije, String Sprat, int IndexSelektovaneVrsteProstorije, String Kvadratura)
        {
            bool checkBrojProstorije = false;
            bool checkSprat = false;
            bool checkVrsta = false;
            bool checkKvadratura = false;
            int indexSelektovaneProstorije = UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.SelectedIndex;

            Regex sablon = new Regex(@"^[0-9a-zA-Z]+$");
            if (sablon.IsMatch(BrojProstorije))
            {
                foreach (Prostorija Soba in UpravnikWindow.GetInstance().ListaProstorija)
                {
                    if (Soba.BrojSobe_ != UpravnikWindow.GetInstance().ListaProstorija[indexSelektovaneProstorije].BrojSobe_ && BrojProstorije.Equals(Soba.BrojSobe_))
                    { 
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                        else
                    {
                        checkBrojProstorije = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9]$");
            if (sablon.IsMatch(Sprat))
                checkSprat = true;
            else
            {
                MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kvadratura))
                checkKvadratura = true;
            else
            {
                MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (IndexSelektovaneVrsteProstorije != -1)
                checkVrsta = true;
            else
            {
                MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (checkKvadratura == true && checkSprat == true && checkBrojProstorije == true && checkVrsta == true)
                return true;
            else
                return false;
        }

        public Model.Enum.VrstaProstorije GetVrstuProstorije(int IndexSelektovaneVrsteProstorije)
        {
            if (IndexSelektovaneVrsteProstorije == 0)
                return Model.Enum.VrstaProstorije.Soba_za_preglede;
            else if (IndexSelektovaneVrsteProstorije == 1)
                return Model.Enum.VrstaProstorije.Operaciona_sala;
            else if (IndexSelektovaneVrsteProstorije == 2)
                return Model.Enum.VrstaProstorije.Soba_za_bolesnike;
            else
                return Model.Enum.VrstaProstorije.Magacin;
        }

        private void OcistiTextPolja()
        {
            UpravnikWindow.GetInstance().BrojProstorijeTextBox.Clear();
            UpravnikWindow.GetInstance().SpratTextBox.Clear();
            UpravnikWindow.GetInstance().KvadraturaTextBox.Clear();
            UpravnikWindow.GetInstance().BrojProstorijeTextBoxIzmeni.Clear();
            UpravnikWindow.GetInstance().SpratTextBoxIzmeni.Clear();
            UpravnikWindow.GetInstance().KvadraturaTextBoxIzmeni.Clear();
        }

        public SkladisteZaProstorije skladisteZaProstorije;
        public SkladisteZaTermine skladisteZaTermine;

    }
}