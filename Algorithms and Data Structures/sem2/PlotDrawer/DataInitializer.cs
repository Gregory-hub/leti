using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Globalization;
using System.Text.RegularExpressions;

public class DataInitializer
{
	private List<double[]> plots = new List<double[]>();
	public List<double[]> Plots {
		get { return plots; }
	}

	public DataInitializer()
	{
		Initialize();
	}

	public void Initialize() {
		string path = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\data.txt");
		StreamReader sr = new StreamReader(path);
		string line = sr.ReadLine();
		double[] data;

		while (line != null && line != "") {
		Regex pattern = new Regex(@"\s+");
			line = pattern.Replace(line, " ");
			string[] nums = line.Split(' ');
			data = Array.ConvertAll(nums, new Converter<string, double>((string s) => Double.Parse(s, new CultureInfo("en-US"))));
			AddPlotData(data);

			line = sr.ReadLine();
		}

		sr.Close();
	}

	private void AddPlotData(double[] data) {
		Plots.Add(data);
	}
}

