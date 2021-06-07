﻿
using Model;
using Repozitorijum;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.DTOs;
using Kontroler;
using Bolnica.Kontroler;

namespace Bolnica.view.LekarView
{
    
    public partial class AnamnezaPage : Page
    {

       
        
        String jmbgPacijenta;
        private PacijentKontroler pacijentKontroler;
        public AnamnezaPage(String jmbg)
        {
           
            InitializeComponent();
            DataContext = this;
            pacijentKontroler = new PacijentKontroler();
            jmbgPacijenta = jmbg;
            Anamneza_Table.ItemsSource = pacijentKontroler.GetByJmbg(jmbg).ZdravstveniKarton.Anamneze;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(jmbgPacijenta);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Anamneza_Table.SelectedIndex != -1) {
                AnamnezaDTO anamnezaDTO = AnamnezaKontroler.GetInstance().getAnamnezaById(((AnamnezaDTO)Anamneza_Table.SelectedItem).IdAnamneze);
               LekarWindow.getInstance().Frame1.Content = new AzurirajAnamnezuPage(anamnezaDTO);
            }
               
        }
    }
}
