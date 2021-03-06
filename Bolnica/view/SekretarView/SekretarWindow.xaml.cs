using Bolnica.view.SekretarView.Obavestenja;
using Bolnica.viewActions;
using Kontroler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Bolnica.view.SekretarView.Lekari;
using Bolnica.view.SekretarView.Pacijenti;
using Bolnica.view.SekretarView.Termini;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.ViewModel.SekretarViewModel;
using LiveCharts;
using LiveCharts.Wpf;
using Model.Enum;

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
        private ProstorijeKontroler prostorijeKontroler;
        private LekKontroler lekKontroler;

        public SekretarWindow(SekretarDTO sekretar)
        {
            InitializeComponent();
            this.DataContext = new SekretarWindowViewModel();

            pacijentKontroler = new PacijentKontroler();
            terminKontroler = new TerminKontroler();
            obavestenjaKontroler = new ObavestenjaKontroler();
            lekarKontroler = new LekarKontroler();
            prostorijeKontroler = new ProstorijeKontroler();
            lekKontroler = new LekKontroler();

            this.sekretar = sekretar;
            ImeS.Content = sekretar.Ime;
            PrezimeS.Content = sekretar.Prezime;

            PacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();

            StatusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
            DispatcherTimer timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.5)};
            timer.Tick += Timer_Tick;
            timer.Start();

            DatumZaTermin.DisplayDateStart = DateTime.Now;
            DatumZaTermin.SelectedDate = DateTime.Now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            StatusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
        }

        private void DodavanjeGostujuceg_Click(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeGostujucegPacijenta(PacijentiPrikaz, TerminiPrikaz, DatumZaTermin);
            Pacijenti.IsSelected = true;
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
                if (!uspesno)
                {
                    MessageBox.Show("Brisanje nije uspešno. Pacijent ima zakazane termine.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                PacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
                SekretarWindow.SortirajDataGrid(PacijentiPrikaz, 0, ListSortDirection.Ascending);
            }
        }

        private void ZakazivanjeTermina_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(null, false, null, DatumZaTermin);
            Termini.IsSelected = true;
            s.ShowDialog();
        }

        private void ZakazivanjeTerminaZaOdabranogPacijenta_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar((PacijentDTO) PacijentiPrikaz.SelectedItem, false, null, DatumZaTermin);
            Termini.IsSelected = true;
            s.ShowDialog();
        }

        private void ZakazivanjeTerminaZaOdabranogLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new ZakazivanjeTerminaSekretar(null, false, (LekarDTO) LekariPrikaz.SelectedItem, DatumZaTermin);
            Termini.IsSelected = true;
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

            var s = new IzmenaTermina(TerminiPrikaz, DatumZaTermin);
            s.ShowDialog();
        }

        private void OtkakazivanjeTermina_Click(object sender, RoutedEventArgs e)
        {
            if (TerminiPrikaz.SelectedIndex == -1)
            {
                MessageBox.Show("Morate izabrati termin koji želite da otkažete.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da otkažete izabrani termin?",
                "Otkazivanje termina", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (izbor == MessageBoxResult.Yes)
            {
                terminKontroler.OtkaziTermin(((TerminPacijentLekarDTO) TerminiPrikaz.SelectedItem).termin.IDTermina);
                TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekar();
                SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
            }
        }

        private void GenerisanjeIzvestaja_Click(object sender, RoutedEventArgs e)
        {
            var s = new GenerisiIzvestaj();
            s.ShowDialog();
        }

        private void Odjava_Click(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("s");
            this.Close();
            s.Show();
        }

        private void DodajObavestenje_Click(object sender, RoutedEventArgs e)
        {
            ((SekretarWindowViewModel) DataContext).DodajObavestenje();
        }

        private void IzmeniObavestenje_Click(object sender, RoutedEventArgs e)
        {
            ((SekretarWindowViewModel) DataContext).IzmeniObavestenje();
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
                bool uspesno = obavestenjaKontroler.ObrisiObavestenje((ObavestenjeDTO) ObavestenjaPrikaz.SelectedItem);
                if (!uspesno)
                {
                    MessageBox.Show("Brisanje obaveštenja nije uspešno.", "Greška", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                ((SekretarWindowViewModel)DataContext).AzurirajObavestenja();
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
            ((SekretarWindowViewModel)DataContext).DodajLekara();
            SortirajDataGrid(LekariPrikaz, 1, ListSortDirection.Ascending);
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
                MessageBox.Show("Morate izabrati lekara koga želite da obrišete.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MessageBoxResult izbor = MessageBox.Show("Da li ste sigurni da želite da obrišete odabranog lekara?",
                "Brisanje lekara", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (izbor == MessageBoxResult.Yes)
            {
                bool uspesno = lekarKontroler.ObrisiLekara(((LekarDTO) LekariPrikaz.SelectedItem).Jmbg);
                if (!uspesno)
                {
                    MessageBox.Show("Brisanje lekara nije uspešno. Lekar ima zakazane termine.", "Greška", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                ((SekretarWindowViewModel)DataContext).AzurirajLekare();
                SortirajDataGrid(LekariPrikaz, 1, ListSortDirection.Ascending);
            }
        }

        private void RadnoVremeLekara_Click(object sender, RoutedEventArgs e)
        {
            var s = new RadnoVremeLekara(((LekarDTO) LekariPrikaz.SelectedItem).Jmbg);
            s.ShowDialog();
        }

        private void Pocetna_Selected(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SekretarWindowViewModel();
            AzurirajDijagramBrojTermina();
            AzurirajPodeluPoPolovima();
            AzurirajDijagramBrojNovihPacijenata();
            UkupanBrojPacijenata.Text = pacijentKontroler.GetAll().Count.ToString();
            UkupanBrojLekara.Text = lekarKontroler.GetAll().Count.ToString();
            UkupanBrojSobaZaPregled.Text =
                prostorijeKontroler.GetBrojProstorija(VrstaProstorije.Soba_za_preglede).ToString();
            UkupanBrojOperacionihSala.Text =
                prostorijeKontroler.GetBrojProstorija(VrstaProstorije.Operaciona_sala).ToString();
        }

        private void Pacijenti_Selected(object sender, RoutedEventArgs e)
        {
            PacijentiPrikaz.ItemsSource = pacijentKontroler.GetAll();
            SortirajDataGrid(PacijentiPrikaz, 0, ListSortDirection.Ascending);
        }

        private void Termini_Selected(object sender, RoutedEventArgs e)
        {
            DatumZaTermin.SelectedDate = DateTime.Now;
        }

        private void Lekari_Selected(object sender, RoutedEventArgs e)
        {
            this.DataContext = new SekretarWindowViewModel();
            //LekariPrikaz.ItemsSource = lekarKontroler.GetAll();
            SortirajDataGrid(LekariPrikaz, 1, ListSortDirection.Ascending);
        }

        private void Prostorije_Selected(object sender, RoutedEventArgs e)
        {
            ProstorijePrikaz.ItemsSource = prostorijeKontroler.GetAll();
            SortirajDataGrid(ProstorijePrikaz, 0, ListSortDirection.Ascending);
        }

        private void Lekovi_Selected(object sender, RoutedEventArgs e)
        {
            LekoviPrikaz.ItemsSource = lekKontroler.GetAll();
            SortirajDataGrid(LekariPrikaz, 0, ListSortDirection.Ascending);
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

        private void AzurirajDijagramBrojTermina()
        {
            List<string> sviDaniUMesecu = GenerisiSveDaneUMesecu();
            BrojTermina.Series = new SeriesCollection()
            {
                new StackedColumnSeries
                {
                    Title = "Pregledi",
                    Values = new ChartValues<int>(terminKontroler.GetMesecnePreglede(sviDaniUMesecu))
                },
                new StackedColumnSeries
                {
                    Title = "Operacije",
                    Values = new ChartValues<int>(terminKontroler.GetMesecneOperacije(sviDaniUMesecu))
                }
            };
            ((SekretarWindowViewModel)DataContext).XOsaBrojTermina = PretvoriListuUNiz(sviDaniUMesecu);
        }

        private string[] PretvoriListuUNiz(List<string> sviDaniUMesecu)
        {
            string[] retVal = new string[sviDaniUMesecu.Count];
            for (int i = 0; i < sviDaniUMesecu.Count; i++)
            {
                retVal[i] = sviDaniUMesecu[i];
            }

            return retVal;
        }

        private List<string> GenerisiSveDaneUMesecu()
        {
            List<string> sviDaniUMesecu = new List<string>();
            var prviDanUMesecu = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var prviDanUSledecemMesecu = prviDanUMesecu.AddMonths(1);

            for (DateTime datum = prviDanUMesecu; datum < prviDanUSledecemMesecu; datum = datum.AddDays(1))
            {
                sviDaniUMesecu.Add(datum.Day.ToString());
            }

            return sviDaniUMesecu;
        }

        private void AzurirajPodeluPoPolovima()
        {
            SeriesCollection podelaPoPolovima = new SeriesCollection()
            {
                new PieSeries
                {
                    Title = "Muškarci", Fill = new SolidColorBrush(Color.FromRgb(66, 55, 208)),
                    Values = new ChartValues<int> {pacijentKontroler.GetBrojMuskihPacijenata()},
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Žene", Fill = new SolidColorBrush(Color.FromRgb(208, 55, 66)),
                    Values = new ChartValues<int> {pacijentKontroler.GetBrojZenskihPacijenata()},
                    DataLabels = true
                }
            };
            PodelaPoPolovima.Series = podelaPoPolovima;
        }

        private void AzurirajDijagramBrojNovihPacijenata()
        {
            List<string> sviDaniUMesecu = GenerisiSveDaneUMesecu();
            BrojNovihPacijenata.Series = new SeriesCollection()
            {
                new LineSeries
                {
                    Title = "Broj novih pacijenata",
                    Values = new ChartValues<int>(pacijentKontroler.GetBrojNovihPacijenataUMesecu(sviDaniUMesecu))
                }
            };
            ((SekretarWindowViewModel)DataContext).XOsaBrojNovihPacijenata = PretvoriListuUNiz(sviDaniUMesecu);
        }


        private void IzmenaProfila_Click(object sender, RoutedEventArgs e)
        {
            var s = new IzmenaProfila(sekretar, ImeS, PrezimeS);
            s.ShowDialog();
        }

        private void IzmenaLozinke_Click(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new IzmenaLozinke(sekretar);
            s.ShowDialog();
        }

        private void Feedback_Click(object sender, ExecutedRoutedEventArgs e)
        {
            var s = new SekretarFeedback(sekretar.Jmbg);
            s.ShowDialog();
        }

        private void DatumZaTermin_OnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            if (Dan.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi datum:";
                if (DatumZaTermin.SelectedDate != null)
                    TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekarZaDan((DateTime)DatumZaTermin.SelectedDate);
                SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
            }
            else if (Nedelja.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi nedelju:";
                if (DatumZaTermin.SelectedDate != null)
                {
                    int razlika = (7 + ((DateTime) DatumZaTermin.SelectedDate).DayOfWeek - DayOfWeek.Monday) % 7;
                    TerminiPrikaz.ItemsSource =
                        terminKontroler.GetBuduciTerminPacLekarZaNedelju(((DateTime) DatumZaTermin.SelectedDate).AddDays(-1 * razlika));
                }

                SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
            }
            else if (Mesec.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi mesec:";
                if (DatumZaTermin.SelectedDate != null)
                    TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekarZaMesec((DateTime)DatumZaTermin.SelectedDate);
                SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
            }
            else if (Godina.IsChecked == true)
            {
                IzaberiDatumLabela.Content = "Izaberi godinu:";
                if (DatumZaTermin.SelectedDate != null)
                    TerminiPrikaz.ItemsSource = terminKontroler.GetBuduciTerminPacLekarZaGodinu((DateTime) DatumZaTermin.SelectedDate);
                SortirajDataGrid(TerminiPrikaz, 0, ListSortDirection.Ascending);
            }
        }

        private void SelectFirstObavestenje(object sender, ExecutedRoutedEventArgs e)
        {
            if (Pocetna.IsSelected)
            {
                ObavestenjaPrikaz.SelectedIndex = 0;
                ObavestenjaPrikaz.Focus();
            }
        }

        private void SelectFirstPacijenta(object sender, ExecutedRoutedEventArgs e)
        {
            if (Pacijenti.IsSelected)
            {
                PacijentiPrikaz.SelectedIndex = 0;
                PacijentiPrikaz.Focus();
            }
        }

        private void SelectFirstTermin(object sender, ExecutedRoutedEventArgs e)
        {
            if (Termini.IsSelected)
            {
                TerminiPrikaz.SelectedIndex = 0;
                TerminiPrikaz.Focus();
            }
        }

        private void SelectFirstLekara(object sender, ExecutedRoutedEventArgs e)
        {
            if (Lekari.IsSelected)
            {
                LekariPrikaz.SelectedIndex = 0;
                LekariPrikaz.Focus();
            }
        }
    }
}
