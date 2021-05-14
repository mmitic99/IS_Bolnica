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
using Bolnica.Kontroler;
using Bolnica.model;
using Bolnica.model.Enum;
using Bolnica.Validacije;
using Kontroler;

namespace Bolnica.view.SekretarView.Lekari
{
    /// <summary>
    /// Interaction logic for DodajRadnoVreme.xaml
    /// </summary>
    public partial class DodajRadnoVreme : Window
    {
        private DataGrid radnoVremePrikaz;
        private string jmbgLekara;
        private RadnoVremeKontroler radnoVremeKontroler;
        public DodajRadnoVreme(DataGrid radnoVremePrikaz, string jmbgLekara)
        {
            InitializeComponent();
            VremePocetka.ItemsSource = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("HH:mm"));
            VremeKraja.ItemsSource = Enumerable.Range(00, 24).Select(i => (DateTime.MinValue.AddHours(i)).ToString("HH:mm"));
            VremePocetka.SelectedIndex = 0;
            VremeKraja.SelectedIndex = 0;

            this.radnoVremePrikaz = radnoVremePrikaz;
            this.jmbgLekara = jmbgLekara;
            radnoVremeKontroler = new RadnoVremeKontroler();
        }

        private void Potvrdi_OnClickdi_Click(object sender, RoutedEventArgs e)
        {
            RadnoVreme radnoVreme = new RadnoVreme()
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
                            radnoVremePrikaz.ItemsSource = radnoVremeKontroler.GetByJmbg(jmbgLekara);
                        }
                        else
                        {
                            MessageBox.Show("Izabrani raspon vremena vec postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    radnoVremePrikaz.ItemsSource = radnoVremeKontroler.GetByJmbg(jmbgLekara);
                }
                else
                {
                    MessageBox.Show("Izabrani raspon vremena vec postoji u bazi.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Datum početka i datum kraja se moraju izabrati.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Otkazi_OnClickzi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
