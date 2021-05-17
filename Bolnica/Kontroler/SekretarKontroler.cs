using Model;
using Servis;
using System;
using System.Collections.Generic;
using Bolnica.DTOs;

namespace Kontroler
{
    public class SekretarKontroler : KorisnikKontroler
    {
        public SekretarKontroler()
        {
            sekretarServis = new SekretarServis();
        }

        public bool RegistrujSekretara(Sekretar sekretar)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            Sekretar sekretar = (Sekretar)sekretarServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
            if (sekretar != null)
                return new SekretarDTO()
                {
                    Jmbg = sekretar.Jmbg, Ime = sekretar.Ime, Prezime = sekretar.Prezime,
                    BracnoStanje = sekretar.BracnoStanje, Adresa = sekretar.Adresa,
                    BrojSlobodnihDana = sekretar.BrojSlobodnihDana, Pol = sekretar.Pol,
                    BrojTelefona = sekretar.BrojTelefona, DatumRodjenja = sekretar.DatumRodjenja,
                    Email = sekretar.Email,
                    Grad = sekretar.Grad, Korisnik = sekretar.Korisnik
                };
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

        public List<Sekretar> GetAll()
        {
            // TODO: implement
            return null;
        }

        public void Save(Sekretar sekretar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Sekretar> sekretari)
        {
            // TODO: implement
        }

        public Servis.SekretarServis sekretarServis;

    }
}