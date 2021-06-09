using Bolnica.DTOs;
using Bolnica.view;
using Bolnica.view.PacijentView;
using Kontroler;
using Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bolnica.viewActions
{
    public class PacijentZakaziTermin : BindableBase
    {
        public bool Demo { get; set; }
        public ObservableCollection<LekarDTO> sviLekari { get; set; }
        private int indeksSelektovanogLekara;
        public int IndeksSelektovanogLekara
        {
            get
            {
                return indeksSelektovanogLekara;
            }
            set
            {
                indeksSelektovanogLekara = value;
                OnPropertyChanged("IndeksSelektovanogLekara");
            }
        }
        public DateTime DisplayDateTimeStart { get; set; }
        public DateTime DisplayDateTimeEnd { get; set; }
        private ObservableCollection<DateTime> selectedDates;
        public ObservableCollection<DateTime> SelectedDates
        {
            get
            {
                return selectedDates;
            }
            set
            {
                selectedDates = value;
                OnPropertyChanged ("SelectedDates");
            }
        }
        private int satnicaPocetak;
        public int SatnicaPocetak
        {
            get
            {
                return satnicaPocetak;
            }
            set
            {
                satnicaPocetak = value;
                OnPropertyChanged("SatnicaPocetak");
            }
        }
        private int satnicaKraj;
        public int SatnicaKraj
        {
            get
            {
                return satnicaKraj;
            }
            set
            {
                satnicaKraj = value;
                OnPropertyChanged("SatnicaKraj");
            }
        }
        private int minutPocetak;
        public int MinutPocetak
        {
            get
            {
                return minutPocetak;
            }
            set
            {
                minutPocetak = value;
                OnPropertyChanged("Minutpocetak");
            }
        }
        private int minutKraj;
        public int MinutKraj
        {
            get
            {
                return minutKraj;
            }
            set
            {
                minutKraj = value;
                OnPropertyChanged("MinutKraj");
            }
        }
        private Boolean satnicaPocetakOpened;
        public Boolean SatnicaPocetakOpened
        {
            get
            {
                return satnicaPocetakOpened;
            }
            set
            {
                satnicaPocetakOpened = value;
                OnPropertyChanged("SatnicaPocetakOpened");
            }
        }
        private Boolean satnicaKrajOpened;
        public Boolean SatnicaKrajOpened
        {
            get
            {
                return satnicaKrajOpened;
            }
            set
            {
                satnicaKrajOpened = value;
                OnPropertyChanged("SatnicaKrajOpened");
            }
        }
        private Boolean minutPocetakOpened;
        public Boolean  MinutPocetakOpened
        {
            get
            {
                return minutPocetakOpened;
            }
            set
            {
                minutPocetakOpened = value;
                OnPropertyChanged("MinutPocetakOpened");
            }
        }
        private Boolean minutKrajOpened;
        public Boolean MinutKrajOpened
        {
            get
            {
                return minutKrajOpened;
            }
            set
            {
                minutKrajOpened = value;
                OnPropertyChanged("MinutKrajOpened");
            }
        }
        private Boolean sviLekariOpened;
        public Boolean SviLekariOpened
        {
            get
            {
                return sviLekariOpened;
            }
            set
            {
                sviLekariOpened = value;
                OnPropertyChanged("SviLekariOpened");
            }
        }
        private Boolean nemaPrioritetChecked;
        public Boolean NemaPrioritetChecked
        {
            get
            {
                return nemaPrioritetChecked;
            }
            set
            {
                nemaPrioritetChecked = value;
                OnPropertyChanged("NemaPrioritetChanged");
            }
        }
        private Boolean lekarPrioritetChecked;
        public Boolean LekarPrioritetChecked
        {
            get
            {
                return lekarPrioritetChecked;
            }
            set
            {
                lekarPrioritetChecked = value;
                OnPropertyChanged("LekarPrioritetChanged");
            }
        }
        private Boolean vremePrioritetChecked;
        public Boolean VremePrioritetChecked
        {
            get
            {
                return vremePrioritetChecked;
            }
            set
            {
                vremePrioritetChecked = value;
                OnPropertyChanged("VremePrioritetChecked");
            }
        }
        private String opisTegobe;
        public String OpisTegobe
        {
            get
            {
                return opisTegobe;
            }
            set
            {
                opisTegobe = value;
                OnPropertyChanged("OpisTegobe");
            }
        }
        public ICommand SelectionCommand { get; private set; }

        private void SelectionChanges(object sender)
        {
            SelectedDatesCollection dates = sender as SelectedDatesCollection;
            this.SelectedDates = dates;
        }

        private DateTime izabraniDan;
        public DateTime IzabraniDan
        {
            get
            {
                return izabraniDan;
            }
            set
            {
                izabraniDan = value;
                OnPropertyChanged("IzabraniDan");
            }
        }
        private LekarDTO izabraniLekar;
        public LekarDTO IzabraniLekar
        {
            get
            {
                return izabraniLekar;
            }
            set
            {
                izabraniLekar = value;
                OnPropertyChanged("IzabraniLekar");
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
        public MyICommand PretragaTermina { get; set; }
        public MyICommand IspravnostVremena { get; set; }
        public RelayCommand Nazad { get; set; }

        public String JmbgPacijenta {get; set;}

        public PacijentZakaziTermin(Pacijent pacijent)
        {
            JmbgPacijenta = pacijent.Jmbg;
            this.SelectionCommand = new DelegateCommand<object>(this.SelectionChanges);
            sviLekari = new ObservableCollection<LekarDTO>(LekarKontroler.getInstance().GetAll());
            this.izabraniLekar = sviLekari[0];
            this.Demo = false;
            this.indeksSelektovanogLekara = 0;
            this.DisplayDateTimeStart = DateTime.Today.Date.AddDays(1);
            this.DisplayDateTimeEnd = DateTime.Today.Date.AddMonths(3); //moze se unapred zakazati 3 meseca
            this.IzabraniDan= DateTime.Today.AddDays(1);
            this.SelectedDates = new ObservableCollection<DateTime>();
            this.SelectedDates.Add(DateTime.Today.Date.AddDays(1));
            this.PretragaTermina = new MyICommand(PretregaTermia);
            this.IspravnostVremena = new MyICommand(ProveraIspravnostiVremena);
            this.ivice = 1;
            this.opisTegobe = "";
            this.Nazad = new RelayCommand(o =>
            {
                var s = MainViewModel.getInstance().CurrentView;
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PreviousView;
                MainViewModel.getInstance().PreviousView = s;
            }
            );
            postaviSateMinute();
            postaviComboBoxNaZatvoreno();
            inicijalnoCekirano();
        }
        public PacijentZakaziTermin(bool demo)
        {
            this.Demo = demo;
            this.JmbgPacijenta = "";
            this.SelectionCommand = new DelegateCommand<object>(this.SelectionChanges);
            sviLekari = GenerisiLekareZaPrikaz();
            this.IzabraniDan = new DateTime(2020, 2, 12);
            this.indeksSelektovanogLekara = 0;
            this.DisplayDateTimeStart = new DateTime(2020, 2, 12);
            this.DisplayDateTimeEnd = new DateTime(2020, 5, 12);
            this.SelectedDates = new ObservableCollection<DateTime>();
            this.SelectedDates.Add(new DateTime(2020, 2, 12));
            this.ivice = 1;
            this.OpisTegobe = "";
            postaviSateMinute();
            postaviComboBoxNaZatvoreno();
            inicijalnoCekirano();
        }

        public void PretregaTermia()
        {
            ParametriZaTrazenjeMogucihTerminaDTO parametriDTO = new ParametriZaTrazenjeMogucihTerminaDTO()
            {
                Pacijent = this.JmbgPacijenta,
                IzabraniDatumi = this.selectedDates,
                IzabraniLekar = this.izabraniLekar,
                PocetnaSatnica = this.satnicaPocetak,
                PocetakMinut = minutPocetak,
                KrajnjaSatnica = satnicaKraj,
                KrajnjiMinuti = minutKraj,
                NemaPrioritet = nemaPrioritetChecked,
                OpisTegobe = opisTegobe,
                PrioritetLekar = lekarPrioritetChecked,
                PriotitetVreme = vremePrioritetChecked,
                trajanjeUMinutama = 30,
                vrstaTermina = 0
            };
            List<TerminDTO> moguciTermini = TerminKontroler.getInstance().NadjiTermineZaParametre(parametriDTO);
            if (moguciTermini.Count > 0)
            {
                MainViewModel.getInstance().MoguciTerminiVM = new MoguciTerminiViewModel(moguciTermini, null, JmbgPacijenta);
                MainViewModel.getInstance().PreviousView = MainViewModel.getInstance().CurrentView;
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().MoguciTerminiVM;
            }
            else
            {
                var s = new Upozorenje("Nema slobodnih termina po traženim kriterujumima!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
        }

        private void inicijalnoCekirano()
        {
            this.NemaPrioritetChecked = true;
            this.LekarPrioritetChecked = false;
            this.VremePrioritetChecked = false;
        }

        private void postaviComboBoxNaZatvoreno()
        {
            this.SatnicaKrajOpened = false;
            this.SatnicaPocetakOpened = false;
            this.MinutPocetakOpened = false;
            this.MinutKrajOpened = false;
            this.sviLekariOpened = false;
        }

        private void postaviSateMinute()
        {
            this.SatnicaPocetak = 0;
            this.SatnicaKraj = 14;
            this.MinutPocetak = 0;
            this.MinutPocetak = 0;
        }

        private ObservableCollection<LekarDTO> GenerisiLekareZaPrikaz()
        {
            ObservableCollection<LekarDTO> lekariZaPeikaz = new ObservableCollection<LekarDTO>();
            LekarDTO lekar1 = new LekarDTO()
            {
                FullName = "dr Pera Perić"
            };
            LekarDTO lekar2 = new LekarDTO()
            {
                FullName = "dr Lazar Jovanović"
            };
            lekariZaPeikaz.Add(lekar1);
            lekariZaPeikaz.Add(lekar2);
            return lekariZaPeikaz;
        }
        private void ProveraIspravnostiVremena()
        {
            TimeSpan tpocetak = new TimeSpan(SatnicaPocetak, MinutPocetak, 0);
            TimeSpan tkraj = new TimeSpan(SatnicaKraj, MinutKraj, 0);
            if (tpocetak >= tkraj)
            {
                modifikujKraj();
            }
        }

        private void modifikujKraj()
        {
            if (MinutPocetak == 0)
            {
                SatnicaKraj = SatnicaPocetak;
                MinutKraj = 1;
            }
            else
            {
                SatnicaKraj = SatnicaPocetak + 1;
                MinutKraj = 0;
            }
            
        }
    }
}