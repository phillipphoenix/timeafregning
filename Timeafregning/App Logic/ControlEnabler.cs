using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

namespace Timeafregning.App_Logic
{
    class ControlEnabler
    {

        private List<Control> controls = new List<Control>();

        public void addControl(params Control[] cList)
        {
            foreach (Control control in cList)
            {
                controls.Add(control);
            }
        }

        public void clear()
        {
            controls.Clear();
        }

        public void enable()
        {
            foreach (Control c in controls)
            {
                c.IsEnabled = true;
            }
        }

        public void disable()
        {
            foreach (Control c in controls)
            {
                c.IsEnabled = false;
            }
        }

    }
}
