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
    struct LegendItem
    {
        public String name;
        public Brush colour;
        public String tooltip;

        public LegendItem(String name, Brush colour) {
            this.name = name;
            this.colour = colour;
            tooltip = null;
        }

        public LegendItem(String name, Brush colour, String tooltip) {
            this.name = name;
            this.colour = colour;
            this.tooltip = tooltip;
        }
    }

    struct BoxValues
    {
        public float value;
        public Brush colour;

        public BoxValues(float value, Brush colour)
        {
            this.value = value;
            this.colour = colour;
        }
    }

    class BoxChart
    {

        private List<ChartBoxList> boxChart = new List<ChartBoxList>();

        // Box chart positioning.
        public int PosX { get; set; }
        public int PosY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int BoxListWidth { get; set; }

        // Box chart labels.
        public String XAxisLabel { get; set; }
        public String YAxisLabel { get; set; }

        private FontFamily defaultFontFamily = new FontFamily("Segoe UI Light");

        // Box chart legend.
        private List<LegendItem> legendItemList = new List<LegendItem>();


        public BoxChart(String xAxisLabel, String yAxisLabel, int boxListWidth)
        {
            // Set variables.
            XAxisLabel = xAxisLabel;
            YAxisLabel = yAxisLabel;
            BoxListWidth = boxListWidth;

            // Default variables.
            PosX = 0;
            PosY = 0;
            Width = 300;
            Height = 200;
        }

        // Adds a legend to the box chart.
        public void addLegendItems(params LegendItem[] legendItems)
        {
            foreach (LegendItem li in legendItems)
            {
                legendItemList.Add(li);
            }
        }

        // Adds a new chart box list to the chart at the indicated position.
        public void addChartBoxList(int posX, int posY, int width, int heightModifier, String name, params BoxValues[] boxValues) {

            // Creates the chart box list.
            ChartBoxList cbl = new ChartBoxList(posX, posY, width, heightModifier);

            // Set the name.
            cbl.Name = name;

            // Adds chart boxes to the chart box list.
            foreach (BoxValues bv in boxValues)
            {
                cbl.addChartBox(bv.value, bv.colour);
            }

            // Add the chart box list.
            boxChart.Add(cbl);

        }

        public void addChartBoxList(int width, int heightModifier, String name, params BoxValues[] boxValues)
        {
            addChartBoxList(0, 0, width, heightModifier, name, boxValues);
        }

        public void addChartBoxList(int width, int heightModifier, params BoxValues[] boxValues)
        {
            addChartBoxList(0, 0, width, heightModifier, "", boxValues);
        }

        private Rectangle[] getDrawableChartBoxes()
        {
            int totalBoxes = 0;
            foreach (ChartBoxList cbl in boxChart)
            {
                totalBoxes += cbl.Count;
            }

            Rectangle[] rectList = new Rectangle[totalBoxes];

            int index = 0;
            for (int i = 0; i < boxChart.Count; i++ )
            {
                ChartBoxList cbl = boxChart[i];
                Rectangle[] recRectList = cbl.getDrawableChartBoxes();

                for (int ii = 0; ii < recRectList.Length; ii++)
                {
                    rectList[index++] = recRectList[ii];
                    Canvas.SetLeft(recRectList[ii], PosX + 125 + (i*BoxListWidth) + cbl.PosX + (ii * cbl.Width));
                    Canvas.SetBottom(recRectList[ii], PosY + 50 + cbl.PosY);
                }
            }

            return rectList;
        }

        public void drawBoxChart(Canvas canvas)
        {
            // Draw all the boxes.
            Rectangle[] rectList = getDrawableChartBoxes();
            foreach (Rectangle rect in rectList)
            {
                canvas.Children.Add(rect);
            }

            // Creating the axis.
            Line xAxis = new Line();
            xAxis.X1 = PosX + 80;
            xAxis.Y1 = PosY + Height - 50;
            xAxis.X2 = PosX + Width - 100;
            xAxis.Y2 = PosY + Height - 50;
            xAxis.Stroke = Brushes.Black;
            xAxis.StrokeThickness = 1;
            canvas.Children.Add(xAxis);

            Line yAxis = new Line();
            yAxis.X1 = PosX + 100;
            yAxis.Y1 = PosY + Height - 30;
            yAxis.X2 = PosX + 100;
            yAxis.Y2 = PosY + 50;
            yAxis.Stroke = Brushes.Black;
            yAxis.StrokeThickness = 1;
            canvas.Children.Add(yAxis);

            // Creating the axis labels.
            Label labelXAxis = new Label();
            labelXAxis.Content = XAxisLabel;
            labelXAxis.FontFamily = defaultFontFamily;
            labelXAxis.FontSize = 20;
            Canvas.SetLeft(labelXAxis, PosX + Width - 170);
            Canvas.SetTop(labelXAxis, PosY + Height - 45);
            canvas.Children.Add(labelXAxis);

            Label labelYAxis = new Label();
            labelYAxis.Content = YAxisLabel;
            labelYAxis.FontFamily = defaultFontFamily;
            labelYAxis.FontSize = 20;
            Canvas.SetLeft(labelYAxis, PosX + 20);
            Canvas.SetTop(labelYAxis, PosY + 50);
            canvas.Children.Add(labelYAxis);

            // Create labels for the chart box lists.
            ChartBoxList cbl;
            Label l;
            for (int i = 0; i < boxChart.Count; i++)
            {
                cbl = boxChart[i];
                if (cbl.Name == null || cbl.Name.Equals("")) { continue; } // If there is no name, create no label!

                l = new Label();
                l.Content = cbl.Name;
                l.FontFamily = defaultFontFamily;
                l.FontSize = 16;

                l.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                System.Windows.Size size = l.DesiredSize;

                Canvas.SetLeft(l, PosX + 125 + (i * BoxListWidth) + cbl.PosX + ((cbl.Count * cbl.Width)/2) - (size.Width/2));
                Canvas.SetTop(l, PosY + Height - 50 + cbl.PosY);
                canvas.Children.Add(l);
            }

            // Create the legend.
            if (legendItemList.Count > 0)
            {

                double rectSize = 16; // The size of both width and height.
                double rectsWidth = legendItemList.Count * (rectSize + 2);
                double labelsWidth = 0;
                double labelHeight = 0;

                Label[] labels = new Label[legendItemList.Count];
                for (int i = 0; i < legendItemList.Count; i++)
                {
                    labels[i] = new Label();
                    labels[i].Content = legendItemList[i].name;
                    labels[i].FontFamily = defaultFontFamily;
                    labels[i].FontSize = 16;

                    if (legendItemList[i].tooltip != null)
                    {
                        labels[i].ToolTip = legendItemList[i].tooltip;
                    }

                    labels[i].Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                    labelsWidth += labels[i].DesiredSize.Width + 2;
                    if (labels[i].DesiredSize.Height > labelHeight)
                    {
                        labelHeight = labels[i].DesiredSize.Height;
                    }
                }

                Rectangle liRect;
                double currentPosX = 0;
                for (int i = 0; i < legendItemList.Count; i++)
                {
                    liRect = new Rectangle();
                    liRect.Width = rectSize;
                    liRect.Height = rectSize;
                    liRect.Fill = legendItemList[i].colour;
                    if (legendItemList[i].tooltip == null)
                    {
                        liRect.ToolTip = legendItemList[i].name;
                    }
                    else
                    {
                        liRect.ToolTip = legendItemList[i].tooltip;
                    }

                    // Position the rectangle and the label.
                    Canvas.SetLeft(liRect, PosX + (Width / 2) - ((rectsWidth + labelsWidth) / 2) + currentPosX);
                    Canvas.SetTop(liRect, PosY + 10);
                    canvas.Children.Add(liRect);
                    Canvas.SetLeft(labels[i], PosX + (Width / 2) - ((rectsWidth + labelsWidth) / 2) + currentPosX + rectSize + 2);
                    Canvas.SetTop(labels[i], PosY + 10 - (labelHeight / 4));
                    canvas.Children.Add(labels[i]);

                    currentPosX = rectSize + 2 + labels[i].DesiredSize.Width + 2 + currentPosX;
                }

                Rectangle legendBox = new Rectangle();
                legendBox.Stroke = Brushes.Gray;
                legendBox.Width = rectsWidth + labelsWidth + 4 + 4;
                legendBox.Height = rectSize + 4 + 4;
                Canvas.SetLeft(legendBox, PosX + (Width / 2) - ((rectsWidth + labelsWidth) / 2) - 4);
                Canvas.SetTop(legendBox, PosY + 10 - 4);
                canvas.Children.Add(legendBox);
            }
        }

    }
}
