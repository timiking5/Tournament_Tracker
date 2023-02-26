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
    /// Логика взаимодействия для CreateTeamUserControl.xaml
    /// </summary>
    public partial class CreateTeamUserControl : UserControl
    {
        public List<PersonModel> availableTeamMembers { get; set; }
        public List<PersonModel> selectedteamMembers { get; set; }

        public CreateTeamUserControl()
        {
            //CreateSampleData();
            LoadListData();
            InitializeComponent();
        }
        private void LoadListData()
        {
            availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
            selectedteamMembers = new List<PersonModel>();
        }
        private void CreateSampleData()
        {
            availableTeamMembers = new List<PersonModel>();
            availableTeamMembers.Add(new PersonModel { FirstName = "Tim", LastName = "Purits" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Akex", LastName = "Bebra" });

            selectedteamMembers = new List<PersonModel>();
            selectedteamMembers.Add(new PersonModel { FirstName = "Jake", LastName = "Paul" });
            selectedteamMembers.Add(new PersonModel { FirstName = "Jon", LastName = "Jones" });
        }
        private void createMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidatePersonForm())
            {
                PersonModel p = new PersonModel();
                p.FirstName = firstNameTextbox.Text;
                p.LastName = lastNameTextbox.Text;
                p.EmailAddress = emailTextbox.Text;
                p.CellphoneNumber = cellphoneNumberTextbox.Text;

                GlobalConfig.Connection.CreatePerson(p);

                MessageBox.Show("Person added",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                firstNameTextbox.Clear();
                lastNameTextbox.Clear();
                emailTextbox.Clear();
                cellphoneNumberTextbox.Clear();
                availableTeamMembers.Add(p);
                selectteamMemberComboBox.Items.Refresh();
            }
        }
        private void RefreshContent()
        {
            teamMembersListBox.Items.Refresh();
            selectteamMemberComboBox.Items.Refresh();
        }
        private bool ValidatePersonForm()
        {
            bool output = true;
            if (firstNameTextbox.Text.Length == 0)
            {
                MessageBox.Show("Empty first name input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;

            }

            if (lastNameTextbox.Text.Length == 0)
            {
                MessageBox.Show("Empty last name input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            if (emailTextbox.Text.Length == 0)
            {
                MessageBox.Show("Empty email adress input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }
            if (cellphoneNumberTextbox.Text.Length == 0)
            {
                MessageBox.Show("Empty cellphone number input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }
            return output;
        }

        private void addMemberButton_Click(object sender, RoutedEventArgs e)
        {
            PersonModel p = (PersonModel)selectteamMemberComboBox.SelectedItem;
            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedteamMembers.Add(p);
                RefreshContent();
            }
        }

        private void removeSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            if (p != null)
            {
                selectedteamMembers.Remove(p);
                availableTeamMembers.Add(p);
                RefreshContent();
            }
        }

        private void createTeamButton_Click(object sender, RoutedEventArgs e)
        {
            TeamModel t = new TeamModel();
            t.TeamName = TeamNameTextbox.Text;
            t.TeamMembers = selectedteamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            foreach (PersonModel p in selectedteamMembers)
            {
                availableTeamMembers.Add(p);
            }
            selectedteamMembers.Clear();
            TeamNameTextbox.Clear();
            MessageBox.Show("Team Created", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshContent();
        }
    }
}
