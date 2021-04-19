using System;
using System.Collections.Generic;

namespace Model
{
    public class ZdravstveniKarton
    {

        public List<Izvestaj> izvestaj { get; set; }


        public ZdravstveniKarton()
        {
            izvestaj = new List<Izvestaj>();
            Alergeni = new List<string>();
        }

        public List<String> Alergeni { get; set; }
        public List<Izvestaj> Izvestaji { get; set; }
    }
}