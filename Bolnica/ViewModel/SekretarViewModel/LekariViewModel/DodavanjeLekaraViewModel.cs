using Bolnica.DTOs;
using Bolnica.viewActions;
using Kontroler;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Bolnica.ViewModel.SekretarViewModel.LekariViewModel
{
    public class DodavanjeLekaraViewModel : BindableBase
    {
        private string _jmbgText;
        private string _imeText;
        private string _prezimeText;
        private string _polText;
        private DateTime _datumText;
        private string _vrstaSpecijalizacijeText;
        private string _brojText;
        private string _emailText;
        private string _adresaText;
        private string _gradText;
        private string _drzavaText;

        public MyICommand PotvrdiCommand { get; set; }
        public MyICommand OtkaziCommand { get; set; }

        private readonly LekarKontroler _lekarKontroler = new LekarKontroler();


        public string JmbgText
        {
            get => _jmbgText;
            set
            {
                if (_jmbgText != value)
                {
                    _jmbgText = value;
                }
            }
        }
        public string ImeText
        {
            get => _imeText;
            set
            {
                if (_imeText != value)
                {
                    _imeText = value;
                }
            }
        }
        public string PrezimeText
        {
            get => _prezimeText;
            set
            {
                if (_prezimeText != value)
                {
                    _prezimeText = value;
                }
            }
        }
        public string PolText
        {
            get => _polText;
            set
            {
                if (_polText != value)
                {
                    _polText = value;
                }
            }
        }
        public DateTime DatumText
        {
            get => _datumText;
            set
            {
                if (_datumText != value)
                {
                    _datumText = value;
                }
            }
        }
        public string VrstaSpecijalizacijeText
        {
            get => _vrstaSpecijalizacijeText;
            set
            {
                if (_vrstaSpecijalizacijeText != value)
                {
                    _vrstaSpecijalizacijeText = value;
                }
            }
        }
        public string BrojText
        {
            get => _brojText;
            set
            {
                if (_brojText != value)
                {
                    _brojText = value;
                }
            }
        }
        public string EmailText
        {
            get => _emailText;
            set
            {
                if (_emailText != value)
                {
                    _emailText = value;
                }
            }
        }
        public string AdresaText
        {
            get => _adresaText;
            set
            {
                if (_adresaText != value)
                {
                    _adresaText = value;
                }
            }
        }
        public string GradText
        {
            get => _gradText;
            set
            {
                if (_gradText != value)
                {
                    _gradText = value;
                }
            }
        }
        public string DrzavaText
        {
            get => _drzavaText;
            set
            {
                if (_drzavaText != value)
                {
                    _drzavaText = value;
                }
            }
        }

        private ObservableCollection<string> _vrstaSpecijalizacije;

        public ObservableCollection<string> VrstaSpecijalizacije
        {
            get => _vrstaSpecijalizacije;
            private set
            {
                _vrstaSpecijalizacije = value;
                OnPropertyChanged("VrstaSpecijalizacije");
            }
        }

        private ObservableCollection<string> _polovi;

        public ObservableCollection<string> Polovi
        {
            get => _polovi;
            private set
            {
                _polovi = value;
                OnPropertyChanged("VrstaSpecijalizacije");
            }
        }

        public DodavanjeLekaraViewModel()
        {
            PotvrdiCommand = new MyICommand(Potvrdi);
            OtkaziCommand = new MyICommand(Otkazi);

            DatumText = DateTime.Now;
            VrstaSpecijalizacije = new ObservableCollection<string>(new SpecijalizacijaKontroler().GetAll());
            VrstaSpecijalizacijeText = VrstaSpecijalizacije[0];
            Polovi = new ObservableCollection<string> {"Muški", "Ženski"};
            PolText = Polovi[0];
        }

        public void Potvrdi()
        {
            if (PrezimeText != null && ImeText != null && JmbgText != null && !JmbgText.Trim().Equals("") && !ImeText.Trim().Equals("") && !PrezimeText.Trim().Equals(""))
            {
                LekarDTO lekar = new LekarDTO()
                {
                    Ime = ImeText,
                    Prezime = PrezimeText,
                    Jmbg = JmbgText,
                    Adresa = AdresaText,
                    BrojTelefona = BrojText,
                    Email = EmailText,
                    NazivGrada = GradText,
                    Drzava = DrzavaText,
                    Korisnik = new KorisnikDTO() { KorisnickoIme = JmbgText, Lozinka = ImeText },
                    Pol = PolText.Equals(Polovi[0]) ? Model.Enum.Pol.Muski : Model.Enum.Pol.Zenski,
                    Specijalizacija = VrstaSpecijalizacijeText,
                    BrojSlobodnihDana = 25,
                    DatumRodjenja = DatumText
            };

                bool uspesno = _lekarKontroler.RegistrujLekara(lekar);
                if (!uspesno)
                {
                    MessageBox.Show("Korisnik sa unetim JMBG već postoji, unesite drugi JMBG!!!", "Upozorenje",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                foreach (Window window in App.Current.Windows)
                {
                    if (window.DataContext is SekretarWindowViewModel dataContext)
                    {
                        dataContext.AzurirajLekare();
                        break;
                    }
                }
                Otkazi();
            }
            else
            {
                MessageBox.Show("Polja JMBG, Ime i Prezime su obavezna!", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void Otkazi()
        {
            foreach (Window window in App.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}