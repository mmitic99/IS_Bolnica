using System;
using System.Collections.Generic;
using Model;

namespace Bolnica.DTOs
{
    public class IzvestajDTO
    {
        public List<Recept> recepti { get; set; }
        public DateTime datum { get; set; }
        public String dijagnoza { get; set; }
    }
}