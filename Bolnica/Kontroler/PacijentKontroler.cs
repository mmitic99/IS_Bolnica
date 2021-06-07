using Bolnica.DTOs;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using Model.Enum;
using Bolnica.Repozitorijum.XmlSkladiste;

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
                FullName = pacijent.FullName,
                Grad = new Grad() { Naziv = pacijent.NazivGrada },
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = pacijent.Korisnik.KorisnickoIme,
                    Lozinka = pacijent.Korisnik.Lozinka,
                    DatumKreiranjaNaloga = DateTime.Now
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

        public void SacuvajKomentarNaDijagnozu(Recept IzabraniRecept, Pacijent pacijent)
        {
            PacijentServis.SacuvajKomentarNaDijagnozu(IzabraniRecept, pacijent);
        }

        public Object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            return PacijentServis.PrijavljivanjeKorisnika(korisnickoIme, lozinka);
        }

        public bool IzmenaLozinke(string jmbg, string staraLozinka, string novaLozinka)
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
                    FullName = pacijent.FullName,
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
                    },
                    datumKreiranjaNaloga = pacijent.Korisnik.DatumKreiranjaNaloga
                });

            }

            return pacijentiDto;
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
                FullName = stariPacijent.FullName,
                Grad = new Grad() { Naziv = stariPacijent.NazivGrada },
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = stariPacijent.Korisnik.KorisnickoIme,
                    Lozinka = stariPacijent.Korisnik.Lozinka,
                    DatumKreiranjaNaloga = stariPacijent.datumKreiranjaNaloga
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
                FullName = noviPacijent.FullName,
                Grad = new Grad() { Naziv = noviPacijent.NazivGrada },
                Korisnik = new Korisnik()
                {
                    KorisnickoIme = noviPacijent.Korisnik.KorisnickoIme,
                    Lozinka = noviPacijent.Korisnik.Lozinka,
                    DatumKreiranjaNaloga = stariPacijent.datumKreiranjaNaloga
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

        public PacijentDTO GetByJmbg(string jmbg)
        {
            Pacijent pacijent = PacijentServis.GetByJmbg(jmbg);
            List<AnamnezaDTO> anamneze = new List<AnamnezaDTO>();
            if (pacijent.ZdravstveniKarton.Anamneze != null)
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

            List<IzvestajDTO> izvestaji = new List<IzvestajDTO>();
            if (pacijent.ZdravstveniKarton.Izvestaj != null)
                foreach (Izvestaj izvestaj in pacijent.ZdravstveniKarton.Izvestaj)
                {
                    izvestaji.Add(new IzvestajDTO()
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
            PacijentDTO pacijentDTO = new PacijentDTO
            {
                Adresa = pacijent.Adresa,
                BracnoStanje = pacijent.BracnoStanje,
                BrojTelefona = pacijent.BrojTelefona,
                DatumRodjenja = pacijent.DatumRodjenja,
                Email = pacijent.Email,
                Ime = pacijent.Ime,
                Jmbg = pacijent.Jmbg,
                Pol = pacijent.Pol,
                Prezime = pacijent.Prezime,
                Zanimanje = pacijent.Zanimanje,
                NazivGrada = pacijent.Grad.Naziv,
                FullName = pacijent.FullName,
                Korisnik = new KorisnikDTO()
                {
                    KorisnickoIme = pacijent.Korisnik.KorisnickoIme,
                    Lozinka = pacijent.Korisnik.Lozinka
                },
                Registrovan = pacijent.Registrovan,
                ZdravstveniKarton = new ZdravstveniKartonDTO
                {
                    Alergeni = alergeni,
                    Anamneze = anamneze,
                    Izvestaj = izvestaji
                },
                datumKreiranjaNaloga = pacijent.Korisnik.DatumKreiranjaNaloga
            };
            return pacijentDTO;
            


        }

        public bool ObrisiPacijenta(string jmbg)
        {
            return PacijentServis.ObrisiPacijenta(jmbg);
        }

        public int GetBrojMuskihPacijenata()
        {
            int broj = 0;

            foreach (Pacijent pacijent in PacijentServis.GetAll())
            {
                if (pacijent.Pol == Pol.Muski) broj++;
            }

            return broj;
        }

        public int GetBrojZenskihPacijenata()
        {
            int broj = 0;

            foreach (Pacijent pacijent in PacijentServis.GetAll())
            {
                if (pacijent.Pol == Pol.Zenski) broj++;
            }

            return broj;
        }

        public IEnumerable<int> GetBrojNovihPacijenataUMesecu(List<string> sviDaniUMesecu)
        {
            List<int> brojNovihPacijenataPoDanu = new List<int>(new int[sviDaniUMesecu.Count]);
            foreach (Pacijent pacijent in PacijentServis.GetAll())
            {
                for (int dan = 1; dan <= sviDaniUMesecu.Count; dan++)
                {
                    if (pacijent.Korisnik.DatumKreiranjaNaloga.Year == DateTime.Now.Year &&
                        pacijent.Korisnik.DatumKreiranjaNaloga.Month == DateTime.Now.Month && pacijent.Korisnik.DatumKreiranjaNaloga.Day == dan)
                    {
                        brojNovihPacijenataPoDanu[dan - 1]++;
                    }
                }
            }
            return brojNovihPacijenataPoDanu;
        }
        
        }
}