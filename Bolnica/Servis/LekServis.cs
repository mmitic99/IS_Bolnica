using Bolnica.DTO;
using Bolnica.Repozitorijum;
using Bolnica.view.LekarView;
using Bolnica.view.UpravnikView;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Bolnica.Servis
{
    public class LekServis
    {
        private static LekServis instance = null;
        public int IdLekaGenerator = 0;
        public static LekServis GetInstance()
        {
            if (instance == null)
            {
                instance = new LekServis();
            }
            return instance;
        }

        public void DodajLek(Lek lek)
        {
            int indexPoslednjegLeka = UpravnikWindow.GetInstance().SviLekovi.Count - 1;
            IdLekaGenerator = UpravnikWindow.GetInstance().SviLekovi[indexPoslednjegLeka].IdLeka;
            lek.IdLeka = ++IdLekaGenerator;
            UpravnikWindow.GetInstance().SviLekovi.Add(lek);
            SkladisteZaLekove.GetInstance().SaveAll(UpravnikWindow.GetInstance().SviLekovi);
            OsveziPrikazLekova();
            OcistiTextPoljaLekova();
        }

        public void IzmeniLek(int index, Lek lek)
        {
            int StariIdLeka = UpravnikWindow.GetInstance().SviLekovi[index].IdLeka;
            UpravnikWindow.GetInstance().SviLekovi[index] = lek;
            UpravnikWindow.GetInstance().SviLekovi[index].IdLeka = StariIdLeka;
            SkladisteZaLekove.GetInstance().SaveAll(UpravnikWindow.GetInstance().SviLekovi);
            OsveziPrikazLekova();
            OcistiTextPoljaLekova();
        }
        public void IzmeniLekLekar(int index,Lek lek)
        {
            int StariIdLeka = LekoviPage.getInstance().Lekovi[index].IdLeka;
            LekoviPage.getInstance().Lekovi[index] = lek;
            LekoviPage.getInstance().Lekovi[index].IdLeka = StariIdLeka;
            SkladisteZaLekove.GetInstance().SaveAll(LekoviPage.getInstance().Lekovi);
            LekoviPage.getInstance().TabelaLekova.ItemsSource = new ObservableCollection<Lek>(LekoviPage.getInstance().Lekovi);
        }

        public void IzbrisiLek(int index)
        {
            UpravnikWindow.GetInstance().SviLekovi.RemoveAt(index);
            SkladisteZaLekove.GetInstance().SaveAll(UpravnikWindow.GetInstance().SviLekovi);
            OsveziPrikazLekova();
        }

        public bool ProveriValidnostLeka(LekDTO lek, String DodajIliIzmeni)
        {
            bool checkVrsta = false;
            bool checkKolicina = false;
            bool checkNaziv = false;
            bool checkKlasa = false;
            bool checkJacina = false;
            bool checkZamenskiLek = false;
            bool checkSastav = false;

            Regex sablon = new Regex(@"^[0-9a-zA-Z\s]+$");
            if (sablon.IsMatch(lek.NazivLeka))
            {

                foreach (Lek l in UpravnikWindow.GetInstance().SviLekovi)
                {
                    if (DodajIliIzmeni.Equals("dodaj"))
                    {
                        if (lek.NazivLeka.Equals(l.NazivLeka))
                        {
                            MessageBox.Show("Već postoji lek sa istim nazivom !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else
                        {
                            checkNaziv = true;
                        }
                    }
                    else
                    {
                        int index = UpravnikWindow.GetInstance().TabelaLekovaIzmeni.SelectedIndex;
                        if (lek.NazivLeka != UpravnikWindow.GetInstance().SviLekovi[index].NazivLeka && lek.NazivLeka.Equals(l.NazivLeka))
                        {
                            MessageBox.Show("Već postoji lek sa istim nazivom !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else
                        {
                            checkNaziv = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Neispravno unet naziv leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(lek.KolicinaLeka))
                checkKolicina = true;
            else
            {
                MessageBox.Show("Neispravno uneta količina leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[1-9]{1}[0-9]*$");
            if (sablon.IsMatch(lek.JacinaLeka))
                checkJacina = true;
            else
            {
                MessageBox.Show("Neispravno uneta jačina leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9a-zA-Z\s]+$");
            if (sablon.IsMatch(lek.ZamenskiLek))
                checkZamenskiLek = true;
            else
            {
                MessageBox.Show("Neispravno unet zamenski lek !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            sablon = new Regex(@"^[0-9a-zA-Z,\s]+$");
            if (sablon.IsMatch(lek.SastavLeka))
                checkSastav = true;
            else
            {
                MessageBox.Show("Neispravno unet sastav leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (DodajIliIzmeni.Equals("dodaj"))
            {
                if (UpravnikWindow.GetInstance().VrstaLeka.SelectedIndex != -1)
                    checkVrsta = true;
                else
                {
                    MessageBox.Show("Selektujte  vrstu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (UpravnikWindow.GetInstance().KlasaLeka.SelectedIndex != -1)
                    checkKlasa = true;
                else
                {
                    MessageBox.Show("Selektujte  klasu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                if (UpravnikWindow.GetInstance().VrstaLekaIzmeni.SelectedIndex != -1)
                    checkVrsta = true;
                else
                {
                    MessageBox.Show("Selektujte  vrstu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (UpravnikWindow.GetInstance().KlasaLekaIzmeni.SelectedIndex != -1)
                    checkKlasa = true;
                else
                {
                    MessageBox.Show("Selektujte  klasu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            if (checkJacina == true && checkKlasa == true && checkKolicina == true && checkVrsta == true && checkNaziv == true && checkSastav == true && checkZamenskiLek == true)
                return true;
            else
                return false;
        }

        public Model.Enum.VrstaLeka GetVrstuLeka(int IndexSelektovaneVrsteLeka)
        {
            if (IndexSelektovaneVrsteLeka == 0)
                return Model.Enum.VrstaLeka.Kapsula;
            else if (IndexSelektovaneVrsteLeka == 1)
                return Model.Enum.VrstaLeka.Tableta;
            else if (IndexSelektovaneVrsteLeka == 2)
                return Model.Enum.VrstaLeka.Sirup;
            else if (IndexSelektovaneVrsteLeka == 3)
                return Model.Enum.VrstaLeka.Sprej;
            else if (IndexSelektovaneVrsteLeka == 4)
                return Model.Enum.VrstaLeka.Gel;
            else
                return Model.Enum.VrstaLeka.SumecaTableta;
        }


        public Model.Enum.KlasaLeka GetKlasuLeka(int IndexSelektovaneKlaseLeka)
        {
            if (IndexSelektovaneKlaseLeka == 0)
                return Model.Enum.KlasaLeka.Analgetik;
            else if (IndexSelektovaneKlaseLeka == 1)
                return Model.Enum.KlasaLeka.Antipiretik;
            else if (IndexSelektovaneKlaseLeka == 2)
                return Model.Enum.KlasaLeka.Antimalarijski_Lek;
            else if (IndexSelektovaneKlaseLeka == 3)
                return Model.Enum.KlasaLeka.Antibiotik;
            else if (IndexSelektovaneKlaseLeka == 4)
                return Model.Enum.KlasaLeka.Antiseptik;
            else if (IndexSelektovaneKlaseLeka == 5)
                return Model.Enum.KlasaLeka.Stabilizator_Raspolozenja;
            else if (IndexSelektovaneKlaseLeka == 6)
                return Model.Enum.KlasaLeka.Hormonska_Zamena;
            else if (IndexSelektovaneKlaseLeka == 7)
                return Model.Enum.KlasaLeka.Oralni_Kontraceptiv;
            else if (IndexSelektovaneKlaseLeka == 8)
                return Model.Enum.KlasaLeka.Stimulant;
            else if (IndexSelektovaneKlaseLeka == 9)
                return Model.Enum.KlasaLeka.Trankvilajzer;
            else
                return Model.Enum.KlasaLeka.Statin;
        }

        private void OsveziPrikazLekova() 
        {
            UpravnikWindow.GetInstance().TabelaLekova.ItemsSource = new ObservableCollection<Lek>(UpravnikWindow.GetInstance().SviLekovi);
            UpravnikWindow.GetInstance().TabelaLekovaIzmeni.ItemsSource = new ObservableCollection<Lek>(UpravnikWindow.GetInstance().SviLekovi);
        }
        private void OcistiTextPoljaLekova() {}
    }
}
