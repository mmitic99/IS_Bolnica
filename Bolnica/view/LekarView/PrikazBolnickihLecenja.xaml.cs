using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.Repozitorijum.XmlSkladiste;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bolnica.view.LekarView
{
    /// <summary>
    /// Interaction logic for PrikazBolnickihLecenja.xaml
    /// </summary>
    public partial class PrikazBolnickihLecenja : Page
    {
        public PrikazBolnickihLecenja()
        {
            InitializeComponent();
            this.DataContext = this;
            Bolnicka_Lecenja_Table.ItemsSource = BolnickaLecenjaKontroler.GetInstance().GetAll();

        }

        private void Button_Click_Produzi(object sender, RoutedEventArgs e)
        {
            List<BolnickoLecenjeDTO> bolnickaLecenjaDTO = BolnickaLecenjaKontroler.GetInstance().GetAll();
            for(int i=0;i<bolnickaLecenjaDTO.Count;i++)
            {
                if (i == Bolnicka_Lecenja_Table.SelectedIndex)
                {
                    bolnickaLecenjaDTO[i].DatumOtpustanja = (DateTime)Kalendar.SelectedDate;
                }
            }
            
            BolnickaLecenjaKontroler.GetInstance().SaveAll(bolnickaLecenjaDTO);
            Bolnicka_Lecenja_Table.ItemsSource = new ObservableCollection<BolnickoLecenjeDTO>(BolnickaLecenjaKontroler.GetInstance().GetAll());
        }

        private void Button_Click_Otpusti(object sender, RoutedEventArgs e)
        {
           List <BolnickoLecenjeDTO> bolnickoLecenjeDTO = BolnickaLecenjaKontroler.GetInstance().GetAll();
            bolnickoLecenjeDTO.RemoveAt(Bolnicka_Lecenja_Table.SelectedIndex);
            BolnickaLecenjaKontroler.GetInstance().SaveAll(bolnickoLecenjeDTO);
            Bolnicka_Lecenja_Table.ItemsSource = new ObservableCollection<BolnickoLecenjeDTO>(BolnickaLecenjaKontroler.GetInstance().GetAll());

        }   

        private void Button_Click_Povratak(object sender, RoutedEventArgs e)
        {
            LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(PacijentInfoPage.getInstance().Jmbg);
        }

        private void Bolnicka_Lecenja_Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BolnickoLecenjeDTO bolnickoLecenjeDTO = (BolnickoLecenjeDTO)Bolnicka_Lecenja_Table.SelectedItem;
            if (bolnickoLecenjeDTO != null)
            {
                Kalendar.DisplayDateStart = bolnickoLecenjeDTO.DatumOtpustanja.AddDays(1);
            }
        }
    }
}
