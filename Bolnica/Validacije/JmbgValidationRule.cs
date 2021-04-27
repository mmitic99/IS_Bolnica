using System;
using System.Windows.Controls;

namespace Bolnica.Validacije
{
    class JmbgValidationRule : ValidationRule
    {
        public long Min
        {
            get;
            set;
        }

        public long Max
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is String)
            {
                long d = long.Parse((string)value);
                if (d < Min) return new ValidationResult(false, "JMBG mora imati 13 cifara.");
                if (d > Max) return new ValidationResult(false, "JMBG mora imati 13 cifara.");
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Nepoznata greška.");
            }
        }
    }
}
