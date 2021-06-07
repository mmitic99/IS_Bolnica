using Bolnica.DTOs;
using Bolnica.Kontroler;
using Kontroler;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
            
            OdmorLabela.Content = lekarKontroler.GetByJmbg(jmbgLekara).BrojSlobodnihDana;
            DatumZaPrikazRadnogVremena.DisplayDateStart = DateTime.Now;
            DatumZaPrikazRadnogVremena.SelectedDate = DateTime.Now;
        }

        private void dodaj_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodajRadnoVreme(jmbgLekara, OdmorLabela, DatumZaPrikazRadnogVremena);
            s.ShowDialog();
        }

        private void Obrisi_Click(object sender, RoutedEventArgs e)
        {
            if (RadnoVremePrikaz.SelectedIndex != -1)
            {
                MessageBoxResult izbor =
                    MessageBox.Show("Da li ste sigurni da želite da obrišete odabrano radno vreme?", "Brisanje lekara",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (izbor == MessageBoxResult.Yes)
                {
                    bool uspesno = radnoVremeKontroler.Obrisi(((RadnoVremeDTO)RadnoVremePrikaz.SelectedItem).IdRadnogVremena);
                    if (uspesno)
                        DatumZaPrikazRadnogVremena.SelectedDate = DateTime.Now;
                    else
                    {
                        MessageBox.Show("Nije moguće obrisati radno vreme, jer postoji zakazan termin.", "Greška", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati radno vreme koje želite da obrišete.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        private void dodajSlobodneDane_Click(object sender, RoutedEventArgs e)
        {
            LekarDTO lekar = lekarKontroler.GetByJmbg(jmbgLekara);
            lekar.BrojSlobodnihDana += 1;
            lekarKontroler.IzmeniLekara(jmbgLekara, lekar);
            OdmorLabela.Content = lekarKontroler.GetByJmbg(jmbgLekara).BrojSlobodnihDana;
        }

        private void DatumZaPrikazRadnogVremena_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            if (Dan.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi datum:";
                if (DatumZaPrikazRadnogVremena.SelectedDate != null)
                    RadnoVremePrikaz.ItemsSource =
                        radnoVremeKontroler.GetRadnoVremeZaDan((DateTime)DatumZaPrikazRadnogVremena.SelectedDate, jmbgLekara);

                SekretarWindow.SortirajDataGrid(RadnoVremePrikaz, 1, ListSortDirection.Ascending);
            }
            else if (Nedelja.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi nedelju:";
                if (DatumZaPrikazRadnogVremena.SelectedDate != null)
                {
                    int razlika =
                        (7 + ((DateTime)DatumZaPrikazRadnogVremena.SelectedDate).DayOfWeek - DayOfWeek.Monday) % 7;
                    RadnoVremePrikaz.ItemsSource =
                        radnoVremeKontroler.GetRadnoVremeZaNedelju(
                            ((DateTime)DatumZaPrikazRadnogVremena.SelectedDate).AddDays(-1 * razlika), jmbgLekara);
                    SekretarWindow.SortirajDataGrid(RadnoVremePrikaz, 1, ListSortDirection.Ascending);
                }
            }
            else if (Mesec.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi mesec:";
                if (DatumZaPrikazRadnogVremena.SelectedDate != null)
                    RadnoVremePrikaz.ItemsSource =
                        radnoVremeKontroler.GetRadnoVremeZaMesec((DateTime)DatumZaPrikazRadnogVremena.SelectedDate, jmbgLekara);
                SekretarWindow.SortirajDataGrid(RadnoVremePrikaz, 1, ListSortDirection.Ascending);
            }
            else if (Godina.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi godinu:";
                if (DatumZaPrikazRadnogVremena.SelectedDate != null)
                    RadnoVremePrikaz.ItemsSource =
                        radnoVremeKontroler.GetRadnoVremeZaGodinu((DateTime)DatumZaPrikazRadnogVremena.SelectedDate, jmbgLekara);
                SekretarWindow.SortirajDataGrid(RadnoVremePrikaz, 1, ListSortDirection.Ascending);
            }
        }

        private void OtkaziClick(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
