using System;

public class FordFulkerson
{
    private bool BFS(int[,] residual_graph, int s, int t, ref int[] path)
    {
        bool[] visited = new bool[residual_graph.GetLength(0)];
        for (int i = 0; i < residual_graph.GetLength(0); ++i)
            visited[i] = false;

        List<int> queue = new List<int>();
        queue.Add(s);
        visited[s] = true;
        path[s] = -1;

        while (queue.Count != 0)
        {
            int u = queue[0];
            queue.RemoveAt(0);

            for (int v = 0; v < residual_graph.GetLength(0); v++)
            {
                if (visited[v] == false && residual_graph[u, v] > 0)
                {
                    if (v == t)
                    {
                        path[v] = u;
                        return true;
                    }
                    queue.Add(v);
                    path[v] = u;
                    visited[v] = true;
                }
            }
        }

        // didn't reach t
        return false;
    }

    public int FindMaxFlow(int[,] graph, int s, int t)
    {
        if (graph.GetLength(0) != graph.GetLength(1)) throw new ArgumentException("Graph adjacency matrix is not square");
        int len = graph.GetLength(0);

        if (s >= len || s < 0) throw new ArgumentException("Invalid s");
        if (t >= len || t < 0) throw new ArgumentException("Invalid t");

        int u, v;
        int[,] residual_graph = new int[len, len];

        for (u = 0; u < len; u++)
            for (v = 0; v < len; v++)
                residual_graph[u, v] = graph[u, v];

        int[] path = new int[len];

        int max_flow = 0;

        // while there is path from source to sink
        while (BFS(residual_graph, s, t, ref path))
        {
            int path_flow = int.MaxValue;
            for (v = t; v != s; v = path[v])
            {
                u = path[v];
                path_flow = Math.Min(path_flow, residual_graph[u, v]);
            }

            for (v = t; v != s; v = path[v])
            {
                u = path[v];
                residual_graph[u, v] -= path_flow;
                residual_graph[v, u] += path_flow;
            }

            max_flow += path_flow;
        }

        return max_flow;
    }
}

