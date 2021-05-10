﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.Repozitorijum;
using Bolnica.model;

namespace Bolnica.Servis
{
    public class VerifikacijaLekaServis
    {
        private static VerifikacijaLekaServis instance = null;
        public static VerifikacijaLekaServis GetInstance()
        {
            if (instance == null)
            {
                instance = new VerifikacijaLekaServis();
            }
            return instance;
        }

        public void PosaljiVerifikacijuLeka(VerifikacijaLeka verifikacijaLeka)
        {
            List<VerifikacijaLeka> SveVerifikacijeLeka = SkladisteZaVerifikacijuLeka.GetInstance().GetAll();
            SveVerifikacijeLeka.Add(verifikacijaLeka);
            SkladisteZaVerifikacijuLeka.GetInstance().SaveAll(SveVerifikacijeLeka);
        }
        public void ObrisiVerifikacijuLeka() { }
        public List<VerifikacijaLeka> GetAll()
        {
            return SkladisteZaVerifikacijuLeka.GetInstance().GetAll();
        }

        public void Save(VerifikacijaLeka verifikacija)
        {
            SkladisteZaVerifikacijuLeka.GetInstance().Save(verifikacija);
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            SkladisteZaVerifikacijuLeka.GetInstance().SaveAll(verifikacije);
        }
    }
}