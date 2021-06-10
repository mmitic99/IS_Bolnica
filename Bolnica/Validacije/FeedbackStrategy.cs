using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bolnica.Validacije
{
    public class FeedbackStrategy : ValidacijaStrategy
    {
        public void IspisiPorukuGreske(int idGreske)
        {
            switch (idGreske)
            {
                case 1:
                    MessageBox.Show("Recenzija uspešno poslata !", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 2:
                    MessageBox.Show("Morate izabrati ocenu za aplikaciju !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 3:
                    MessageBox.Show("Unesite sadržaj recenzije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
