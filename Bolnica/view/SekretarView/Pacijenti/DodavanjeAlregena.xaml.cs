using System.Collections.ObjectModel;
using System.Windows;

namespace Bolnica.view.SekretarView.Pacijenti
{
    /// <summary>
    /// Interaction logic for DodavanjeAlregena.xaml
    /// </summary>
    public partial class DodavanjeAlregena : Window
    {
        ObservableCollection<string> alergeni;
        public DodavanjeAlregena(ObservableCollection<string> alergeni)
        {
            InitializeComponent();
            this.alergeni = alergeni;
            alergen.Focus();
        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            if(!alergen.Text.Equals("") && !alergeni.Contains(alergen.Text))
            {
                alergeni.Add(alergen.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("Polje za unos alergena ne može biti prazno.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void otkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
