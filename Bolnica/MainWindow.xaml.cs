using Bolnica.view;
using Model;
using System.Collections.Generic;
using System.Windows;

namespace Bolnica
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
       }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("u");
            this.Close();
            s.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("s");
            this.Close();
            s.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("l");
            this.Close();
            s.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var s = new Prijavljivanje("p");
            this.Close();
            s.Show();
        }
    }
}
