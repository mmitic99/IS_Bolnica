using Bolnica.DTOs;
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
    public class ObavestenjaViewModel
    {
        private ObavestenjaKontroler ObavestenjaKontroler;
        public ObservableCollection<Obavestenje> podsetnici
        {
            get
            {
                return new ObservableCollection<Obavestenje>(ObavestenjaKontroler.DobaviPodsetnikeZaTerapiju(Pacijent));
            }
        }

        public List<ObavestenjeDTO> obavestenja
        {
            get
            {
                return ObavestenjaKontroler.GetOavestenjaByJmbg(JmbgKorisnika);
            }
        }
        public string JmbgKorisnika { get; set; }
        public Pacijent Pacijent { get;}

        public ObavestenjaViewModel(Pacijent pacijent)
        {
            this.ObavestenjaKontroler = new ObavestenjaKontroler();
            this.JmbgKorisnika = pacijent.Jmbg;
            this.Pacijent = pacijent;

        }
    }
}
