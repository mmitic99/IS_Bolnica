using Bolnica.DTOs;
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
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Bolnica.Servis
{
    public class LekServis
    {
        private static LekServis instance = null;
        public int IdLekaGenerator = 0;
        public ISkladisteZaLekove skladisteZaLekove;
        public static LekServis GetInstance()
        {
            if (instance == null)
            {
                instance = new LekServis();
            }
            return instance;
        }

        public LekServis()
        {
            skladisteZaLekove = new SkladisteZaLekoveXml();
        }

        public void DodajLek(Lek lek)
        {
            List<Lek> SviLekovi = SkladisteZaLekoveXml.GetInstance().GetAll();
            int indexPoslednjegLeka = SviLekovi.Count - 1;
            IdLekaGenerator = SviLekovi[indexPoslednjegLeka].IdLeka;
            lek.IdLeka = ++IdLekaGenerator;
            SviLekovi.Add(lek);
            skladisteZaLekove.SaveAll(SviLekovi);
        }

        public void IzmeniLek(int index, Lek lek)
        {
            List<Lek> SviLekovi = SkladisteZaLekoveXml.GetInstance().GetAll();
            int StariIdLeka = SviLekovi[index].IdLeka;
            SviLekovi[index] = lek;
            SviLekovi[index].IdLeka = StariIdLeka;
            skladisteZaLekove.SaveAll(SviLekovi);
        }

        public void IzmeniLekLekar(int index,Lek lek)
        {
            int StariIdLeka = LekoviPage.getInstance().Lekovi[index].IdLeka;
            LekoviPage.getInstance().Lekovi[index] = lek;
            LekoviPage.getInstance().Lekovi[index].IdLeka = StariIdLeka;
            skladisteZaLekove.SaveAll(LekoviPage.getInstance().Lekovi);
            LekoviPage.getInstance().TabelaLekova.ItemsSource = new ObservableCollection<Lek>(LekoviPage.getInstance().Lekovi);
        }

        public void IzbrisiLek(int index)
        {
            List<Lek> SviLekovi = SkladisteZaLekoveXml.GetInstance().GetAll();
            SviLekovi.RemoveAt(index);
            skladisteZaLekove.SaveAll(SviLekovi);
        }

        private bool Validiraj(Regex sablon, String unos)
        {
            if (sablon.IsMatch(unos))
                return true;
            else
                return false;
        }

        private bool ValidirajNazivLeka(LekValidacijaDTO lek, String vrstaOperacije, int indexSelektovanogLeka)
        {
            Regex sablon = new Regex(@"^[0-9a-zA-Z\s]+$");
            List<Lek> SviLekovi = SkladisteZaLekoveXml.GetInstance().GetAll();
            if (sablon.IsMatch(lek.NazivLeka))
            {
                foreach (Lek l in SviLekovi)
                {
                    if (vrstaOperacije.Equals("dodaj"))
                    {
                        if (lek.NazivLeka.Equals(l.NazivLeka))
                        {
                            MessageBox.Show("Već postoji lek sa istim nazivom !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else if (vrstaOperacije.Equals("izmeni"))
                    {
                        if (lek.NazivLeka != SviLekovi[indexSelektovanogLeka].NazivLeka && lek.NazivLeka.Equals(l.NazivLeka))
                        {
                            MessageBox.Show("Već postoji lek sa istim nazivom !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private bool ValidirajComboBoxoveLeka(LekValidacijaDTO lek, String vrstaOperacije)
        {
            if (vrstaOperacije.Equals("dodaj"))
            {
                if (lek.VrstaLeka == -1 || lek.KlasaLeka == -1)
                {
                    MessageBox.Show("Selektujte  vrstu/klasu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            else if (vrstaOperacije.Equals("izmeni"))
            {
                if (lek.VrstaLeka == -1 || lek.KlasaLeka == -1)
                {
                    MessageBox.Show("Selektujte  vrstu/klasu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                return true;
            }
            return false;
}

public bool ProveriValidnostLeka(LekValidacijaDTO lek, String DodajIliIzmeni, int selektovaniLek)
        {
            if (ValidirajNazivLeka(lek, DodajIliIzmeni, selektovaniLek) == false)
            {
                MessageBox.Show("Neispravno unet naziv leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), lek.KolicinaLeka) == false)
            {
                MessageBox.Show("Neispravno uneta količina leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), lek.JacinaLeka) == false)
            {
                MessageBox.Show("Neispravno uneta jačina leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[0-9a-zA-Z\s]+$"), lek.ZamenskiLek) == false)
            {
                MessageBox.Show("Neispravno unet zamenski lek !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Validiraj(new Regex(@"^[0-9a-zA-Z,\s]+$"), lek.SastavLeka) == false)
            {
                MessageBox.Show("Neispravno unet sastav leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ValidirajComboBoxoveLeka(lek, DodajIliIzmeni) == true)
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

        public List<Lek> GetAll()
        {
            return skladisteZaLekove.GetAll();
        }

        public void Save(Lek lek)
        {
            skladisteZaLekove.Save(lek);
        }

        public void SaveAll(List<Lek> lekovi)
        {
            skladisteZaLekove.SaveAll(lekovi);
        }
        public Lek getById(int id)
        {
            return skladisteZaLekove.getById(id);
        }
    }
}
