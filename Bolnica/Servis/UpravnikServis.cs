using Model;
using System;
using System.Collections.Generic;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;

namespace Servis
{
    public class UpravnikServis : KorisnikServis
    {

        public UpravnikServis()
        {
            skladisteUpravnik = SkladisteUpravnikXml.GetInstance();
        }

        public bool RegistrujUpravnika(Upravnik upravnik)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Upravnik> upravnici = skladisteUpravnik.GetAll();

            Upravnik upravnik = new Upravnik();

            foreach (Upravnik upravnik1 in upravnici)
            {
                if (upravnik1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    upravnik = upravnik1;
                    if (upravnik1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return upravnik;
                    }
                }
            }
            return null;
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Upravnik> GetAll()
        {
            return skladisteUpravnik.GetAll();
        }

        public void Save(Model.Upravnik upravnik)
        {
            skladisteUpravnik.Save(upravnik);
        }

        public void SaveAll(List<Upravnik> upravnici)
        {
            skladisteUpravnik.SaveAll(upravnici);
        }

        public ISkladisteUpravnik skladisteUpravnik;

    }
}