namespace lab2;


class Node {
	private int id;
	private int value;

	public int Id {
		set { id = value; }
		get { return id; }
	}

	public int Value {
		set { this.value = value; }
		get { return value; }
	}

	public Node(int id, int value) {
		this.id = id;
		this.Value = value;
	}
}

class FibNode : Node {
	private LinkedList<FibNode> children = new LinkedList<FibNode>();
	private bool lost_child = false;
	private LinkedListNode<FibNode>? parent;

	public LinkedList<FibNode> Children {
		get { return children; }
	}

	public bool LostChild {
		get { return lost_child; }
		set { lost_child = value; }
	}

	public int Degree {
		get { return Children.Count; }
	}

	public LinkedListNode<FibNode>? Parent {
		get { return parent; }
		set { parent = value; }
	}

	public void AddChild(FibNode child, LinkedListNode<FibNode>? node) {
		if (node is not null && !Object.ReferenceEquals(node.Value, this)) {
			Console.WriteLine($"{this.Id}, {node.Value.Id}, {Object.ReferenceEquals(node.Value, this)}");
			throw new ArgumentException("node must contain this exact FibNode instance");
		}
		child.Parent = node;
		Children.AddLast(child);
	}

	public FibNode(int id, int value) : base(id, value) {}
}
