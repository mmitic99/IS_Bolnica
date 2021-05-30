using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Linq;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.DTOs;

namespace Servis
{
    public class SekretarServis : KorisnikServis
    {
        public SekretarServis()
        {
            skladisteSekretara = SkladisteSekretaraXml.GetInstance();
        }

        public bool RegistrujSekretara(Sekretar sekretar)
        {
            List<Sekretar> sekretari = skladisteSekretara.GetAll();

            for (int i = 0; i < sekretari.Count; i++)
            {
                if (sekretari.ElementAt(i).Jmbg.Equals(sekretar.Jmbg))
                {
                    return false;
                }
            }
            skladisteSekretara.Save(sekretar);
            return true;
        }

        public object PrijavljivanjeKorisnika(string korisnickoIme, string lozinka)
        {
            List<Sekretar> sekretari = skladisteSekretara.GetAll();

            Sekretar sekretar = new Sekretar();

            foreach (Sekretar sekretar1 in sekretari)
            {
                if (sekretar1.Korisnik.KorisnickoIme.Equals(korisnickoIme))
                {
                    sekretar = sekretar1;
                    if (sekretar1.Korisnik.Lozinka.Equals(lozinka))
                    {
                        return sekretar;
                    }
                }
            }
            return null;
        }

        public bool IzmenaLozinke(string staraLozinka, string novaLozinka)
        {
            throw new NotImplementedException();
        }

        public bool IzmenaLozinke(string jmbgSekretara, string staraLozinka, string novaLozinka)
        {
            Sekretar sekretar = skladisteSekretara.GetByJmbg(jmbgSekretara);

            ObrisiSekretara(jmbgSekretara);
            sekretar.Korisnik.Lozinka = novaLozinka;
            skladisteSekretara.Save(sekretar);

            return true;
        }

        public bool IzmenaKorisnickogImena(string staroKorisnickoIme, string novoKorisnickoIme)
        {
            throw new NotImplementedException();
        }

        public List<Sekretar> GetAll()
        {
            return skladisteSekretara.GetAll();
        }

        public void Save(Model.Sekretar sekretar)
        {
            skladisteSekretara.Save(sekretar);
        }

        public void SaveAll(List<Sekretar> sekretari)
        {
            skladisteSekretara.SaveAll(sekretari);
        }

        public ISkladisteSekretara skladisteSekretara;

        public bool IzmeniSekretara(string sekretarJmbg, Sekretar noviSekretar)
        {
            List<Sekretar> pacijenti = skladisteSekretara.GetAll();
            bool uspesno = ObrisiSekretara(sekretarJmbg);
            if (uspesno)
            {
                uspesno = RegistrujSekretara(noviSekretar);
            }
            return uspesno;
        }

        private bool ObrisiSekretara(string sekretarJmbg)
        {
            List<Sekretar> sekretari = skladisteSekretara.GetAll();
            bool uspesno = false;
            foreach (Sekretar sekretar in sekretari)
            {
                if (sekretar.Jmbg.Equals(sekretarJmbg))
                {
                    uspesno = sekretari.Remove(sekretar);
                    skladisteSekretara.SaveAll(sekretari);
                    break;
                }
            }
            return uspesno;
        }
    }
}