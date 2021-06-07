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
            {
                SekretarDTO sekretarDto = new SekretarDTO()
                {
                    Jmbg = sekretar.Jmbg, Ime = sekretar.Ime, Prezime = sekretar.Prezime,
                    BracnoStanje = sekretar.BracnoStanje, Adresa = sekretar.Adresa,
                    BrojSlobodnihDana = sekretar.BrojSlobodnihDana, Pol = sekretar.Pol,
                    BrojTelefona = sekretar.BrojTelefona, DatumRodjenja = sekretar.DatumRodjenja,
                    Email = sekretar.Email, Korisnik = new KorisnikDTO()
                    {
                        KorisnickoIme = sekretar.Korisnik.KorisnickoIme,
                        Lozinka = sekretar.Korisnik.Lozinka
                    }
                };
                if (sekretar.Grad == null)
                    sekretarDto.NazivGrada = "";
                return sekretarDto;
            }

            return null;
        }

        public bool IzmenaLozinke(string jmbgSekretara, string staraLozinka, string novaLozinka)
        {
            return sekretarServis.IzmenaLozinke(jmbgSekretara, staraLozinka, novaLozinka);
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

        public bool IzmeniSekretara(string sekretarJmbg, SekretarDTO noviSekretar)
        {
            return sekretarServis.IzmeniSekretara(sekretarJmbg, new Sekretar()
            {
                Jmbg = noviSekretar.Jmbg,
                Ime = noviSekretar.Ime,
                Prezime = noviSekretar.Prezime,
                BracnoStanje = noviSekretar.BracnoStanje,
                Adresa = noviSekretar.Adresa,
                BrojSlobodnihDana = noviSekretar.BrojSlobodnihDana,
                Pol = noviSekretar.Pol,
                BrojTelefona = noviSekretar.BrojTelefona,
                DatumRodjenja = noviSekretar.DatumRodjenja,
                Email = noviSekretar.Email,
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = noviSekretar.Korisnik.KorisnickoIme,
                    Lozinka = noviSekretar.Korisnik.Lozinka
                },
                Grad = new Grad()
                {
                    Naziv = noviSekretar.NazivGrada
                }
            });
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }
    }
}