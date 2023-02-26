using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для CreateTournamentUserControl.xaml
    /// </summary>
    public partial class CreateTournamentUserControl : UserControl, IPrizeRequester
    {
        public List<TeamModel> availableTeams { get; set; }
        public List<TeamModel> selectedTeams { get; set; }
        public List<PrizeModel> selectedPrizes { get; set; }
        ITournamentSender caller;
        public CreateTournamentUserControl(ITournamentSender callingForm)
        {
            LoadData();
            InitializeComponent();
            caller = callingForm;
        }
        private void LoadData()
        {
            availableTeams = GlobalConfig.Connection.GetTeam_All();
            selectedTeams = new List<TeamModel>();
            selectedPrizes = new List<PrizeModel>();
        }

        private void addTeamButton_Click(object sender, RoutedEventArgs e)
        {
            TeamModel team = (TeamModel)selectTeamComboBox.SelectedItem;
            if (team != null)
            {
                selectedTeams.Add(team);
                availableTeams.Remove(team);
                RefreshContent();
            }
        }
        private void RefreshContent()
        {
            selectTeamComboBox.Items.Refresh();
            selectedTeamsListBox.Items.Refresh();
            prizesListBox.Items.Refresh();
        }

        private void removeSelectedPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            TeamModel team = (TeamModel)selectedTeamsListBox.SelectedItem;
            if (team != null)
            {
                selectedTeams.Remove(team);
                availableTeams.Add(team);
                RefreshContent();
            }
        }

        private void createTeamHyperLink_Click(object sender, RoutedEventArgs e)
        {
            // TODO - wire up a hyperlink click
        }

        public void PrizeComplete(PrizeModel model)
        {
            selectedPrizes.Add(model);
            RefreshContent();
        }

        private void createPrizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window prizeWindow = new CreatePrizeWindow(this);
            prizeWindow.Show();
        }

        private void removeSelectedPrizeButton_Click(object sender, RoutedEventArgs e)
        {
            PrizeModel prize = (PrizeModel)prizesListBox.SelectedItem;
            if (prize != null)
            {
                selectedPrizes.Remove(prize);
                RefreshContent();
            }
        }
        private void createTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate data

            decimal fee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeTextBox.Text, out fee);

            // Create tournament model

            TournamentModel tm = new TournamentModel();
            tm.TournamentName = tournamentNameTextBox.Text;
            if (!feeAcceptable)
            {
                MessageBox.Show("You need to input a valid entry fee!",
                    "Invalid Fee",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            tm.EntryFee = fee;

            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;

            // Wire matchups up
            TournamentLogic.CreateRounds(tm);

            // Create tournament entry
            // Create all of the prizes entries
            // Create all of the teams entries

            GlobalConfig.Connection.CreateTournament(tm);
            caller.sendTournament(tm);
        }
    }
}
