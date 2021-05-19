using Bolnica.viewActions;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Bolnica.DTOs;
using Bolnica.model;

namespace Repozitorijum
{
    public class SkladisteZaTermine
    {
        public String Lokacija { get; set; }
        private List<Termin> termini;

        private static SkladisteZaTermine instance = null;

        public static SkladisteZaTermine getInstance()
        {
            if (instance == null)
            {
                instance = new SkladisteZaTermine();
            }
            return instance;
        }


        public SkladisteZaTermine()
        {
            this.termini = new List<Termin>();
            Lokacija = "..\\..\\SkladistePodataka\\termini.xml";
        }

        public List<Termin> GetAll()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));


            if (File.Exists(Lokacija))
            {
                StreamReader reader = new StreamReader(Lokacija);
                termini = (List<Termin>)serializer.Deserialize(reader);
                reader.Close();
            }
            return termini;
        }

        public List<TerminPacijentLekarDTO> GetBuduciTerminPacLekar()
        {

            List<TerminPacijentLekarDTO> termini = new List<TerminPacijentLekarDTO>();
            foreach (Termin termin in SkladisteZaTermine.getInstance().GetAll())
            {
                if (termin.DatumIVremeTermina >= DateTime.Now)
                {
                    Pacijent pacijent = SkladistePacijenta.GetInstance().getByJmbg(termin.JmbgPacijenta);
                    Lekar lekar = SkladisteZaLekara.GetInstance().getByJmbg(termin.JmbgLekara);
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
                    TerminPacijentLekarDTO termin1 = new TerminPacijentLekarDTO { termin = new TerminDTO()
                    {
                        JmbgLekara = termin.JmbgLekara,
                        IDTermina = termin.IDTermina,
                        brojSobe = termin.brojSobe,
                        VrstaTermina = termin.VrstaTermina,
                        TrajanjeTermina = termin.TrajanjeTermina,
                        opisTegobe = termin.opisTegobe,
                        JmbgPacijenta = termin.JmbgPacijenta,
                        DatumIVremeTermina = termin.DatumIVremeTermina,
                        IdProstorije = termin.IdProstorije
                    }, pacijent = new PacijentDTO()
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
                    }, lekar =  new LekarDTO()
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
                        FullName = lekar.FullName
                    }
                    };
                    termini.Add(termin1);
                }
            }

            return termini;
        }


        public List<Termin> getByJmbg(String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            List<Termin> sviTerminiSaProstorijama = new List<Termin>();
            foreach (Termin t in sviTermini)
            {
              //  t.IdProstorije = SkladisteZaLekara.GetInstance().getByJmbg(t.JmbgLekara).IdOrdinacija;
              //  sviTerminiSaProstorijama.Add(t);
                if (t.JmbgPacijenta.Equals(jmbg))
                {
                    odgovTermini.Add(t);
                }
            }
           // this.SaveAll(sviTerminiSaProstorijama);
            return odgovTermini;
        }
        public List<Termin> getByDateForLekar(DateTime datum,String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.JmbgLekara.Equals(jmbg) & t.DatumIVremeTermina.Date.Equals(datum.Date))
                {
                    odgovTermini.Add(t);
                }
            }
            return odgovTermini;
        }
        public List<Termin> getByJmbgLekar(String jmbg)
        {
            List<Termin> odgovTermini = new List<Termin>();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.JmbgLekara.Equals(jmbg))
                {
                    odgovTermini.Add(t);
                }
            }
            return odgovTermini;
        }

        public Termin getById(String id)
        {
            Termin ter = new Termin();
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.IDTermina.Equals(id))
                {
                    ter = t;
                    break;
                }
            }
            return ter;
        }

        public void RemoveByID(String id)
        {
            List<Termin> sviTermini = this.GetAll();
            foreach (Termin t in sviTermini)
            {
                if (t.IDTermina.Equals(id))
                {
                    sviTermini.Remove(t);
                    break;
                }
            }
            this.SaveAll(sviTermini);
        }


        public void Save(Termin termin)
        {
            termini = GetAll();

            termini.Add(termin);

            SaveAll(termini);
        }

        public void SaveAll(List<Termin> termini)
        {
            StreamWriter stream = new StreamWriter(Lokacija);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Termin>));
                serializer.Serialize(stream, termini);
            }
            finally
            {
                stream.Close();
            }
        }

    }


}