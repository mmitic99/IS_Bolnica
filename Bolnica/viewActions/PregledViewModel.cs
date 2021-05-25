using Bolnica.model;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class PregledViewModel : BindableBase
    {
        public Recept Recept { get; set; }
        public DateTime datum { get; set; }
        public String terapija { get; set; }
        public String lekar { get; set; }
        public String dijagnoza { get; set; }
        public KomentariKorisnikaViewModel KomentariKorisnika { get; set; }
        public Pacijent pacijent {get; set;}

        public PregledViewModel()
        {

        }

        public PregledViewModel(Recept recept, Pacijent pacijent)
        {

            Recept = recept;
            datum = recept.DatumIzdavanja;
            terapija = recept.TerminiUzimanjaLeka.Count.ToString()+"x "+ recept.ImeLeka+" "+recept.BrojDana+" dana" ;
            lekar = recept.ImeDoktora;
            dijagnoza = recept.Dijagnoza;
            this.KomentariKorisnika = new KomentariKorisnikaViewModel(recept, pacijent);
        }
    }
}
