using System;
using System.Collections.Generic;
using Model.Enum;

namespace Model
{
    public class KorisnickeAktivnostiNaAplikaciji
    {
        public String JmbgKorisnika { get; set; }
        public List<KorisnickaAktivnost> AktivnostiKorisnika { get; set; }
        public DateTime BlokiranDo { get; set;}
        public int BrojPutaBlokiranja { get; set;}

        public KorisnickeAktivnostiNaAplikaciji(String JmbgKorisnika)
        {
            this.JmbgKorisnika = JmbgKorisnika;
            AktivnostiKorisnika = new List<KorisnickaAktivnost>();
            BrojPutaBlokiranja = 0;
        }
        public KorisnickeAktivnostiNaAplikaciji()
        {
        }
    }
   

   
      
   
  
}