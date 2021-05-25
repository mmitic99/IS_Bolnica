using Bolnica.model;
using System;
using System.Windows;
using Bolnica.DTOs;

namespace Model
{
    public class Obavestenje : Notifikacija
    { 
        public DateTime kvartalnaAnketa { get; set; }
        public PrikacenaAnketaPoslePregledaDTO anketaOLekaru {get; set;}

        public Obavestenje()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is Obavestenje obavestenje &&
                   VremeObavestenja == obavestenje.VremeObavestenja &&
                   Naslov == obavestenje.Naslov &&
                   Sadrzaj == obavestenje.Sadrzaj &&
                   JmbgKorisnika == obavestenje.JmbgKorisnika &&
                   kvartalnaAnketa.Equals(obavestenje.kvartalnaAnketa);
        }
    }
}