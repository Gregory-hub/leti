using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				Title = "X",
			});
			cartesianChart1.AxisY.Add(new Axis
			{
				Title = "Y",
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
			foreach (double[] plot in initializer.Plots)
			{
				ChartValues<ObservableValue> values = new ChartValues<ObservableValue>();
				for (int i = 0; i < plot.Length; i++)
				{
					values.Add(new ObservableValue(plot[i]));
				}
				cartesianChart1.Series.Add(new LineSeries
				{
					//Title = "Biba",
					Values = values,
					//Stroke = System.Windows.Media.Brushes.Violet,
					//Fill = System.Windows.Media.Brushes.Transparent,
				});
			}

			//cartesianChart1.Series.Add(new LineSeries
			//{
			//	Title = "Biba",
			//	Values = new ChartValues<ObservableValue>
			//	{
			//		new ObservableValue(1),
			//		new ObservableValue(3),
			//		new ObservableValue(4),
			//		new ObservableValue(25),
			//		new ObservableValue(17),
			//	},
			//	Stroke = System.Windows.Media.Brushes.Violet,
			//	Fill = System.Windows.Media.Brushes.Transparent,
			//});

		}
	}
}

