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
        public RelayCommand PacijentTerminCommand { get; set; }
        public RelayCommand ObavestenjaCommand { get; set; }
        public PacijentTerminiModel PacijentTerminiVM { get; set; }
        public ObavestenjaViewModel ObavestenjaVM { get; set; }
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
            PacijentTerminiVM = new PacijentTerminiModel();
            ObavestenjaVM = new ObavestenjaViewModel();
            CurrentView = PacijentTerminiVM;

            PacijentTerminCommand = new RelayCommand(o => 
            {
                CurrentView = PacijentTerminiVM;
            });

            ObavestenjaCommand = new RelayCommand(o =>
            {
                CurrentView = ObavestenjaVM;
            });
        }
    }
}
