
using Bolnica.DTOs;
using System;
using System.Collections.Generic;

namespace Model
{
    public class ZdravstveniKarton
    {
        public List<Izvestaj> Izvestaj { get; set; }      
        public List<Anamneza> Anamneze { get; set; }

        public ZdravstveniKarton()
        {
            Izvestaj = new List<Izvestaj>();
            Alergeni = new List<string>();
            Anamneze = new List<Anamneza>();
        }

        public List<String> Alergeni { get; set; }

        
        
    }
}