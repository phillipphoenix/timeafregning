using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeafregning.App_Logic
{
    class WeekYear
    {

        public static String[] getWeeks()
        {
            int nbOfWeeks = 53;
            String[] weeks = new String[nbOfWeeks];

            for (int i = 0; i < nbOfWeeks; i++)
            {
                weeks[i] = (i + 1).ToString();
            }

            return weeks;
        }

        public static String[] getYears()
        {
            int nbOfYears = 100, startingYear = 2000;
            String[] years = new String[nbOfYears];

            for (int i = 0; i < nbOfYears; i++)
            {
                years[i] = (startingYear + i).ToString();
            }

            return years;
        }

    }
}
