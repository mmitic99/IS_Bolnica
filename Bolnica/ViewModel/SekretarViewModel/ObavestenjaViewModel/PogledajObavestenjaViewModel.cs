using System.Windows;
using Bolnica.DTOs;
using Bolnica.viewActions;

namespace Bolnica.ViewModel.SekretarViewModel.ObavestenjaViewModel
{
    public class PogledajObavestenjaViewModel
    {
        private ObavestenjeDTO _selectedObavestenje;
        private string naslovText;
        private string sadrzajText;
        private string datumText;
        public ObavestenjeDTO SelectedObavestenje { get; set; }

        public MyICommand OtkaziCommand { get; set; }

        public PogledajObavestenjaViewModel(ObavestenjeDTO selectedObavestenje)
        {
            SelectedObavestenje = selectedObavestenje;
            naslovText = SelectedObavestenje.Naslov;
            sadrzajText = SelectedObavestenje.Sadrzaj;
            datumText = "Datum i vreme objavljivanja: " +  SelectedObavestenje.VremeObavestenja.ToString("dd.MM.yyyy HH:mm");
            OtkaziCommand = new MyICommand(Otkazi);
        }
        public string NaslovText
        {
            get => naslovText;
            set {}
        }
        public string SadrzajText
        {
            get => sadrzajText;
            set { }
        }
        public string DatumText
        {
            get => datumText;
            set { }
        }
        public void Otkazi()
        {
            foreach (Window window in Application.Current.Windows)
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