using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bolnica.Validacije
{
    class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string emailTrimed = (String)value;
                emailTrimed.Trim();

                if (!string.IsNullOrEmpty(emailTrimed))
                {
                    bool hasWhitespace = emailTrimed.Contains(" ");

                    int indexOfAtSign = emailTrimed.LastIndexOf('@');

                    if (indexOfAtSign > 0 && !hasWhitespace)
                    {
                        string afterAtSign = emailTrimed.Substring(indexOfAtSign + 1);

                        int indexOfDotAfterAtSign = afterAtSign.LastIndexOf('.');

                        if (indexOfDotAfterAtSign > 0 && afterAtSign.Substring(indexOfDotAfterAtSign).Length > 1)
                            return new ValidationResult(true, null);
                    }
                }
                return new ValidationResult(false, "Email adresa nije u dobrom formatu.");

            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška.");
            }
        }
    }
}
