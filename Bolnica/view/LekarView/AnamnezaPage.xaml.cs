using Bolnica.model;
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for AnamnezaPage.xaml
    /// </summary>
    public partial class AnamnezaPage : Page
    {

        public List<Anamneza> Anamneze { get; set; }
        public ZdravstveniKarton Karton;
        public Pacijent pacijent;
        public AnamnezaPage(String jmbg)
        {
           
            InitializeComponent();
            Anamneze = new List<Anamneza>();
            pacijent = SkladistePacijenta.GetInstance().getByJmbg(jmbg);
            Karton = pacijent.zdravstveniKarton;
            Anamneze = pacijent.zdravstveniKarton.Anamneze;

            Console.WriteLine(jmbg);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }
    }
}
