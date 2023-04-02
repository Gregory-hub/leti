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
        int source = 3;

        Dijkstra algorithm = new Dijkstra();
        algorithm.SetMatrix(graph);
        double[] result = algorithm.Run(source, HeapType.Binary);
        Console.Write("Binary heap: ");
        for (int i = 0; i < result.Length; i++) {
            Console.Write(Convert.ToString(result[i]) + ' ');
        }
        Console.WriteLine();

        result = algorithm.Run(source, HeapType.Fibonacci);
        Console.Write("Fibonacci heap: ");
        for (int i = 0; i < result.Length; i++) {
            Console.Write(Convert.ToString(result[i]) + ' ');
        }
        Console.WriteLine();

        // PlotDataCreator dataCreator = new PlotDataCreator("data.txt");
        // Console.WriteLine("Running...");
        // dataCreator.CreatePlotData(15, 800);
        // Console.WriteLine("Success!");

        Console.ReadLine();
    }
}
