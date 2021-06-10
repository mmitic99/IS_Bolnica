using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum;
using Bolnica.model;
using Bolnica.Repozitorijum.ISkladista;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.Repozitorijum.Factory.VerifikacijaLekaFactory;

namespace Bolnica.Servis
{
    public class VerifikacijaLekaServis : IVerifikacijaLekaServis
    {
        private static VerifikacijaLekaServis instance = null;
        public ISkladisteZaVerifikacijuLeka skladisteZaVerifikacijuLeka;
        
        public static VerifikacijaLekaServis GetInstance()
        {
            if (instance == null)
            {
                instance = new VerifikacijaLekaServis();
            }
            return instance;
        }

        public VerifikacijaLekaServis()
        {
            skladisteZaVerifikacijuLeka = new SkladisteZaVerifikacijuLekaXml();
        }

        public void PosaljiVerifikacijuLeka(IVerifikacijaLeka verifikacijaLeka)
        {
           List<IVerifikacijaLeka> SveVerifikacijeLeka = skladisteZaVerifikacijuLeka.GetAll().Cast<IVerifikacijaLeka>().ToList(); 
            SveVerifikacijeLeka.Add(verifikacijaLeka);
            skladisteZaVerifikacijuLeka.SaveAll(SveVerifikacijeLeka.Cast<VerifikacijaLeka>().ToList());
        }

        public void ObrisiVerifikacijuLeka(String idVerifikacije)
        {
            List<IVerifikacijaLeka> SveVerifikacije = skladisteZaVerifikacijuLeka.GetAll().Cast<IVerifikacijaLeka>().ToList();
            for (int i = 0; i < SveVerifikacije.Count; i++)
            {
                if (idVerifikacije.Equals(SveVerifikacije[i].IdVerifikacijeLeka))
                {
                    SveVerifikacije.RemoveAt(i);
                    break;
                }
            }
            skladisteZaVerifikacijuLeka.SaveAll(SveVerifikacije.Cast<VerifikacijaLeka>().ToList());
        }

        public List<IVerifikacijaLeka> GetAll()
        {
            return skladisteZaVerifikacijuLeka.GetAll().Cast<IVerifikacijaLeka>().ToList();
        }

        public List<IVerifikacijaLeka> GetObavestenjaByJmbg(String jmbg)
        {
            return skladisteZaVerifikacijuLeka.GetObavestenjaByJmbg(jmbg).Cast<IVerifikacijaLeka>().ToList();
        }

        public void Save(IVerifikacijaLeka verifikacija)
        {
            IVerifikacijaLeka verifikacijaNova = VerifikacijaLekaFactory.CreateVerifikacijaLeka();
            skladisteZaVerifikacijuLeka.Save((VerifikacijaLeka)verifikacijaNova);
        }

        public void SaveAll(List<IVerifikacijaLeka> verifikacije)
        {
            skladisteZaVerifikacijuLeka.SaveAll(verifikacije.Cast<VerifikacijaLeka>().ToList());
        }
    }
}
