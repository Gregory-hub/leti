namespace lab2;

class FibonacciHeap {
	private LinkedList roots = new LinkedList();
	private int length = 0;
	private FibNode? min;
	private int max_degree = 0;

	public LinkedList Roots {
		get { return roots; }
	}
	public FibNode? Min {
		get { return min; }
		set { min = value; }
	}
	public int MaxDegree {
		get { return max_degree; }
	}
	public int Length {
		get { return length; }
	}

	public FibonacciHeap(FibNode root) {
		Insert(root);
		Min = root;
	}

	private List<FibNode> GetNodes(FibNode? root) {
		List<FibNode>? nodes = new List<FibNode>();
		if (root is null) return nodes;

		nodes.Add(root);
		FibNode? child = root.Children.First;
		while (child is not null) {
			nodes.AddRange(GetNodes(child) ?? new List<FibNode>());
			child = child.Next;
		}
		return nodes;
	}

	public List<FibNode> GetAllNodes() {
		List<FibNode>? nodes = new List<FibNode>();

        FibNode? root = Roots.First;
        while (root is not null) {
			nodes.AddRange(GetNodes(root) ?? new List<FibNode>());
            root = root.Next;
        }
		return nodes;
	}

	public void Print() {
        FibNode? root = Roots.First;
        Console.Write("HEAP ROOTS: ");
        while (root is not null) {
            Console.Write($"({root.Id}, {root.Value}) ");
            root = root.Next;
        }
		List<FibNode> nodes = GetAllNodes();
		foreach (FibNode node in nodes) {
			Console.Write($"\nnode ({node.Id}, {node.Value}): ");

			FibNode? child = node.Children.First;
			while (child is not null) {
				Console.Write($"({child.Id}, {child.Value}) ");
				child = child.Next;
			}
		}
		Console.WriteLine("\n");
	}

	public void Insert(FibNode node) {
		roots.AddLast(node);
		if (Min is null || node.Value < Min.Value) {
			Min = node;
		}
		if (node.Degree > MaxDegree) max_degree = node.Degree;
		length++;
	}

	public FibNode? ExtractMin() {
		FibNode? min = Min;
		if (min is null) return null;
		FibNode? child = min.Children.First;
		FibNode? next_child;
		while (child is not null) {
			next_child = child.Next;
			min.Children.Remove(child);
			Roots.AddLast(child);
			if (child.Degree > MaxDegree) max_degree = child.Degree;
			child = next_child;
		}
		Roots.Remove(min);
		Min = null;
		length--;

		Compress();

		return min;
	}

	public FibNode Merge(FibNode node1, FibNode node2) {
		if (node2.Value < node1.Value) {
			FibNode tmp = node1;
			node1 = node2;
			node2 = tmp;
		}

		Roots.Remove(node2);
		node1.AddChild(node2);
		if (node1.Degree > MaxDegree) {
			max_degree = node1.Degree;
		}

		return node1;
	}

	public void Compress() {
		if (Roots.Count == 0) {
			Min = null;
			return;
		}
		FibNode?[] roots_by_degrees = new FibNode[MaxDegree + Convert.ToInt32(Math.Log(Roots.Count, 2)) + 1];
		FibNode? node = Roots.First;
		FibNode? next;
		while (node is not null) {
			next = node.Next;

			while (roots_by_degrees[node.Degree] is not null) {
				node = Merge(roots_by_degrees[node.Degree], node);
				roots_by_degrees[node.Degree - 1] = null;
			}
			roots_by_degrees[node.Degree] = node;

			node = next;
		}

		max_degree = 0;
		node = Roots.First;
		while (node is not null) {
			if (node.Degree > max_degree) max_degree = node.Degree;
			if (Min is null || node.Value < Min.Value) Min = node;

			node = node.Next;
		}
	}

	public FibNode? Search(int id, LinkedList roots) {
		FibNode? root = roots.First;
		while (root is not null) {
			if (root.Id == id) return root;
			FibNode? node = Search(id, root.Children);
			if (node is not null) return node;
			root = root.Next;
		}
		return null;
	}

	public void DecreaseKey(FibNode node, double value) {
		if (node is null) return;
		if (node.Value < value) throw new ArgumentException("value is bigger than node.Value");
		node.Value = value;
		FibNode? parent = node.Parent;
		if (parent is not null && value < parent.Value) {
			CutOut(node);
		}
		if (value < Min.Value) Min = node;
	}

	private void CutOut(FibNode node) {
		FibNode? parent = node.Parent;
		if (parent is not null) {
			parent.Children.Remove(node);
			Roots.AddLast(node);
			if (node.Degree > max_degree) max_degree = node.Degree;

			node.Parent = null;
			node.LostChild = false;
			if (!parent.LostChild) {
				parent.LostChild = true;
			}
			else CutOut(parent);
		}
	}
}
