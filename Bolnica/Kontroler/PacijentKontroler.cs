using Bolnica.DTOs;
using Model;
using Servis;
using System;
using System.Collections.Generic;

namespace Kontroler
{
    public class PacijentKontroler : KorisnikKontroler
    {
        public PacijentServis PacijentServis;
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
            PacijentServis = new PacijentServis();
            instance = this;
        }

        public bool RegistrujPacijenta(PacijentDTO pacijent)
        {
            List<Anamneza> anamneze = new List<Anamneza>();
            if (pacijent.ZdravstveniKarton.Anamneze != null)
                foreach (AnamnezaDTO anamneza in pacijent.ZdravstveniKarton.Anamneze)
                {
                    anamneze.Add(new Anamneza()
                    {
                        AnamnezaDijalog = anamneza.AnamnezaDijalog,
                        DatumAnamneze = anamneza.DatumAnamneze,
                        IdAnamneze = anamneza.IdAnamneze,
                        ImeLekara = anamneza.ImeLekara
                    });
                }

            List<Izvestaj> izvestaji = new List<Izvestaj>();
            if (pacijent.ZdravstveniKarton.Izvestaj != null)
                foreach (IzvestajDTO izvestaj in pacijent.ZdravstveniKarton.Izvestaj)
                {
                    izvestaji.Add(new Izvestaj()
                    {
                        datum = izvestaj.datum,
                        dijagnoza = izvestaj.dijagnoza,
                        recepti = izvestaj.recepti
                    });
                }

            List<string> alergeni = new List<string>();
            if (pacijent.ZdravstveniKarton.Alergeni != null)
            {
                alergeni = pacijent.ZdravstveniKarton.Alergeni;
            }

            return PacijentServis.RegistrujPacijenta(new Pacijent()
            {
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
                Grad = new Grad() { Naziv = pacijent.NazivGrada },
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = pacijent.Korisnik.KorisnickoIme,
                    Lozinka = pacijent.Korisnik.Lozinka
                },
                Registrovan = pacijent.Registrovan,
                ZdravstveniKarton = new ZdravstveniKarton()
                {
                    Alergeni = alergeni,
                    Anamneze = anamneze,
                    Izvestaj = izvestaji
                }
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

        public Object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return PacijentServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
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
            List<Pacijent> pacijenti = PacijentServis.GetAll();
            List<PacijentDTO> pacijentiDto = new List<PacijentDTO>();
            foreach (Pacijent pacijent in pacijenti)
            {
                List<string> alergeni = new List<string>();
                List<AnamnezaDTO> anamneze = new List<AnamnezaDTO>();
                List<IzvestajDTO> izvestaji = new List<IzvestajDTO>();
                if (pacijent.ZdravstveniKarton != null)
                {
                    foreach (Anamneza anamneza in pacijent.ZdravstveniKarton.Anamneze)
                    {
                        anamneze.Add(new AnamnezaDTO()
                        {
                            AnamnezaDijalog = anamneza.AnamnezaDijalog,
                            DatumAnamneze = anamneza.DatumAnamneze,
                            IdAnamneze = anamneza.IdAnamneze,
                            ImeLekara = anamneza.ImeLekara
                        });
                    }

                    foreach (Izvestaj izvestaj in pacijent.ZdravstveniKarton.Izvestaj)
                    {
                        izvestaji.Add(new IzvestajDTO()
                        {
                            datum = izvestaj.datum,
                            dijagnoza = izvestaj.dijagnoza,
                            recepti = izvestaj.recepti
                        });
                    }

                    alergeni = pacijent.ZdravstveniKarton.Alergeni;
                }
                pacijentiDto.Add(new PacijentDTO()
                {
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
                    NazivGrada = pacijent.Grad.Naziv,
                    Korisnik = new KorisnikDTO()
                    {
                        KorisnickoIme = pacijent.Korisnik.KorisnickoIme,
                        Lozinka = pacijent.Korisnik.Lozinka
                    },
                    Registrovan = pacijent.Registrovan,
                    ZdravstveniKarton = new ZdravstveniKartonDTO()
                    {
                        Alergeni = alergeni,
                        Anamneze = anamneze,
                        Izvestaj = izvestaji
                    }
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
            List<Anamneza> anamneze = new List<Anamneza>();
            if (stariPacijent.ZdravstveniKarton.Anamneze != null)
                foreach (AnamnezaDTO anamneza in stariPacijent.ZdravstveniKarton.Anamneze)
                {
                    anamneze.Add(new Anamneza()
                    {
                        AnamnezaDijalog = anamneza.AnamnezaDijalog,
                        DatumAnamneze = anamneza.DatumAnamneze,
                        IdAnamneze = anamneza.IdAnamneze,
                        ImeLekara = anamneza.ImeLekara
                    });
                }

            List<Izvestaj> izvestaji = new List<Izvestaj>();
            if (stariPacijent.ZdravstveniKarton.Izvestaj != null)
                foreach (IzvestajDTO izvestaj in stariPacijent.ZdravstveniKarton.Izvestaj)
                {
                    izvestaji.Add(new Izvestaj()
                    {
                        datum = izvestaj.datum,
                        dijagnoza = izvestaj.dijagnoza,
                        recepti = izvestaj.recepti
                    });
                }

            List<Anamneza> anamnezeNovi = new List<Anamneza>();
            if (noviPacijent.ZdravstveniKarton.Anamneze != null)
                foreach (AnamnezaDTO anamneza in noviPacijent.ZdravstveniKarton.Anamneze)
                {
                    anamneze.Add(new Anamneza()
                    {
                        AnamnezaDijalog = anamneza.AnamnezaDijalog,
                        DatumAnamneze = anamneza.DatumAnamneze,
                        IdAnamneze = anamneza.IdAnamneze,
                        ImeLekara = anamneza.ImeLekara
                    });
                }

            List<Izvestaj> izvestajiNovi = new List<Izvestaj>();
            if (noviPacijent.ZdravstveniKarton.Izvestaj != null)
                foreach (IzvestajDTO izvestaj in noviPacijent.ZdravstveniKarton.Izvestaj)
                {
                    izvestaji.Add(new Izvestaj()
                    {
                        datum = izvestaj.datum,
                        dijagnoza = izvestaj.dijagnoza,
                        recepti = izvestaj.recepti
                    });
                }

            List<string> alergeni = new List<string>();
            if (stariPacijent.ZdravstveniKarton.Alergeni != null)
            {
                alergeni = stariPacijent.ZdravstveniKarton.Alergeni;
            }

            List<string> alergeniNovi = new List<string>();
            if (noviPacijent.ZdravstveniKarton.Alergeni != null)
            {
                alergeniNovi = noviPacijent.ZdravstveniKarton.Alergeni;
            }
            return PacijentServis.IzmeniPacijenta(new Pacijent()
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
                Grad = new Grad() { Naziv = stariPacijent.NazivGrada },
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = stariPacijent.Korisnik.KorisnickoIme,
                    Lozinka = stariPacijent.Korisnik.Lozinka
                },
                Registrovan = stariPacijent.Registrovan,
                ZdravstveniKarton = new ZdravstveniKarton()
                {
                    Alergeni = alergeni,
                    Anamneze = anamneze,
                    Izvestaj = izvestaji
                }
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
                Grad = new Grad() { Naziv = noviPacijent.NazivGrada },
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = noviPacijent.Korisnik.KorisnickoIme,
                    Lozinka = noviPacijent.Korisnik.Lozinka
                },
                Registrovan = noviPacijent.Registrovan,
                ZdravstveniKarton = new ZdravstveniKarton()
                {
                    Alergeni = alergeniNovi,
                    Anamneze = anamnezeNovi,
                    Izvestaj = izvestajiNovi
                }
            });

        }

        public List<Recept> DobaviRecepePacijenta(string jmbg)
        {
            return PacijentServis.DobaviReceptePacijenta(jmbg);
        }

        public Pacijent GetByJmbg(string jmbg)
        {
            return PacijentServis.GetByJmbg(jmbg);
        }

        public bool ObrisiPacijenta(string jmbg)
        {
            return PacijentServis.ObrisiPacijenta(jmbg);
        }
    }
}