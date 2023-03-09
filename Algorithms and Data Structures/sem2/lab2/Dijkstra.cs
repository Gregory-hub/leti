namespace lab2;


class Dijkstra {
	private int?[,] graph;

	public Dijkstra(int?[,] graph) {
		this.graph = graph;
	}

	public int?[] run(int start_index) {
		int?[] distances = new int?[graph.GetLength(0)];
		for (int i = 0; i < distances.Length; i++) {
			distances[i] = null;
		}
		distances[start_index] = 0;

		Node start = new Node(start_index, 0);
		BinHeap heap = new BinHeap(start);

		while (heap.Length > 0) {
			Node current = heap.Pop_min();
			for (int i = 0; i < graph.GetLength(1); i++) {
				if (i != current.Id && graph[current.Id,i] is not null) {
					if (distances[i] is null) {
						distances[i] = distances[current.Id] + graph[current.Id,i];
						heap.Insert(new Node(i, Convert.ToInt32(distances[i])));
					}
					else if (distances[i] > distances[current.Id] + graph[current.Id,i]) {
						distances[i] = distances[current.Id] + graph[current.Id,i];
						int node_i = heap.SearchById(i);
						heap.Elements[node_i].Value = Convert.ToInt32(distances[i]);
						heap.Sift_up(node_i);
					}
				}
			}
		}

		return distances;
	}
}
