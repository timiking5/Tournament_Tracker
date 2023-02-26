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
using System.Windows.Shapes;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerUIFixed.Interfaces;

namespace TrackerUIFixed
{
    /// <summary>
    /// Логика взаимодействия для CreatePrizeWindow.xaml
    /// </summary>
    public partial class CreatePrizeWindow : Window
    {
        IPrizeRequester callingForm;
        public CreatePrizeWindow(IPrizeRequester caller)
        {
            InitializeComponent();
            callingForm = caller;
        }

        private void createPrizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            { 
                PrizeModel model = new PrizeModel(placeNumberTextBox.Text,
                    placeNameTextBox.Text,
                    prizeAmountTextBox.Text,
                    prizePercentageTextBox.Text);

                GlobalConfig.Connection.CreatePrize(model);

                placeNameTextBox.Clear();
                placeNumberTextBox.Clear();
                prizeAmountTextBox.Text = "0";
                prizePercentageTextBox.Text = "0";

                callMessageBox("Prize created", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                callingForm.PrizeComplete(model);
            }
        }
        private bool ValidateForm()
        {
            bool output = true;
            int placeNumber = 0;
            if (!int.TryParse(placeNumberTextBox.Text, out placeNumber))
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

            if (placeNameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Empty place name input",
                    "Incorrect input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                output = false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;
            bool prizeAmountValid = decimal.TryParse(prizeAmountTextBox.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageTextBox.Text, out prizePercentage);

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

            if (prizePercentage < 0 || prizePercentage > 100)
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
