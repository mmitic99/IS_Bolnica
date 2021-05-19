using System.Windows;
using System.Windows.Controls;
using Bolnica.DTOs;
using Kontroler;

namespace Bolnica.view.SekretarView.Termini
{
    /// <summary>
    /// Interaction logic for DodavanjePacijentaTerminu.xaml
    /// </summary>
    public partial class DodavanjePacijentaTerminu : Window
    {
        private TerminDTO termin;
        private TextBox pacijent;
        private PacijentKontroler pacijentKontroler;
        public DodavanjePacijentaTerminu(TerminDTO termin, TextBox pacijent)
        {
            InitializeComponent();
            pacijentKontroler = new PacijentKontroler();

            DodavanjePacijentaTerminuPrikaz.ItemsSource = pacijentKontroler.GetAll();
            this.termin = termin;
            this.pacijent = pacijent;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (DodavanjePacijentaTerminuPrikaz.SelectedIndex != -1)
            {
                PacijentDTO izabraniPacijent = (PacijentDTO)DodavanjePacijentaTerminuPrikaz.SelectedItem;
                termin.JmbgPacijenta = izabraniPacijent.Jmbg;
                pacijent.Text = izabraniPacijent.Ime + " " + izabraniPacijent.Prezime;
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate izabrati pacijenta koga želite da dodate.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
