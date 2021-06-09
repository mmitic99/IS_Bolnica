using Bolnica.DTOs;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Bolnica.model;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.Servis;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using TextAlignment = iText.Layout.Properties.TextAlignment;

namespace Servis
{
    public class TerminServis
    {
        public ISkladisteZaTermine skladisteZaTermine;
        public ISkladisteZaObavestenja skladisteZaObavestenja;
        public ISkladisteZaProstorije skladisteZaProstorije;
        public ISkladisteZaLekara skladisteZaLekara;
        public static TerminServis instance = null;
        private PacijentServis PacijentServis;
        private ProstorijeServis ProstorijeServis;
        public const int MAX_BR_TERMINA_PRIKAZ = 10;
        public const int MAX_BR_TERMINA_PRIKAZ_SEKRETAR = 50;

        public static TerminServis getInstance()
        {
            if (instance == null)
            {
                instance = new TerminServis();
            }
            return instance;
        }

        public TerminServis()
        {
            instance = this;
            skladisteZaObavestenja = SkladisteZaObavestenjaXml.GetInstance();
            skladisteZaTermine = new SkladisteZaTermineXml();
            skladisteZaProstorije = new SkladisteZaProstorijeXml();
            skladisteZaLekara = SkladisteZaLekaraXml.GetInstance();
            PacijentServis = new PacijentServis();
            ProstorijeServis = new ProstorijeServis();
        }

        public bool ZakaziTermin(Termin termin)
        {
            termin.IdProstorije = ProstorijeServis.GetPrvaPogodna(termin);
            skladisteZaTermine.Save(termin);
            return true;
        }

        public bool OtkaziTermin(String IdTermina)
        {
            skladisteZaTermine.RemoveById(IdTermina);
            return true;
        }

