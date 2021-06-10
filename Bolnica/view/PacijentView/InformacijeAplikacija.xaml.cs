using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.viewActions;
using System;
using System.Windows;
using System.Windows.Controls;


namespace Bolnica.view.PacijentView
{
    public partial class InformacijeAplikacija : UserControl
    {
        private FeedbackKontroler FeedbackKontroler;
        public InformacijeAplikacija()
        {
            InitializeComponent();
            this.FeedbackKontroler = new FeedbackKontroler();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FeedbackDTO feedback = new FeedbackDTO()
            {
                DatumIVreme = DateTime.Now,
                JmbgKorisnika = MainViewModel.getInstance().JmbgPacijenta,
                Ocena = vratiOcenu(OcenaAplikacije),
                Sadrzaj = Komentar.Text
            };

            if (FeedbackKontroler.Save(feedback))
            {
                Komentar.Text = "";
                isklucioRB(OcenaAplikacije);
            }
            else
            {
                var s = new Upozorenje("Morate uneti ocenu!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
        }

        private void isklucioRB(StackPanel ocenaAplikacije)
        {
            foreach (RadioButton RB in OcenaAplikacije.Children)
            {
                if ((bool)RB.IsChecked)
                {
                    RB.IsChecked =false;
                    break;
                }
            }
        }

        private int vratiOcenu(StackPanel OcenaAplikacije)
        {
            int povratna = -1;
            foreach (RadioButton RB in OcenaAplikacije.Children)
            {
                if ((bool)RB.IsChecked)
                {
                    povratna = Int32.Parse((String)RB.Content);
                    break;
                }
            }
            return povratna;
        }
    }
}
