
using System;
using System.Collections.Generic;

namespace Model
{
    public class ZdravstveniKarton
    {


        public List<Izvestaj> izvestaj { get; set; }      
        public List<Anamneza> Anamneze { get; set; }

       


        public ZdravstveniKarton()
        {
            izvestaj = new List<Izvestaj>();
            Alergeni = new List<string>();
            Anamneze = new List<Anamneza>();
        }

        public List<String> Alergeni { get; set; }

        public Anamneza getAnamnezaById(String Id)
        {
            List<Anamneza> anamneze = this.Anamneze;
            Anamneza a1 = new Anamneza();
            foreach (Anamneza a in anamneze)
            {
                if (a.IdAnamneze != null)
                {
                    if (a.IdAnamneze.Equals(Id))
                    {
                        a1 = a;
                        break;
                    }
                }
            }
            return a1;
        }
        public Anamneza IzmenaAnamneze(String id,String dijalog)
        {
            
            Anamneza anamneza = this.getAnamnezaById(id);
            anamneza.AnamnezaDijalog = dijalog;
            return anamneza;
        }
    }
}