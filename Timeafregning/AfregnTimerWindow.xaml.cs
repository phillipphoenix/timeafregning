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

        private ObservableCollection<CustomerHours> customerHours = new ObservableCollection<CustomerHours>();

        public AfregnTimerWindow()
        {
            InitializeComponent();

            // Read in all customers.
            readCustomers(customerHours);

            // Set the customers data as the datagrid's data.
            timeafregningstabelDataGrid.ItemsSource = customerHours;
        }

        private void tilfojKundeButton_Click(object sender, RoutedEventArgs e)
        {
            customerHours.Add(new CustomerHours());
        }

        private void afregnTimerButton_Click(object sender, RoutedEventArgs e)
        {
            countAll();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            writeCustomers(customerHours);
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

        private void readCustomers(ObservableCollection<CustomerHours> customerHours)
        {

            String fullPathDir = Environment.CurrentDirectory + @"\data\";
            String fullPathFile = fullPathDir + @"customers.txt";

            if (File.Exists(fullPathFile))
            {
                String[] customerNames = File.ReadAllLines(fullPathFile);

                foreach (String customerName in customerNames)
                {
                    customerHours.Add(new CustomerHours() { Name = customerName });
                }
            }
        }

        private void writeCustomers(ObservableCollection<CustomerHours> customerHours)
        {

            String[] customerNames = new String[customerHours.Count];

            for (int i = 0; i < customerHours.Count; i++ )
            {
                customerNames[i] = customerHours[i].Name;
            }

            String fullPathDir = Environment.CurrentDirectory + @"\data\";
            String fullPathFile = fullPathDir + @"customers.txt";

            Debug.WriteLine(fullPathDir);

            if (!Directory.Exists(fullPathDir))
            {
                Directory.CreateDirectory(fullPathDir);
            }

            File.WriteAllLines(fullPathFile, customerNames);
        }

    }
}
