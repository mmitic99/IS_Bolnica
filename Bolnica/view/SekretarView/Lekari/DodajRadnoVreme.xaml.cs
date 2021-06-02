using Bolnica.Kontroler;
using Bolnica.model.Enum;
using Kontroler;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;

namespace Bolnica.view.SekretarView.Lekari
{
    /// <summary>
    /// Interaction logic for DodajRadnoVreme.xaml
    /// </summary>
    public partial class DodajRadnoVreme : Window
    {
        private string jmbgLekara;
        private RadnoVremeKontroler radnoVremeKontroler;
        private LekarKontroler lekarKontroler;
        private Label odmorLabela;
        private DatePicker datumZaPrikazRadnogVremena;
        public DodajRadnoVreme(string jmbgLekara, Label odmorLabela, DatePicker datumZaPrikazRadnogVremena)
        {
            InitializeComponent();
            VremePocetka.ItemsSource = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("HH:mm"));
            VremeKraja.ItemsSource = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("HH:mm"));
            VremePocetka.SelectedIndex = 0;
            VremeKraja.SelectedIndex = 0;

            this.datumZaPrikazRadnogVremena = datumZaPrikazRadnogVremena;
            this.odmorLabela = odmorLabela;
            this.jmbgLekara = jmbgLekara;
            radnoVremeKontroler = new RadnoVremeKontroler();
            lekarKontroler = new LekarKontroler();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            RadnoVremeDTO radnoVreme = new RadnoVremeDTO()
            {
                JmbgLekara = jmbgLekara
            };

            if (Status.SelectedIndex == 0)
            {
                radnoVreme.StatusRadnogVremena = StatusRadnogVremena.Radi;
            }
            else if (Status.SelectedIndex == 1)
            {
                radnoVreme.StatusRadnogVremena = StatusRadnogVremena.NaOdmoru;
            }
            else if (Status.SelectedIndex == 2)
            {
                radnoVreme.StatusRadnogVremena = StatusRadnogVremena.SlobodanDan;
            }

            if (DatumPocetka.SelectedDate != null && DatumKraja.SelectedDate != null)
            {
                if (DatumPocetka.SelectedDate <= DatumKraja.SelectedDate)
                {
                    if (DatumPocetka.SelectedDate <= DatumKraja.SelectedDate)
                    {

                        string[] vremePocetka = ((string)VremePocetka.SelectedItem).Split(':');
                        int satiPocetak = int.Parse(vremePocetka[0]);
                        int minutiPocetak = int.Parse(vremePocetka[1]);

                        string[] vremeKraja = ((string)VremeKraja.SelectedItem).Split(':');
                        int satiKraj = int.Parse(vremeKraja[0]);
                        int minutiKraj = int.Parse(vremeKraja[1]);

                        radnoVreme.DatumIVremePocetka = ((DateTime)DatumPocetka.SelectedDate).AddHours(satiPocetak).AddMinutes(minutiPocetak);
                        radnoVreme.DatumIVremeZavrsetka = ((DateTime)DatumKraja.SelectedDate).AddHours(satiKraj).AddMinutes(minutiKraj);

                        bool uspesno = radnoVremeKontroler.Save(radnoVreme);

                        if (uspesno)
                        {
                            this.Close();
                            datumZaPrikazRadnogVremena.SelectedDate = radnoVreme.DatumIVremePocetka;
                        }
                        else if (radnoVreme.StatusRadnogVremena == StatusRadnogVremena.NaOdmoru &&
                                 (radnoVreme.DatumIVremeZavrsetka - radnoVreme.DatumIVremePocetka).TotalDays > lekarKontroler.GetByJmbg(jmbgLekara).BrojSlobodnihDana
                                 )
                        {
                            MessageBox.Show("Broj dana za odmor mora biti manji od " + odmorLabela.Content + " .", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Izabrani raspon vremena je zauzet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Datum početka mora biti manji od datuma kraja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Datum početka mora biti manji od datuma kraja.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (DatumPocetka.SelectedDate != null && Status.SelectedIndex == 2)
            {
                radnoVreme.DatumIVremePocetka = (DateTime)DatumPocetka.SelectedDate;
                radnoVreme.DatumIVremeZavrsetka = ((DateTime)DatumPocetka.SelectedDate).AddDays(1);

                bool uspesno = radnoVremeKontroler.Save(radnoVreme);

                if (uspesno)
                {
                    this.Close();
                    datumZaPrikazRadnogVremena.SelectedDate = radnoVreme.DatumIVremePocetka;
                }
                else
                {
                    MessageBox.Show("Izabrani datum je zauzet.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Datum početka i datum kraja se moraju izabrati.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Status_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Status.SelectedIndex == 0)
            {
                if (DatumPocetka != null) DatumPocetka.Visibility = Visibility.Visible;
                if (VremePocetka != null) VremePocetka.Visibility = Visibility.Visible;
                if (DatumKraja != null) DatumKraja.Visibility = Visibility.Visible;
                if (VremeKraja != null) VremeKraja.Visibility = Visibility.Visible;
                if (datumVremePLabela != null) datumVremePLabela.Visibility = Visibility.Visible;
                if (datumVremeZLabela != null) datumVremeZLabela.Visibility = Visibility.Visible;
                if (datumPLabela != null) datumPLabela.Visibility = Visibility.Hidden;
                if (datumZLabela != null) datumZLabela.Visibility = Visibility.Hidden;
            }
            else if (Status.SelectedIndex == 1)
            {
                DatumPocetka.Visibility = Visibility.Visible;
                VremePocetka.Visibility = Visibility.Hidden;
                DatumKraja.Visibility = Visibility.Visible;
                VremeKraja.Visibility = Visibility.Hidden;
                datumVremePLabela.Visibility = Visibility.Hidden;
                datumVremeZLabela.Visibility = Visibility.Hidden;
                datumPLabela.Visibility = Visibility.Visible;
                datumZLabela.Visibility = Visibility.Visible;
            }
            else if (Status.SelectedIndex == 2)
            {
                DatumPocetka.Visibility = Visibility.Visible;
                VremePocetka.Visibility = Visibility.Hidden;
                DatumKraja.Visibility = Visibility.Hidden;
                VremeKraja.Visibility = Visibility.Hidden;
                datumVremePLabela.Visibility = Visibility.Hidden;
                datumVremeZLabela.Visibility = Visibility.Hidden;
                datumPLabela.Visibility = Visibility.Hidden;
                datumZLabela.Visibility = Visibility.Hidden;
                datumLabela.Visibility = Visibility.Visible;
            }
        }
    }
}
