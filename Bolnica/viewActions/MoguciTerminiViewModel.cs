using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.DTOs;
using Bolnica.model;
using System.Collections.ObjectModel;
using Kontroler;
using Bolnica.view;
using Bolnica.view.PacijentView;

namespace Bolnica.viewActions
{
    public class MoguciTerminiViewModel : BindableBase
    {
        public static MoguciTermini instance = null;
        private MoguciTerminiViewModel ViewModel;
        private MainViewModel MainViewModel;
        private TerminKontroler TerminKontroler;
        private KorisnickeAktivnostiPacijentaKontroler KorisnickeAktivnostiPacijentaKontroler;
        public string pozivaoc { get; set; }
        public string jmbg { get; set; }
        private bool demo;
        public const int MAX_BROJ_ZAKAZANIH = 5;
        private ObservableCollection<TerminDTO> terminiZaPrikazivanje;
        public ObservableCollection<TerminDTO> TerminiZaPrikazivanje
        {
            get
            {
                return terminiZaPrikazivanje;
            }
            set
            {
                terminiZaPrikazivanje = value;
                OnPropertyChanged("TerminiZaPrikazivanje");
            }
        }
        private TerminDTO izabraniTermin;
        public TerminDTO IzabraniTermin
        {
            get
            {
                return izabraniTermin;
            }
            set
            {
                izabraniTermin = value;
                OnPropertyChanged("IzabraniTermin");
            }
        }
        private int indeksIzabranogTermina;
        public int IndeksIzabranogTermina
        {
            get
            {
                return indeksIzabranogTermina;
            }
            set
            {
                indeksIzabranogTermina = value;
                OnPropertyChanged("IndeksIzabranogTermina");
            }
        }
        private int ivice;
        public int Ivice
        {
            get
            {
                return ivice;
            }
            set
            {
                ivice = value;
                OnPropertyChanged("Ivice");
            }
        }
        public MyICommand Zakazi { get; set; }
        public RelayCommand Nazad { get; set; }
    
        public MoguciTerminiViewModel(List<TerminDTO> terminiZaPrikazivanje = null, string pocetna = null, string jmbg = null)
        {
            MainViewModel = MainViewModel.getInstance();
            this.TerminKontroler = new TerminKontroler();
            this.KorisnickeAktivnostiPacijentaKontroler = new KorisnickeAktivnostiPacijentaKontroler();

            this.jmbg = jmbg;
            if(terminiZaPrikazivanje!=null)
            {
                this.terminiZaPrikazivanje = new ObservableCollection<TerminDTO>(terminiZaPrikazivanje);
            } 
            else
            {
                this.terminiZaPrikazivanje = new ObservableCollection<TerminDTO>();
            }
            this.pozivaoc = pocetna;
            this.ivice = 1;
            this.Zakazi = new MyICommand(ZakaziTermin);
            this.Nazad = new RelayCommand(o =>
            {
                var s = MainViewModel.CurrentView;
                MainViewModel.CurrentView = MainViewModel.PreviousView;
                MainViewModel.PreviousView = s;
            }
            );
        }

        public MoguciTerminiViewModel(bool v)
        {
            this.demo = v;
            this.ivice = 1;
            this.TerminiZaPrikazivanje = NapraviRandomTerminiZaPrikazivanje();
        }
        public void ZakaziTermin()
        {
            if (pozivaoc != null)
            {
                TerminKontroler.IzmeniTermin(IzabraniTermin, PacijentZakazaniTermini.getInstance().prikazTermina.SelectedItem);
                MainViewModel.CurrentView = MainViewModel.PacijentTerminiVM;
                KorisnickeAktivnostiPacijentaKontroler.DodajOdlaganje(ViewModel.jmbg);
            }
            else
            {
                if (KorisnickeAktivnostiPacijentaKontroler.DobaviBrojZakazanihPregledaUBuducnosti(jmbg) >= MAX_BROJ_ZAKAZANIH - 1)
                {
                    var s = new UpozorenjePredBan("z", IzabraniTermin);
                    s.Owner = PacijentMainWindow.getInstance();
                    s.ShowDialog();
                }
                else
                {
                    TerminKontroler.ZakaziTermin(IzabraniTermin);
                    KorisnickeAktivnostiPacijentaKontroler.DodajZakazivanje(ViewModel.jmbg);
                    MainViewModel.CurrentView = MainViewModel.PacijentTerminCommand;

                }
            }
        }
        private ObservableCollection<TerminDTO> NapraviRandomTerminiZaPrikazivanje()
        {
            ObservableCollection<TerminDTO> izgenerisaniTermini = new ObservableCollection<TerminDTO>();
            TerminDTO termin1 = new TerminDTO()
            {
                DatumIVremeTermina = new DateTime(2020, 2, 25, 11, 0, 0),
                lekar = "dr Lazar Jovanović",
                brojSobe = "201"                    
            };
            TerminDTO termin2 = new TerminDTO()
            {
                DatumIVremeTermina = new DateTime(2020, 2, 25, 11, 30, 0),
                lekar = "dr Pera Perić",
                brojSobe = "118"
            };
            TerminDTO termin3 = new TerminDTO()
            {
                DatumIVremeTermina = new DateTime(2020, 2, 23, 6, 0, 0),
                lekar = "dr Lazar Jovanović",
                brojSobe = "201"
            };
            TerminDTO termin4 = new TerminDTO()
            {
                DatumIVremeTermina = new DateTime(2020, 2, 23, 7, 0, 0),
                lekar = "dr Lazar Jovanović",
                brojSobe = "201"
            };
            TerminDTO termin5 = new TerminDTO()
            {
                DatumIVremeTermina = new DateTime(2020, 2, 24, 7, 0, 0),
                lekar = "dr Dimitra Jovanović",
                brojSobe = "301"
            };
            izgenerisaniTermini.Add(termin1);
            izgenerisaniTermini.Add(termin2);
            izgenerisaniTermini.Add(termin3);
            izgenerisaniTermini.Add(termin4);
            izgenerisaniTermini.Add(termin5);
            return izgenerisaniTermini;
        }
    }
}
