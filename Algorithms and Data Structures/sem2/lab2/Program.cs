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
        FibNode node1 = new FibNode(0, 1);
        FibNode node2 = new FibNode(1, 2);
        FibNode node3 = new FibNode(2, 3);
        FibNode node4 = new FibNode(3, 4);
        FibonacciHeap heap = new FibonacciHeap(node1);

        heap.Insert(node2);
        heap.Insert(node3);
        heap.Insert(node4);

        heap.Merge(node1, node2);
        heap.Merge(node4, node3);
        heap.Merge(node1, node3);

        FibNode? min = heap.ExtractMin();
        Console.WriteLine($"Min: {(min is not null ? min.Value : null)}");

        LinkedListNode<FibNode>? node = heap.Roots.First;
        Console.Write("heap: ");
        for (int i = 0; i < heap.Roots.Count; i++) {
            Console.Write($"{node.Value.Value} ");
            node = node.Next;
        }

        Console.Write("\nnode1: ");
        node = node1.Children.First;
        for (int i = 0; i < node1.Children.Count; i++) {
            Console.Write($"{node.Value.Value} ");
            node = node.Next;
        }

        Console.Write("\nnode2: ");
        node = node2.Children.First;
        for (int i = 0; i < node2.Children.Count; i++) {
            Console.Write($"{node.Value.Value} ");
            node = node.Next;
        }

        Console.Write("\nnode3: ");
        node = node3.Children.First;
        for (int i = 0; i < node3.Children.Count; i++) {
            Console.Write($"{node.Value.Value} ");
            node = node.Next;
        }

        Console.Write("\nnode4: ");
        node = node4.Children.First;
        for (int i = 0; i < node4.Children.Count; i++) {
            Console.Write($"{node.Value.Value} ");
            node = node.Next;
        }

        Console.ReadLine();
    }
}
