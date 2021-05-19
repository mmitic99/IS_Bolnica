using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolnica.DTOs;
using Bolnica.model;

namespace Bolnica.viewActions
{
    class MoguciTerminiViewModel
    {
        public List<TerminDTO> terminiZaPrikazivanje;
        public string pozivaoc { get; set; }
        public string jmbg { get; set; }
        public MoguciTerminiViewModel(List<TerminDTO> terminiZaPrikazivanje = null, string pocetna = null, string jmbg = null)
        {
            this.jmbg = jmbg;
            if(terminiZaPrikazivanje!=null)
            {
                this.terminiZaPrikazivanje = terminiZaPrikazivanje;
            } 
            else
            {
                this.terminiZaPrikazivanje = new List<TerminDTO>();
            }
            this.pozivaoc = pocetna;
        }
    }
}
