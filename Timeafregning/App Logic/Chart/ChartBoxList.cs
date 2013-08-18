using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using System.Diagnostics;

namespace Timeafregning.App_Logic.Chart
{
    class ChartBoxList
    {

        // VARIABLES AND PROPERTIES!

        private List<ChartBox> chartBoxList = new List<ChartBox>();

        private float heightModifier;
        private int width;

        public int PosX { get; set; }
        public int PosY { get; set; }
        public float HeightModifier {
            get { return heightModifier; }
            set { heightModifier = value; updateChartBoxes(); } 
        }
        public int Width {
            get { return width; }
            set {width = value; updateChartBoxes(); }
        }

        public int Count
        {
            get { return chartBoxList.Count; }
        }

        public String Name { get; set; }

        // MAIN BODY!

        public ChartBoxList(int posX, int posY, int width, int heightModifier)
        {
            PosX = posX;
            PosY = posY;
            Width = width;
            HeightModifier = heightModifier;
        }

        public void addChartBox(float value, Brush colour)
        {
            ChartBox cb = new ChartBox();
            cb.Value = value;
            cb.Colour = colour;

            chartBoxList.Add(cb);
        }

        public void updateChartBoxes()
        {
            foreach (ChartBox cb in chartBoxList) {
                cb.HeightModifier = HeightModifier;
                cb.Width = Width;
            }
        }

        public Rectangle[] getDrawableChartBoxes()
        {
            Rectangle[] rectList = new Rectangle[chartBoxList.Count];

            for (int i = 0; i < chartBoxList.Count; i++)
            {
                ChartBox cb = chartBoxList[i];

                Rectangle rect = cb.getDrawableBox();
                if (Width > 0)
                {
                    rect.Width = Width;
                }
                if (HeightModifier > 0)
                {
                    rect.Height = cb.Value * HeightModifier;
                }

                Canvas.SetLeft(rect,PosX + (i*Width));
                Canvas.SetBottom(rect, PosY);

                rectList[i] = rect;
            }

            return rectList;
        }

    }
}
