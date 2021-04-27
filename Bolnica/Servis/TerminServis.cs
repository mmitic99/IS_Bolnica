using Bolnica.viewActions;
using Model;
using Repozitorijum;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Servis
{
    public class TerminServis
    {
        public SkladisteZaTermine skladisteZaTermine;
        public SkladisteZaObavestenja skladisteZaObavestenja;
        public static TerminServis instance =null;
        public const int MAX_BR_TERMINA_PRIKAZ =10;

        public static TerminServis getInstance()
        {
            if(instance == null)
            {
                return new TerminServis();
            }
            else
            {
                return instance;
            }
        }



        public TerminServis()
        {
            instance = this;
            skladisteZaObavestenja = SkladisteZaObavestenja.GetInstance();
            skladisteZaTermine = new SkladisteZaTermine();
        }

        public bool ZakaziTermin(Model.Termin termin)
        {
            skladisteZaTermine.Save(termin);

            return true;
        }

        public bool OtkaziTermin(Model.Termin termin)
        {
            skladisteZaTermine.RemoveByID(termin.IDTermina);
            return true;
        }

        public bool IzmeniTermin(Termin termin, string stariIdTermina = null)
        {
            Termin stariTermin;
            if(stariIdTermina!=null)
            {
                stariTermin = SkladisteZaTermine.getInstance().getById(stariIdTermina);
                skladisteZaTermine.RemoveByID(stariIdTermina);

            }
            else
            {
                stariTermin = skladisteZaTermine.getById(termin.IDTermina);
                skladisteZaTermine.RemoveByID(termin.IDTermina);
            }
            bool uspesno = true;
            
            termin.IDTermina = termin.generateRandId();

            Obavestenje obavestenjePacijent = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgPacijenta,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + stariTermin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now,
                Podsetnik = false
                
            };
            skladisteZaObavestenja.Save(obavestenjePacijent);

            Obavestenje obavestenjeLekar = new Obavestenje
            {
                JmbgKorisnika = termin.JmbgLekara,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + stariTermin.DatumIVremeTermina + "" +
                " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            };
            skladisteZaObavestenja.Save(obavestenjeLekar);

            skladisteZaTermine.Save(termin);

            return uspesno;
        }

        public List<Termin> NadjiTermineZaParametre(String jmbgLekara, String jmbgPacijenta, List<DateTime> izabraniDani, TimeSpan pocetak, TimeSpan kraj, int prioritet, String tegobe, Termin termin = null)
        {
            List<DateTime> dani = DobaviMoguceSveDaneZakazivanja(termin);
            List<Termin> moguciTermini = new List<Termin>();
            pocetak = KonvertujMinute(pocetak);

            if (prioritet == 0) //nema prioritet
            {
                moguciTermini = DobaviTermineZaInterval(dani, new TimeSpan(6,0,0), new TimeSpan(20,0,0), jmbgPacijenta, tegobe);
            }
            else if (prioritet == 1)
            {
                moguciTermini = DobaviTermineZaInterval(izabraniDani, pocetak, kraj, jmbgPacijenta, tegobe, jmbgLekara);
                if (moguciTermini.Count < 10)
                    DodajJosTermina(moguciTermini, DobaviTermineZaInterval(dani, new TimeSpan(6, 0, 0), new TimeSpan(20, 0, 0), jmbgPacijenta, tegobe, jmbgLekara));
                
            }
            else if (prioritet == 2)
            {
                    moguciTermini = DobaviTermineZaInterval(izabraniDani, pocetak, kraj, jmbgPacijenta, tegobe, jmbgLekara); //sa lekarom
                    if (moguciTermini.Count < 10)
                    DodajJosTermina(moguciTermini, DobaviTermineZaInterval(izabraniDani, pocetak, kraj, jmbgPacijenta, tegobe));                
            }
            return moguciTermini;
        }

        private void DodajJosTermina(List<Termin> moguciTermini, List<Termin> dodatniTermini)
        {
            int i = 0;
            while (moguciTermini.Count < 10 && i < dodatniTermini.Count)
            {
                if (terminNijeUListi(moguciTermini, dodatniTermini[i]))
                    moguciTermini.Add(dodatniTermini[i]);
                i++;
            }
        }

        private TimeSpan KonvertujMinute(TimeSpan pocetak)
        {
            if (pocetak.TotalMinutes % 30 != 0)
            {
                double dodaj = 30 - (pocetak.TotalMinutes % 30);
                //konverzija da minuti termina pocinju uvek sa brojem koji je deljiv sa 30
                pocetak += TimeSpan.FromMinutes(dodaj);
            }
            return pocetak;
        }

        private List<DateTime> DobaviMoguceSveDaneZakazivanja(Termin termin)
        {
            List<DateTime> dani = new List<DateTime>();
            if (termin == null)
            {
                dani = DobaviSveDaneTriMesecaUnapred();
            }
            else
            {
                dani = nabaviMoguceDanePomeranja(termin.DatumIVremeTermina);
            }
            return dani;
        }

        private List<DateTime> nabaviMoguceDanePomeranja(DateTime datumIVremeTermina)
        {
            List<DateTime> moguciDaniPomeranja = new List<DateTime>();
            datumIVremeTermina = datumIVremeTermina.Date;
            DateTime prvi = datumIVremeTermina.AddDays(-3);
            DateTime poslednji = datumIVremeTermina.AddDays(3);
            while(prvi<poslednji || prvi.Equals(poslednji))
            {
                if (prvi > DateTime.Today) 
                {
                    moguciDaniPomeranja.Add(prvi);
                 }
                prvi = prvi.AddDays(1);
            }
            return moguciDaniPomeranja;
        }

        private List<DateTime> DobaviSveDaneTriMesecaUnapred()
        {
            List<DateTime> moguciDaniZakazivanja = new List<DateTime>();
            DateTime moguciDatum = dobaviSutrasnjiDan();
            while(moguciDatum < dobaviSutrasnjiDan().AddMonths(3))
            {
                moguciDaniZakazivanja.Add(moguciDatum);
                moguciDatum = moguciDatum.AddDays(1);
            }
            return moguciDaniZakazivanja;
        }

        private bool terminNijeUListi(List<Termin> moguciTermini, Termin termin)
        {
            bool nijeUListi = true;
            foreach(Termin t in moguciTermini)
            {
                if(t.IDTermina.Equals(termin.IDTermina))
                {
                    nijeUListi = false;
                    break;
                }
            }
            return nijeUListi;
        }

        private DateTime dobaviSutrasnjiDan()
        {
            DateTime datum = DateTime.Today + new TimeSpan(6,0,0);
            datum = datum.AddDays(1);
            return datum;
        }

        public DateTime dobaviSledeciMoguciTajming(DateTime datumVreme)
        {
            if (datumVreme.Hour < 19 || datumVreme.Minute == 0)
            {
                datumVreme = datumVreme.AddMinutes(30);
            }
            else
            {
                datumVreme = datumVreme.AddDays(1);
                datumVreme = datumVreme.Date + new TimeSpan(6, 0, 0);
            }
            return datumVreme;
        }
        /*
         Funkcija koja nalazi sve termine slobodne u u prosledjeno vreme, ako je prosledjen lekar nalazi se termin samo za tog lekara, ako ne onda za sve lekare
         */
        public List<Termin> nadjiTermineZaTacnoVreme(DateTime vreme, int trajanje, String jmbgLekara = null)
        {
            List<Termin> terminiZaTacnoVreme = new List<Termin>();
            List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();
            foreach (Lekar lekar in lekari)
            {
               if(vreme > DateTime.Today.AddDays(1) 
                   && LekarServis.getInstance().DaLiJeLekarSlobodan(lekar.Jmbg, vreme, trajanje)
                   && (jmbgLekara==null || lekar.Jmbg== jmbgLekara))
               {
                  Termin t = new Termin()
                  {
                      JmbgLekara = lekar.Jmbg,
                      DatumIVremeTermina = vreme,
                      TrajanjeTermina = trajanje
                  };
                      terminiZaTacnoVreme.Add(t);
                    }
                }
            return terminiZaTacnoVreme;     
        }

        public bool OdloziTermin(Termin termin)
        {
            return true;
        }   

        public void RemoveByID(string iDTermina)
        {
            skladisteZaTermine.RemoveByID(iDTermina);
        }

        /*
         * Funkcija koja dobavlja sve moguce termine pregleda u prosledjenom intervalu, za ili ne za prosledjenog lekara
         */
        public List<Termin> DobaviTermineZaInterval( List<DateTime> dani, TimeSpan pocetak, TimeSpan kraj, String jmbgPacijenta = null, String tegobe = null, String jmbgLekara = null)
        {
            List<Termin> moguciBuduciTermini = new List<Termin>();
            foreach(DateTime dan in dani)
             {
                DateTime danMinVreme = dan.Date + pocetak;
                DateTime danMaxVreme = dan.Date + kraj;
                while(danMinVreme.AddMinutes(30) <= danMaxVreme
                    && moguciBuduciTermini.Count<MAX_BR_TERMINA_PRIKAZ)
                { 
                    moguciBuduciTermini.AddRange(napraviPredlogePacijentu(nadjiTermineZaTacnoVreme(danMinVreme, 30, jmbgLekara), jmbgPacijenta, tegobe));
                    danMinVreme = dobaviSledeciMoguciTajming(danMinVreme);
                }
                if (moguciBuduciTermini.Count >= 10) break;
             }
            return moguciBuduciTermini;
        }

 

        /*
         * Personalizovani termini, dodat je pacijent, tip pregleda, id i trajanje
         * u slucaju da imamo pacijenta kome trazimo pregled, pretrazujemo da li je on slobodan i filtriramo nadjen termine za njega
         */
        public List<Termin> napraviPredlogePacijentu(List<Termin> tajminzi, String jmbgPacijenta, String tegobe)
        {
            if (jmbgPacijenta != null)
            {
                List<Termin> personalizovaniPredlozeniTermini = new List<Termin>();
                foreach (Termin termin in tajminzi)
                {
                    if (PacijentServis.getInstance().DaLiJePacijentSlobodan(jmbgPacijenta, termin.DatumIVremeTermina, (int)termin.TrajanjeTermina))
                    {
                        termin.JmbgPacijenta = jmbgPacijenta;
                        termin.opisTegobe = tegobe;
                        termin.VrstaTermina = Model.Enum.VrstaPregleda.Pregled;
                        termin.IDTermina = termin.generateRandId();
                        personalizovaniPredlozeniTermini.Add(termin);
                    }
                }
                return personalizovaniPredlozeniTermini;
            }
            else
            {
                return tajminzi;
            }

        }

        public bool ProveriTermin(Model.Termin termin)
        {
            // TODO: implement
            return false;
        }

        public List<Termin> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Termin termin)
        {
            // TODO: implement
        }

        public void SaveAll(List<Termin> termini)
        {
            // TODO: implement
        }



        internal IEnumerable GetBuduciTerminPacLekar()
        {
            return skladisteZaTermine.GetBuduciTerminPacLekar();
        }
    }
}