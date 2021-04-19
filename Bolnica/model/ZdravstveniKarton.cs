using System;
using System.Collections.Generic;

namespace Model
{
    public class ZdravstveniKarton
    {
        public System.Collections.ArrayList izvestaj;
        public List<Izvestaj> izvestaji { get; set; }


        public ZdravstveniKarton()
        {
            izvestaj = new System.Collections.ArrayList();
            Alergeni = new List<string>();
        }
        public System.Collections.ArrayList GetIzvestaj()
        {
            if (izvestaj == null)
                izvestaj = new System.Collections.ArrayList();
            return izvestaj;
        }


        public void SetIzvestaj(System.Collections.ArrayList newIzvestaj)
        {
            RemoveAllIzvestaj();
            foreach (Izvestaj oIzvestaj in newIzvestaj)
                AddIzvestaj(oIzvestaj);
        }


        public void AddIzvestaj(Izvestaj newIzvestaj)
        {
            if (newIzvestaj == null)
                return;
            if (this.izvestaj == null)
                this.izvestaj = new System.Collections.ArrayList();
            if (!this.izvestaj.Contains(newIzvestaj))
                this.izvestaj.Add(newIzvestaj);
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

    }
}