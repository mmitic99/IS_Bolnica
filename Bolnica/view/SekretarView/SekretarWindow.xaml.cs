using Bolnica.view.SekretarView.Obavestenja;
using Bolnica.viewActions;
using Kontroler;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Bolnica.view.SekretarView.Lekari;
using Bolnica.view.SekretarView.Pacijenti;
using Bolnica.view.SekretarView.Termini;
using System.Windows.Controls;
using Bolnica.DTOs;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        private SekretarDTO sekretar;
        private PacijentKontroler pacijentKontroler;
        private TerminKontroler terminKontroler;
        private ObavestenjaKontroler obavestenjaKontroler;
        private LekarKontroler lekarKontroler;

        public SekretarWindow(SekretarDTO sekretar)
        {
            InitializeComponent();

            pacijentKontroler = new PacijentKontroler();
            terminKontroler = new TerminKontroler();
            obavestenjaKontroler = new ObavestenjaKontroler();
            lekarKontroler = new LekarKontroler();

            this.sekretar = sekretar;
            ImeS.Content = sekretar.Ime;
            PrezimeS.Content = sekretar.Prezime;

            PacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
            TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
            LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
            ObavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg(sekretar.Jmbg);

            StatusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
            DispatcherTimer timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.5)};
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            StatusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
        }

        private void DodavanjeGostujuceg_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeGostujucegPacijenta(PacijentiPrikaz, TerminiPrikaz);
            s.ShowDialog();
        }

        private void DodavanjePacijenta_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijenta(PacijentiPrikaz);
            s.ShowDialog();
        }

        private void IzmenaPacijenta_Click(object sender, RoutedEventArgs e)
        {
            if (PacijentiPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati pacijenta koga želite da izmenite.", "Greška", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var s = new IzmenaPacijenta(PacijentiPrikaz);
            s.ShowDialog();
            
        }

        private void BrisanjePacijenta_Click(object sender, RoutedEventArgs e)
        {
            if (PacijentiPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati pacijenta koga želite da obrišete.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete odabranog pacijenta?",
                "Brisanje pacijenta", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (izbor == MessageBoxResult.Yes)
            {
                bool uspesno = pacijentKontroler.ObrisiPacijenta(((PacijentDTO) PacijentiPrikaz.SelectedItem).Jmbg);
                if(uspesno)
                {
                    PacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                    SekretarWindow.SortirajDataGrid(PacijentiPrikaz, 0, ListSortDirection.Ascending);
                }
                else
                {
                    MessageBox.Show("Brisanje nije uspešno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ZakazivanjeTermina_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(TerminiPrikaz, null);
            s.ShowDialog();
        }

        private void IzmeniTermin_Click(object sender, RoutedEventArgs e)
        {
            if (TerminiPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati termin koji želite da izmenite.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            
            var s = new IzmenaTermina(TerminiPrikaz);
            s.ShowDialog();
        }

        private void OtkakazivanjeTermina_Click(object sender, RoutedEventArgs e)
        {
            if (TerminiPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati termin koji želite da otkažete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da otkažete izabrani termin?",
                "Otkazivanje termina", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (izbor == MessageBoxResult.Yes)
            {
                terminKontroler.OtkaziTermin(((TerminPacijentLekar) TerminiPrikaz.SelectedItem).termin);
                TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
                SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
            }
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("s");
            this.Close();
            s.Show();
        }

        private void ZakazivanjeTerminaZaOdabranogPacijenta_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(TerminiPrikaz, (PacijentDTO)PacijentiPrikaz.SelectedItem);
            s.ShowDialog();
        }


        private void DodajObavestenje_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeObavestenja(ObavestenjaPrikaz);
            s.ShowDialog();
        }

        private void IzmeniObavestenje_Click(object sender, RoutedEventArgs e)
        {
            if (ObavestenjaPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da izmenite.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var s = new IzmenaObavestenja(ObavestenjaPrikaz);
            s.ShowDialog();
        }

        private void ObrisiObavestenje_Click(object sender, RoutedEventArgs e)
        {
            if (ObavestenjaPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da obrišete.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MessageBoxResult izbor =
                MessageBox.Show("Da li ste sigurni da želite da obrišete izabrano obaveštenje?",
                    "Brisanje obaveštenja", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (izbor == MessageBoxResult.Yes)
            {
                bool uspesno = obavestenjaKontroler.obrisiObavestenje((ObavestenjeDTO) ObavestenjaPrikaz.SelectedItem);
                if (!uspesno)
                {
                    MessageBox.Show("Brisanje obaveštenja nije uspešno.", "Greška", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                
                ObavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg("-1");
            }
        }

        private void PogledajObavestenje_Click(object sender, EventArgs e)
        {
            if (ObavestenjaPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati obaveštenje koje želite da pogledate.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var s = new PogledajObavestenje((ObavestenjeDTO) ObavestenjaPrikaz.SelectedItem);
            s.Show();
        }

        private void DodavanjeLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeLekara(LekariPrikaz);
            s.ShowDialog();
        }

        private void IzmenaLekara_Click(object sender, RoutedEventArgs e)
        {
            if (LekariPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati lekara koga želite da izmenite.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            var s = new IzmenaLekara(LekariPrikaz);
            s.ShowDialog();
        }

        private void BrisanjeLekara_Click(object sender, RoutedEventArgs e)
        {
            if (LekariPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati lekara koga želite da obrišete.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete odabranog lekara?",
                "Brisanje lekara", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (izbor == MessageBoxResult.Yes)
            {
                bool uspesno = lekarKontroler.ObrisiLekara(((LekarDTO) LekariPrikaz.SelectedItem).Jmbg);
                if (!uspesno)
                {
                    MessageBox.Show("Lekar nije uspešno obrisan.", "Greška", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
                SortirajDataGrid(LekariPrikaz, 1, ListSortDirection.Ascending);
            }
        }

        private void ZakazivanjeTerminaZaOdabranogLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(TerminiPrikaz, null, false, (LekarDTO)LekariPrikaz.SelectedItem);
            s.ShowDialog();
        }

        private void RadnoVremeLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new RadnoVremeLekara(((LekarDTO)LekariPrikaz.SelectedItem).Jmbg);
            s.ShowDialog();
        }
        private void Pocetna_Selected(object sender, RoutedEventArgs e)
        {
            ObavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg("-1");
        }

        private void Pacijenti_Selected(object sender, RoutedEventArgs e)
        {
            PacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
            SortirajDataGrid(PacijentiPrikaz, 0, ListSortDirection.Ascending);
        }

        private void Termini_Selected(object sender, RoutedEventArgs e)
        {
            TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
            SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
        }
        private void Lekari_Selected(object sender, RoutedEventArgs e)
        {
            LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
            SortirajDataGrid(LekariPrikaz, 1, ListSortDirection.Ascending);
        }
        public static void SortirajDataGrid(DataGrid dataGrid, int kolona, ListSortDirection sortDirection)
        {
            var column = dataGrid.Columns[kolona];
            dataGrid.Items.SortDescriptions.Clear();
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;
            dataGrid.Items.Refresh();
        }
    }
}
