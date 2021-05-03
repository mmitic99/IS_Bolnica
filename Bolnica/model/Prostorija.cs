using Model.Enum;
using System;
using System.Collections.Generic;

namespace Model
{
    public class Prostorija
    {
        public int IdProstorije { get; set; }
        public int Sprat { get; set; }
        public String BrojSobe { get; set; }
        public VrstaProstorije VrstaProstorije { get; set; }
        private bool RenoviraSe;
        private double Kvadratura;
        private List<StacionarnaOprema> Staticka { get; set; }
        private List<PotrosnaOprema> Potrosna { get; set; }

        public void RenovirajProstoriju()
        {
            // TODO: implement
        }

        public void ZavrsiRenoviranje()
        {
            // TODO: implement
        }

        public InventarZaProstorije inventarZaProstorije;
        public System.Collections.ArrayList termin;


        public Prostorija() {}

        public Prostorija(String BrojProstorije, int Sprat, Model.Enum.VrstaProstorije Vrsta, double Kvadratura)
        {
            this.BrojSobe = BrojProstorije;
            this.Sprat = Sprat;
            this.VrstaProstorije = Vrsta;
            this.Kvadratura = Kvadratura;
        }

        public Prostorija(int s, String brs)
        {
            this.inventarZaProstorije = new InventarZaProstorije();
            this.termin = new System.Collections.ArrayList();
            this.VrstaProstorije = VrstaProstorije.Soba_za_preglede;
            this.Sprat = s;
            this.BrojSobe = brs;
        }

        public System.Collections.ArrayList GetTermin()
        {
            if (termin == null)
                termin = new System.Collections.ArrayList();
            return termin;
        }


        public void SetTermin(System.Collections.ArrayList newTermin)
        {
            RemoveAllTermin();
            foreach (Termin oTermin in newTermin)
                AddTermin(oTermin);
        }


        public void AddTermin(Termin newTermin)
        {
            if (newTermin == null)
                return;
            if (this.termin == null)
                this.termin = new System.Collections.ArrayList();
            if (!this.termin.Contains(newTermin))
            {
                this.termin.Add(newTermin);
                newTermin.SetProstorija(this);
            }
        }


        public void RemoveTermin(Termin oldTermin)
        {
            if (oldTermin == null)
                return;
            if (this.termin != null)
                if (this.termin.Contains(oldTermin))
                {
                    this.termin.Remove(oldTermin);
                    oldTermin.SetProstorija((Prostorija)null);
                }
        }


        public void RemoveAllTermin()
        {
            if (termin != null)
            {
                System.Collections.ArrayList tmpTermin = new System.Collections.ArrayList();
                foreach (Termin oldTermin in termin)
                    tmpTermin.Add(oldTermin);
                termin.Clear();
                foreach (Termin oldTermin in tmpTermin)
                    oldTermin.SetProstorija((Prostorija)null);
                tmpTermin.Clear();
            }
        }


        public int Sprat_
        {
            get
            {
                return Sprat;
            }
            set
            {
                if (value != Sprat)
                {
                    Sprat = value;
                }
            }
        }

        public String BrojSobe_
        {
            get
            {
                return BrojSobe;
            }
            set
            {
                if (value != BrojSobe)
                {
                    BrojSobe = value;
                }
            }
        }

        public VrstaProstorije VrstaProstorije_
        {
            get
            {
                return VrstaProstorije;
            }
            set
            {
                if (value != VrstaProstorije)
                {
                    VrstaProstorije = value;
                }
            }
        }

        public bool RenoviraSe_
        {
            get
            {
                return RenoviraSe;
            }
            set
            {
                if (value != RenoviraSe)
                {
                    RenoviraSe = value;
                }
            }
        }

        public double Kvadratura_
        {
            get
            {
                return Kvadratura;
            }
            set
            {
                if (value != Kvadratura)
                {
                    Kvadratura = value;
                }
            }
        }
        
        public List<StacionarnaOprema> Staticka_
        {
            get
            {
                return Staticka;
            }
            set
            {
                if (value != Staticka)
                {
                    Staticka = value;
                }
            }
        }

        public List<PotrosnaOprema> Potrosna_
        {
            get
            {
                return Potrosna;
            }
            set
            {
                if (value != Potrosna)
                {
                    Potrosna = value;
                }
            }
        }

        public int IdProstorije_
        {
            get
            {
                return IdProstorije;
            }
            set
            {
                if (value != IdProstorije)
                {
                    IdProstorije = value;
                }
            }
        }
    }

}