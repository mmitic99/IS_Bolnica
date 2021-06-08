using Bolnica.DTOs;
using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class UpravnikKontroler : KorisnikKontroler
    {
        public UpravnikKontroler()
        {
            upravnikServis = new UpravnikServis();
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            Upravnik upravnik = (Upravnik)upravnikServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
            if (upravnik != null)
            {
                UpravnikDTO upravnikDTO = new UpravnikDTO();
                upravnikDTO.Ime = upravnik.Ime;
                upravnikDTO.Prezime = upravnik.Prezime;
                upravnikDTO.Jmbg = upravnik.Jmbg;
                return upravnikDTO;
            }
            return null;
        }

        public bool IzmenaLozinke(string jmbg, string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Upravnik> GetAll()
        {
            return upravnikServis.GetAll();
        }

        public void Save(Model.Upravnik upravnik)
        {
            upravnikServis.Save(upravnik);
        }

        public void SaveAll(List<Upravnik> upravnici)
        {
            upravnikServis.SaveAll(upravnici);
        }
        public Servis.UpravnikServis upravnikServis;

    }
}