using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Bolnica.Validacije
{
    public class ProstorijaStrategy : ValidacijaStrategy
    {
        public void IspisiPorukuGreske(int idGreske)
        {
            switch (idGreske) 
            {
                case 1:
                    MessageBox.Show("Neispravno unet broj prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 2:
                    MessageBox.Show("Neispravno unet sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 3:
                    MessageBox.Show("Neispravno uneta kvadratura prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 4:
                    MessageBox.Show("Selektujte vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 5:
                    MessageBox.Show("Prostorija ima zakazan termin ili preraspodelu opreme u tom rasponu datuma !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 6:
                    MessageBox.Show("Već je zakazano renoviranje za datu prostoriju !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 7:
                    MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 8:
                    MessageBox.Show("Ne možete oduzeti više potrošne opreme od onoliko koliko je ima u magacinu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 9:
                    MessageBox.Show("Ne možete prebaciti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 10:
                    MessageBox.Show("Ne možete oduzeti više statičke opreme od onoliko koliko je ima u prostoriji !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 11:
                    MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 12:
                    MessageBox.Show("Neispravno unet naziv opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 13:
                    MessageBox.Show("Neispravno uneta količina opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 14:
                    MessageBox.Show("Neispravno uneta izmena količine opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 15:
                    MessageBox.Show("Neispravno uneta količine opreme koja se prebacuje !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 16:
                    MessageBox.Show("Selektujte način pretrage iz padajućeg menija !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 17:
                    MessageBox.Show("Nevalidna pretraga !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
