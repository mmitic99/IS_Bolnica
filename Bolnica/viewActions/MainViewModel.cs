using Bolnica.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    class MainViewModel : ObseravableObject
    {
        public static MainViewModel instance = null;

        public static MainViewModel getInstance()
        {
            return instance;
        }
        public RelayCommand PacijentTerminCommand { get; set; }
        public RelayCommand ObavestenjaCommand { get; set; }
        public RelayCommand PacijentZakaziTerminCommand { get; set; }
        public RelayCommand MoguciTerminiCommand { get; set; }
        public RelayCommand PomeranjeTerminaCommand { get; set; }

        public PacijentTerminiModel PacijentTerminiVM { get; set; }
        public ObavestenjaViewModel ObavestenjaVM { get; set; }
        public PacijentZakaziTermin PacijentZakaziVM { get; set; }
        public MoguciTerminiViewModel MoguciTerminiVM { get; set; }
        public PomeranjeTerminaViewModel PomeranjeTerminaVM { get; set; }
        public PrikazJednogObavestenjaPacijentaViewModel PrikazObavestenjaVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            instance = this;
            PacijentTerminiVM = new PacijentTerminiModel();
            ObavestenjaVM = new ObavestenjaViewModel();
            PacijentZakaziVM = new PacijentZakaziTermin();
            MoguciTerminiVM = new MoguciTerminiViewModel();
            PomeranjeTerminaVM = new PomeranjeTerminaViewModel();
            PrikazObavestenjaVM = new PrikazJednogObavestenjaPacijentaViewModel();
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
        }
    }
}
