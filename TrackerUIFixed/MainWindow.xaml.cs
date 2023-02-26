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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ITournamentSender
    {
        public MainWindow()
        {
            GlobalConfig.InitializeConnections(DatabaseType.Sql);
            InitializeComponent();
            switchToDashBoard();
        }

        private void SwitchToCreatePrizeButton_Click(object sender, RoutedEventArgs e)
        {
            mainCC.Content = new CreatePrizeUserControl();
        }

        private void SwitchToTeamButton_Click(object sender, RoutedEventArgs e)
        {
            mainCC.Content = new CreateTeamUserControl();
        }
        private void SwitchToCreateTournamentButtonPrize_Click(object sender, RoutedEventArgs e)
        {
            mainCC.Content = new CreateTournamentUserControl(this);
        }
        private void switchToTournamentDashBoard_Click(object sender, RoutedEventArgs e)
        {
            switchToDashBoard();
        }

        public void sendTournament(TournamentModel model)
        {
            mainCC.Content = new TournamentViewerUserInterface(model);
        }
        private void switchToDashBoard()
        {
            mainCC.Content = new DashBoardFormUserControl(this);
        }
    }
}
