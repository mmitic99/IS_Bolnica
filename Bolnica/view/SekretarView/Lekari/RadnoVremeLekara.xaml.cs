using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.model;
using Bolnica.model.Enum;
using Kontroler;
using Model;

namespace Bolnica.view.SekretarView.Lekari
{
    /// <summary>
    /// Interaction logic for RadnoVremeLekara.xaml
    /// </summary>
    public partial class RadnoVremeLekara : Window
    {
        private RadnoVremeKontroler radnoVremeKontroler;
        private LekarKontroler lekarKontroler;
        private string jmbgLekara;
        public RadnoVremeLekara(string jmbgLekara)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            this.jmbgLekara = jmbgLekara;
            radnoVremeKontroler = new RadnoVremeKontroler();
            lekarKontroler = new LekarKontroler();

            RadnoVremePrikaz.ItemsSource = radnoVremeKontroler.GetByJmbg(jmbgLekara);
            OdmorLabela.Content = lekarKontroler.GetByJmbg(jmbgLekara).BrojSlobodnihDana;
        }

        private void dodaj_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodajRadnoVreme(RadnoVremePrikaz, jmbgLekara, OdmorLabela);
            s.ShowDialog();
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (RadnoVremePrikaz.SelectedIndex != -1)
            {
                MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete odabrano radno vreme?", "Brisanje lekara", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (izbor == MessageBoxResult.Yes)
                {
                    bool uspesno = radnoVremeKontroler.Obrisi(((RadnoVreme)RadnoVremePrikaz.SelectedItem).IdRadnogVremena);
                    if (uspesno)
                        RadnoVremePrikaz.ItemsSource = radnoVremeKontroler.GetByJmbg(jmbgLekara);
                    else
                    {
                        MessageBox.Show("Radno vreme nije uspešno obrisano.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati radno vreme koje želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void dodajSlobodneDane_Click(object sender, RoutedEventArgs e)
        {
            LekarDTO lekar = lekarKontroler.GetByJmbg(jmbgLekara);
            lekar.BrojSlobodnihDana += 1;
            lekarKontroler.IzmeniLekara(jmbgLekara, lekar);
            OdmorLabela.Content = lekarKontroler.GetByJmbg(jmbgLekara).BrojSlobodnihDana;
        }
    }
}
