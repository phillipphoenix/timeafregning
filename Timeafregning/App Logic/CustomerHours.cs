using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;

namespace Timeafregning.App_Logic
{
    class CustomerHours
    {

        // Datagrid fields.
        private float hours, sickHours;
        public String Name { get; set; }
        public float Hours { get { return hours; } set { hours = float.Parse(value.ToString()); } }
        public float SickHours { get { return sickHours; } set { sickHours = float.Parse(value.ToString()); } }

    }
}
