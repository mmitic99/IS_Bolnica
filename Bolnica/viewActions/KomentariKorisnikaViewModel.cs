using Bolnica.model;
using Kontroler;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class KomentariKorisnikaViewModel : BindableBase
    {
        public Pacijent Pacijent { get; set; }
        public Recept IzabraniRecept { get; set; }
        public ObservableCollection<Beleska> KomentariKorisnika
        {
            get;
            set;
 
        }
        public MyICommand AddNoteCommand { get; set; }
        private BeleskaViewModel trenutnaBeleska = new BeleskaViewModel()
        {
    
        };
        private PacijentKontroler PacijentKontroler;
        private bool _RadioButtonChecked;
        public bool RadioButtonChecked
        {
            get => _RadioButtonChecked;
            set
            {
                _RadioButtonChecked = value;
            }
        }

        public KomentariKorisnikaViewModel(Recept recept, Pacijent pacijent)
        {
            this.Pacijent = pacijent;
            this.KomentariKorisnika = new ObservableCollection<Beleska>();
            this.PacijentKontroler = new PacijentKontroler();
            this.IzabraniRecept = recept;
            this.KomentariKorisnika = new ObservableCollection<Beleska>(IzabraniRecept.KomentariPacijenta);
            AddNoteCommand = new MyICommand(OnAdd);
        }
        
        public BeleskaViewModel TrenutnaBeleska
        {
            get { return trenutnaBeleska; }
            set
            {
                trenutnaBeleska = value;
                OnPropertyChanged("TrenutnaBeleska");
            }
        }

        public void OnAdd()
        {
            TrenutnaBeleska.Validate();
            if (TrenutnaBeleska.IsValid)
            {
                Beleska beleska =(new Beleska()
                {
                    Naslov = TrenutnaBeleska.Naslov,
                    Opis = TrenutnaBeleska.Opis
                });
                IzabraniRecept.KomentariPacijenta.Add(beleska);
                PacijentKontroler.SacuvajKomentarNaDijagnozu(IzabraniRecept, Pacijent);
                TrenutnaBeleska.Naslov = "";
                TrenutnaBeleska.Opis = "";
                KomentariKorisnika.Add(beleska);

                if(RadioButtonChecked)
                {
                    MainViewModel.getInstance().DodavanjePodsetnikaVM = new DodavanjePodsetnikaViewModel(Pacijent, beleska);
                    MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().DodavanjePodsetnikaVM;
                }
            }
       }

        
    }
}
