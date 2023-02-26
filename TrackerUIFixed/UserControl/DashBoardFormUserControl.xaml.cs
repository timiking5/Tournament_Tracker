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
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerUIFixed.Interfaces;

namespace TrackerUIFixed
{
    /// <summary>
    /// Логика взаимодействия для DashBoardForm.xaml
    /// </summary>
    public partial class DashBoardFormUserControl : UserControl
    {
        public List<TournamentModel> tournaments { get; set; }
        private ITournamentSender callingForm;
        public DashBoardFormUserControl(ITournamentSender caller)
        {
            callingForm = caller;
            LoadList();
            InitializeComponent();
        }
        private void LoadList()
        {
            tournaments = GlobalConfig.Connection.GetTournament_All();
        }

        private void loadTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            TournamentModel tm = (TournamentModel)chooseTournamentComboBox.SelectedItem;
            if (tm != null)
            {
                callingForm.sendTournament(tm);
            }
            else
            {
                MessageBox.Show("Select a tournament, please",
                    "Empty tournament",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}
