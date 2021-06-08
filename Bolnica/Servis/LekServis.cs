using Bolnica.DTOs;
using Bolnica.Repozitorijum;
using Bolnica.view.LekarView;
using Bolnica.view.UpravnikView;
using Model;
using System;
using Model.Enum;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.Validacije;

namespace Bolnica.Servis
{
    public class LekServis
    {
        private static LekServis instance = null;
        public int IdLekaGenerator = 0;
        public ISkladisteZaLekove skladisteZaLekove;
        public ValidacijaContext validacija = new ValidacijaContext(new LekStrategy());
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
            List<Lek> SviLekovi = skladisteZaLekove.GetAll();
            int indexPoslednjegLeka = SviLekovi.Count - 1;
            IdLekaGenerator = SviLekovi[indexPoslednjegLeka].IdLeka;
            lek.IdLeka = ++IdLekaGenerator;
            SviLekovi.Add(lek);
            skladisteZaLekove.SaveAll(SviLekovi);
        }

        public void IzmeniLek(int index, Lek lek)
        {
            List<Lek> SviLekovi = skladisteZaLekove.GetAll();
            int StariIdLeka = SviLekovi[index].IdLeka;
            SviLekovi[index] = lek;
            SviLekovi[index].IdLeka = StariIdLeka;
            skladisteZaLekove.SaveAll(SviLekovi);
        }

        public void IzmeniLekLekar(int index,Lek lek)
        {
            List<Lek> SviLekovi = skladisteZaLekove.GetAll();
            int StariIdLeka = LekoviPage.getInstance().Lekovi[index].IdLeka;
            SviLekovi[index] = lek;
            SviLekovi[index].IdLeka = StariIdLeka;
            skladisteZaLekove.SaveAll(SviLekovi);
            LekoviPage.getInstance().TabelaLekova.ItemsSource = new ObservableCollection<LekDTO>(LekoviPage.getInstance().Lekovi);
        }

        public void IzbrisiLek(int index)
        {
            List<Lek> SviLekovi = skladisteZaLekove.GetAll();
            SviLekovi.RemoveAt(index);
            skladisteZaLekove.SaveAll(SviLekovi);
        }

        private bool Validiraj(Regex sablon, String unos)
        {
            bool validno = true;
            if (!sablon.IsMatch(unos))
                validno = false;
            return validno;
        }

        private bool ValidirajNazivLeka(LekValidacijaDTO lek, String vrstaOperacije, int indexSelektovanogLeka)
        {
            bool validno = true;
            Regex sablon = new Regex(@"^[0-9a-zA-ZŠĐŽĆČšđžćč\s]+$");
            List<Lek> SviLekovi = skladisteZaLekove.GetAll();
            if (sablon.IsMatch(lek.NazivLeka))
            {
                foreach (Lek l in SviLekovi)
                {
                    if (vrstaOperacije.Equals("dodaj") && lek.NazivLeka.Equals(l.NazivLeka))
                    { //6
                        validacija.IspisiGresku(6);
                        validno = false;
                    }
                    else if (vrstaOperacije.Equals("izmeni"))
                    {
                        if (lek.NazivLeka != SviLekovi[indexSelektovanogLeka].NazivLeka && lek.NazivLeka.Equals(l.NazivLeka))
                        { //6
                            validacija.IspisiGresku(6);
                            validno = false;
                        }
                    }
                }
            }
            return validno;
        }
        private bool ValidirajComboBoxoveLeka(LekValidacijaDTO lek)
        {
            bool validno = true;
            if (lek.VrstaLeka == -1 || lek.KlasaLeka == -1)
            { //7
                validacija.IspisiGresku(7);
                validno = false;
            }
            return validno;
        }

public bool ProveriValidnostLeka(LekValidacijaDTO lek, String DodajIliIzmeni, int selektovaniLek)
        {
            bool validno = true;
            int idGreske = 0;
            if (ValidirajNazivLeka(lek, DodajIliIzmeni, selektovaniLek) == false)
            {
                validno = false;
                idGreske = 1;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), lek.KolicinaLeka) == false)
            {
                validno = false;
                idGreske = 2;
            }

            if (Validiraj(new Regex(@"^[1-9]{1}[0-9]*$"), lek.JacinaLeka) == false)
            {
                validno = false;
                idGreske = 3;
            }

            if (Validiraj(new Regex(@"^[0-9a-zA-ZšđžćčŠĐŽĆČ\s]+$"), lek.ZamenskiLek) == false)
            {
                validno = false;
                idGreske = 4;
            }

            if (Validiraj(new Regex(@"^[0-9a-zA-ZŠĐŽĆČšđžćč,\s]+$"), lek.SastavLeka) == false)
            {
                validno = false;
                idGreske = 5;
            }

            if (ValidirajComboBoxoveLeka(lek) == false)
                validno = false;
            validacija.IspisiGresku(idGreske);
            return validno;
        }

        public Model.Enum.VrstaLeka GetVrstuLeka(int IndexSelektovaneVrsteLeka)
        {
            VrstaLeka vrsta = VrstaLeka.SumecaTableta;
            if (IndexSelektovaneVrsteLeka == 0)
                vrsta = VrstaLeka.Kapsula;
            else if (IndexSelektovaneVrsteLeka == 1)
                vrsta = VrstaLeka.Tableta;
            else if (IndexSelektovaneVrsteLeka == 2)
                vrsta = VrstaLeka.Sirup;
            else if (IndexSelektovaneVrsteLeka == 3)
                vrsta = VrstaLeka.Sprej;
            else if (IndexSelektovaneVrsteLeka == 4)
                vrsta = VrstaLeka.Gel;
            return vrsta;
        }


        public Model.Enum.KlasaLeka GetKlasuLeka(int IndexSelektovaneKlaseLeka)
        {
            KlasaLeka klasa = KlasaLeka.Statin;
            if (IndexSelektovaneKlaseLeka == 0)
                klasa = KlasaLeka.Analgetik;
            else if (IndexSelektovaneKlaseLeka == 1)
                klasa = KlasaLeka.Antipiretik;
            else if (IndexSelektovaneKlaseLeka == 2)
                klasa = KlasaLeka.Antimalarijski_Lek;
            else if (IndexSelektovaneKlaseLeka == 3)
                klasa = KlasaLeka.Antibiotik;
            else if (IndexSelektovaneKlaseLeka == 4)
                klasa = KlasaLeka.Antiseptik;
            else if (IndexSelektovaneKlaseLeka == 5)
                klasa = KlasaLeka.Stabilizator_Raspolozenja;
            else if (IndexSelektovaneKlaseLeka == 6)
                klasa = KlasaLeka.Hormonska_Zamena;
            else if (IndexSelektovaneKlaseLeka == 7)
                klasa = KlasaLeka.Oralni_Kontraceptiv;
            else if (IndexSelektovaneKlaseLeka == 8)
                klasa = KlasaLeka.Stimulant;
            else if (IndexSelektovaneKlaseLeka == 9)
                klasa = KlasaLeka.Trankvilajzer;
            return klasa;
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
            return skladisteZaLekove.GetById(id);
        }
    }
}
