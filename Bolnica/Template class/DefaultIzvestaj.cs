using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bolnica.Template_class
{
    public abstract class DefaultIzvestaj
    {
        public void GenerisiIzvestaj(DateTime pocetak,DateTime kraj)
        {
            GenerisiDokument(pocetak,kraj);
            PrikaziObavestenje();
        }
        public abstract void GenerisiDokument(DateTime pocetak,DateTime kraj);
        public abstract void PrikaziObavestenje();
    }
}
