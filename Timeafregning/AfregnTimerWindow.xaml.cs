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
using System.Windows.Shapes;

using System.Diagnostics;
using System.IO;

using Timeafregning.App_Logic;

namespace Timeafregning
{
    /// <summary>
    /// Interaction logic for AfregnTimerWindow.xaml
    /// </summary>
    public partial class AfregnTimerWindow : Window
    {

        private ControlEnabler enabler = new ControlEnabler();
        private ObservableCollection<CustomerHours> customerHours = new ObservableCollection<CustomerHours>();
        private bool fileLoaded = false;
        private String week, year;

        public AfregnTimerWindow()
        {
            InitializeComponent();

            // Set available weeks and years in the selection boxes.
            weekComboBox.ItemsSource = WeekYear.getWeeks();
            yearComboBox.ItemsSource = WeekYear.getYears();

            // Add all controls, who need to be enabled after selecting week and year to the enabler.
            enabler.addControl(tilfojKundeButton, afregnTimerButton, timeafregningstabelDataGrid, pengePrTimeBox, returtimerBox);
            enabler.disable();
        }

        private void tilfojKundeButton_Click(object sender, RoutedEventArgs e)
        {
            customerHours.Add(new CustomerHours());
        }

        private void afregnTimerButton_Click(object sender, RoutedEventArgs e)
        {
            countAll();
        }

        private void afregnTimerButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            String weekBoxText = weekComboBox.Text, yearBoxText = yearComboBox.Text;
            if (!weekBoxText.Equals("") && !yearBoxText.Equals(""))
            {
                // If file is loaded, ask to save the file.
                if (fileLoaded)
                {
                    MessageBoxResult result = MessageBox.Show("Vil du gerne gemme den fil du har arbejdet med, inden du henter den nye?", "Gem fil før ny hentes?", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        Debug.WriteLine("Saving file: " + week + year + ".tas");
                        // If the user want to save the current file, save it.
                        FileSaver.saveFile(week, year, pengePrTimeBox.Text, returtimerBox.Text, customerHours);
                    }
                }

                // Unload customerHours list.
                customerHours = null;
                // Load file ( or create if it doesn't exist ).
                FileSaver.loadFile(weekBoxText, yearBoxText);
                // Update fields in the window.
                customerHours = FileSaver.getCustomerHours();
                timeafregningstabelDataGrid.ItemsSource = customerHours;
                pengePrTimeBox.Text = FileSaver.getMoneyHour().ToString();
                returtimerBox.Text = FileSaver.getReturnHours().ToString();
                weekShownLabel.Content = weekBoxText;
                yearShownLabel.Content = yearBoxText;
                // Save the week and year of the file currently getting edited.
                week = weekBoxText;
                year = yearBoxText;
                // Enable editing.
                enabler.enable();
                // Set the fileLoaded to true.
                fileLoaded = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FileSaver.writeCustomers(customerHours);
        }

        private void countAll()
        {
            // Count all hours.
            float hours = 0, sickHours = 0, total = 0;

            foreach (CustomerHours ch in customerHours)
            {
                hours += ch.Hours;
                sickHours += ch.SickHours;
            }

            total = hours - sickHours;

            // Set the labels displaying the hours.
            setAfregningsLabels(hours, sickHours, total);

            // Calculate the total money earned.
            float moneyHour = 0f, totalMoney = 0f;
            float hoursReturned = 0;

            moneyHour = float.Parse(pengePrTimeBox.Text);
            hoursReturned = float.Parse(returtimerBox.Text);

            totalMoney = (total - hoursReturned) * moneyHour;

            // Set the total money label.
            pengeTotalLabel.Content = totalMoney.ToString("0.00") + " kr.";


        }

        private void setAfregningsLabels(float hours, float sickHours, float total)
        {
            timetalLabel.Content = hours.ToString("0.00") + " t";
            sygetimerLabel.Content = sickHours.ToString("0.00") + " t";
            totalLabel.Content = total.ToString("0.00") + " t";
        }

    }
}
