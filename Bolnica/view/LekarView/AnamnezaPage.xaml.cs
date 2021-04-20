﻿
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
        public Pacijent pacijent;
        public AnamnezaPage(String jmbg)
        {
           
            InitializeComponent();
            DataContext = this;
            Anamneze = new List<Anamneza>();
            pacijent = SkladistePacijenta.GetInstance().getByJmbg(jmbg);
            Anamneze = pacijent.zdravstveniKarton.Anamneze;

            Console.WriteLine(jmbg);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(pacijent.Jmbg);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Anamneza_Table.SelectedIndex != -1) {
                Anamneza anamneza = pacijent.zdravstveniKarton.getAnamnezaById(((Anamneza)Anamneza_Table.SelectedItem).IdAnamneze);
               LekarWindow.getInstance().Frame1.Content = new AzurirajAnamnezuPage(anamneza);
            }
               
        }
    }
}