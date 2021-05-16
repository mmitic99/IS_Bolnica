using Bolnica.DTOs;
using Model;
using Repozitorijum;
using System;
using System.Collections;
using System.Collections.Generic;
using Bolnica.model;
using Bolnica.Servis;

namespace Servis
{
    public class TerminServis
    {
        public SkladisteZaTermine skladisteZaTermine;
        public SkladisteZaObavestenja skladisteZaObavestenja;
        public static TerminServis instance = null;
        public const int MAX_BR_TERMINA_PRIKAZ = 10;
        public const int MAX_BR_TERMINA_PRIKAZ_SEKRETAR = 50;

        public static TerminServis getInstance()
        {
            if (instance == null)
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
            termin.IdProstorije = ProstorijeServis.GetInstance().GetPrvaPogodna(termin);
            skladisteZaTermine.Save(termin);

            return true;
        }

        public bool OtkaziTermin(String IdTermina)
        {
            skladisteZaTermine.RemoveByID(IdTermina);
            return true;
        }

        public bool IzmeniTermin(Termin termin, string stariIdTermina = null)
        {
            Termin stariTermin;
            if (stariIdTermina != null)
            {
                stariTermin = SkladisteZaTermine.getInstance().getById(stariIdTermina);
                skladisteZaTermine.RemoveByID(stariIdTermina);
            }
            else
            {
                stariTermin = skladisteZaTermine.getById(termin.IDTermina);
                skladisteZaTermine.RemoveByID(termin.IDTermina);
            }
            termin.IDTermina = termin.generateRandId();
            skladisteZaTermine.Save(termin);

            GenerisiObavestenjaZaIzmenuTermina(termin, stariTermin);
            return true;
        }

        private void GenerisiObavestenjaZaIzmenuTermina(Termin termin, Termin stariTermin)
        {
            skladisteZaObavestenja.Save(new Obavestenje
            {
                JmbgKorisnika = termin.JmbgPacijenta,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " +
                          stariTermin.DatumIVremeTermina + "" +
                          " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now,
                Podsetnik = false
            });

            skladisteZaObavestenja.Save(new Obavestenje
            {
                JmbgKorisnika = termin.JmbgLekara,
                Naslov = "Izmena zakazanog termina",
                Sadrzaj = "Poštovani, obaveštavamo vas da je termin koji ste imali zakazan za " + stariTermin.DatumIVremeTermina + "" +
                          " je pomeren na " + termin.DatumIVremeTermina + ".",
                VremeObavestenja = DateTime.Now
            });
        }

        internal List<Termin> NadjiSveTerminePacijentaIzBuducnosti(string jmbgKorisnkika)
        {
            List<Termin> sviTerminiKorisnika = SkladisteZaTermine.getInstance().getByJmbg(jmbgKorisnkika); //vraca samo za pazijenta
            List<Termin> sviTerminiKorisnikaIzBuducnosti = new List<Termin>();
            foreach (Termin t in sviTerminiKorisnika)
            {
                if (t.DatumIVremeTermina > DateTime.Now)
                {
                    sviTerminiKorisnikaIzBuducnosti.Add(t);
                }
            }
            return sviTerminiKorisnikaIzBuducnosti;
        }

        internal List<Termin> getByJmbgPacijenta(string jmbg)
        {
            return SkladisteZaTermine.getInstance().getByJmbg(jmbg);
        }

