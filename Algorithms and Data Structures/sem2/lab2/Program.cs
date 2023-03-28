namespace lab2;

class Program {
    static void Main(string[] args)
    {
        // adjacency matrix
        int?[,] graph = {
            {0, 1, null, null},
            {null, 0, 3, 1},
            {1, null, 0, null},
            {null, 1, 1, 0}
        };
        int source = 0;

        Dijkstra algorithm = new Dijkstra(graph);
        int?[] result = algorithm.Run(source, HeapType.Binary);
        for (int i = 0; i < result.Length; i++) {
            if (result[i] is not null) {
                Console.Write(Convert.ToString(result[i]) + ' ');
            }
            else {
                Console.Write("null ");
            }
        }
        Console.WriteLine();

        result = algorithm.Run(source, HeapType.Fibonacci);
        for (int i = 0; i < result.Length; i++) {
            if (result[i] is not null) {
                Console.Write(Convert.ToString(result[i]) + ' ');
            }
            else {
                Console.Write("null ");
            }
        }

        Console.WriteLine();

        Console.ReadLine();
    }
}
