namespace lab2;

class FibonacciHeap {
	private LinkedList<FibNode> roots = new LinkedList<FibNode>();
	public LinkedList<FibNode> Roots {
		get { return roots; }
	}

	private FibNode? min;
	public FibNode? Min {
		get { return min; }
		set { min = value; }
	}
	
	private int max_degree = 0;
	public int MaxDegree {
		get { return max_degree; }
	}

	private int count = 0;
	public int Count {
		get { return count; }
	}

	public FibonacciHeap(FibNode root) {
		Insert(root);
		Min = root;
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

	public void Merge(FibNode node1, FibNode node2) {
		if (node2.Value < node1.Value) {
			FibNode tmp = node1;
			node1 = node2;
			node2 = tmp;
		}

		node1.Children.AddLast(node2);
		Roots.Remove(node2);
		if (node1.Degree > MaxDegree) {
			max_degree = node1.Degree;
		}
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

		FibNode[] roots_by_degrees = new FibNode[Convert.ToInt32(Math.Log(Count, 2)) + 1];
		LinkedListNode<FibNode>? node = roots.First;

		return min;
	}
}
