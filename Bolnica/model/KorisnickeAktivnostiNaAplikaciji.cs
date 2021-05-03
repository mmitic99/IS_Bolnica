using System;
using System.Collections.Generic;
using Model.Enum;

namespace Model
{
    public class KorisnickeAktivnostiNaAplikaciji
    {

        public String JmbgKorisnika { get; set; }
        public VrstaKorisnikaAplikacije TrenutnoSeTretiraKao { get; set; }
        public List<KorisnickaAktivnost> AktivnostiKorisnika { get; set; }
        public DateTime BlokiranDo { get; set; }
        public int BrojPutaBlokiranja { get; set; }

        public KorisnickeAktivnostiNaAplikaciji(String JmbgKorisnika)
        {
            this.JmbgKorisnika = JmbgKorisnika;
            TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Normalan;
            AktivnostiKorisnika = new List<KorisnickaAktivnost>();
            BrojPutaBlokiranja = 0;
        }

        public KorisnickeAktivnostiNaAplikaciji()
        {

        }
        public bool BlokirajKorisnika()
        {
            TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Spam;
            ++BrojPutaBlokiranja;
            BlokiranDo = DateTime.Today.AddDays(BrojPutaBlokiranja * 14);
            return true;
        }

        public bool OdblokirajKorisnika()
        {
            TrenutnoSeTretiraKao = VrstaKorisnikaAplikacije.Normalan;
            return true;
        }

        public bool OznaciDaJeZakazaoPrevisePregledaUnapred()
        {
            // TODO: implement
            return false;
        }
    }
   

   
      
   
  
}