using Bolnica.view.LekarView;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Bolnica.model;
using Bolnica.Repozitorijum.XmlSkladiste;
using Bolnica.DTOs;
using Kontroler;
using Servis;
using Bolnica.Kontroler;
using System.Windows.Forms;
using System.Windows.Input;
using Help = Bolnica.view.LekarView.Help;
using Bolnica.view.LekarView.Help;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for PregledWindow.xaml
    /// </summary>
    public partial class LekarWindow : Window
    {
        private static LekarWindow instance = null;


        public static LekarWindow getInstance()
        {
            return instance;
        }
       
        

        public Lekar lekarTrenutni;
        public LekarDTO lekarDTO;
     


        public LekarWindow(LekarDTO lekar)
        {
            lekarDTO = lekar;
            InitializeComponent();
            this.DataContext = this;
            Frame1.Content = new TerminiPage(lekar);
            lekarTrenutni = LekarServis.getInstance().GetByJmbg(lekar.Jmbg);
            
            instance = this;
            AddHotKeys();

        }
        private void AddHotKeys()
        {
            try
            {
                RoutedCommand firstSettings = new RoutedCommand();
                firstSettings.InputGestures.Add(new KeyGesture(Key.L, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(firstSettings, My_first_event_handler));

                RoutedCommand secondSettings = new RoutedCommand();
                secondSettings.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(secondSettings, My_second_event_handler));

                RoutedCommand thirdSettings = new RoutedCommand();
                thirdSettings.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(thirdSettings, My_third_event_handler));

                RoutedCommand fourthSettings = new RoutedCommand();
                fourthSettings.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(fourthSettings, My_fourth_event_handler));

                RoutedCommand fifthSettings = new RoutedCommand();
                fifthSettings.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(fifthSettings, My_fifth_event_handler));

                RoutedCommand sixhtSettings = new RoutedCommand();
                sixhtSettings.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(sixhtSettings, My_sixht_event_handler));

                RoutedCommand seventhSettings = new RoutedCommand();
                seventhSettings.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(seventhSettings, My_seventh_event_handler));

                RoutedCommand eighthSettings = new RoutedCommand();
                eighthSettings.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(eighthSettings, My_eight_event_handler));

                RoutedCommand ninthSettings = new RoutedCommand();
                ninthSettings.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(ninthSettings, My_ninth_event_handler));

                RoutedCommand help = new RoutedCommand();
                help.InputGestures.Add(new KeyGesture(Key.F1));
                CommandBindings.Add(new CommandBinding(help, Help_handler));

            }
            catch (Exception err)
            {
                //handle exception error
            }
        }
        private void My_first_event_handler(object sender, ExecutedRoutedEventArgs e)
        {
            Frame1.Content = new LekoviPage();
        }

        private void My_second_event_handler(object sender, RoutedEventArgs e)
        {
            Frame1.Content = new PacijentInfoPage(null);
        }

        private void My_third_event_handler(object sender, RoutedEventArgs e)
        {
            Frame1.Content = new LekarObavestenjaPage();
        }

        private void My_fourth_event_handler(object sender, RoutedEventArgs e)
        {
            Frame1.Content = new TerminiPage(lekarDTO);
        }

        private void My_fifth_event_handler(object sender, RoutedEventArgs e)
        {
            Frame1.Content = new LekarProfilPage();
        }

        private void My_sixht_event_handler(object sender, RoutedEventArgs e)
        {
           
                if (PacijentInfoPage.getInstance()==null || PacijentInfoPage.getInstance().ComboBox1.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Nije izabran pacijent!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    Frame1.Content = new IzdavanjeReceptaPage();
            
        }

        private void My_seventh_event_handler(object sender, RoutedEventArgs e)
        {
            if (PacijentInfoPage.getInstance() == null || PacijentInfoPage.getInstance().ComboBox1.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Nije izabran pacijent!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                String Jmbg = ((PacijentDTO)PacijentInfoPage.getInstance().ComboBox1.SelectedItem).Jmbg;
                Frame1.Content = new ZakazivanjeTerminaPage(Jmbg);
                    }
        }
        private void My_eight_event_handler(object sender, RoutedEventArgs e)
        {
            if (PacijentInfoPage.getInstance() == null || PacijentInfoPage.getInstance().ComboBox1.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Nije izabran pacijent!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                String Jmbg = ((PacijentDTO)PacijentInfoPage.getInstance().ComboBox1.SelectedItem).Jmbg;
                Frame1.Content = new AnamnezaPage(Jmbg);
            }
        }
        private void My_ninth_event_handler(object sender, RoutedEventArgs e)
        {

            Frame1.Content = new PrikazBolnickihLecenja();
            
        }
        private void Help_handler(object sender, RoutedEventArgs e)
        {
            var z = new HelpF1();
            z.Show();
           

        }
    }



}  

