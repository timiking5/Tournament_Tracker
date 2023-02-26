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
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUIFixed
{
    /// <summary>
    /// Логика взаимодействия для CreatePrizeUserControl.xaml
    /// </summary>
    public partial class CreatePrizeUserControl : UserControl
    {
        public CreatePrizeUserControl()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                PrizeModel model = new PrizeModel(placeNameTextbox.Text,
                    placeNumberTextbox.Text,
                    prizeAmountTextbox.Text,
                    prizePercentageTextbox.Text);
                
                GlobalConfig.Connection.CreatePrize(model);
                placeNameTextbox.Clear();
                placeNumberTextbox.Clear();
                prizeAmountTextbox.Text = "0";
                prizePercentageTextbox.Text = "0";
                callMessageBox("Prize created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private bool ValidateForm()
        {
            bool output = true;
            int placeNumber = 0;
            if (!int.TryParse(placeNumberTextbox.Text, out placeNumber))
            {
                MessageBox.Show("Incorrect place number input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            if (placeNumber < 1)
            {
                MessageBox.Show("Incorrect place number input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            if (placeNameTextbox.Text.Length == 0)
            {
                MessageBox.Show("Empty place name input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;
            bool prizeAmountValid = decimal.TryParse(prizeAmountTextbox.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageTextbox.Text, out prizePercentage);

            if (!prizeAmountValid && !prizePercentageValid)
            {
                MessageBox.Show("You have to input prize amount or percentage",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                MessageBox.Show("Incorrect prize amount or percentage input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            if (prizePercentage < 0 || prizePercentage >100)
            {
                MessageBox.Show("Incorrect prize percentage input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            return output;
        }
        void callMessageBox(string message, string title, MessageBoxButton button, MessageBoxImage image)
        {
            MessageBox.Show(message, title, button, image);  // what is better - calling it via this function or manually call it
        }
    }
}
