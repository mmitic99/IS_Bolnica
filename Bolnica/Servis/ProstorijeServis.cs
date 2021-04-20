using Bolnica.view.UpravnikView;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void DodajStacionarnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka_.Add(new StacionarnaOprema(tipOpreme, kolicina));
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[i].Staticka_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina(); 
            OcistiTextPoljaStatickeOpremeUMagacinu();
        }

        public void IzbrisiStacionarnuOpremuIzMagacina(int index)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Staticka_.RemoveAt(index);
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[i].Staticka_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaStatickeOpremeMagacina();
        }
            public void DodajPotrosnuOpremuUMagacin(String tipOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++) 
            { 
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna_.Add(new PotrosnaOprema(tipOpreme, kolicina));
                    UpravnikWindow.GetInstance().PotrosnaMagacin = prostorije[i].Potrosna_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaDinamickeOpremeMagacina(); 
            OcistiTextPoljaDinamickeOpremeUMagacinu();
        }

        public void IzbrisiPotrosnuOpremuIzMagacina(int index)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                {
                    prostorije[i].Potrosna_.RemoveAt(index);
                    UpravnikWindow.GetInstance().PotrosnaMagacin = prostorije[i].Potrosna_;
                }
            }
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            OsveziPrikazTabelaDinamickeOpremeMagacina();
        }

        public void IzmeniStacionarnuOpremuUMagacinu(int index, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka_[index].Kolicina_ + kolicina) >= 0)
                {
                    prostorije[i].Staticka_[index].Kolicina_ += kolicina;
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[i].Staticka_;
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                    OsveziPrikazTabelaStatickeOpremeMagacina();
                    OcistiTextPoljaStatickeOpremeUMagacinu();
                    break;
                }
                else if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Staticka_[index].Kolicina_ + kolicina) < 0)
                    MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void IzmeniDinamickuOpremuUMagacinu(int index, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            for (int i = 0; i < prostorije.Count; i++)
            {
                if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna_[index].KolicinaOpreme_ += kolicina) >= 0)
                {
                    UpravnikWindow.GetInstance().PotrosnaMagacin = prostorije[i].Potrosna_;
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                    OsveziPrikazTabelaDinamickeOpremeMagacina();
                    OcistiTextPoljaDinamickeOpremeUMagacinu();
                    break;
                }
                else if (prostorije[i].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin && (prostorije[i].Potrosna_[index].KolicinaOpreme_ += kolicina) < 0)
                    MessageBox.Show("Ne možete oduzeti više potrošne opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            OsveziPrikazProstorija();
            OcistiTextPoljaProstorije();
        }

        public void IzmeniProstoriju(int index, Prostorija p)
        {
            int StariIdProstorije = UpravnikWindow.GetInstance().ListaProstorija[index].IdProstorije_;
            UpravnikWindow.GetInstance().ListaProstorija[index] = p;
            UpravnikWindow.GetInstance().ListaProstorija[index].IdProstorije_ = StariIdProstorije;
            SkladisteZaProstorije.GetInstance().SaveAll(UpravnikWindow.GetInstance().ListaProstorija);
            OsveziPrikazProstorija();
            OcistiTextPoljaProstorije();
        }

        public void IzbrisiProstoriju(int index)
        {
            UpravnikWindow.GetInstance().ListaProstorija.RemoveAt(index);
            OsveziPrikazProstorija();
        }

        public void PrebaciStacionarnuOpremuUProstoriju(int indexIzKojeProstorije, int indexUKojuProstoriju, String nazivOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            bool nazivOpremeVecPrisutan = false;
            int indexOpreme = UpravnikWindow.GetInstance().TabelaOpremeIzKojeSePrebacuje.SelectedIndex;
            for (int i = 0; i < prostorije[indexUKojuProstoriju].Staticka_.Count; i++)
            {
                if (prostorije[indexUKojuProstoriju].Staticka_[i].TipStacionarneOpreme_.Equals(nazivOpreme))
                {
                    nazivOpremeVecPrisutan = true;
                    if (prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ - kolicina >= 0)
                    {
                        prostorije[indexUKojuProstoriju].Staticka_[i].Kolicina_ += kolicina;
                        prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ -= kolicina;
                        UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima = prostorije[indexIzKojeProstorije].Staticka_;
                        UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexUKojuProstoriju].Staticka_;
                        if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        {
                            UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                        { 
                            UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                            OsveziPrikazTabelaStatickeOpremeMagacina();
                        }
                        SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                        OsveziPrikazTabelaOpreme();
                    }
                    else
                    {
                        MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        UpravnikWindow.GetInstance().KolicinaOpremeSKojomSeRadi_Copy.Focus();
                    }
                }
            }
            if (nazivOpremeVecPrisutan == false)
            {
                if (prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ - kolicina >= 0)
                {
                    prostorije[indexUKojuProstoriju].Staticka_.Add(new StacionarnaOprema(nazivOpreme, kolicina));
                    prostorije[indexIzKojeProstorije].Staticka_[indexOpreme].Kolicina_ -= kolicina;
                    UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima = prostorije[indexIzKojeProstorije].Staticka_;
                    UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexUKojuProstoriju].Staticka_;
                    if (prostorije[indexIzKojeProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexIzKojeProstorije].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    if (prostorije[indexUKojuProstoriju].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    {
                        UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexUKojuProstoriju].Staticka_;
                        OsveziPrikazTabelaStatickeOpremeMagacina();
                    }
                    SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                    OsveziPrikazTabelaOpreme();
                }
                else
                {
                    MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    UpravnikWindow.GetInstance().KolicinaOpremeSKojomSeRadi_Copy.Focus();
                }
            }
        }

        public void IzbrisiStacionarnuOpremuIzProstorije(int indexProstorije, int indexOpreme)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            prostorije[indexProstorije].Staticka_.RemoveAt(indexOpreme);
            if (prostorije[indexProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexProstorije].Staticka_;
            SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
            UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexProstorije].Staticka_;
            OsveziPrikazTabelaStatickeOpremeMagacina();
            OsveziPrikazTabelaOpreme();
        }

        public void IzmeniStacionarnuOpremuProstorije(int indexProstorije, int indexOpreme, int kolicina)
        {
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            if ((prostorije[indexProstorije].Staticka_[indexOpreme].Kolicina_ + kolicina) < 0)
                MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                prostorije[indexProstorije].Staticka_[indexOpreme].Kolicina_ += kolicina;
                if (prostorije[indexProstorije].VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
                    UpravnikWindow.GetInstance().StacionarnaMagacin = prostorije[indexProstorije].Staticka_;
                SkladisteZaProstorije.GetInstance().SaveAll(prostorije);
                UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje = prostorije[indexProstorije].Staticka_;
                OsveziPrikazTabelaStatickeOpremeMagacina();
                OsveziPrikazTabelaOpreme();
            }
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

        public bool ProveriValidnostOpreme(String NazivOpreme, String Kolicina)
        {
            bool checkNaziv = false;
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[a-zA-ZŠĐŽĆČšđžćč]*$");
            if (sablon.IsMatch(NazivOpreme))
            {
                checkNaziv = true;
            }
            else
            {
                MessageBox.Show("Neispravno unet naziv opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(Kolicina))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta količina opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (checkNaziv == true && checkKolicina == true)
                return true;
            else
                return false;
        }

        public bool ProveriValidnostKolicineOpreme(String Kolicina)
        {
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[+-]?[0-9]*$");
            if (sablon.IsMatch(Kolicina) && !Kolicina.Equals(""))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta izmena količine opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (checkKolicina == true)
                return true;
            else
                return false;
        }

        public bool ProveriValidnostKolicineOpremePriPrebacivanju(String Kolicina)
        {
            bool checkKolicina = false;

            Regex sablon = new Regex(@"^[+]?[0-9]*$");
            if (sablon.IsMatch(Kolicina) && !Kolicina.Equals(""))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta količine opreme koja se prebacuje !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (checkKolicina == true)
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

        public List<StacionarnaOprema> GetStacionarnaOpremaProstorije(int index)
        {
            List<StacionarnaOprema> oprema = new List<StacionarnaOprema>();
            List<Prostorija> prostorije = SkladisteZaProstorije.GetInstance().GetAll();
            oprema = prostorije[index].Staticka_;
            return oprema;
        }

        public void OsveziPrikazProstorija()
        {
            UpravnikWindow.GetInstance().TabelaProstorija.Items.Refresh();
            UpravnikWindow.GetInstance().TabelaProstorijaIzmeni.Items.Refresh();
            UpravnikWindow.GetInstance().TabelaProstorijaIzKojeSePrebacujeOprema.Items.Refresh();
            UpravnikWindow.GetInstance().TabelaProstorijeUKojuSePrebacujeOprema.Items.Refresh();
        }

        private void OsveziPrikazTabelaStatickeOpremeMagacina()
        {
            UpravnikWindow.GetInstance().TabelaStatickeMagacinIzmeni.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaMagacin);
            UpravnikWindow.GetInstance().TabelaStatickeMagacin.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaMagacin);
        }

        private void OsveziPrikazTabelaDinamickeOpremeMagacina()
        {
            UpravnikWindow.GetInstance().TabelaDinamickeMagacinIzmeni.ItemsSource = new ObservableCollection<PotrosnaOprema>(UpravnikWindow.GetInstance().PotrosnaMagacin);
            UpravnikWindow.GetInstance().TabelaDinamickeMagacin.ItemsSource = new ObservableCollection<PotrosnaOprema>(UpravnikWindow.GetInstance().PotrosnaMagacin);
        }

        private void OsveziPrikazTabelaOpreme()
        {
            UpravnikWindow.GetInstance().TabelaOpremeIzKojeSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaOpremaOdKojeSeUzima);
            UpravnikWindow.GetInstance().TabelaOpremeUKojuSePrebacuje.ItemsSource = new ObservableCollection<StacionarnaOprema>(UpravnikWindow.GetInstance().StacionarnaOpremaUKojuSeDodaje);
        }

        private void OcistiTextPoljaProstorije()
        {
            UpravnikWindow.GetInstance().BrojProstorijeTextBox.Clear();
            UpravnikWindow.GetInstance().SpratTextBox.Clear();
            UpravnikWindow.GetInstance().KvadraturaTextBox.Clear();
            UpravnikWindow.GetInstance().BrojProstorijeTextBoxIzmeni.Clear();
            UpravnikWindow.GetInstance().SpratTextBoxIzmeni.Clear();
            UpravnikWindow.GetInstance().KvadraturaTextBoxIzmeni.Clear();
        }

        private void OcistiTextPoljaStatickeOpremeUMagacinu()
        {
            UpravnikWindow.GetInstance().KolicinaStatickeOpreme.Clear();
            UpravnikWindow.GetInstance().KolicinaStatickeOpremeIzmeni.Clear();
            UpravnikWindow.GetInstance().NazivStatickeOpreme.Clear();
            UpravnikWindow.GetInstance().NazivStatickeOpremeIzmeni.Clear();
        }

        private void OcistiTextPoljaDinamickeOpremeUMagacinu()
        {
            UpravnikWindow.GetInstance().KolicinaDinamickeOpreme.Clear();
            UpravnikWindow.GetInstance().KolicinaDinamickeOpremeIzmeni.Clear();
            UpravnikWindow.GetInstance().NazivDinamickeOpreme.Clear();
            UpravnikWindow.GetInstance().NazivDinamickeOpremeIzmeni.Clear();
        }

        public SkladisteZaProstorije skladisteZaProstorije;
        public SkladisteZaTermine skladisteZaTermine;

    }
}