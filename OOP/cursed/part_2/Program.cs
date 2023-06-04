using System;
using System.Collections.Generic;


namespace part_2
{
    internal class Program
    {
        public static void Main()
        {
            // input
            int[,] graph = new int[,] {
                { 0, 8, 9, 5, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 9, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 8, 0, 8, 0, 0, 3, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0 },
                { 0, 0, 8, 0, 0, 0, 3, 0, 0, 5, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 7, 0, 0, 0 },
                { 0, 0, 8, 0, 7, 0, 0, 0, 0, 0, 0, 3, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 5, 0 },
                { 0, 0, 0, 0, 0, 0, 10, 0, 0, 0, 3, 0, 0, 7 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6 },
                { 0, 0, 0, 0, 0, 0, 10, 0, 8, 0, 0, 0, 0, 7 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 4 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
            int t = 0;
            int s = 13;

            // finding flow
            FordFulkerson flow_finder = new FordFulkerson();

            int max_flow = -1;
            try
            {
                max_flow = flow_finder.FindMaxFlow(graph, t, s);
            }
            catch (ArgumentException e)
            {
                Console.Write("Error: ");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            // output
            Console.WriteLine("Initial graph:");
            for (int u = 0; u < graph.GetLength(0); u++)
            {
                for (int v = 0; v < graph.GetLength(1); v++)
                    Console.Write("{0, -2} ", graph[u, v]);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine($"Source: {t}");
            Console.WriteLine($"Sink: {s}");
            Console.WriteLine();
            Console.WriteLine($"Maximum flow is {max_flow}");
        }
    }
}

