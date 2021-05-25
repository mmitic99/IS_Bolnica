using Bolnica.DTOs;
using Bolnica.model;
using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.Kontroler
{
    public class ZakazanaPreraspodelaStatickeOpremeKontroler
    {
        private static ZakazanaPreraspodelaStatickeOpremeKontroler instance = null;

        public static ZakazanaPreraspodelaStatickeOpremeKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new ZakazanaPreraspodelaStatickeOpremeKontroler();
            }
            return instance;
        }

        public void ZakaziPreraspodeluStatickeOpreme(ZakazanaPreraspodelaStatickeOpremeDTO preraspodelaDTO)
        {
            ZakazanaPreraspodelaStatickeOpreme preraspodela = new ZakazanaPreraspodelaStatickeOpreme
                                                                  (
                                                                  preraspodelaDTO.BrojProstorijeIzKojeSePrenosiOprema,
                                                                  preraspodelaDTO.BrojProstorijeUKojuSePrenosiOprema,
                                                                  preraspodelaDTO.DatumIVremePreraspodele,
                                                                  preraspodelaDTO.TrajanjePreraspodele,
                                                                  preraspodelaDTO.NazivOpreme,
                                                                  preraspodelaDTO.KolicinaOpreme
                                                                  );
            ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().ZakaziPreraspodeluStatickeOpreme(preraspodela);
        }

        public void OtkaziPreraspodeluStatickeOpreme(int index)
        {
            ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().OtkaziPreraspodeluStatickeOpreme(index);
        }

        public bool ProveraValidnostiPreraspodeleOpreme(String trajanje)
        {
            return ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().ProveraValidnostiPreraspodeleOpreme(trajanje);
        }

        public List<ZakazanaPreraspodelaStatickeOpremeDTO> GetAll()
        {
            List<ZakazanaPreraspodelaStatickeOpremeDTO> preraspodele = new List<ZakazanaPreraspodelaStatickeOpremeDTO>();
            foreach (ZakazanaPreraspodelaStatickeOpreme preraspodela in ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().GetAll())
            {
                preraspodele.Add(new ZakazanaPreraspodelaStatickeOpremeDTO()
                {
                    BrojProstorijeIzKojeSePrenosiOprema = preraspodela.BrojProstorijeIzKojeSePrenosiOprema,
                    BrojProstorijeUKojuSePrenosiOprema = preraspodela.BrojProstorijeUKojuSePrenosiOprema,
                    DatumIVremePreraspodele = preraspodela.DatumIVremePreraspodele,
                    TrajanjePreraspodele = preraspodela.TrajanjePreraspodele,
                    NazivOpreme = preraspodela.NazivOpreme,
                    KolicinaOpreme = preraspodela.KolicinaOpreme,
                    IdProstorijeIzKojeSePrenosiOprema = preraspodela.IdProstorijeIzKojeSePrenosiOprema,
                    IdProstorijeUKojUSePrenosiOprema = preraspodela.IdProstorijeUKojUSePrenosiOprema
                });
            }
            return preraspodele;
        }

        public void Save(ZakazanaPreraspodelaStatickeOpreme zakazanaPreraspodela)
        {
            ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().Save(zakazanaPreraspodela);
        }

        public void SaveAll(List<ZakazanaPreraspodelaStatickeOpreme> zakazanePreraspodele)
        {
            ZakazanaPreraspodelaStatickeOpremeServis.GetInstance().SaveAll(zakazanePreraspodele);
        }
    }
}


