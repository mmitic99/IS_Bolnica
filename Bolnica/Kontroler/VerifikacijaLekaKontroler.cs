﻿using Bolnica.model;
using Bolnica.Servis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.model;
using Bolnica.Servis;

namespace Bolnica.Kontroler
{
    public class VerifikacijaLekaKontroler
    {
        private static VerifikacijaLekaKontroler instance = null;
        public static VerifikacijaLekaKontroler GetInstance()
        {
            if (instance == null)
            {
                instance = new VerifikacijaLekaKontroler();
            }
            return instance;
        }


        public void PosaljiVerifikacijuLeka(VerifikacijaLeka verifikacijaLeka)
        {
            VerifikacijaLekaServis.GetInstance().PosaljiVerifikacijuLeka(verifikacijaLeka);
        }

        public void ObrisiVerifikacijuLeka() { }

        public List<VerifikacijaLeka> GetAll()
        {
            return VerifikacijaLekaServis.GetInstance().GetAll();
        }

        public void Save(VerifikacijaLeka verifikacija)
        {
            VerifikacijaLekaServis.GetInstance().Save(verifikacija);
        }

        public void SaveAll(List<VerifikacijaLeka> verifikacije)
        {
            VerifikacijaLekaServis.GetInstance().SaveAll(verifikacije);
        }
    }
}