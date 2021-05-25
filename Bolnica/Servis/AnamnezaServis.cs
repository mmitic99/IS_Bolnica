using Bolnica.DTOs;
using Bolnica.Repozitorijum;
using Bolnica.Repozitorijum.XmlSkladiste;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Servis
{
    public class AnamnezaServis
    {
        public static AnamnezaServis instance;
        
        public static AnamnezaServis GetInstance()
        {
            if (instance == null) return new AnamnezaServis();
            else return instance;
        }

        public AnamnezaServis()
        {
            instance = this;
        }
        public Anamneza getAnamnezaById(String Id)
        {
            List<Anamneza> sveAnamneze = new List<Anamneza>();
            foreach (Pacijent pacijent in SkladistePacijentaXml.GetInstance().GetAll())
            {
                foreach(Anamneza anamneza in pacijent.ZdravstveniKarton.Anamneze)
                {
                    sveAnamneze.Add(anamneza);
                }
            }
            Anamneza anamnezaNova = new Anamneza();
            foreach (Anamneza anamneza in sveAnamneze)
            {
                if (anamneza.IdAnamneze != null)
                {
                    if (anamneza.IdAnamneze.Equals(Id))
                    {
                        anamnezaNova = anamneza;
                        break;
                    }
                }
            }
            return anamnezaNova;
        }
        public Anamneza IzmenaAnamneze(String id, String dijalog)
        {

            Anamneza anamneza = AnamnezaServis.GetInstance().getAnamnezaById(id);
            anamneza.AnamnezaDijalog = dijalog;
            return anamneza;
        }
    }
}
