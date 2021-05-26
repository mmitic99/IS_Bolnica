using Bolnica.DTOs;
using Bolnica.Servis;
using Kontroler;
using Model;
using Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class AnamnezaKontroler
    {
        public static AnamnezaKontroler instance;

        public static AnamnezaKontroler GetInstance()
        {
            if (instance == null) return new AnamnezaKontroler();
            else return instance;
        }

        public AnamnezaKontroler()
        {
            instance = this;
        }
        public AnamnezaDTO getAnamnezaById(String Id)
        {
            Anamneza anamneza = AnamnezaServis.GetInstance().getAnamnezaById(Id);
            AnamnezaDTO anamnezaDTO = new AnamnezaDTO()
            {
                AnamnezaDijalog = anamneza.AnamnezaDijalog,
                DatumAnamneze = anamneza.DatumAnamneze,
                IdAnamneze = anamneza.IdAnamneze,
                ImeLekara = anamneza.ImeLekara
            };
            return anamnezaDTO;
        }
        public void IzmenaAnamneze(String id, String dijalog,PacijentDTO pacijent)
        {

            Pacijent pacijentIzmena = PacijentServis.GetInstance().GetByJmbg(pacijent.Jmbg);
            AnamnezaServis.GetInstance().IzmenaAnamneze(id,dijalog,pacijentIzmena);
        }
    }
}
