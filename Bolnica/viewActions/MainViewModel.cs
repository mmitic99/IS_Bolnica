using Bolnica.view;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class MainViewModel : ObseravableObject
    {
        public static MainViewModel instance = null;
        public Pacijent Pacijent { get; set; }

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

        public PacijentTerminiViewModel PacijentTerminiVM { get; set; }
        public ObavestenjaViewModel ObavestenjaVM { get; set; }
        public PacijentZakaziTermin PacijentZakaziVM { get; set; }
        public MoguciTerminiViewModel MoguciTerminiVM { get; set; }
        public PomeranjeTerminaViewModel PomeranjeTerminaVM { get; set; }
        public PrikazJednogObavestenjaPacijentaViewModel PrikazObavestenjaVM { get; set; }
        public ZdravstveniKartonViewModel ZdravstveniKartonVM { get; set; }
        public PrikazKvartalneAnketeViewModel PrikazKvartalneAnketeVM { get; set; }
        public AnketaOLekaruViewModel AnketaOLekaruVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(object ulogovaniPacijent)
        {
            this.Pacijent = (Pacijent)ulogovaniPacijent;
            instance = this;
            PacijentTerminiVM = new PacijentTerminiViewModel(Pacijent);
            ObavestenjaVM = new ObavestenjaViewModel(Pacijent);
            PacijentZakaziVM = new PacijentZakaziTermin(Pacijent);
            MoguciTerminiVM = new MoguciTerminiViewModel();
            PomeranjeTerminaVM = new PomeranjeTerminaViewModel(Pacijent);
            PrikazObavestenjaVM = new PrikazJednogObavestenjaPacijentaViewModel();
            PrikazKvartalneAnketeVM = new PrikazKvartalneAnketeViewModel();
            AnketaOLekaruVM = new AnketaOLekaruViewModel();

            CurrentView = PacijentTerminiVM;

            PacijentTerminCommand = new RelayCommand(o =>
            {
                CurrentView = PacijentTerminiVM;
            });

            ObavestenjaCommand = new RelayCommand(o =>
            {
                CurrentView = ObavestenjaVM;
            });

            PacijentZakaziTerminCommand = new RelayCommand(o =>
            {
                CurrentView = PacijentZakaziVM;
            });

            MoguciTerminiCommand = new RelayCommand(o =>
            {
                CurrentView = MoguciTerminiVM;
            });

            PomeranjeTerminaCommand = new RelayCommand(o =>
            {
                CurrentView = PomeranjeTerminaVM;
            }
            );

            ZdravstevniKartonCommand = new RelayCommand(o =>
            {
                CurrentView = ZdravstveniKartonVM;
            }
            );

            KvartalnaAnketaCommand = new RelayCommand(o =>
            {
                CurrentView = PrikazKvartalneAnketeVM;
            }
            );

            AnketaOLekaruCommand = new RelayCommand(o =>
            {
                CurrentView = AnketaOLekaruVM;
            }
            );


            PrikazObavestenjaCommand = new RelayCommand(o =>
            {
                CurrentView = PrikazObavestenjaVM;
            }
            );
        }
        public MainViewModel()
        {
          
        }


    }
}
