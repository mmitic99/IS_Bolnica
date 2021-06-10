using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.view;
using Bolnica.view.PacijentView;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Bolnica.DTOs.ReceptDTO;


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
                Grad = new Grad(){Naziv = lekar.NazivGrada, drzava = new Drzava(){Naziv = lekar.Drzava}},
                Korisnik = new Korisnik(){KorisnickoIme = lekar.Korisnik.KorisnickoIme, Lozinka = lekar.Korisnik.Lozinka},
                Specijalizacija = new Specijalizacija(){VrstaSpecijalizacije =  lekar.Specijalizacija},
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName,
                ImeiSpecijalizacija = lekar.ImeiSpecijalizacija
            });
        }



        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            Lekar lekar = (Lekar) lekarServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
            if (lekar != null)
            {
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
                    NazivGrada = lekar.Grad.Naziv,
                    Korisnik = new KorisnikDTO()
                        {KorisnickoIme = lekar.Korisnik.KorisnickoIme, Lozinka = lekar.Korisnik.Lozinka},
                    Specijalizacija = lekar.Specijalizacija.ToString(),
                    BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                    IdOrdinacija = lekar.IdOrdinacija,
                    FullName = lekar.FullName,
                    ImeiSpecijalizacija = lekar.ImeiSpecijalizacija,
                    Drzava = lekar.Grad.drzava.Naziv
                };
            }

            return null;
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
                    NazivGrada = lekar.Grad.Naziv,
                    Korisnik = new KorisnikDTO() { KorisnickoIme = lekar.Korisnik.KorisnickoIme, Lozinka = lekar.Korisnik.Lozinka },
                    Specijalizacija = lekar.Specijalizacija.VrstaSpecijalizacije,
                    BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                    IdOrdinacija = lekar.IdOrdinacija,
                    FullName = lekar.FullName,
                    ImeiSpecijalizacija = lekar.ImeiSpecijalizacija,
                    Drzava = lekar.Grad.drzava.Naziv
                });
            }

            return lekariDto;
        }

        public void Save(Model.Lekar lekar)
        {
            lekarServis.Save(lekar);
        }

        public void SaveAll(List<Lekar> lekari)
        {
            lekarServis.SaveAll(lekari);
        }

        public Servis.LekarServis lekarServis;

        internal int DobaviIndeksSelektovanogLekara(Object termin)
        {
           return LekarServis.getInstance().DobaviIndeksSelectovanogLekara((Termin)termin);
        }
        public void IzdajRecept(ReceptiDTO parametri)
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
                NazivGrada = lekar.Grad.Naziv,
                Korisnik = new KorisnikDTO() { KorisnickoIme = lekar.Korisnik.KorisnickoIme, Lozinka = lekar.Korisnik.Lozinka },
                Specijalizacija = lekar.Specijalizacija.VrstaSpecijalizacije,
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName,
                ImeiSpecijalizacija = lekar.ImeiSpecijalizacija,
                Drzava = lekar.Grad.drzava.Naziv
            };
        }
        public List<int> DobijTerminePijenja(String terminiPijenja)
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
                Grad = new Grad() { Naziv = lekar.NazivGrada, drzava = new Drzava(){Naziv = lekar.Drzava}},
                Korisnik = new Korisnik() { KorisnickoIme = lekar.Korisnik.KorisnickoIme, Lozinka = lekar.Korisnik.Lozinka },
                Specijalizacija = new Specijalizacija() { VrstaSpecijalizacije = lekar.Specijalizacija },
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName,
                ImeiSpecijalizacija = lekar.ImeiSpecijalizacija
            });
            return true;
        }
        public LekarDTO trenutnoUlogovaniLekar()
        {
            Lekar lekar = LekarServis.getInstance().trenutnoUlogovaniLekar();
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
                NazivGrada = lekar.Grad.Naziv,
                Korisnik = new KorisnikDTO() { KorisnickoIme = lekar.Korisnik.KorisnickoIme, Lozinka = lekar.Korisnik.Lozinka },
                Specijalizacija = lekar.Specijalizacija.VrstaSpecijalizacije,
                BrojSlobodnihDana = lekar.BrojSlobodnihDana,
                IdOrdinacija = lekar.IdOrdinacija,
                FullName = lekar.FullName,
                ImeiSpecijalizacija = lekar.ImeiSpecijalizacija,
                Drzava = lekar.Grad.drzava.Naziv
            };
        }

        public bool IzmenaLozinke(string jmbg, string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }
        public bool Validiraj(Regex sablon, String unos)
        {
            bool validno;
            if (sablon.IsMatch(unos))
                validno= true;
            else
                validno = false;
            return validno;
        }
    }
}