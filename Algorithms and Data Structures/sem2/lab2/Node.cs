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
	private LinkedList<LinkedListNode<FibNode>>? children;

	public LinkedList<LinkedListNode<FibNode>>? Children {
		get { return children; }
		set { children = value; }
	}

	public FibNode(int id, int value) : base(id, value) {}
}
