using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;


namespace PlotDrawer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
			cartesianChart1.LegendLocation = LegendLocation.Top;
			cartesianChart1.AxisX.Add(new Axis
			{
				Title = "Text length",
				//Labels = new[] { "128", "256", "512", "1024", "2048" }
			});

			cartesianChart1.AxisY.Add(new Axis
			{
				Title = "Time",
				MinValue = 0,
				Separator = new Separator
				{
					Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Brushes.Gray.Color),
					StrokeThickness = .5,
					StrokeDashArray = new DoubleCollection { 5 },
					//Step = 10,
				}
			});

			DataInitializer initializer = new DataInitializer();
			this.Text = initializer.Title;

			foreach (DataInitializer.Plot plot in initializer.Plots)
			{
				ChartValues<ObservableValue> values = new ChartValues<ObservableValue>();
				for (int i = 0; i < plot.Data.Length; i += 50)
				{
					values.Add(new ObservableValue(plot.Data[i]));
				}
				cartesianChart1.Series.Add(new LineSeries
				{
					Title = plot.Name,
					Values = values,
					//Stroke = System.Windows.Media.Brushes.Violet,
					//Fill = System.Windows.Media.Brushes.Transparent,
				});
			}

			Bitmap bmp = new Bitmap(cartesianChart1.Width, cartesianChart1.Height);
			cartesianChart1.DrawToBitmap(bmp, cartesianChart1.RectangleToScreen(new Rectangle(0, 0, cartesianChart1.Width, cartesianChart1.Height)));
		}
	}
}

