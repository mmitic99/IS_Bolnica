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
using Kontroler;

namespace Bolnica.view.SekretarView.Termini
{
    /// <summary>
    /// Interaction logic for GenerisiIzvestaj.xaml
    /// </summary>
    public partial class GenerisiIzvestaj : Window
    {
        private TerminKontroler terminKontroler;
        public GenerisiIzvestaj()
        {
            InitializeComponent();
            terminKontroler = new TerminKontroler();
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (DatumPocetka.SelectedDate == null || DatumZavrsetka.SelectedDate == null)
            {
                MessageBox.Show("Morate izabrati vremenski period.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (DatumPocetka.SelectedDate > DatumZavrsetka.SelectedDate)
            {
                MessageBox.Show("Datum početka mora biti manji od datuma završetka.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            string putanjaIzvestaja = terminKontroler.GenerisiIzvestaj((DateTime)DatumPocetka.SelectedDate, (DateTime)DatumZavrsetka.SelectedDate);

            if(putanjaIzvestaja != null)
            {
                MessageBox.Show("Izveštaj se nalazi na sledećoj putanji: " + putanjaIzvestaja, "Obaveštenje",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void Otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
