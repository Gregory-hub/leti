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
	private LinkedList children = new LinkedList();
	private bool lost_child = false;
	private FibNode? parent;
	private FibNode? next;
	private FibNode? previous;
	private LinkedList? list;

	public LinkedList Children {
		get { return children; }
	}

	public LinkedList? List {
		get { return list; }
		set { list = value; }
	}

	public bool LostChild {
		get { return lost_child; }
		set { lost_child = value; }
	}

	public int Degree {
		get { return Children.Count; }
	}

	public FibNode? Parent {
		get { return parent; }
		set { parent = value; }
	}

	public FibNode? Next {
		get { return next; }
		set { next = value; }
	}

	public FibNode? Previous {
		get { return previous; }
		set { previous = value; }
	}

	public void AddChild(FibNode child) {
		child.Parent = this;
		Children.AddLast(child);
	}

	public FibNode(int id, int value) : base(id, value) {}
}
