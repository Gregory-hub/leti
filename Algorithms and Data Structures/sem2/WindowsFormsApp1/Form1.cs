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


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

			cartesianChart1.LegendLocation = LegendLocation.Top;

			cartesianChart1.Series.Add(new LineSeries
			{
				Title = null,
				PointGeometry = null,
				Values = new ChartValues<ObservablePoint>
				{
					new ObservablePoint(0, 0),
					new ObservablePoint(4, 0),
				},
				Stroke = System.Windows.Media.Brushes.Black,
				Fill = System.Windows.Media.Brushes.Transparent,
			});

			cartesianChart1.Series.Add(new LineSeries
			{
				Title = "Biba",
				Values = new ChartValues<ObservableValue>
				{
					new ObservableValue(1),
					new ObservableValue(3),
					new ObservableValue(4),
					new ObservableValue(5),
					new ObservableValue(8),
				},
				Stroke = System.Windows.Media.Brushes.Violet,
				Fill = System.Windows.Media.Brushes.Transparent,
			});

			cartesianChart1.Series.Add(new LineSeries
			{
				Values = new ChartValues<ObservableValue>
				{
					new ObservableValue(10),
					new ObservableValue(20),
					new ObservableValue(28),
					new ObservableValue(34),
					new ObservableValue(39),
				},
				Stroke = System.Windows.Media.Brushes.DarkRed,
				Fill = System.Windows.Media.Brushes.Transparent,
			});

			cartesianChart1.AxisX.Add(new Axis
			{
				Title = "X",
			});

			cartesianChart1.AxisY.Add(new Axis
			{
				Title = "Y",
				Separator = new Separator
				{
					Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Brushes.Gray.Color),
					StrokeThickness = .5,
					Step = 10,
				}
			});

        }
    }
}

