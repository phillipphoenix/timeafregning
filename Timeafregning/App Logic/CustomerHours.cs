using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeafregning.App_Logic
{
    class CustomerHours
    {

        // Datagrid fields.
        private int hours, sickHours;
        public String Name { get; set; }
        public int Hours { get { return hours; } set { hours = (int)value; } }
        public int SickHours { get { return sickHours; } set { sickHours = (int)value; } }

    }
}
