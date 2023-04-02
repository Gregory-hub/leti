namespace lab2;


enum HeapType {	Binary, Fibonacci }


class Dijkstra {
	private double[,]? graph;

	public void SetMatrix(double[,]? graph) {
		if (graph is not null && graph.GetLength(0) != graph.GetLength(1)) throw new InvalidDataException("Graph adjacency matrix is not square");
		if (graph is not null && graph.Length == 0) graph = null;
		this.graph = graph;
	}

	public double[] Run(int start_index, HeapType heap_type) {
		if (graph is null) throw new InvalidDataException("Graph is not initialized. Run SetMatrix first");
		double[] distances;
		if (heap_type == HeapType.Binary) {
			distances = RunWithBinHeap(start_index);
		}
		else {
			distances = RunWithFibonacciHeap(start_index);
		}

		return distances;
	}

	private double[] RunWithBinHeap(int start_index) {
		Node start = new Node(start_index, 0);
		BinHeap heap = new BinHeap(start);

		double[] distances = new double[graph.GetLength(0)];
		for (int i = 0; i < distances.Length; i++) {
			distances[i] = Double.PositiveInfinity;
		}
		distances[start_index] = 0;

		Node? current = heap.ExtractMin();
		while (current is not null) {
			for (int i = 0; i < graph.GetLength(1); i++) {
				if (distances[current.Id] + graph[current.Id,i] < distances[i]) {
					distances[i] = distances[current.Id] + graph[current.Id,i];

					int node_id = heap.Search(i);
					if (node_id == -1) heap.Insert(new Node(i, distances[i]));
					else heap.Elements[node_id].Value = distances[i];
				}
			}
			current = heap.ExtractMin();
		}

		return distances;
	}

	private double[] RunWithFibonacciHeap(int start_index) {
		FibNode start = new FibNode(start_index, 0);
		FibonacciHeap heap = new FibonacciHeap(start);

		double[] distances = new double[graph.GetLength(0)];
		for (int i = 0; i < distances.Length; i++) {
			distances[i] = Double.PositiveInfinity;
		}
		distances[start_index] = 0;

		FibNode? current = heap.ExtractMin();
		while (current is not null) {
			for (int i = 0; i < graph.GetLength(1); i++) {
				if (distances[current.Id] + graph[current.Id,i] < distances[i]) {
					distances[i] = distances[current.Id] + graph[current.Id,i];

					FibNode? node = heap.Search(i, heap.Roots);
					if (node is null) heap.Insert(new FibNode(i, distances[i]));
					else heap.DecreaseKey(node, distances[i]);
				}
			}
			// Console.WriteLine($"({current.Id}, {current.Value})");
			current = heap.ExtractMin();
		}

		return distances;
	}
}
