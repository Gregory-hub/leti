namespace lab2;


class Program {
    static void Main(string[] args)
    {
        // adjacency matrix
        int?[,] graph = {
            {0, 1, null, 1},
            {null, 0, 3, 1},
            {1, null, 0, null},
            {null, 1, 1, 0}
        };
        // int source = 0;

        // Dijkstra algorithm = new Dijkstra(graph);
        // int?[] result = algorithm.run(source);
        // for (int i = 0; i < result.Length; i++) {
        //     if (result[i] is not null) {
        //         Console.Write(Convert.ToString(result[i]) + ' ');
        //     }
        //     else {
        //         Console.Write("null ");
        //     }
        // }
        // Console.WriteLine();
        // FibonacciHeap heap = new FibonacciHeap(new Node(0, 14));
        FibNode node = new FibNode(2, 15);
        Console.WriteLine(node.Id);
        Console.WriteLine(node.Value);
        Console.WriteLine(node.Children);

        Console.ReadLine();
    }
}
