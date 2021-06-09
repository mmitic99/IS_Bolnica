using Bolnica.view;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Bolnica.viewActions
{
    public class MainViewModel : ObseravableObject
    {
        public static MainViewModel instance = null;
        public Pacijent Pacijent { get; set; }
        public String JmbgPacijenta { get; set; }

        public static MainViewModel getInstance()
        {
            return instance;
        }
        public RelayCommand PacijentTerminCommand { get; set; }
        public RelayCommand ObavestenjaCommand { get; set; }
        public RelayCommand PacijentZakaziTerminCommand { get; set; }
        public RelayCommand MoguciTerminiCommand { get; set; }
        public RelayCommand PomeranjeTerminaCommand { get; set; }
        public RelayCommand ZdravstevniKartonCommand { get; set; }
        public RelayCommand KvartalnaAnketaCommand { get; set; }
        public RelayCommand PrikazObavestenjaCommand { get; set; }
        public RelayCommand AnketaOLekaruCommand { get; set; }
        public RelayCommand PodesavanjeObavestenjaCommand { get; set; }
        public RelayCommand DodavnjePodsetnikaCommand { get; set; }
        public RelayCommand PomocCommand { get; set; }
        public RelayCommand VratiSeNazaCommand { get; set; }
        public RelayCommand PregledKalendaraCommand { get; set; }
        public RelayCommand DemoCommand { get; set; }
        public RelayCommand InfoCommand { get; set; }

        public PacijentTerminiViewModel PacijentTerminiVM { get; set; }
        public ObavestenjaViewModel ObavestenjaVM { get; set; }
        public PacijentZakaziTermin PacijentZakaziVM { get; set; }
        public MoguciTerminiViewModel MoguciTerminiVM { get; set; }
        public PomeranjeTerminaViewModel PomeranjeTerminaVM { get; set; }
        public PrikazJednogObavestenjaPacijentaViewModel PrikazObavestenjaVM { get; set; }
        public ZdravstveniKartonViewModel ZdravstveniKartonVM { get; set; }
        public PrikazKvartalneAnketeViewModel PrikazKvartalneAnketeVM { get; set; }
        public AnketaOLekaruViewModel AnketaOLekaruVM { get; set; }
        public PodesavanjeObavestenjaViewModel PodesavanjeObavestenjaVM { get; set; }
        public DodavanjePodsetnikaViewModel DodavanjePodsetnikaVM { get; set; }
        public KomentariKorisnikaViewModel KomentariKorisnikaVM { get; set; }
        public PomocTerminiViewModel PomocTerminiVM { get; set; }
        public PomocObavestenjaViewModel PomocObavestenjaVM { get; set; }
        public KalendarViewModel KalendarVM { get; set; }
        public PacijentZakaziTermin PacijentZakaziDEMO = new PacijentZakaziTermin(true);
        public MoguciTerminiViewModel MoguciTerminiDEMO { get; set; }
        public InformacijeAplikacijaViewModel InformacijeAplikacijaVM { get; set; }


        private object _currentView;
        private object _previousView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        public object PreviousView
        {
            get { return _previousView; }
            set { _previousView = value;
                OnPropertyChanged();
            }
        }

        Thread nit;

        public MainViewModel(object ulogovaniPacijent)
        {
            this.nit = new Thread(new ThreadStart(DemoDesavanja));
            this.Pacijent = (Pacijent)ulogovaniPacijent;
            this.JmbgPacijenta = Pacijent.Jmbg;
            instance = this;
            PacijentTerminiVM = new PacijentTerminiViewModel(Pacijent);
            ObavestenjaVM = new ObavestenjaViewModel(Pacijent);
            PacijentZakaziVM = new PacijentZakaziTermin(Pacijent);
            MoguciTerminiVM = new MoguciTerminiViewModel();
            PomeranjeTerminaVM = new PomeranjeTerminaViewModel(Pacijent);
            PrikazObavestenjaVM = new PrikazJednogObavestenjaPacijentaViewModel();
            PrikazKvartalneAnketeVM = new PrikazKvartalneAnketeViewModel();
            AnketaOLekaruVM = new AnketaOLekaruViewModel();
            ZdravstveniKartonVM = new ZdravstveniKartonViewModel(Pacijent);
            PodesavanjeObavestenjaVM = new PodesavanjeObavestenjaViewModel(Pacijent);
            DodavanjePodsetnikaVM = new DodavanjePodsetnikaViewModel(Pacijent);
            PomocObavestenjaVM = new PomocObavestenjaViewModel();
            KalendarVM = new KalendarViewModel();
            InformacijeAplikacijaVM = new InformacijeAplikacijaViewModel(JmbgPacijenta);

            PacijentZakaziDEMO = new PacijentZakaziTermin(true);

            CurrentView = PacijentTerminiVM;
            PreviousView = PacijentTerminiVM;

            PacijentTerminCommand = new RelayCommand(o =>
            {
                PacijentTerminiVM = new PacijentTerminiViewModel(Pacijent);
                PreviousView = CurrentView;
                CurrentView = PacijentTerminiVM;
            });

            ObavestenjaCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = ObavestenjaVM;
            });

            PacijentZakaziTerminCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = PacijentZakaziVM;
            });

            MoguciTerminiCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = MoguciTerminiVM;
            });

            PomeranjeTerminaCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = PomeranjeTerminaVM;
            }
            );

            ZdravstevniKartonCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = ZdravstveniKartonVM;
            }
            );

            KvartalnaAnketaCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = PrikazKvartalneAnketeVM;
            }
            );

            AnketaOLekaruCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = AnketaOLekaruVM;
            }
            );


            PrikazObavestenjaCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = PrikazObavestenjaVM;
            }
            );

            PodesavanjeObavestenjaCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = PrikazObavestenjaVM;
            }
            );

            DodavnjePodsetnikaCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = DodavanjePodsetnikaVM;
            }
            );

            VratiSeNazaCommand = new RelayCommand(o =>
            {
                var s = CurrentView;
                CurrentView = PreviousView;
                PreviousView = s;
            }
            );

            PomocTerminiVM = new PomocTerminiViewModel(VratiSeNazaCommand);


            PomocCommand = new RelayCommand(o =>
            {
                if (CurrentView == PacijentTerminiVM || CurrentView == PacijentZakaziVM || CurrentView == MoguciTerminiVM || CurrentView == PomeranjeTerminaVM)
                {
                    PreviousView = CurrentView;
                    CurrentView = PomocTerminiVM;
                }
               else if(CurrentView == ObavestenjaVM || CurrentView == DodavanjePodsetnikaVM || CurrentView == PodesavanjeObavestenjaVM || CurrentView == PrikazObavestenjaVM)
                {
                    PreviousView = CurrentView;
                    CurrentView = PomocObavestenjaVM;
                }
            });

            PregledKalendaraCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                CurrentView = KalendarVM;
            }
            );

            DemoCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                PacijentZakaziDEMO = new PacijentZakaziTermin(true);
                CurrentView = PacijentZakaziDEMO;
                nit.Start();
            }
            );

            InfoCommand = new RelayCommand(o =>
            {
                PreviousView = CurrentView;
                InformacijeAplikacijaVM = new InformacijeAplikacijaViewModel(JmbgPacijenta);
                CurrentView = InformacijeAplikacijaVM;
            }
            );
        }

        internal void PrekiniDemo()
        {
            if (nit.IsAlive)
            {
                nit.Abort();
                CurrentView = PreviousView;
                this.nit = new Thread(new ThreadStart(DemoDesavanja));
            }
        }

        private void DemoDesavanja()
        {
            Thread.Sleep(2000);
            PacijentZakaziDEMO.OpisTegobe = "Ovde unesiteku tegobu";
            Thread.Sleep(2000);
            PacijentZakaziDEMO.OpisTegobe = PacijentZakaziDEMO.OpisTegobe + "\nNa primer: imam užasu glavobolju već 4 dana.";
            Thread.Sleep(2000);
            PacijentZakaziDEMO.SviLekariOpened = true;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.IndeksSelektovanogLekara = 1;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.SviLekariOpened = false;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.IzabraniDan = new DateTime(2020, 2, 25);
            Thread.Sleep(2000);

            PacijentZakaziDEMO.SatnicaPocetakOpened = true;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.SatnicaPocetak = 4;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.SatnicaPocetakOpened = false;
            Thread.Sleep(2000);

            PacijentZakaziDEMO.MinutPocetakOpened = true;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.MinutPocetak = 1;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.MinutPocetakOpened = false;
            Thread.Sleep(2000);

            PacijentZakaziDEMO.SatnicaKrajOpened = true;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.SatnicaKraj = 6;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.SatnicaKrajOpened = false;
            Thread.Sleep(2000);

            PacijentZakaziDEMO.MinutKrajOpened = true;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.MinutKraj = 0;
            Thread.Sleep(2000);
            PacijentZakaziDEMO.MinutKrajOpened = false;
            Thread.Sleep(2000);

            PacijentZakaziDEMO.VremePrioritetChecked = true;
            PacijentZakaziDEMO.LekarPrioritetChecked = false;
            PacijentZakaziDEMO.NemaPrioritetChecked = false;
            Thread.Sleep(2000);

            PacijentZakaziDEMO.Ivice = 4;
            Thread.Sleep(2000);

            MoguciTerminiDEMO = new MoguciTerminiViewModel(true);
            CurrentView = MoguciTerminiDEMO;
            Thread.Sleep(2000);

            MoguciTerminiDEMO.IndeksIzabranogTermina = 2;
            Thread.Sleep(2000);

            MoguciTerminiDEMO.Ivice = 4;
            Thread.Sleep(2000);

            CurrentView = PreviousView;
            this.nit = new Thread(new ThreadStart(DemoDesavanja));
        }

        private ObservableCollection<DateTime> DobaviNekeDatume()
        {
            ObservableCollection<DateTime> selektovaniDatumi = new ObservableCollection<DateTime>();
            selektovaniDatumi.Add(new DateTime(2020, 2, 20));
            selektovaniDatumi.Add(new DateTime(2020, 2, 22));
            selektovaniDatumi.Add(new DateTime(2020, 2, 23));
            return selektovaniDatumi;
        }

        public MainViewModel()
        {
          
        }


    }
}
