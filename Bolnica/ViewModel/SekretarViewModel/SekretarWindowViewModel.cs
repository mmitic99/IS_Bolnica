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
        private ObavestenjeDTO _selectedObavestenje;

        public ObavestenjeDTO SelectedObavestenje
        {
            get => _selectedObavestenje;
            set 
            {
                _selectedObavestenje = value;
            }
        }

        public MyICommand DodajObavestenjeCommand { get; set; }
        public MyICommand IzmeniObavestenjeCommand { get; set; }

        private ObavestenjaKontroler obavestenjaKontroler = new ObavestenjaKontroler();

        public SekretarWindowViewModel()
        {
            PrikazObavestenja = new ObservableCollection<ObavestenjeDTO>(obavestenjaKontroler.GetObavestenjaByJmbg("-1"));
            DodajObavestenjeCommand = new MyICommand(DodajObavestenje);
            IzmeniObavestenjeCommand = new MyICommand(IzmeniObavestenje);
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
        public void IzmeniObavestenje()
        {
            if (SelectedObavestenje == null)
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da izmenite.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var s = new IzmenaObavestenja(SelectedObavestenje);
            s.ShowDialog();
        }
    }
}