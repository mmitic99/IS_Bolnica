using System;
using System.Collections.Generic;

namespace Model
{
    public class ZdravstveniKarton
    {
        public List<Izvestaj> izvestaj;

        public ZdravstveniKarton()
        {
            izvestaj = new List<Izvestaj>();
            Alergeni = new List<string>();
        }
        public List<Izvestaj> GetIzvestaj()
        {
            if (izvestaj == null)
                izvestaj = new List<Izvestaj>();
            return izvestaj;
        }


        public void SetIzvestaj(List<Izvestaj> newIzvestaj)
        {
            RemoveAllIzvestaj();
            foreach (Izvestaj oIzvestaj in newIzvestaj)
                AddIzvestaj(oIzvestaj);
        }


        public void AddIzvestaj(Izvestaj newIzvestaj)
        {
            /*if (newIzvestaj == null)
                return;
            if (this.izvestaj == null)
                this.izvestaj = List<Izvestaj>();
            if (!this.izvestaj.Contains(newIzvestaj))
                this.izvestaj.Add(newIzvestaj);
            */

        }


        public void RemoveIzvestaj(Izvestaj oldIzvestaj)
        {
            if (oldIzvestaj == null)
                return;
            if (this.izvestaj != null)
                if (this.izvestaj.Contains(oldIzvestaj))
                    this.izvestaj.Remove(oldIzvestaj);
        }


        public void RemoveAllIzvestaj()
        {
            if (izvestaj != null)
                izvestaj.Clear();
        }

        public List<String> Alergeni { get; set; }
        public List<Izvestaj> Izvestaji { get; set; }
    }
}