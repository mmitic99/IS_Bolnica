using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.DTOs
{
    public class UpravnikDTO : Radnik
    {
        public UpravnikDTO()
        {
            Ime = "Mihailo";
            Prezime = "Majstorovic";
            Jmbg = "1903999772025";
        }
    }
}
