using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace Timeafregning.App_Logic
{
    class FileSaver
    {

        private static float moneyHour, returnHours;
        private static ObservableCollection<CustomerHours> localCustomerHours;

        /*
         * Saves a file with all the customerHours information to a file with the name: #week#year.tas
         * The format of the file is:
         * 
         * #kr.pr.time
         * #returtimer
         * #kunde,#timetal,#sygetimer
         * ...
         * ...
         * #kunde,#timetal,#sygetimer
         * 
         * */
        public static void saveFile(String week, String year, String moneyHour, String returnHours, ObservableCollection<CustomerHours> customerHours)
        {

            String fullPathDir = Environment.CurrentDirectory + @"\data\saveFiles\";
            String fullPathFile = fullPathDir + week + year + ".tas";

            Debug.WriteLine(fullPathDir);

            // Check if directory exists and create it if not.
            if (!Directory.Exists(fullPathDir))
            {
                Directory.CreateDirectory(fullPathDir);
            }

            // If a file already exists, delete it, so we can save a new one.
            if (File.Exists(fullPathFile)) {
                File.Delete(fullPathFile);
            }

            // Create a stream writer to write to the file.
            using (StreamWriter writer = new StreamWriter(fullPathFile)) {
                // Write kr.pr.time and returtimer to the file.
                writer.WriteLine(moneyHour);
                writer.WriteLine(returnHours);

                // Write all customerHours data to the file.
                foreach (CustomerHours ch in customerHours)
                {
                    String data = ch.Name + "," + ch.Hours + "," + ch.SickHours;
                    writer.WriteLine(data);
                }

                // Flush the buffer.
                writer.Flush();
            }

        }

        public static void loadFile(String week, String year)
        {
            String fullPathDir = Environment.CurrentDirectory + @"\data\saveFiles\";
            String fullPathFile = fullPathDir + week + year + ".tas";

            Debug.WriteLine(fullPathDir);

            // Check if directory exists, before venturing further.
            if (Directory.Exists(fullPathDir))
            {
                // Check if file exists, before venturing further.
                if (File.Exists(fullPathFile))
                {
                    float mh = 0, rh = 0;
                    ObservableCollection<CustomerHours> chList = new ObservableCollection<CustomerHours>();

                    // Create a reader to read the file.
                    using (StreamReader reader = new StreamReader(fullPathFile))
                    {

                        String line;

                        // Read kr.pr.time and returtimer.
                        if ((line = reader.ReadLine()) != null)
                        {
                            mh = float.Parse(line);
                        }

                        if ((line = reader.ReadLine()) != null)
                        {
                            rh = float.Parse(line);
                        }

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!line.Equals(""))
                            {
                                String[] cInfo = line.Split(',');
                                chList.Add(new CustomerHours() { Name = cInfo[0], Hours = float.Parse(cInfo[1]), SickHours = float.Parse(cInfo[2]) });
                            }
                        }

                        // Set the static variables' information.
                        moneyHour = mh;
                        returnHours = rh;
                        localCustomerHours = chList;
                    }
                }
                else // If file doesn't exist, create it!
                {
                    // Create default file.
                    createDefaultFile(fullPathFile);
                }
            }
            else // If directory doesn't exist, create it!
            {
                // Create Directory.
                Directory.CreateDirectory(fullPathDir);
                
                // Create default file.
                createDefaultFile(fullPathFile);
            }
        }

        private static void createDefaultFile(String fullPathFile)
        {
            // Create a stream writer to write to the file.
            using (StreamWriter writer = new StreamWriter(fullPathFile))
            {

                // Get default customerHours list.
                ObservableCollection<CustomerHours> customerHours = new ObservableCollection<CustomerHours>();
                readCustomers(customerHours);

                // Write kr.pr.time and returtimer to the file.
                writer.WriteLine("0");
                writer.WriteLine("0");

                // Write the default customerHours data to the file.
                foreach (CustomerHours ch in customerHours)
                {
                    String data = ch.Name + "," + 0 + "," + 0;
                    writer.WriteLine(data);
                }

                // Flush the buffer.
                writer.Flush();

                // Set the static variables' information.
                moneyHour = 0;
                returnHours = 0;
                localCustomerHours = customerHours;
            }
        }

        // Getters for the loaded variables.
        public static float getMoneyHour()
        {
            return moneyHour;
        }

        public static float getReturnHours()
        {
            return returnHours;
        }

        public static ObservableCollection<CustomerHours> getCustomerHours()
        {
            return localCustomerHours;
        }

        public static void readCustomers(ObservableCollection<CustomerHours> customerHours)
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

        public static void writeCustomers(ObservableCollection<CustomerHours> customerHours)
        {

            String[] customerNames = new String[customerHours.Count];

            for (int i = 0; i < customerHours.Count; i++)
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
