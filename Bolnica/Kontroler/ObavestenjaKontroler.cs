using Model;
using Servis;
using System;
using System.Collections.Generic;
using Bolnica.DTOs;

namespace Kontroler
{
    public class ObavestenjaKontroler
    {
        public static ObavestenjaKontroler instance =null;

        public static ObavestenjaKontroler getInstance()
        {
            if(instance==null)
            {
                return new ObavestenjaKontroler();
            }
            else
            {
                return instance;
            }
        }
        public ObavestenjaKontroler()
        {
            instance = this;
            obavestenjaServis = new ObavestenjaServis();
        }



        public List<Obavestenje> GetAll()
        {
            return obavestenjaServis.GetAll();
        }

        public bool Save(ObavestenjeDTO obavestenje)
        {
            obavestenjaServis.Save(new Obavestenje()
            {
                JmbgKorisnika = obavestenje.JmbgKorisnika,
                Naslov = obavestenje.Naslov,
                Podsetnik = obavestenje.Podsetnik,
                Sadrzaj = obavestenje.Sadrzaj,
                Vidjeno = obavestenje.Vidjeno,
                VremeObavestenja = obavestenje.VremeObavestenja,
                anketaOLekaru = obavestenje.anketaOLekaru,
                kvartalnaAnketa = obavestenje.kvartalnaAnketa
            });
            return true;
        }

        public void SaveAll(List<Obavestenje> obavestenje)
        {
            obavestenjaServis.SaveAll(obavestenje);
        }

        public List<ObavestenjeDTO> GetByJmbg(string jmbg)
        {
            List<Obavestenje> obavestenja = obavestenjaServis.GetByJmbg(jmbg);
            List<ObavestenjeDTO> obavestenjaDto = new List<ObavestenjeDTO>();
            foreach (Obavestenje obavestenje in obavestenja)
            {
                obavestenjaDto.Add(new ObavestenjeDTO(){
                    JmbgKorisnika = obavestenje.JmbgKorisnika,
                    Naslov = obavestenje.Naslov,
                    Podsetnik = obavestenje.Podsetnik,
                    Sadrzaj = obavestenje.Sadrzaj,
                    Vidjeno = obavestenje.Vidjeno,
                    VremeObavestenja = obavestenje.VremeObavestenja,
                    anketaOLekaru = obavestenje.anketaOLekaru,
                    kvartalnaAnketa = obavestenje.kvartalnaAnketa
                });
            }

            return obavestenjaDto;
        }

        public List<Obavestenje> GetPodsetnici(string jmbg)
        {
            return ObavestenjaServis.getInstance().GetPodsetnici(jmbg);
        }

        public bool NapraviPodsetnik(string jmbgPacijenta, Recept r, int hours)
        {
            return ObavestenjaServis.getInstance().NapraviPodsetnik(jmbgPacijenta, r, hours);
        }

        public List<Obavestenje> DobaviPodsetnikeZaTerapiju(string jmbgPacijenta)
        {
            return ObavestenjaServis.getInstance().DobaviPodsetnikeZaTerapiju(jmbgPacijenta);
        }

        public bool IzmeniObavestenje(ObavestenjeDTO staroObavestenje, ObavestenjeDTO novoObavestenje)
        {
            return obavestenjaServis.IzmeniObavestenje(new Obavestenje()
            {
                JmbgKorisnika = staroObavestenje.JmbgKorisnika,
                Naslov = staroObavestenje.Naslov,
                Podsetnik = staroObavestenje.Podsetnik,
                Sadrzaj = staroObavestenje.Sadrzaj,
                Vidjeno = staroObavestenje.Vidjeno,
                VremeObavestenja = staroObavestenje.VremeObavestenja,
                anketaOLekaru = staroObavestenje.anketaOLekaru,
                kvartalnaAnketa = staroObavestenje.kvartalnaAnketa
            }, new Obavestenje()
            {
                JmbgKorisnika = novoObavestenje.JmbgKorisnika,
                Naslov = novoObavestenje.Naslov,
                Podsetnik = novoObavestenje.Podsetnik,
                Sadrzaj = novoObavestenje.Sadrzaj,
                Vidjeno = novoObavestenje.Vidjeno,
                VremeObavestenja = novoObavestenje.VremeObavestenja,
                anketaOLekaru = novoObavestenje.anketaOLekaru,
                kvartalnaAnketa = novoObavestenje.kvartalnaAnketa
            });
        }

        public Servis.ObavestenjaServis obavestenjaServis;

        public bool ObrisiObavestenje(ObavestenjeDTO obavestenje)
        {
            return obavestenjaServis.ObrisiObavestenje(new Obavestenje()
            {
                JmbgKorisnika = obavestenje.JmbgKorisnika, Naslov = obavestenje.Naslov,
                Podsetnik = obavestenje.Podsetnik, Sadrzaj = obavestenje.Sadrzaj, Vidjeno = obavestenje.Vidjeno,
                VremeObavestenja = obavestenje.VremeObavestenja, anketaOLekaru = obavestenje.anketaOLekaru,
                kvartalnaAnketa = obavestenje.kvartalnaAnketa
            });
        }

        internal void PosaljiKvartalnuAnketu()
        {
            ObavestenjaServis.getInstance().PosaljiKvartalnuAnketu();
        }

        internal void PosaljiAnketuOLekaru(string JmbgPacijenta, string JmbgLekara)
        {
            ObavestenjaServis.getInstance().PosaljiAnketuOLekaru(JmbgPacijenta, JmbgLekara);
        }
    }
}