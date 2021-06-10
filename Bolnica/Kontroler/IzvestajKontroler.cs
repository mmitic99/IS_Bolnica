using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Template_class;
using Model;

namespace Bolnica.Kontroler
{
    public class IzvestajKontroler
    {
        public static IzvestajKontroler instance;

        public static IzvestajKontroler GetInstance()
        {
            if (instance == null) return new IzvestajKontroler();
            else return instance;
        }

        public IzvestajKontroler()
        {
            instance = this;
        }
        public void KreirajIzvestajPacijenta(DateTime pocetakIntervala, DateTime krajIntervala)
        {
            DefaultIzvestaj izvestaj = new IzvestajPacijentServis();
            izvestaj.GenerisiIzvestaj(pocetakIntervala, krajIntervala);
        }
        public void KreirajIzvestajLekara(DateTime pocetakIntervala, DateTime krajIntervala)
        {
            DefaultIzvestaj izvestaj = new IzvestajLekarServis();
            izvestaj.GenerisiIzvestaj(pocetakIntervala, krajIntervala);
        }
        public void KreirajIzvestajUpravnika(DateTime pocetakIntervala, DateTime krajIntervala)
        {
            DefaultIzvestaj izvestaj = new IzvestajUpravnikServis();
            izvestaj.GenerisiIzvestaj(pocetakIntervala, krajIntervala);
        }
        public void KreirajIzvestajSekretara(DateTime pocetakIntervala, DateTime krajIntervala)
        {
            DefaultIzvestaj izvestaj = new IzvestajSekretarServis();
            izvestaj.GenerisiIzvestaj(pocetakIntervala, krajIntervala);
        }
    }
}
