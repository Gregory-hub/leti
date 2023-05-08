using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PlotDrawer
{
	public class DataInitializer
	{
		public struct Plot
		{
			public string Name;
			public double[] Data;

			public Plot(string name, double[] data)
			{
				Name = name;
				Data = data;
			}
		}

		private List<Plot> plots = new List<Plot>();
		public List<Plot> Plots
		{
			get { return plots; }
		}

		public string Title;

		public DataInitializer()
		{
			Initialize();
		}

		public void Initialize()
		{
			this.Title = Globals.title;

			string path = Directory.GetCurrentDirectory() + "\\data.txt";
			StreamReader sr = new StreamReader(path);
			string line = sr.ReadLine();
			double[] data;
			string name;
			string[] nums;

			while (line != null && line != "")
			{
				name = line.Split(':')[0];
				line = line.Split(':')[1];
				Regex pattern = new Regex(@"\s+");
				line = pattern.Replace(line, " ");
				nums = line.Trim().Split(' ');
				data = Array.ConvertAll(nums, new Converter<string, double>((string s) => Double.Parse(s, new CultureInfo("en-US"))));
				AddPlotData(name, data);

				line = sr.ReadLine();
			}

			sr.Close();
		}

		private void AddPlotData(string name, double[] data)
		{
			Plots.Add(new Plot(name, data));
		}
	}
}