        internal List<Termin> NadjiTermineZaParametre(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
        {
            parametri.Pocetak = KonvertujMinute(parametri.Pocetak);
            parametri.SviMoguciDaniZakazivanja = DobaviMoguceSveDaneZakazivanja(parametri.PrethodnoZakazaniTermin);
            return nadjiTerminePoPriritetu(parametri);
        }

        public Termin GetById(string idTermina)
        {
            return skladisteZaTermine.getById(idTermina);
        }

        public List<Termin> nadjiTerminePoPriritetu(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
        {
            List<Termin> moguciTermini = new List<Termin>();
            switch (parametri.Prioritet)
            {
                case 0:
                    moguciTermini = NadjiSveTermineBezPriotiteta(parametri);
                    break;
                case 1:
                    moguciTermini = NadjiSveTermineSaPriritetomIzabranogLekara(parametri);
                    break;
                case 2:
                    moguciTermini = NadjiSveTermineSaPriritetomIzabranogVremena(parametri);
                    break;
            }
            return moguciTermini;
        }

        public List<Termin> NadjiSveTermineBezPriotiteta(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
        {
            List<Termin> moguciTermini = new List<Termin>();
            ParametriZaTermineUIntervaluDTO parametriZaPotraguTerminaUNekomIntervalu = new ParametriZaTermineUIntervaluDTO
            {
                jmbgPacijenta = parametri.JmbgPacijenta,
                OpisTegobe = parametri.tegobe,
                Pocetak = PocetakRadnogVremena(),
                Kraj = KrajRadnogVremena(),
                Trajanje = parametri.trajanjeUMinutama,
                vrstaPregleda = parametri.vrstaTermina,
                SviDani = parametri.SviMoguciDaniZakazivanja
            };
            moguciTermini = DobaviTermineZaInterval(parametriZaPotraguTerminaUNekomIntervalu);


            return moguciTermini;
        }

        public List<Termin> NadjiSveTermineSaPriritetomIzabranogLekara(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
        {
            List<Termin> moguciTermini = new List<Termin>();
            ParametriZaTermineUIntervaluDTO parametriZaPotraguTerminaUNekomIntervalu = new ParametriZaTermineUIntervaluDTO()
            {
                jmbgPacijenta = parametri.JmbgPacijenta,
                jmbgLekara = parametri.JmbgLekara,
                OpisTegobe = parametri.tegobe,
                Pocetak = parametri.Pocetak,
                vrstaPregleda = parametri.vrstaTermina,
                Kraj = parametri.Kraj,
                SviDani = parametri.IzabraniDani,
                Trajanje = parametri.trajanjeUMinutama,
                sekretar = parametri.sekretar

            };
            moguciTermini = DobaviTermineZaInterval(parametriZaPotraguTerminaUNekomIntervalu);
            if (moguciTermini.Count < (parametri.sekretar ? MAX_BR_TERMINA_PRIKAZ_SEKRETAR : MAX_BR_TERMINA_PRIKAZ)) //ako nema dovoljno termina u tom izabranom intervalu pretrazuje dalje termine za izabranog lekara
            {
                parametriZaPotraguTerminaUNekomIntervalu.Pocetak = PocetakRadnogVremena();
                parametriZaPotraguTerminaUNekomIntervalu.Kraj = KrajRadnogVremena();
                parametriZaPotraguTerminaUNekomIntervalu.SviDani = parametri.SviMoguciDaniZakazivanja;
                DodajJosTermina(moguciTermini, DobaviTermineZaInterval(parametriZaPotraguTerminaUNekomIntervalu));
            }

            return moguciTermini;
        }

        public List<Termin> NadjiHitanTermin(string jmbgPacijenta, string vrstaSpecijalizacije)
        {
            List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();
            List<Termin> moguciTermini = new List<Termin>();
            List<DateTime> vremena = DobaviVremeZaHitanTermin();

            foreach (DateTime datumIVreme in vremena)
            {
                foreach (Lekar lekar in lekari)
                {
                    if (lekar.Specijalizacija != null && lekar.Specijalizacija.VrstaSpecijalizacije.Equals(vrstaSpecijalizacije))
                    {
                        if (LekarRadi(lekar.Jmbg, datumIVreme))
                        {
                            Termin termin = new Termin
                            {
                                DatumIVremeTermina = datumIVreme,
                                JmbgLekara = lekar.Jmbg,
                                brojSobe = (SkladisteZaProstorije.GetInstance().getById(lekar.IdOrdinacija)).BrojSobe,
                                JmbgPacijenta = null
                            };

                            Termin terminDatumVreme = GetTerminZaDatumILekara(datumIVreme, lekar.Jmbg);

                            if (terminDatumVreme != null)
                            {
                                termin.JmbgPacijenta = terminDatumVreme.JmbgPacijenta;
                                termin.IDTermina = terminDatumVreme.IDTermina;
                            }

                            moguciTermini.Add(termin);
                        }
                    }
                }
            }
            List<Termin> sortiraniTermini = new List<Termin>();
            List<Termin> terminiSaPomeranjem = new List<Termin>();

            foreach (Termin termin in moguciTermini)
            {
                if (termin.JmbgPacijenta == null)
                {
                    sortiraniTermini.Add(termin);
                }
                else
                {
                    terminiSaPomeranjem.Add(termin);
                }
            }
            foreach (Termin termin in terminiSaPomeranjem)
            {
                sortiraniTermini.Add(termin);
            }
            return sortiraniTermini;
        }

        private static List<DateTime> DobaviVremeZaHitanTermin()
        {
            List<DateTime> vremena = new List<DateTime>();

            if (DateTime.Now.Minute % 30 == 0)
            {
                vremena.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0));
                vremena.Add(DateTime.Now.AddMinutes(30));
            }
            else
            {
                double dodaj = 30 - (DateTime.Now.Minute % 30);
                DateTime pocetak = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                pocetak = pocetak.AddMinutes(dodaj);
                vremena.Add(pocetak);
                vremena.Add(pocetak.AddMinutes(30));
            }

            return vremena;
        }

        public List<Termin> NadjiSveTermineSaPriritetomIzabranogVremena(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
        {
            List<Termin> moguciTermini = new List<Termin>();
            ParametriZaTermineUIntervaluDTO parametriZaPotraguTerminaUNekomIntervalu = new ParametriZaTermineUIntervaluDTO()
            {
                jmbgPacijenta = parametri.JmbgPacijenta,
                jmbgLekara = parametri.JmbgLekara,
                OpisTegobe = parametri.tegobe,
                vrstaPregleda = parametri.vrstaTermina,
                Pocetak = parametri.Pocetak,
                Kraj = parametri.Kraj,
                SviDani = parametri.IzabraniDani,
                Trajanje = parametri.trajanjeUMinutama,
                sekretar = parametri.sekretar
            };
            moguciTermini = DobaviTermineZaInterval(parametriZaPotraguTerminaUNekomIntervalu); //sa lekarom
            if (moguciTermini.Count < (parametri.sekretar ? MAX_BR_TERMINA_PRIKAZ_SEKRETAR : MAX_BR_TERMINA_PRIKAZ)) //ako nema dovoljno termina za izabranom lekaru za taj interval dodaje se jos termina bez lekara
            {
                parametriZaPotraguTerminaUNekomIntervalu.jmbgLekara = null;
                DodajJosTermina(moguciTermini, DobaviTermineZaInterval(parametriZaPotraguTerminaUNekomIntervalu));

            }
            return moguciTermini;
        }

        /*
    * Funkcija koja dobavlja sve moguce termine pregleda u prosledjenom intervalu, za ili ne za prosledjenog lekara
    */
        private List<Termin> DobaviTermineZaInterval(ParametriZaTermineUIntervaluDTO parametri)
        {
            ParametriZaNalazenjeTerminaZaTacnoVreme parametriTacnoVreme = new ParametriZaNalazenjeTerminaZaTacnoVreme();
            parametriTacnoVreme.JmbgLekara = parametri.jmbgLekara;
            parametriTacnoVreme.trajanjeUMinutama = parametri.Trajanje;
            parametriTacnoVreme.vrstaPregleda = parametri.vrstaPregleda;
            List<Termin> moguciBuduciTermini = new List<Termin>();
            foreach (DateTime dan in parametri.SviDani)
            {
                DateTime danMinVreme = dan.Date + parametri.Pocetak;
                DateTime danMaxVreme = dan.Date + parametri.Kraj;
                while (danMinVreme.AddMinutes(parametri.Trajanje) <= danMaxVreme
                    && moguciBuduciTermini.Count < (parametri.sekretar ? MAX_BR_TERMINA_PRIKAZ_SEKRETAR : MAX_BR_TERMINA_PRIKAZ))
                {
                    parametriTacnoVreme.TacnoVreme = danMinVreme;
                    ParamsToPickAppointmentsAppropriateForSpecificPatientDTO parametriZaPacijenta = new ParamsToPickAppointmentsAppropriateForSpecificPatientDTO(
                        parametri.jmbgPacijenta, parametri.vrstaPregleda, nadjiTermineZaTacnoVreme(parametriTacnoVreme), parametri.OpisTegobe);
                    moguciBuduciTermini.AddRange(napraviPredlogePacijentu(parametriZaPacijenta));
                    danMinVreme = dobaviSledeciMoguciTajming(danMinVreme);
                }
                if (moguciBuduciTermini.Count >= (parametri.sekretar ? MAX_BR_TERMINA_PRIKAZ_SEKRETAR : MAX_BR_TERMINA_PRIKAZ)) break;
            }
            return moguciBuduciTermini;
        }

        /*
         Funkcija koja nalazi sve termine slobodne u u prosledjeno vreme, ako je prosledjen lekar nalazi se termin samo za tog lekara, ako ne onda za sve lekare
         */
        public List<Termin> nadjiTermineZaTacnoVreme(ParametriZaNalazenjeTerminaZaTacnoVreme parametri)
        {
            List<Termin> terminiZaTacnoVreme = new List<Termin>();
            List<Lekar> lekari = SkladisteZaLekara.GetInstance().GetAll();
            foreach (Lekar lekar in lekari)
            {
                if (parametri.TacnoVreme > DateTime.Today.AddDays(1)
                    && (parametri.JmbgLekara == null || lekar.Jmbg == parametri.JmbgLekara)
                    && LekarServis.getInstance().DaLiJeLekarSlobodan(new ParamsToCheckAvailabilityOfDoctorDTO(parametri.JmbgLekara, parametri.TacnoVreme, parametri.trajanjeUMinutama))
                    && ProstorijeServis.GetInstance().PostojiSlobodnaProstorija(new ParamsToCheckAvailabilityOfRoomDTO(parametri.vrstaPregleda, parametri.TacnoVreme, parametri.trajanjeUMinutama)))
                {
                    Termin t = new Termin()
                    {
                        JmbgLekara = lekar.Jmbg,
                        DatumIVremeTermina = parametri.TacnoVreme,
                        TrajanjeTermina = parametri.trajanjeUMinutama
                    };
                    terminiZaTacnoVreme.Add(t);
                }
            }
            return terminiZaTacnoVreme;
        }

        /*
        * Personalizovani termini, dodat je pacijent, tip pregleda, id i trajanje
        * u slucaju da imamo pacijenta kome trazimo pregled, pretrazujemo da li je on slobodan i filtriramo nadjen termine za njega
        */
        public List<Termin> napraviPredlogePacijentu(ParamsToPickAppointmentsAppropriateForSpecificPatientDTO parametri)
        {
            if (parametri.Id != null)
            {
                List<Termin> personalizovaniPredlozeniTermini = new List<Termin>();
                foreach (Termin termin in parametri.PossibleAppointmentsTimings)
                {
                    if (PacijentServis.GetInstance().DaLiJePacijentSlobodan(new ParamsToCheckAvailabilityOfPatientDTO(parametri.Id, termin.DatumIVremeTermina, (int)termin.TrajanjeTermina)))
                    {
                        termin.JmbgPacijenta = parametri.Id;
                        termin.opisTegobe = parametri.SymptomsOfIllness;
                        termin.VrstaTermina = parametri.AppointmentKind;
                        termin.IDTermina = termin.generateRandId();
                        personalizovaniPredlozeniTermini.Add(termin);
                    }
                }
                return personalizovaniPredlozeniTermini;
            }
            else
            {
                return parametri.PossibleAppointmentsTimings;
            }

        }
        public TimeSpan PocetakRadnogVremena()
        {
            return new TimeSpan(6, 0, 0);
        }

        public TimeSpan KrajRadnogVremena()
        {
            return new TimeSpan(20, 0, 0);
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

        public DateTime? DobaviPrviMoguciDanZakazivanja(Termin termin)
        {
            return DobaviMoguceSveDaneZakazivanja(termin)[0];
        }

        public DateTime? DobaviPPoslednjiMogiDanZakazivanja(Termin termin)
        {
            List<DateTime> sviMoguciDani = DobaviMoguceSveDaneZakazivanja(termin);
            return sviMoguciDani[sviMoguciDani.Count - 1];
        }

        public List<DateTime> DobaviMoguceSveDaneZakazivanja(Termin termin)
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
            while (prvi < poslednji || prvi.Equals(poslednji))
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
            while (moguciDatum < dobaviSutrasnjiDan().AddMonths(3))
            {
                moguciDaniZakazivanja.Add(moguciDatum);
                moguciDatum = moguciDatum.AddDays(1);
            }
            return moguciDaniZakazivanja;
        }

        private bool terminNijeUListi(List<Termin> moguciTermini, Termin termin)
        {
            bool nijeUListi = true;
            foreach (Termin t in moguciTermini)
            {
                if (t.IDTermina.Equals(termin.IDTermina))
                {
                    nijeUListi = false;
                    break;
                }
            }
            return nijeUListi;
        }

        private DateTime dobaviSutrasnjiDan()
        {
            DateTime datum = DateTime.Today + new TimeSpan(6, 0, 0);
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

        public bool OdloziTermin(Termin termin)
        {
            return true;
        }

        public void RemoveByID(string iDTermina)
        {
            skladisteZaTermine.RemoveByID(iDTermina);
        }

        public Termin GetTerminZaDatumILekara(DateTime datumIVreme, string jmbgLekara)
        {
            foreach (Termin termin in skladisteZaTermine.GetAll())
            {
                if (termin.DatumIVremeTermina.Equals(datumIVreme) && termin.JmbgLekara.Equals(jmbgLekara))
                {
                    return termin;
                }
            }
            return null;
        }

        private bool LekarRadi(string jmbgLekara, DateTime datumIVreme)
        {
            bool radi = false;
            foreach (RadnoVreme radnoVreme in radnoVremeServis.GetByJmbgAkoRadi(jmbgLekara))
            {
                if (datumIVreme.Date >= radnoVreme.DatumIVremePocetka.Date &&
                    datumIVreme.AddMinutes(30).Date <=
                    radnoVreme.DatumIVremeZavrsetka.Date &&
                    datumIVreme.TimeOfDay >= radnoVreme.DatumIVremePocetka.TimeOfDay &&
                    datumIVreme.AddMinutes(30).TimeOfDay <=
                    radnoVreme.DatumIVremeZavrsetka.TimeOfDay)
                {
                    radi = true;
                    break;
                }
            }

            return radi;
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


        private RadnoVremeServis radnoVremeServis = new RadnoVremeServis();
    }
}