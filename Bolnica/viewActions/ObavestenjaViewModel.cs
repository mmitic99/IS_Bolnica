using Bolnica.DTOs;
using Bolnica.Kontroler;
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
    public class ObavestenjaViewModel
    {
        private ObavestenjaKontroler ObavestenjaKontroler;
        private PodsetniciKontroler PodsetniciKontroler;
        public ObservableCollection<Notifikacija> podsetnici
        {
            get
            {
                ObservableCollection<Notifikacija> notifikacijas = new ObservableCollection<Notifikacija>(PodsetniciKontroler.DobaviPodsetnikeZaTerapiju(Pacijent));
                return notifikacijas;
            }
        }

        public List<ObavestenjeDTO> obavestenja
        {
            get
            { 

                return ObavestenjaKontroler.GetObavestenjaByJmbg(JmbgKorisnika).OrderByDescending(x => x.VremeObavestenja).ToList();
            }
        }
        public string JmbgKorisnika { get; set; }
        public Pacijent Pacijent { get;}

        public ObavestenjaViewModel(Pacijent pacijent)
        {
            this.ObavestenjaKontroler = new ObavestenjaKontroler();
            this.PodsetniciKontroler = new PodsetniciKontroler();
            this.JmbgKorisnika = pacijent.Jmbg;
            this.Pacijent = pacijent;

        }
    }
}
