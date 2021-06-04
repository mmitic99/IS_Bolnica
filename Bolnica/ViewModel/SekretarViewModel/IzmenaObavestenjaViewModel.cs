using System;
using System.Windows;
using Bolnica.DTOs;
using Bolnica.viewActions;
using Kontroler;

namespace Bolnica.ViewModel.SekretarViewModel
{
    public class IzmenaObavestenjaViewModel
    {
        private ObavestenjeDTO selectedObavestenje;
        private string naslovText;
        private string sadrzajText;

        public MyICommand PotvrdiCommand { get; set; }
        public MyICommand OtkaziCommand { get; set; }

        private ObavestenjaKontroler obavestenjaKontroler = new ObavestenjaKontroler();

        public string NaslovText
        {
            get => naslovText;
            set
            {
                if (naslovText != value)
                {
                    naslovText = value;
                }
            }
        }
        public string SadrzajText
        {
            get => sadrzajText;
            set
            {
                if (sadrzajText != value)
                {
                    sadrzajText = value;
                }
            }
        }

        public ObavestenjeDTO SelectedObavestenje { get; set; }

        public IzmenaObavestenjaViewModel(ObavestenjeDTO selectedObavestenje)
        {
            SelectedObavestenje = selectedObavestenje;
            NaslovText = selectedObavestenje.Naslov;
            SadrzajText = selectedObavestenje.Sadrzaj;
            PotvrdiCommand = new MyICommand(Potvrdi);
            OtkaziCommand = new MyICommand(Otkazi);
        }

        public void Potvrdi()
        {
            if (!NaslovText.Equals("") || !SadrzajText.Equals(""))
            {
                ObavestenjeDTO novoObavestenje = new ObavestenjeDTO
                {
                    Naslov = naslovText,
                    Sadrzaj = sadrzajText,
                    VremeObavestenja = DateTime.Now,
                    JmbgKorisnika = SelectedObavestenje.JmbgKorisnika,
                    Podsetnik = SelectedObavestenje.Podsetnik
                };

                bool uspesno = obavestenjaKontroler.IzmeniObavestenje(SelectedObavestenje, novoObavestenje);
                if (!uspesno)
                {
                    MessageBox.Show("Desila se greška prilikom izmene obaveštenja.", "Greška", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                foreach (Window window in App.Current.Windows)
                {
                    if (window.DataContext is SekretarWindowViewModel sekretarWindowViewModel)
                    {
                        sekretarWindowViewModel.AzurirajObavestenja();
                        break;
                    }
                }
                Otkazi();

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