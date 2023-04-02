namespace lab2;

class Program {
    static void Main(string[] args)
    {
        // adjacency matrix

        double inf = Double.PositiveInfinity;
        double[,] graph = {
            {0, 2, inf, 1, 4},
            {inf, 0, inf, 1, 5},
            {1, inf, 0, inf, 4},
            {inf, 1, inf, 0, 3},
            {2, inf, inf, inf, 0}
        };
        // int source = 0;

        // Dijkstra algorithm = new Dijkstra();
        // algorithm.SetMatrix(graph);
        // double[] result = algorithm.Run(source, HeapType.Binary);
        // for (int i = 0; i < result.Length; i++) {
        //     Console.Write(Convert.ToString(result[i]) + ' ');
        // }
        // Console.WriteLine();

        // result = algorithm.Run(source, HeapType.Fibonacci);
        // for (int i = 0; i < result.Length; i++) {
        //     Console.Write(Convert.ToString(result[i]) + ' ');
        // }
        // Console.WriteLine();

        PlotDataCreator dataCreator = new PlotDataCreator("data.txt");
        Console.WriteLine("Running...");
        dataCreator.CreatePlotData(20, 800);
        Console.WriteLine("Success!");

        Console.ReadLine();
    }
}
