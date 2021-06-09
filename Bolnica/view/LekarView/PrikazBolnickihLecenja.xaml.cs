using Bolnica.DTOs;
using Bolnica.Kontroler;
using Bolnica.Repozitorijum.XmlSkladiste;
using Kontroler;
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
            setToolTip(LekarProfilPage.isToolTipVisible);
            List<ProstorijaDTO> prostorije = new List<ProstorijaDTO>();

            foreach (ProstorijaDTO p in ProstorijeKontroler.GetInstance().GetAll())
            {
                if (p.VrstaProstorije == Model.Enum.VrstaProstorije.Soba_za_bolesnike)
                    prostorije.Add(p);
            }
            ProstorijaBox.ItemsSource = prostorije;
        }

        private void Button_Click_Produzi(object sender, RoutedEventArgs e)
        {
            if (Bolnicka_Lecenja_Table.SelectedIndex != -1)
            {
                List<BolnickoLecenjeDTO> bolnickaLecenjaDTO = BolnickaLecenjaKontroler.GetInstance().GetAll();
                for (int i = 0; i < bolnickaLecenjaDTO.Count; i++)
                {
                    if (i == Bolnicka_Lecenja_Table.SelectedIndex)
                    {
                        bolnickaLecenjaDTO[i].DatumOtpustanja = (DateTime)Kalendar.SelectedDate;
                    }
                }

                BolnickaLecenjaKontroler.GetInstance().SaveAll(bolnickaLecenjaDTO);
                Bolnicka_Lecenja_Table.ItemsSource = new ObservableCollection<BolnickoLecenjeDTO>(BolnickaLecenjaKontroler.GetInstance().GetAll());
            }else
                MessageBox.Show("Označite pacijenta kojeg želite da produžite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Button_Click_Otpusti(object sender, RoutedEventArgs e)
        {
            if (Bolnicka_Lecenja_Table.SelectedIndex != -1)
            {

               
                    List<BolnickoLecenjeDTO> bolnickaLecenja = BolnickaLecenjaKontroler.GetInstance().GetAll();
                    
                    foreach (BolnickoLecenjeDTO bolnickoLecenje in bolnickaLecenja.ToList())
                    {
                        if (bolnickoLecenje.jmbgPacijenta.Equals(((BolnickoLecenjeDTO)Bolnicka_Lecenja_Table.SelectedItem).jmbgPacijenta))
                        {
                            bolnickaLecenja.Remove(bolnickoLecenje);
                            
                        }
                    }

                BolnickaLecenjaKontroler.GetInstance().SaveAll(bolnickaLecenja);

                if (ProstorijaBox.SelectedItem != null)
                {
                    List<BolnickoLecenjeDTO> bolnickoLecenjeDTO = BolnickaLecenjaKontroler.GetInstance().GetAll();
                    List<BolnickoLecenjeDTO> novoBolnickoLecenjeDTO = new List<BolnickoLecenjeDTO>();
                    foreach (BolnickoLecenjeDTO bolnickoLecenje in bolnickoLecenjeDTO)
                    {
                        if (bolnickoLecenje.idProstorije.Equals(((ProstorijaDTO)ProstorijaBox.SelectedItem).IdProstorije))
                            novoBolnickoLecenjeDTO.Add(bolnickoLecenje);
                    }


                    Bolnicka_Lecenja_Table.ItemsSource = new ObservableCollection<BolnickoLecenjeDTO>(novoBolnickoLecenjeDTO);
                }
                else
                Bolnicka_Lecenja_Table.ItemsSource = new ObservableCollection<BolnickoLecenjeDTO>(BolnickaLecenjaKontroler.GetInstance().GetAll());
            
            }
            else
            {
                MessageBox.Show("Označite pacijenta kojeg želite da otpustite !", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }   

        private void Button_Click_Povratak(object sender, RoutedEventArgs e)
        {
            if (PacijentInfoPage.getInstance() == null || PacijentInfoPage.getInstance().ComboBox1.SelectedItem == null)
            {
                LekarWindow.getInstance().Frame1.Content = new PacijentInfoPage(null);
            }
            else
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
        private void setToolTip(bool Prikazi)
        {


            if (Prikazi)
            {
                Style style = new Style(typeof(ToolTip));
                style.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
                style.Seal();
                this.Resources.Add(typeof(ToolTip), style);


            }
            else
            {
                this.Resources.Remove(typeof(ToolTip));
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<BolnickoLecenjeDTO> bolnickoLecenjeDTO = BolnickaLecenjaKontroler.GetInstance().GetAll();
            List<BolnickoLecenjeDTO> novoBolnickoLecenjeDTO = new List<BolnickoLecenjeDTO>();
            foreach (BolnickoLecenjeDTO bolnickoLecenje in bolnickoLecenjeDTO)
            {
                if (bolnickoLecenje.idProstorije.Equals(((ProstorijaDTO)ProstorijaBox.SelectedItem).IdProstorije))
                    novoBolnickoLecenjeDTO.Add(bolnickoLecenje);
            }
           
          
            Bolnicka_Lecenja_Table.ItemsSource = new ObservableCollection<BolnickoLecenjeDTO>(novoBolnickoLecenjeDTO);
        }
    }
}
