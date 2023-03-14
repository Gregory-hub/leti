namespace lab2;

class FibonacciHeap {
	private LinkedList<FibNode> roots = new LinkedList<FibNode>();
	private int count = 0;
	private FibNode? min;
	private int max_degree = 0;

	public LinkedList<FibNode> Roots {
		get { return roots; }
	}
	public FibNode? Min {
		get { return min; }
		set { min = value; }
	}
	public int MaxDegree {
		get { return max_degree; }
	}
	public int Count {
		get { return count; }
	}

	public FibonacciHeap(FibNode root) {
		Insert(root);
		Min = root;
	}

	private List<FibNode>? GetNodes(LinkedListNode<FibNode>? root) {
		if (root is null) return null;

		List<FibNode>? nodes = new List<FibNode>();

		nodes.Add(root.Value);
		LinkedListNode<FibNode>? child = root.Value.Children.First;
		while (child is not null) {
			nodes.AddRange(GetNodes(child) ?? new List<FibNode>());
			child = child.Next;
		}
		return nodes;
	}

	public List<FibNode>? GetAllNodes() {
		List<FibNode>? nodes = new List<FibNode>();

        LinkedListNode<FibNode>? root = Roots.First;
        while (root is not null) {
			nodes.AddRange(GetNodes(root));
            root = root.Next;
        }
		return nodes;
	}

	public void Print() {
        LinkedListNode<FibNode>? root = Roots.First;
        Console.Write("HEAP ROOTS: ");
        while (root is not null) {
            Console.Write($"({root.Value.Id}, {root.Value.Value}) ");
            root = root.Next;
        }
		List<FibNode>? nodes = GetAllNodes();
		foreach (FibNode node in nodes) {
			Console.Write($"\nnode ({node.Id}, {node.Value}): ");

			LinkedListNode<FibNode>? child = node.Children.First;
			while (child is not null) {
				Console.Write($"({child.Value.Id}, {child.Value.Value}) ");
				child = child.Next;
			}
		}
		Console.WriteLine();
	}

	public void Insert(FibNode node) {
		roots.AddLast(new LinkedListNode<FibNode>(node));
		if (Min is null || node.Value < Min.Value) {
			Min = node;
		}
		if (node.Degree > MaxDegree) {
			max_degree = node.Degree;
		}
		count++;
	}

	public LinkedListNode<FibNode> Merge(LinkedListNode<FibNode> node1, LinkedListNode<FibNode> node2) {
		if (node2.Value.Value < node1.Value.Value) {
			LinkedListNode<FibNode> tmp = node1;
			node1 = node2;
			node2 = tmp;
		}

		node1.Value.Children.AddLast(node2.Value);
		Roots.Remove(node2);
		if (node1.Value.Degree > MaxDegree) {
			max_degree = node1.Value.Degree;
		}

		return node1;
	}

	public FibNode? ExtractMin() {
		FibNode? min = Min;
		if (min is null) return null;
		LinkedListNode<FibNode>? child = min.Children.First;
		while (child is not null) {
			Roots.AddLast(child.Value);
			child = child.Next;
		}
		Roots.Remove(min);
		count--;
		if (Count == 0) return min;

		LinkedListNode<FibNode>?[] roots_by_degrees = new LinkedListNode<FibNode>[MaxDegree + Convert.ToInt32(Math.Log(Roots.Count, 2)) + 1];
		LinkedListNode<FibNode>? node = Roots.First;
		LinkedListNode<FibNode>? next;
		while (node is not null) {
			next = node.Next;

			while (roots_by_degrees[node.Value.Degree] is not null) {
				node = Merge(roots_by_degrees[node.Value.Degree], node);
				roots_by_degrees[node.Value.Degree - 1] = null;
			}
			roots_by_degrees[node.Value.Degree] = node;

			node = next;
		}

		return min;
	}
}
