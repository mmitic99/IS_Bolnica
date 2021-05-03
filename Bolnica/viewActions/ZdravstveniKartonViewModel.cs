using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    class ZdravstveniKartonViewModel
    {
        public Pacijent pacijent { get; set; }
        public List<PregledViewModel> odradjeniPregledi { get; set; }
        public ZdravstveniKartonViewModel()
        {

        }

        public ZdravstveniKartonViewModel(Pacijent p)
        {
            pacijent = p;
            odradjeniPregledi = new List<PregledViewModel>();
            foreach(Izvestaj izvestaj in p.zdravstveniKarton.izvestaj)
                foreach(Recept recept in izvestaj.recepti)
                {
                    PregledViewModel pregled = new PregledViewModel(recept);
                    odradjeniPregledi.Add(pregled);
                }
        }
    }
}
