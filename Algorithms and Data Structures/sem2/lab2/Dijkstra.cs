namespace lab2;


enum HeapType {	Binary, Fibonacci }


class Dijkstra {
	private int?[,] graph;

	public Dijkstra(int?[,] graph) {
		if (graph.GetLength(0) != graph.GetLength(1)) throw new InvalidDataException("Graph adjacency matrix is not square");
		this.graph = graph;
	}

	public int?[] Run(int start_index, HeapType heap_type) {
		int?[] distances;
		if (heap_type == HeapType.Binary) {
			distances = RunWithBinHeap(start_index);
		}
		else {
			distances = RunWithFibonacciHeap(start_index);
		}

		return distances;
	}

	private int?[] RunWithBinHeap(int start_index) {
		Node start = new Node(start_index, 0);
		BinHeap heap = new BinHeap(start);

		int?[] distances = new int?[graph.GetLength(0)];
		for (int i = 0; i < distances.Length; i++) {
			distances[i] = null;
		}
		distances[start_index] = 0;

		while (heap.Length > 0) {
			Node? current = heap.ExtractMin();
			for (int i = 0; i < graph.GetLength(1); i++) {
				if (i != current.Id && graph[current.Id,i] is not null) {
					if (distances[i] is null) {
						distances[i] = distances[current.Id] + graph[current.Id,i];
						heap.Insert(new Node(i, Convert.ToInt32(distances[i])));
					}
					else if (distances[i] > distances[current.Id] + graph[current.Id,i]) {
						distances[i] = distances[current.Id] + graph[current.Id,i];
						int node_i = heap.Search(i);
						heap.Elements[node_i].Value = Convert.ToInt32(distances[i]);
						heap.Sift_up(node_i);
					}
				}
			}
		}

		return distances;
	}

	private int?[] RunWithFibonacciHeap(int start_index) {
		FibNode start = new FibNode(start_index, 0);
		FibonacciHeap heap = new FibonacciHeap(start);

		int?[] distances = new int?[graph.GetLength(0)];
		for (int i = 0; i < distances.Length; i++) {
			distances[i] = null;
		}
		distances[start_index] = 0;

		while (heap.Length > 0) {
			FibNode? current = heap.ExtractMin();
			for (int i = 0; i < graph.GetLength(1); i++) {
				if (i != current.Id && graph[current.Id,i] is not null) {
					if (distances[i] is null) {
						distances[i] = distances[current.Id] + graph[current.Id,i];
						heap.Insert(new FibNode(i, Convert.ToInt32(distances[i])));
					}
					else if (distances[i] > distances[current.Id] + graph[current.Id,i]) {
						distances[i] = distances[current.Id] + graph[current.Id,i];
						FibNode? node = heap.Search(i, heap.Roots);
						heap.DecreaseKey(i, Convert.ToInt32(distances[i]));
					}
				}
			}
		}
		return distances;
	}
}
