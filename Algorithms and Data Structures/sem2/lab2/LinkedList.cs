namespace lab2;

class LinkedList {
	private FibNode? first;
	private FibNode? last;
	private int count = 0;

	public FibNode? First {
		get { return first; }
		set { first = value; }
	}

	public FibNode? Last {
		get { return last; }
		set { last = value; }
	}

	public int Count {
		get { return count; }
		set { count = value; }
	}

	public void AddLast(FibNode node) {
		if (Object.ReferenceEquals(node.List, this)) throw new InvalidOperationException("Node is already in this list");
		if (First is null) { 
			First = node;
			Last = node;
		}
		else {
			Last.Next = node;
			node.Previous = Last;
			Last = node;
		}
		node.List = this;
		Count++;
	}

	public void Remove(FibNode node) {
		if (!Object.ReferenceEquals(node.List, this)) throw new InvalidOperationException("Node does not belong this list");

		if (node.Previous is not null && node.Next is not null) {
			node.Next.Previous = node.Previous;
			node.Previous.Next = node.Next;
		}
		else if (node.Next is not null) {
			First = node.Next;
			node.Next.Previous = null;
		}
		else if (node.Previous is not null) {
			Last = node.Previous;
			node.Previous.Next = null;
		}
		else {
			First = null;
			Last = null;
		}
		node.Next = null;
		node.Previous = null;
		node.List = null;
		Count--;
	}
}
