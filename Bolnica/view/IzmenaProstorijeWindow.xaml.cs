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
    /// Interaction logic for IzmenaProstorijeWindow.xaml
    /// </summary>
    public partial class IzmenaProstorijeWindow : Window
    {
        private UpravnikWindow upravnikWindow;
        private Prostorija p;
        SkladisteZaProstorije skladiste = new SkladisteZaProstorije();
        private int indexSelektovanog;
        private bool checkBrojProstorije, checkVrstaProstorije, checkSprat, checkKvadratura;

        public IzmenaProstorijeWindow(UpravnikWindow uw, int index)
        {
            InitializeComponent();
            checkBrojProstorije = true;
            checkVrstaProstorije = true;
            checkSprat = true;
            checkKvadratura = true;
            p = uw.lista[index];
            upravnikWindow = uw;
            indexSelektovanog = index;
            this.BrojProstorijeTextBox.Text = uw.lista.ElementAt(index).BrojSobe_;
            this.SpratTextBox.Text = uw.lista.ElementAt(index).Sprat_.ToString();
            this.KvadraturaTextBox.Text = uw.lista.ElementAt(index).Kvadratura_.ToString();
            p.BrojSobe_ = uw.lista.ElementAt(index).BrojSobe_;
            p.Sprat_ = uw.lista.ElementAt(index).Sprat_;
            p.Kvadratura_ = uw.lista.ElementAt(index).Kvadratura_;
            if (uw.lista.ElementAt(index).RenoviraSe_ == true)
            {
                this.RenoviranjeCheckBox.IsChecked = true;
                p.RenoviraSe_ = true;
            }
            else
            {
                this.RenoviranjeCheckBox.IsChecked = false;
                p.RenoviraSe_ = false;
            }
            if (uw.lista.ElementAt(index).VrstaProstorije_ == Model.Enum.VrstaProstorije.Soba_za_preglede)
            {
                this.VrstaProstorijeComboBox.Text = "Soba za preglede" ;
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Soba_za_preglede;
            }
            else if (uw.lista.ElementAt(index).VrstaProstorije_ == Model.Enum.VrstaProstorije.Operaciona_sala)
            {
                this.VrstaProstorijeComboBox.Text = "Operaciona sala";
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Operaciona_sala;
            }
            else if (uw.lista.ElementAt(index).VrstaProstorije_ == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
            {
                this.VrstaProstorijeComboBox.Text = "Soba za bolesnike";
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Soba_za_bolesnike;
            }
            else if (uw.lista.ElementAt(index).VrstaProstorije_ == Model.Enum.VrstaProstorije.Magacin)
            {
                this.VrstaProstorijeComboBox.Text = "Magacin";
                p.VrstaProstorije_ = Model.Enum.VrstaProstorije.Magacin;
            }
        }

        private void PotvrdiClick(object sender, RoutedEventArgs e)
        {
            if (checkBrojProstorije == true && checkVrstaProstorije == true && checkSprat == true && checkKvadratura == true)
            {
                upravnikWindow.lista[indexSelektovanog] = p;
                skladiste.SaveAll(upravnikWindow.lista);
                upravnikWindow.TabelaProstorija.Items.Refresh();
                this.Close();
            }
            else
            {
                MessageBox.Show("Niste validno popunili polja !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OdustaniClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RenoviranjeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            p.RenoviraSe_ = true;
        }

        private void RenoviranjeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            p.RenoviraSe_ = false;
        }

        private void SpratTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex regSprat = new Regex("[0-9]{1}");
            if (regSprat.IsMatch(SpratTextBox.Text))
            {
                p.Sprat_ = Int32.Parse(SpratTextBox.Text);
                checkSprat = true;
            }
            //else if (SpratTextBox.Text.Equals("")) { checkSprat = false; }
            else
            {
                MessageBox.Show("Uneli ste nepostojeći sprat !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                SpratTextBox.Text = "";
                checkSprat = false;
            }
        }

        private void KvadraturaTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex regKvadratura = new Regex("[1-9]{1}[0-9]*");
            if (regKvadratura.IsMatch(KvadraturaTextBox.Text))
            {
                p.Kvadratura_ = Double.Parse(KvadraturaTextBox.Text);
                checkKvadratura = true;
            }
           // else if (KvadraturaTextBox.Text.Equals("")) { checkKvadratura = false; }
            else
            {
                MessageBox.Show("Kvadratura predstavlja broj u metrima kvadratnim !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                KvadraturaTextBox.Text = "";
                checkKvadratura = false;
            }
        }

        private void BrojProstorijeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Regex regBroj = new Regex("[0-9a-zA-Z]+");
            if (regBroj.IsMatch(BrojProstorijeTextBox.Text))
            {
                p.BrojSobe_ = BrojProstorijeTextBox.Text;
                 checkBrojProstorije = true;
            }
          //  else if (BrojProstorijeTextBox.Text.Equals("")) { checkBrojProstorije = false; }
            else
            {
                MessageBox.Show(" Broj prostorije se mora sastojati od brojeva i slova !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                BrojProstorijeTextBox.Text = "";
                checkBrojProstorije = false;
            }
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
        }
    }
}