﻿using Model;
using Repozitorijum;
using System.Windows;
using System.Windows.Controls;

namespace Bolnica.view.SekretarView
{
    /// <summary>
    /// Interaction logic for DodavanjePacijentaTerminu.xaml
    /// </summary>
    public partial class DodavanjePacijentaTerminu : Window
    {
        Termin termin;
        TextBox pacijent;
        public DodavanjePacijentaTerminu(Termin termin, TextBox pacijent)
        {
            InitializeComponent();
            DodavanjePacijentaTerminuPrikaz.ItemsSource = SkladistePacijenta.GetInstance().GetAll();
            this.termin = termin;
            this.pacijent = pacijent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DodavanjePacijentaTerminuPrikaz.SelectedIndex != -1)
            {
                Pacijent selPac = (Pacijent)DodavanjePacijentaTerminuPrikaz.SelectedItem;
                termin.JmbgPacijenta = selPac.Jmbg;
                pacijent.Text = selPac.Ime + " " + selPac.Prezime;
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}