        public bool IzmeniTermin(Termin termin, string stariIdTermina = null)
        {
            Termin stariTermin;
            if (stariIdTermina != null)
            {
                stariTermin = skladisteZaTermine.GetById(stariIdTermina);
                skladisteZaTermine.RemoveById(stariIdTermina);
            }
            else
            {
                stariTermin = skladisteZaTermine.GetById(termin.IDTermina);
                skladisteZaTermine.RemoveById(termin.IDTermina);
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
                VremeObavestenja = DateTime.Now
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

        public Dictionary<DateTime, String> getNotesForTheMonth(DateTime targetDate, string jmbgPacijenta)
        {
            List<Termin> terminiUProsledjenomMesecu = getTerminiUMesecu(targetDate, jmbgPacijenta);
            Dictionary<DateTime, String> beleskeZaSvakiDan = new Dictionary<DateTime, string>();
            foreach(Termin termin in terminiUProsledjenomMesecu)
            {
                if (!beleskeZaSvakiDan.ContainsKey(termin.DatumIVremeTermina.Date))
                {
                    beleskeZaSvakiDan.Add(termin.DatumIVremeTermina.Date ,termin.DatumIVremeTermina.ToString("HH:mm") + " " + termin.VrstaTermina +" "+ termin.lekar + ProstorijeServis.GetIspisSobe(termin.IdProstorije) + "\n");
                }
                else
                {
                    String beleska = beleskeZaSvakiDan[termin.DatumIVremeTermina.Date];
                    beleska = beleska + termin.DatumIVremeTermina.ToString("HH:mm") + " " + termin.VrstaTermina +" "+ termin.lekar+ProstorijeServis.GetIspisSobe(termin.IdProstorije)+ "\n";
                    beleskeZaSvakiDan.Remove(termin.DatumIVremeTermina.Date);
                    beleskeZaSvakiDan.Add(termin.DatumIVremeTermina.Date, beleska);
                }
            }
            return beleskeZaSvakiDan;
        }

        public int DobaviBrojZakazanihTerminaPacijentaIzBuducnosti(string jmbg)
        {
            return DobaviSveTerminePacijentaIzBuducnosti(jmbg).Count;
        }

        private List<Termin> getTerminiUMesecu(DateTime targetDate, string jmbgPacijenta)
        {
            return GetByJmbgPacijentaVremenskiPeriod(targetDate, targetDate.AddMonths(1).Date.AddSeconds(-1), jmbgPacijenta);
        }

        public List<Termin> DobaviSveTerminePacijentaIzBuducnosti(string jmbgKorisnkika)
        {
            List<Termin> sviTerminiKorisnika = skladisteZaTermine.GetByJmbgPacijenta(jmbgKorisnkika); //vraca samo za pazijenta
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

        public List<Termin> GetByJmbgPacijenta(string jmbg)
        {
            return skladisteZaTermine.GetByJmbgPacijenta(jmbg);
        }

        public List<Termin> GetByJmbgPacijentaVremenskiPeriod(DateTime pocetak, DateTime kraj, string jmbgPacijenta)
        {
            List<Termin> sviTerminiPacijenta = GetByJmbgPacijenta(jmbgPacijenta);
            List<Termin> filtriraniTerminiPacijenta = new List<Termin>();
            foreach(Termin t in sviTerminiPacijenta)
            {
                if(t.DatumIVremeTermina>=pocetak && t.DatumIVremeTermina<=kraj)
                {
                    filtriraniTerminiPacijenta.Add(t);
                }
            }
            return filtriraniTerminiPacijenta;
        }


        public List<Termin> NadjiTermineZaParametre(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
        {
            parametri.Pocetak = KonvertujMinuteNaBrojDeljivSa30(parametri.Pocetak);
            parametri.SviMoguciDaniZakazivanja = DobaviMoguceSveDaneZakazivanja(parametri.PrethodnoZakazaniTermin);
            return NadjiTerminePoPriritetu(parametri);
        }

        public Termin GetById(string idTermina)
        {
            return skladisteZaTermine.GetById(idTermina);
        }

        public List<Termin> NadjiTerminePoPriritetu(ParametriZaTrazenjeTerminaKlasifikovanoDTO parametri)
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
            //ako nema dovoljno termina u tom izabranom intervalu pretrazuje dalje termine za izabranog lekara
            if (moguciTermini.Count < (parametri.sekretar ? MAX_BR_TERMINA_PRIKAZ_SEKRETAR : MAX_BR_TERMINA_PRIKAZ))
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
            List<Termin> moguciTermini = DobaviMoguceHitneTermine(vrstaSpecijalizacije);
            List<Termin> sortiraniTermini = SortirajHitneTermine(moguciTermini);
            return sortiraniTermini;
        }

        private List<Termin> SortirajHitneTermine(List<Termin> moguciTermini)
        {
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
            sortiraniTermini.AddRange(terminiSaPomeranjem);
            return sortiraniTermini;
        }

        private List<Termin> DobaviMoguceHitneTermine(string vrstaSpecijalizacije)
        {
            List<Termin> moguciTermini = new List<Termin>();
            List<DateTime> vremena = DobaviVremenaZaHitneTermine();
            foreach (DateTime datumIVreme in vremena)
            {
                moguciTermini.AddRange(DobaviHitneTermineZaSveLekare(vrstaSpecijalizacije, datumIVreme));
            }
            return moguciTermini;
        }

        private IEnumerable<Termin> DobaviHitneTermineZaSveLekare(string vrstaSpecijalizacije, DateTime datumIVreme)
        {
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            List<Termin> moguciTermini = new List<Termin>();
            foreach (Lekar lekar in lekari)
            {
                if (lekar.Specijalizacija != null && lekar.Specijalizacija.VrstaSpecijalizacije.Equals(vrstaSpecijalizacije)
                                                  && DaLiLekarRadi(lekar.Jmbg, datumIVreme))
                {
                    moguciTermini.Add(DobaviHitanTerminZaLekara(datumIVreme, lekar));
                }
            }
            return moguciTermini;
        }

        private Termin DobaviHitanTerminZaLekara(DateTime datumIVreme, Lekar lekar)
        {
            Termin termin = new Termin
            {
                DatumIVremeTermina = datumIVreme,
                JmbgLekara = lekar.Jmbg,
                brojSobe = (skladisteZaProstorije.GetById(lekar.IdOrdinacija)).BrojSobe,
                JmbgPacijenta = null
            };
            Termin terminDatumVreme = GetTerminZaDatumILekara(datumIVreme, lekar.Jmbg);
            if (terminDatumVreme == null) return termin;

            termin.JmbgPacijenta = terminDatumVreme.JmbgPacijenta;
            termin.IDTermina = terminDatumVreme.IDTermina;
            return termin;
        }

        private static List<DateTime> DobaviVremenaZaHitneTermine()
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
            List<Lekar> lekari = skladisteZaLekara.GetAll();
            foreach (Lekar lekar in lekari)
            {
                if (parametri.TacnoVreme > DateTime.Today.AddDays(1)
                    && (parametri.JmbgLekara == null || lekar.Jmbg == parametri.JmbgLekara)
                    && LekarServis.getInstance().DaLiJeLekarSlobodan(new ParamsToCheckAvailabilityOfDoctorDTO(lekar.Jmbg, parametri.TacnoVreme, parametri.trajanjeUMinutama))
                    && ProstorijeServis.PostojiSlobodnaProstorija(new ParamsToCheckAvailabilityOfRoomDTO(parametri.vrstaPregleda, parametri.TacnoVreme, parametri.trajanjeUMinutama)))
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
                    if (PacijentServis.DaLiJePacijentSlobodan(new ParamsToCheckAvailabilityOfPatientDTO(parametri.Id, termin.DatumIVremeTermina, (int)termin.TrajanjeTermina)))
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

        private TimeSpan KonvertujMinuteNaBrojDeljivSa30(TimeSpan pocetak)
        {
            if (pocetak.TotalMinutes % 30 != 0)
            {
                double dodaj = 30 - (pocetak.TotalMinutes % 30);
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

        public void RemoveByID(string iDTermina)
        {
            skladisteZaTermine.RemoveById(iDTermina);
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

        private bool DaLiLekarRadi(string jmbgLekara, DateTime datumIVreme)
        {
            bool lekarRadi = false;
            foreach (RadnoVreme radnoVreme in radnoVremeServis.GetByJmbgAkoRadi(jmbgLekara))
            {
                if (LekarRadi(datumIVreme, radnoVreme))
                {
                    lekarRadi = true;
                    break;
                }
            }
            return lekarRadi;
        }

        private static bool LekarRadi(DateTime datumIVreme, RadnoVreme radnoVreme)
        {
            return datumIVreme.Date >= radnoVreme.DatumIVremePocetka.Date &&
                   datumIVreme.AddMinutes(30).Date <= radnoVreme.DatumIVremeZavrsetka.Date &&
                   datumIVreme.TimeOfDay >= radnoVreme.DatumIVremePocetka.TimeOfDay &&
                   datumIVreme.AddMinutes(30).TimeOfDay <= radnoVreme.DatumIVremeZavrsetka.TimeOfDay;
        }

        public List<Termin> GetAll()
        {
            return skladisteZaTermine.GetAll();
        }

        public void Save(Termin termin)
        {
            skladisteZaTermine.Save(termin);
        }

        public void SaveAll(List<Termin> termini)
        {
            skladisteZaTermine.SaveAll(termini);
        }

        public IEnumerable GetBuduciTerminPacLekar()
        {
            return skladisteZaTermine.GetBuduciTerminPacLekar();
        }
        public List<Termin> GetByDateForLekar(DateTime datum, String jmbgLekara)
        {
            return skladisteZaTermine.GetByDateForLekar(datum, jmbgLekara);
        }


        private RadnoVremeServis radnoVremeServis = new RadnoVremeServis();

        //HCI zahtev:
        public string GenerisiIzvestaj(DateTime datumPocetka, DateTime datumZavrsetka)
        {
            string putanjaIzvestaja = "..\\..\\..\\IzvestajSekretar.pdf";

            try
            {
                PdfWriter writer = new PdfWriter(putanjaIzvestaja);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                PdfFont normalnFont = PdfFontFactory.CreateFont("c:/windows/fonts/arial.ttf", "Identity-H", true);

                document.SetFont(normalnFont);

                Paragraph header = new Paragraph("Zakazani pregledi i operacije u periodu: " + datumPocetka.Date.ToString("dd.MM.yyyy") +
                                                 " - " + datumZavrsetka.Date.ToString("dd.MM.yyyy"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(18);
                document.Add(header);
                document.Add(new Paragraph("\n"));

                Table tabela = new Table(5).SetFontSize(12);

                tabela.AddCell(new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold()
                    .Add(new Paragraph("Datum i vreme početka")));
                tabela.AddCell(new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold()
                    .Add(new Paragraph("Vrsta")));
                tabela.AddCell(new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold()
                    .Add(new Paragraph("Pacijent")));
                tabela.AddCell(new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold()
                    .Add(new Paragraph("Lekar")));
                tabela.AddCell(new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold()
                    .Add(new Paragraph("Broj sobe")));

                foreach (Termin termin in GetTermineUIntervalu(datumPocetka, datumZavrsetka))
                {
                    tabela.AddCell(new Cell(1, 1).Add(new Paragraph(termin.DatumIVremeTermina.ToString("dd.MM.yyyy HH:mm"))));
                    tabela.AddCell(new Cell(1, 1).Add(new Paragraph(termin.VrstaTermina.ToString())));
                    tabela.AddCell(new Cell(1, 1).Add(new Paragraph(termin.pacijent)));
                    tabela.AddCell(new Cell(1, 1).Add(new Paragraph(termin.lekar)));
                    tabela.AddCell(new Cell(1, 1).Add(new Paragraph(termin.brojSobe)));

                }
                document.Add(tabela);

                document.Add(new Paragraph("\n\n" + "Datum i vreme:"));
                document.Add(new Paragraph(DateTime.Now.ToString("dd.MM.yyyy HH:mm")));

                document.Close();

                return Path.GetFullPath(putanjaIzvestaja);
            }
            catch (IOException e)
            {
                return "otvoren";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        private List<Termin> GetTermineUIntervalu(DateTime datumPocetka, DateTime datumZavrsetka)
        {
            List<Termin> termini = new List<Termin>();
            foreach (Termin termin in GetAll())
            {
                if (termin.DatumIVremeTermina >= datumPocetka && termin.DatumIVremeTermina <= datumZavrsetka)
                {
                    termini.Add(termin);
                }
            }
            return termini.OrderBy(termin => termin.DatumIVremeTermina).ToList();
        }

        public List<Termin> GetByJmbgLekar(string tJmbgLekara)
        {
            return skladisteZaTermine.GetByJmbgLekar(tJmbgLekara);
        }
    }
    
}