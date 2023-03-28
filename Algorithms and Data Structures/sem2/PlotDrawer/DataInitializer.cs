using System;
using System.Collections.Generic;


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
		double[] data = new double[] { 1, 1.2, 4 };
		AddPlotData(data);
		data = new double[] { 4.5, 3.2, 1 };
		AddPlotData(data);
	}

	private void AddPlotData(double[] data) {
		Plots.Add(data);
	}
}

