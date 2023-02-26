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

namespace TrackerUIFixed
{
    /// <summary>
    /// Логика взаимодействия для TournamentViewerUserInterface.xaml
    /// </summary>
    public partial class TournamentViewerUserInterface : UserControl
    {
        private TournamentModel tournament;
        public List<MatchupModel> displayedMatchups { get; set; } = new List<MatchupModel>();
        public List<int> rounds { get; set; }
        public TournamentViewerUserInterface(TournamentModel tournamentModel)
        {
            tournament = tournamentModel;
            tournament.OnTournamentComplete += Tournament_OnTournamentComplete;
            LoadRounds();
            InitializeComponent();
            LoadFormData();
        }

        private void Tournament_OnTournamentComplete(object? sender, DateTime e)
        {
            MessageBox.Show("Tournament is complete!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            // TODO - maybe more work?
        }

        private void LoadFormData()
        {
            tournamentNameLabel.Content = tournament.TournamentName;
        }
        private void RefreshContent()
        {
            matchupListBox.ItemsSource = displayedMatchups;
            matchupListBox.Items.Refresh();
            // roundComboBox.Items.Refresh();
        }
        private void LoadRounds()
        {
            rounds = new List<int>();
            rounds.Add(1);
            int currRound = 1;
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                }
            }
        }

        private void roundComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMatchups();
        }
        private void LoadMatchups()
        {
            if (roundComboBox.SelectedIndex > -1) { 
                int round = (int)roundComboBox.SelectedItem;
                if (round > 0)
                {
                    foreach (List<MatchupModel> matchups in tournament.Rounds)
                    {
                        bool flag = unplayedOnlyCheckbox.IsChecked ?? false;
                        if (matchups.First().MatchupRound == round)
                        {
                            displayedMatchups.Clear();
                            foreach (MatchupModel matchup in matchups)
                            {
                                if (matchup.Winner == null || !flag)
                                {
                                    displayedMatchups.Add(matchup);
                                }
                            }
                            RefreshContent();
                            // MessageBox.Show($"{matchupListBox.Items.Count} - {displayedMatchups.Count}");
                        }
                    }
                }
                DisplayMatchupWindow();
            }
        }
        private void DisplayMatchupWindow()
        {
            Visibility myVisibility;
            bool isEnababled = (displayedMatchups.Count > 0);
            if (isEnababled)
            {
                myVisibility = Visibility.Visible;
            }
            else
            {
                myVisibility = Visibility.Hidden;
            }
            teamOneLabel.Visibility = myVisibility;
            teamTwoLabel.Visibility = myVisibility;
            teamOneScoreLabel.Visibility = myVisibility;
            teamTwoScoreLabel.Visibility = myVisibility;
            teamOneScoreTextBox.Visibility = myVisibility;
            teamTwoScoreTextBox.Visibility = myVisibility;
            submitButton.Visibility = myVisibility;
            vsLabel.Visibility = myVisibility;
        }
        private void matchupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMatchup();
        }
        private void LoadMatchup()
        {
            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            if (m != null)
            {
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[i].TeamCompeting != null)
                        {
                            teamOneLabel.Content = m.Entries[i].TeamCompeting.TeamName;
                            teamOneScoreTextBox.Text = m.Entries[i].Score.ToString();
                        }
                        else
                        {
                            teamOneLabel.Content = "Empty team";
                            teamOneScoreTextBox.Text = "";
                        }
                    }
                    if (i == 1)
                    {
                        if (m.Entries[i].TeamCompeting != null)
                        {
                            teamTwoLabel.Content = m.Entries[i].TeamCompeting.TeamName;
                            teamTwoScoreTextBox.Text = m.Entries[i].Score.ToString();
                        }
                        else
                        {
                            teamTwoLabel.Content = "Empty team";
                            teamTwoScoreTextBox.Text = "";
                        }
                    }
                }
                if (m.Entries.Count == 1)
                {
                    teamTwoLabel.Content = "Empty team";
                    teamTwoScoreTextBox.Text = "";
                }
            }
        }

        private void unplayedOnlyCheckbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            LoadMatchups();
            // RefreshContent();
        }
        private string ValidateData()
        {
            string output = "";
            double teamOneScore = 0;
            double teamTwoScore = 0;
            bool scoreOneValid = double.TryParse(teamOneScoreTextBox.Text, out teamOneScore);
            bool scoreTwoValid = double.TryParse(teamTwoScoreTextBox.Text, out teamTwoScore);

            if (!scoreOneValid)
            {
                output = "The score one is not a valid number.";
            }
            else if (!scoreTwoValid)
            {
                output = "The score two is not a valid number.";
            }
            else if (teamOneScore == 0 && teamTwoScore == 0)
            {
                output = "Both scores are zeros.";
            }
            else if (teamOneScore == teamTwoScore)
            {
                output = "Ties are unsupported.";
            }

            return output;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData() != "")
            {
                MessageBox.Show(ValidateData(),
                    "Invalid Data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            double teamOneScore = 0;
            double teamTwoScore = 0;
            MatchupModel matchup = (MatchupModel)matchupListBox.SelectedItem;
            if (matchup != null)
            {
                for (int i = 0; i < matchup.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (matchup.Entries[i].TeamCompeting != null)
                        {
                            bool scoreValid = double.TryParse(teamOneScoreTextBox.Text, out teamOneScore);
                            if (scoreValid)
                            {
                                matchup.Entries[i].Score = teamOneScore;
                            }
                            else
                            {
                                MessageBox.Show("Incorrect score input for 1st team",
                                    "Inccorect input",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                                return;
                            }
                        }
                    }
                    if (i == 1)
                    {
                        if (matchup.Entries[i].TeamCompeting != null)
                        {
                            bool scoreValid = double.TryParse(teamTwoScoreTextBox.Text, out teamTwoScore);
                            if (scoreValid)
                            {
                                matchup.Entries[i].Score = teamTwoScore;
                            }
                            else
                            {
                                MessageBox.Show("Incorrect score input for 2nd team",
                                    "Inccorect input",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                                return;
                            }
                        }
                    }
                }
                /*
                if (teamOneScore > teamTwoScore)
                {
                    matchup.Winner = matchup.Entries[0].TeamCompeting;
                }
                else if (teamTwoScore > teamOneScore)
                {
                    matchup.Winner = matchup.Entries[1].TeamCompeting;
                }
                else
                {
                    MessageBox.Show("Ties are unsupported",
                                    "Unsupported feature",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                    return;
                }

                foreach (List<MatchupModel> round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == matchup.Id)
                                {
                                    me.TeamCompeting = matchup.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
                */
                TournamentLogic.UpdateTournamentResults(tournament);
                LoadMatchups();
                GlobalConfig.Connection.UpdateMatchup(matchup);
            }
        }
    }
}
