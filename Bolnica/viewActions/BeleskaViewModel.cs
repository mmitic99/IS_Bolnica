using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bolnica.viewActions
{
    public class BeleskaViewModel : ValidationBase
    {
        private string naslov;
        private string opis;
        public string Naslov
        {
            get { return naslov; }
            set
            {
                if (naslov != value)
                {
                    naslov = value;
                    OnPropertyChanged("Naslov");
                }
            }
        }

        public string Opis
        {
            get { return opis; }
            set
            {
                if (opis != value)
                {
                    opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Naslov))
            {
                this.ValidationErrors["Naslov"] = "Naslov je neophodan.";
            }
            if (string.IsNullOrWhiteSpace(this.Opis))
            {
                this.ValidationErrors["Opis"] = "Opis ne može da bude prazan.";
            }
        }
    }
}