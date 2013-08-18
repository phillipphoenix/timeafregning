using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Timeafregning.App_Logic.Chart
{

    class ChartBox
    {
        // The value of the box, for instance the number of hours.
        public float Value { get; set; }
        // The displayed height is equal to the value times the height modifier.
        public float HeightModifier { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public Brush Colour { get; set; }

        public ChartBox()
        {
            Value = 1;
            HeightModifier = 1;
            Width = 1;
            Colour = Brushes.Green;
        }

        public Rectangle getDrawableBox()
        {
            Rectangle rect = new Rectangle();
            rect.Width = Width;
            rect.Height = Value * HeightModifier;
            rect.Fill = Colour;
            rect.ToolTip = Value;
            Canvas.SetLeft(rect, PosX);
            Canvas.SetBottom(rect, PosY);

            return rect;
        }

    }
}
