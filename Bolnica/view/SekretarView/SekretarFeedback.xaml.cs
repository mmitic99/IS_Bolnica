using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bolnica.DTOs;
using Bolnica.Kontroler;
using MessageBox = System.Windows.MessageBox;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for SekretarFeedback.xaml
    /// </summary>
    public partial class SekretarFeedback : Window
    {
        private FeedbackKontroler feedbackKontroler;
        private string Jmbg;
        public SekretarFeedback(String jmbg)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            feedbackKontroler = new FeedbackKontroler();
            Jmbg = jmbg;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if (sadrzaj.Text.Trim().Length == 0)
            {
                MessageBox.Show("Polje za sadržaj ne može ostati prazno.", "Greška", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            feedbackKontroler.Save(new FeedbackDTO()
            {
                DatumIVreme = DateTime.Now,
                JmbgKorisnika = Jmbg,
                Ocena = ocena.SelectedIndex + 1,
                Sadrzaj = sadrzaj.Text
            });
            MessageBox.Show("Hvala Vam na feedback-u", "Poruka", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();

        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
