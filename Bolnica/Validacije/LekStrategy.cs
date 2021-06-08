using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bolnica.Validacije
{
    public class LekStrategy : ValidacijaStrategy
    {
        public void IspisiPorukuGreske(int idGreske)
        {
            switch (idGreske)
            {
                case 1:
                    MessageBox.Show("Neispravno unet naziv leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 2:
                    MessageBox.Show("Neispravno uneta količina leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 3:
                    MessageBox.Show("Neispravno uneta jačina leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 4:
                    MessageBox.Show("Neispravno unet zamenski lek !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 5:
                    MessageBox.Show("Neispravno unet sastav leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 6:
                    MessageBox.Show("Već postoji lek sa istim nazivom !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 7:
                    MessageBox.Show("Selektujte  vrstu/klasu leka !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
