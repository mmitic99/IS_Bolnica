using Bolnica.DTOs;
using Bolnica.Kontroler;
using Kontroler;
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

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class LekarFeedbackWindow : Window
    {
        private FeedbackKontroler feedbackKontroler;
        public LekarFeedbackWindow()
        {
            InitializeComponent();
            feedbackKontroler = new FeedbackKontroler();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox.SelectedItem != null)
            {
                FeedbackDTO feedbackDTO = new FeedbackDTO()
                {
                    DatumIVreme = DateTime.Now,
                    JmbgKorisnika = LekarKontroler.getInstance().trenutnoUlogovaniLekar().Jmbg,
                    Ocena = ComboBox.SelectedIndex + 1,
                    Sadrzaj = txt.Text
                };
                feedbackKontroler.Save(feedbackDTO);
                this.Close();

            }
            else
                MessageBox.Show("Gas");


        }
    }
}