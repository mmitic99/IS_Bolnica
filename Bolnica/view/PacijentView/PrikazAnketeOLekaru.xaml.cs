using Bolnica.viewActions;
using System;
using System.Collections.Generic;
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
using Bolnica.DTOs;
using Bolnica.Kontroler;

namespace Bolnica.view.PacijentView
{
    public partial class PrikazAnketeOLekaru : UserControl
    {
        private MainViewModel MainViewModel;
        private AnketaOLekaruViewModel ViewModel;
        private AnketeOLekaruKontroler AnketeOLekaruKontroler;
        public PrikazAnketeOLekaru()
        {
            InitializeComponent();
            this.MainViewModel = MainViewModel.getInstance();
            this.ViewModel = MainViewModel.AnketaOLekaruVM;
            this.AnketeOLekaruKontroler = new AnketeOLekaruKontroler();
            ImeLekara.Text = ViewModel.PunoImeLekara;
            Nazad.Command = MainViewModel.PrikazObavestenjaCommand;

        }

        private void SacuvajAnketu_Click(object sender, RoutedEventArgs e)
        {

            PopunjenaAnketaPoslePregledaObjectDTO popunjena = new PopunjenaAnketaPoslePregledaObjectDTO()
            {
                JmbgLekara = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.anketaOLekaru.JmbgLekara,
                Komentar = Komentar.Text,
                Ocena = vratiOcenu(OcenaLekara),
                IDAnkete = MainViewModel.getInstance().PrikazObavestenjaVM.obavestenje.anketaOLekaru.IDAnkete
            };
            if(AnketeOLekaruKontroler.SacuvajAnketuOLekaru(popunjena))
            {
                MainViewModel.CurrentView = MainViewModel.PrikazObavestenjaVM;
            }
            else
            {
                var s = new Upozorenje("Morate uneti sve ocene!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
        }

        private object vratiOcenu(StackPanel OcenaLekara)
        {
            foreach (RadioButton RB in OcenaLekara.Children)
            {
                if ((bool)RB.IsChecked)
                {
                    return RB.Content;
                }
            }
            return "-1";

        }
    }
}
