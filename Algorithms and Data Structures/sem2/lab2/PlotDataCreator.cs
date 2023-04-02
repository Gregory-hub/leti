using System.Diagnostics;
using System.Globalization;

namespace lab2;

class PlotDataCreator
{
	private string path = "";
	public string Path {
		get { return path; }
		set { path = value; }
	}

	public PlotDataCreator(string filename) {
		Path = Directory.GetCurrentDirectory() + "\\" + filename;
	}

	private double[,] CreateAdjacencyMatix(int size, int connectivity)
	// connectivity is average percent of non-infinity numbers in matrix
	{
		if (connectivity < 0 || connectivity > 100) throw new InvalidDataException("Connectivity is not in this range: [0, 100]");

		Random random = new Random();
		double[,] matrix = new double[size,size];
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++) {
				if (i == j || random.Next(0, 101) > connectivity) matrix[i,j] = Double.PositiveInfinity;
				else matrix[i,j] = random.Next(1, 1001);
			}
		}

		return matrix;
	}

	private void WriteToFile(string title, double[] distribution, bool append = true)
	{
		StreamWriter sw = new StreamWriter(Path, append);
		sw.Write($"{title}: ");
		for (int i = 0; i < distribution.Count(); i++)
		{
			sw.Write(distribution[i].ToString(CultureInfo.CreateSpecificCulture("En-US")));
			if (i != distribution.Count() - 1) sw.Write(" ");
		}
		sw.Write("\n");
		sw.Close();
	}

	public double[] GetTimeDistribution(Dijkstra algorithm, HeapType heaptype, int connectivity, int data_size)
	{
		double[] runtimes = new double[data_size];

		Stopwatch watch = new Stopwatch();
		for (int size = 1; size <= data_size; size++)
		{
			algorithm.SetMatrix(CreateAdjacencyMatix(size, connectivity));
			watch.Start();
			algorithm.Run(0, heaptype);
			watch.Stop();
			runtimes[size - 1] = watch.ElapsedMilliseconds * 10e-6;
		}

		return runtimes;
	}

	public void CreatePlotData(int connectivity, int data_size) {
		Dijkstra dijkstra = new Dijkstra();
		double[] binHeapData = GetTimeDistribution(dijkstra, HeapType.Binary, connectivity, data_size);
		WriteToFile("Binary heap", binHeapData, false);
		double[] fibHeapData = GetTimeDistribution(dijkstra, HeapType.Fibonacci, connectivity, data_size);
		WriteToFile("Fibonacci heap", fibHeapData, true);
	}
}
