using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bolnica.Validacije
{
    public class SelekcijaStrategy : ValidacijaStrategy
    {
        public void IspisiPorukuGreske(int idGreske)
        {
            switch (idGreske)
            {
                case 1:
                    MessageBox.Show("Označite prostoriju koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 2:
                    MessageBox.Show("Označite prostoriju koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 3:
                    MessageBox.Show("Označite statičku opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 4:
                    MessageBox.Show("Označite potrošnu opremu koju želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 5:
                    MessageBox.Show("Označite prostoriju iz desne tabele u koju želite da prebacite opremu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 6:
                    MessageBox.Show("Označite prostoriju iz leve tabele iz koje želite da prebacite opremu !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 7:
                    MessageBox.Show("Označili ste dve iste prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 8:
                    MessageBox.Show("Označite statičku opremu iz leve tabele za koju želite da zakažete prebacivanje !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 9:
                    MessageBox.Show("Označite statičku opremu iz leve tabele koju želite da prebacite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 10:
                    MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 11:
                    MessageBox.Show("Označite statičku opremu iz desne tabele koju želite da obrišete iz prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 12:
                    MessageBox.Show("Označite datum preraspodele opreme !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 13:
                    MessageBox.Show("Označite preraspodelu iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 14:
                    MessageBox.Show("Označite renoviranje iz tabele koju želite da otkažete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 15:
                    MessageBox.Show("Označite lek koji želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 16:
                    MessageBox.Show("Označite lek koji želite da izmenite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case 17:
                    MessageBox.Show("Označite verifikaciju koje želite da obrišete !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    break;
            }
        }
    }
}
