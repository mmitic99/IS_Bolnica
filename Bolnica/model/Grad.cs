using System;

namespace Model
{
    public class Grad
    {
        public Drzava drzava { get; set; }

        public String Naziv { get; set; }
        public String PostanskiBroj { get; set; }

        public Grad()
        {
            this.drzava = new Drzava();
        }

    }
}