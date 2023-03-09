namespace lab2;

class FibonacciHeap {
	private LinkedList<FibNode> roots = new LinkedList<FibNode>();
	
	public FibonacciHeap(FibNode root) {
		roots.AddLast(new LinkedListNode<FibNode>(root));
		Console.WriteLine(roots.First.Value.Id);
	}
}
