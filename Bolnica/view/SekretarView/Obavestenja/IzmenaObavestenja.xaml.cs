﻿using Kontroler;
using Model;
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

namespace Bolnica.view.SekretarView.Obavestenja
{
    /// <summary>
    /// Interaction logic for IzmenaObavestenja.xaml
    /// </summary>
    public partial class IzmenaObavestenja : Window
    {
        private DataGrid obavestenjaPrikaz;
        private Obavestenje obavestenje;
        private ObavestenjaKontroler obavestenjaKontroler = new ObavestenjaKontroler();
        public IzmenaObavestenja(DataGrid obavestenjaPrikaz)
        {
            InitializeComponent();
            this.obavestenjaPrikaz = obavestenjaPrikaz;
            obavestenje = (Obavestenje)obavestenjaPrikaz.SelectedItem;
            naslov.Text = obavestenje.Naslov;
            sadrzaj.Text = obavestenje.Sadrzaj;
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            Obavestenje novoObavestenje = new Obavestenje
            {
                Naslov = naslov.Text,
                Sadrzaj = sadrzaj.Text,
                VremeObavestenja = DateTime.Now,
                JmbgKorisnika = obavestenje.JmbgKorisnika,
                Podsetnik = obavestenje.Podsetnik
            };

            bool uspesno = obavestenjaKontroler.IzmeniObavestenje(obavestenje, novoObavestenje);
            if (uspesno)
            {
                obavestenjaPrikaz.ItemsSource = ObavestenjaKontroler.getInstance().GetByJmbg("-1");
                obavestenjaPrikaz.ScrollIntoView(novoObavestenje);
                obavestenjaPrikaz.SelectedItem = novoObavestenje;
                this.Close();
            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}