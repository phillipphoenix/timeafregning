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
using Timeafregning.App_Logic.Chart;

using System.Diagnostics;

namespace Timeafregning
{
    /// <summary>
    /// Interaction logic for ReportsWindow.xaml
    /// </summary>
    public partial class ReportsWindow : Window
    {

        private FontFamily defaultFontFamily = new FontFamily("Segoe UI Light");

        public ReportsWindow()
        {
            InitializeComponent();
        }

        private void reportsCanvas_Initialized(object sender, EventArgs e)
        {
            // Getting the Canvas.
            Canvas canvas = reportsCanvas;

            // Create boxes.
            BoxChart bc = new BoxChart("Uger", "Timer", 55);

            bc.addLegendItems(new LegendItem("Arbejdstimer", Brushes.Green), new LegendItem("Sygetimer", Brushes.Red), new LegendItem("Total", Brushes.Blue,"Arbejdstimer - Sygetimer = Total"));

            bc.addChartBoxList(15, 1, "32", new BoxValues(200, Brushes.Green), new BoxValues(120, Brushes.Red), new BoxValues(90,Brushes.Blue));
            bc.addChartBoxList(15, 1, "33", new BoxValues(240, Brushes.Green), new BoxValues(100, Brushes.Red), new BoxValues(120, Brushes.Blue));
            bc.addChartBoxList(15, 1, "34", new BoxValues(180, Brushes.Green), new BoxValues(110, Brushes.Red), new BoxValues(80, Brushes.Blue));

            bc.Width = canvas.Width;
            bc.Height = canvas.Height;

            bc.drawBoxChart(canvas);
        }
    }
}
