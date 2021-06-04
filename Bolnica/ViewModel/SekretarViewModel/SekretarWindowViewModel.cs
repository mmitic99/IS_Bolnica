using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Bolnica.DTOs;
using Bolnica.view.SekretarView.Obavestenja;
using Bolnica.viewActions;
using Kontroler;

namespace Bolnica.ViewModel.SekretarViewModel
{
    public class SekretarWindowViewModel : BindableBase
    {
        public string[] XOsaBrojTermina { get; set; }
        public Func<double, string> YOsaBrojTermina { get; set; }
        public string[] XOsaBrojNovihPacijenata { get; set; }
        public Func<double, string> YOsaBrojNovihPacijenata { get; set; }

        private ObservableCollection<ObavestenjeDTO> _prikazObavestenja;

        public ObservableCollection<ObavestenjeDTO> PrikazObavestenja
        {
            get => _prikazObavestenja;
            set
            {
                _prikazObavestenja = value;
                OnPropertyChanged("PrikazObavestenja");
            }
        }

        public MyICommand DodajObavestenjeCommand { get; set; }

        private static ObavestenjaKontroler obavestenjaKontroler = new ObavestenjaKontroler();

        public SekretarWindowViewModel()
        {
            PrikazObavestenja = new ObservableCollection<ObavestenjeDTO>(obavestenjaKontroler.GetObavestenjaByJmbg("-1"));
            DodajObavestenjeCommand = new MyICommand(DodajObavestenje);
        }

        public void AzurirajObavestenja()
        {
            PrikazObavestenja = new ObservableCollection<ObavestenjeDTO>(obavestenjaKontroler.GetObavestenjaByJmbg("-1"));
            
        }

        public void DodajObavestenje()
        {
            var s = new DodavanjeObavestenja();
            s.ShowDialog();
        }
    }
}