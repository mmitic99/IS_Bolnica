using Model;
using Model.Skladista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bolnica.view
{
    /// <summary>
    /// Interaction logic for DodavanjeProstorijeWindow.xaml
    /// </summary>
    public partial class DodavanjeProstorijeWindow : Window
    {
        SkladisteZaProstorije skladiste = new SkladisteZaProstorije();
        private UpravnikWindow upravnikWindow;
        private Prostorija p;
        private bool checkBrojProstorije, checkVrstaProstorije, checkSprat, checkKvadratura;
        public DodavanjeProstorijeWindow(UpravnikWindow uw)
        {
            InitializeComponent();
            checkBrojProstorije = false;
            checkVrstaProstorije = false;
            checkSprat = false;
            checkKvadratura = false;
            p = new Prostorija();
            upravnikWindow = uw;
            PotvrdiButton.IsEnabled = false;
            p.RenoviraSe_ = false;
        }

        private void PotvrdiClick(object sender, RoutedEventArgs e)
        {
            upravnikWindow.lista.Add(p);
            skladiste.Save(p);
            upravnikWindow.TabelaProstorija.Items.Refresh();
            
            this.Close();
        }

        private void OdustaniClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void BrojProstorijeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regBroj = new Regex("[0-9a-zA-Z]+");
            if (regBroj.IsMatch(BrojProstorijeTextBox.Text))
            {
                foreach (Prostorija Soba in upravnikWindow.lista)
                {
                    if (BrojProstorijeTextBox.Text.Equals(Soba.BrojSobe_))
                    {
                        MessageBox.Show("Već postoji soba sa istim brojem !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        checkBrojProstorije = false;
                        break;
                    }
                    else
                    {
                        p.BrojSobe_ = BrojProstorijeTextBox.Text;
                        checkBrojProstorije = true;
                    }
                }
            }
            else if (BrojProstorijeTextBox.Text.Equals("")) { }
            else
            {
                MessageBox.Show(" Broj prostorije se mora sastojati od brojeva i slova !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                BrojProstorijeTextBox.Text = "";
                checkBrojProstorije = false;
            }
            if (checkBrojProstorije == true && checkVrstaProstorije == true && checkSprat == true && checkKvadratura == true)
                PotvrdiButton.IsEnabled = true;
        }

        private void VrstaProstorijeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VrstaProstorijeComboBox.SelectedIndex.Equals(-1))
            {
                MessageBox.Show("Odaberite vrstu prostorije !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                checkVrstaProstorije = false;
            }
            else if (VrstaProstorijeComboBox.SelectedIndex.Equals(0))
            {
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Soba_za_preglede;
                checkVrstaProstorije = true;
            }
            else if (VrstaProstorijeComboBox.SelectedIndex.Equals(1))
            {
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Operaciona_sala;
                checkVrstaProstorije = true;
            }
            else if (VrstaProstorijeComboBox.SelectedIndex.Equals(2))
            {
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Soba_za_bolesnike;
                checkVrstaProstorije = true;
            }
            else if (VrstaProstorijeComboBox.SelectedIndex.Equals(3))
            {
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Magacin;
                checkVrstaProstorije = true;
            }
            if (checkBrojProstorije == true && checkVrstaProstorije == true && checkSprat == true && checkKvadratura == true)
                PotvrdiButton.IsEnabled = true;
        }

        private void SpratTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regSprat = new Regex("[0-9]{1,1}");
            if (regSprat.IsMatch(SpratTextBox.Text))
            {
                p.Sprat_ = Int32.Parse(SpratTextBox.Text);
                checkSprat = true;
            }
            else if (SpratTextBox.Text.Equals("")) { }
            else
            {
                MessageBox.Show("Uneli ste nepostojeći sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                SpratTextBox.Text = "";
                checkSprat = false;
            }
            if (checkBrojProstorije == true && checkVrstaProstorije == true && checkSprat == true && checkKvadratura == true)
                PotvrdiButton.IsEnabled = true;
        }

        private void KvadraturaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regKvadratura = new Regex("[1-9]{1}[0-9]*");
            if (regKvadratura.IsMatch(KvadraturaTextBox.Text))
            {
                p.Kvadratura_ = Double.Parse(KvadraturaTextBox.Text);
                checkKvadratura = true;
            }
            else if (KvadraturaTextBox.Text.Equals("")) { }
            else
            {
                MessageBox.Show("Kvadratura predstavlja broj u metrima kvadratnim !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                KvadraturaTextBox.Text = "";
                checkKvadratura = false;
            }
            if (checkBrojProstorije == true && checkVrstaProstorije == true && checkSprat == true && checkKvadratura == true)
                PotvrdiButton.IsEnabled = true;
        }
    }
}
