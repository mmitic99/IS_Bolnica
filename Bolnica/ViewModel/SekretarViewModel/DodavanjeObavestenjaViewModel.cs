using Bolnica.DTOs;
using Bolnica.viewActions;
using Kontroler;
using System;
using System.Windows;

namespace Bolnica.ViewModel.SekretarViewModel
{
    public class DodavanjeObavestenjaViewModel
    {
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

        public DodavanjeObavestenjaViewModel()
        {
            PotvrdiCommand = new MyICommand(Potvrdi);
            OtkaziCommand = new MyICommand(Otkazi);
        }

        public void Potvrdi()
        {
            if (SadrzajText == null || NaslovText == null || (NaslovText.Equals("") && SadrzajText.Equals("")))
            {
                MessageBox.Show("Polja za naslov i sadržaj ne mogu ostati prazna.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            ObavestenjeDTO obavestenje = new ObavestenjeDTO
            {
                JmbgKorisnika = "-1",
                Naslov = NaslovText,
                Sadrzaj = SadrzajText,
                Podsetnik = false,
                VremeObavestenja = DateTime.Now,
                Vidjeno = false
            };

            bool uspesno = obavestenjaKontroler.Save(obavestenje);

            if (!uspesno)
            {
                MessageBox.Show("Desila se greška prilikom kreiranja obaveštenja.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            foreach (Window window in App.Current.Windows)
            {
                if (window.DataContext is SekretarWindowViewModel)
                {
                    ((SekretarWindowViewModel)window.DataContext).AzurirajObavestenja();
                    break;
                }
            }

            Otkazi();
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