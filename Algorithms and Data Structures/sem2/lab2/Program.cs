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
        FibNode node1 = new FibNode(0, 0);
        FibNode node2 = new FibNode(1, 1);
        FibNode node3 = new FibNode(2, 2);
        FibNode node4 = new FibNode(3, 3);
        FibNode node5 = new FibNode(4, 4);
        FibNode node6 = new FibNode(5, 5);
        FibNode node7 = new FibNode(6, 6);
        FibNode node8 = new FibNode(7, 7);
        // FibNode node9 = new FibNode(8, 8);

        FibonacciHeap heap = new FibonacciHeap(node2);
        heap.Insert(node1);
        // heap.Insert(node3);
        // heap.Insert(node4);
        // heap.Insert(node5);
        // heap.Insert(node6);
        // heap.Insert(node7);
        // heap.Insert(node8);
        // heap.Insert(node9);
        heap.Compress();
        heap.Print();
        heap.ExtractMin();

        // heap.DecreaseKey(7, -1);
        // heap.DecreaseKey(1, -1);
        heap.Print();
        FibNode? min = heap.ExtractMin();
        if (min is not null) Console.WriteLine($"({min.Id}, {min.Value})");
        else Console.WriteLine("Min is null");

        heap.Compress();
        heap.Print();

        min = heap.ExtractMin();
        Console.WriteLine($"Min Id: {(min is not null ? min.Id : null)}\n");
        heap.Print();

        Console.ReadLine();
    }
}
