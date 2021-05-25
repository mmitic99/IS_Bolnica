using System.Globalization;
using System.Windows.Controls;

namespace Bolnica.Validacije
{
    public class PraznoPoljeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value != null)
                {
                    var s = value as string;
                    if (s.Trim().Length > 0)
                        return new ValidationResult(true, null);
                }

                return new ValidationResult(false, "Polje ne može biti prazno.");
            }
            catch
            {
                return new ValidationResult(false, "Nepoznata greška.");
            }
        }
    }
}