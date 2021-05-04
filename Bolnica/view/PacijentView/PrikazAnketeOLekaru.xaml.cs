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
    /// <summary>
    /// Interaction logic for PrikazAnketeOLekaru.xaml
    /// </summary>
    public partial class PrikazAnketeOLekaru : UserControl
    {
        public PrikazAnketeOLekaru()
        {
            InitializeComponent();
            ImeLekara.Text = MainViewModel.getInstance().AnketaOLekaruVM.PunoImeLekara;
            Nazad.Command = MainViewModel.getInstance().PrikazObavestenjaCommand;

        }

        private void SacuvajAnketu_Click(object sender, RoutedEventArgs e)
        {
            PopunjenaAnketaPoslePregledaObjectDTO popunjena = new PopunjenaAnketaPoslePregledaObjectDTO()
            {
                JmbgLekara = MainViewModel.getInstance().AnketaOLekaruVM.anketa.JmbgLekara,
                Komentar = Komentar.Text,
                Ocena = vratiOcenu(OcenaLekara),
                IDAnkete = MainViewModel.getInstance().AnketaOLekaruVM.IdAnkete
            };
            if(AnketeKontroler.GetInstance().SacuvajAnketuOLekaru(popunjena))
            {
                MainViewModel.getInstance().CurrentView = MainViewModel.getInstance().PrikazObavestenjaVM;
            }
            else
            {
                var s = new Upozorenje("Morate uneti sve ocene!");
                s.Owner = PacijentMainWindow.getInstance();
                s.ShowDialog();
            }
        }

        private object vratiOcenu(StackPanel strucnostOsoblja)
        {
            foreach (RadioButton RB in strucnostOsoblja.Children)
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
