using Bolnica.DTO;
using Bolnica.view;
using Bolnica.view.PacijentView;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using Bolnica.DTOs;
using static Bolnica.DTO.ReceptDTO;

namespace Kontroler
{
    public class LekarKontroler : KorisnikKontroler
    {
        public static LekarKontroler instance = null;

        public static LekarKontroler getInstance()
        {
            if(instance==null)
            {
                return new LekarKontroler();
            }
            else
            {
                return instance;
            }
        }
        public LekarKontroler()
        {
            instance = this;
            lekarServis = new LekarServis();
        }

        public bool RegistrujLekara(LekarDTO lekar)
        {
            return lekarServis.RegistrujLekara(new Lekar()
            {
                Ime = lekar.Ime,
                Prezime = lekar.Prezime,
                BracnoStanje = lekar.BracnoStanje,
                Adresa = lekar.Adresa,
                Jmbg = lekar.Jmbg,
                Zanimanje = lekar.Zanimanje,
                Pol = lekar.Pol,
                DatumRodjenja = lekar.DatumRodjenja,
                BrojTelefona = lekar.BrojTelefona,
                Email = lekar.Email,
                Grad = lekar.Grad,
                Korisnik = lekar.Korisnik,
                Specijalizacija = lekar.Specijalizacija,
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName
            });
        }

        public bool DodajObavestenje(Obavestenje obavestenje)
        {
            // TODO: implement
            return false;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return lekarServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<LekarDTO> GetAll()
        {
            List<Lekar> lekari = lekarServis.GetAll();
            List<LekarDTO> lekariDto = new List<LekarDTO>();
            foreach (Lekar lekar in lekari)
            {
                lekariDto.Add(new LekarDTO()
                {
                    Ime = lekar.Ime,
                    Prezime = lekar.Prezime,
                    BracnoStanje = lekar.BracnoStanje,
                    Adresa = lekar.Adresa,
                    Jmbg = lekar.Jmbg,
                    Zanimanje = lekar.Zanimanje,
                    Pol = lekar.Pol,
                    DatumRodjenja = lekar.DatumRodjenja,
                    BrojTelefona = lekar.BrojTelefona,
                    Email = lekar.Email,
                    Grad = lekar.Grad,
                    Korisnik = lekar.Korisnik,
                    Specijalizacija = lekar.Specijalizacija,
                    BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                    IdOrdinacija = lekar.IdOrdinacija,
                    FullName = lekar.FullName
                });
            }

            return lekariDto;
        }

        public void Save(Model.Lekar lekar)
        {
            // TODO: implement
        }

        public void SaveAll(List<Lekar> lekari)
        {
            // TODO: implement
        }

        public Servis.LekarServis lekarServis;

        internal int DobaviIndeksSelektovanogLekara(Object termin)
        {
           return LekarServis.getInstance().DobaviIndeksSelectovanogLekara((Termin)termin);
        }
        public void izdajRecept(ReceptiDTO parametri)
        {
            LekarServis.getInstance().izdajRecept(parametri);
        }
        public LekarDTO GetByJmbg(string jmbg)
        {
            Lekar lekar = lekarServis.GetByJmbg(jmbg);
            return new LekarDTO()
            {
                Ime = lekar.Ime,
                Prezime = lekar.Prezime,
                BracnoStanje = lekar.BracnoStanje,
                Adresa = lekar.Adresa,
                Jmbg = lekar.Jmbg,
                Zanimanje = lekar.Zanimanje,
                Pol = lekar.Pol,
                DatumRodjenja = lekar.DatumRodjenja,
                BrojTelefona = lekar.BrojTelefona,
                Email = lekar.Email,
                Grad = lekar.Grad,
                Korisnik = lekar.Korisnik,
                Specijalizacija = lekar.Specijalizacija,
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName
            };
        }
        public List<int> dobijTerminePijenja(String terminiPijenja)
        {
            return LekarServis.getInstance().dobijTerminePijenja(terminiPijenja);
        }

        public bool ObrisiLekara(string jmbg)
        {
            return lekarServis.ObrisiLekara(jmbg);
        }

        public bool IzmeniLekara(string jmbgLekara, LekarDTO lekar)
        {
            lekarServis.IzmeniLekara(jmbgLekara, new Lekar()
            {
                Ime = lekar.Ime,
                Prezime = lekar.Prezime,
                BracnoStanje = lekar.BracnoStanje,
                Adresa = lekar.Adresa,
                Jmbg = lekar.Jmbg,
                Zanimanje = lekar.Zanimanje,
                Pol = lekar.Pol,
                DatumRodjenja = lekar.DatumRodjenja,
                BrojTelefona = lekar.BrojTelefona,
                Email = lekar.Email,
                Grad = lekar.Grad,
                Korisnik = lekar.Korisnik,
                Specijalizacija = lekar.Specijalizacija,
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName
            });
            return true;
        }
    }
}