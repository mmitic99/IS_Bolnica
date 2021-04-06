using Model;
using System.Windows;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for SekretarWindow.xaml
    /// </summary>
    public partial class SekretarWindow : Window
    {
        Sekretar sekretar;
        public SekretarWindow(Sekretar sekretar)
        {
            InitializeComponent();

            this.sekretar = sekretar;
            /*
            imeS.Content = sekretar.Ime;
            prezimeS.Content = sekretar.Prezime;


            statusBar.Text = DateTime.Now.ToString("dddd, dd.MM.yyyy HH:mm:ss");
            pacijentiPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.5);
            timer.Tick += Timer_Tick;
            timer.Start();*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new PrikazPacijenata();
            s.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjePacijenta();
            s.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new DodavanjeGostujucegPacijenta();
            s.Show();
        }
    }
}
