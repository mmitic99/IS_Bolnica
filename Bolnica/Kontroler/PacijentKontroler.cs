using Model;
using Servis;
using System;
using System.Collections.Generic;
using Bolnica.DTOs;

namespace Kontroler
{
    public class PacijentKontroler : KorisnikKontroler
    {
        public PacijentServis pacijentServis;
        public static PacijentKontroler instance = null;

        public static PacijentKontroler GetInstance()
        {
            if (instance == null)
            {
                return new PacijentKontroler();
            }

            return instance;
        }
        public PacijentKontroler()
        {
            instance = this;
            pacijentServis = new PacijentServis();
            instance = this;
        }

        public bool RegistrujPacijenta(PacijentDTO pacijent)
        {
            return pacijentServis.RegistrujPacijenta(new Pacijent(){
                Ime = pacijent.Ime,
                Prezime = pacijent.Prezime,
                BracnoStanje = pacijent.BracnoStanje,
                Adresa = pacijent.Adresa,
                Jmbg = pacijent.Jmbg,
                Zanimanje = pacijent.Zanimanje,
                Pol = pacijent.Pol,
                DatumRodjenja = pacijent.DatumRodjenja,
                BrojTelefona = pacijent.BrojTelefona,
                Email = pacijent.Email,
                Grad = pacijent.Grad,
                Korisnik = pacijent.Korisnik,
                Registrovan = pacijent.Registrovan,
                zdravstveniKarton = pacijent.zdravstveniKarton
            });
        }

        public bool DodajAlergen(String alergen)
        {
            // TODO: implement
            return false;
        }

        public bool DodajObavestenje(Model.Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public bool DodajIzvestaj(Izvestaj izvestaj)
        {
            // TODO: implement
            return false;
        }

        public bool IzmeniIzvestaj(Izvestaj izvestaj)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return pacijentServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<PacijentDTO> GetAll()
        {
            List<Pacijent> pacijenti = pacijentServis.GetAll();
            List<PacijentDTO> pacijentiDto = new List<PacijentDTO>();
            foreach (Pacijent pacijent in pacijenti)
            {
                pacijentiDto.Add(new PacijentDTO()
                {
                    Ime = pacijent.Ime, Prezime = pacijent.Prezime, BracnoStanje = pacijent.BracnoStanje,
                    Adresa = pacijent.Adresa, Jmbg = pacijent.Jmbg, Zanimanje = pacijent.Zanimanje, Pol = pacijent.Pol,
                    DatumRodjenja = pacijent.DatumRodjenja, BrojTelefona = pacijent.BrojTelefona,
                    Email = pacijent.Email, Grad = pacijent.Grad, Korisnik = pacijent.Korisnik,
                    Registrovan = pacijent.Registrovan, zdravstveniKarton = pacijent.zdravstveniKarton
                });
            }

            return pacijentiDto;
        }

        public void Save(Pacijent pacijent)
        {
            // TODO: implement
        }

        public void SaveAll(List<Pacijent> pacijenti)
        {
            // TODO: implement
        }

        public bool IzmeniPacijenta(PacijentDTO stariPacijent, PacijentDTO noviPacijent)
        {
            return pacijentServis.IzmeniPacijenta(new Pacijent()
            {
                Ime = stariPacijent.Ime,
                Prezime = stariPacijent.Prezime,
                BracnoStanje = stariPacijent.BracnoStanje,
                Adresa = stariPacijent.Adresa,
                Jmbg = stariPacijent.Jmbg,
                Zanimanje = stariPacijent.Zanimanje,
                Pol = stariPacijent.Pol,
                DatumRodjenja = stariPacijent.DatumRodjenja,
                BrojTelefona = stariPacijent.BrojTelefona,
                Email = stariPacijent.Email,
                Grad = stariPacijent.Grad,
                Korisnik = stariPacijent.Korisnik,
                Registrovan = stariPacijent.Registrovan,
                zdravstveniKarton = stariPacijent.zdravstveniKarton
            }, new Pacijent()
            {
                Ime = noviPacijent.Ime,
                Prezime = noviPacijent.Prezime,
                BracnoStanje = noviPacijent.BracnoStanje,
                Adresa = noviPacijent.Adresa,
                Jmbg = noviPacijent.Jmbg,
                Zanimanje = noviPacijent.Zanimanje,
                Pol = noviPacijent.Pol,
                DatumRodjenja = noviPacijent.DatumRodjenja,
                BrojTelefona = noviPacijent.BrojTelefona,
                Email = noviPacijent.Email,
                Grad = noviPacijent.Grad,
                Korisnik = noviPacijent.Korisnik,
                Registrovan = noviPacijent.Registrovan,
                zdravstveniKarton = noviPacijent.zdravstveniKarton
            });
        }

        public List<Recept> DobaviRecepePacijenta(string jmbg)
        {
            return pacijentServis.DobaviReceptePacijenta(jmbg);
        }

        public Pacijent GetByJmbg(string jmbg)
        {
            return pacijentServis.GetByJmbg(jmbg);
        }

        public bool ObrisiPacijenta(string jmbg)
        {
            return pacijentServis.ObrisiPacijenta(jmbg);
        }
    }
}