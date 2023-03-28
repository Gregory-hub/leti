namespace lab2;


class BinHeap {
	private List<Node> elements = new List<Node>();
	public List<Node> Elements {
		set { elements = value; }
		get { return elements; }
	}

	public int Length {
		get { return elements.Count; }
	}

	public BinHeap(Node root) {
		Insert(root);
	}

	public void Print() {
		elements.ForEach(el => Console.Write(Convert.ToString(el.Value) + ' '));
		Console.WriteLine();
	}

	public int Search(int id) {
		for (int i = 0; i < Length; i++) {
			if (Elements[i].Id == id) return i;
		}
		return -1;
	}

	public int GetParentIndex(int el_index) {
		if (el_index == 0) return -1;
		return (el_index - 1) / 2;
	}

	public int[] GetChildrenIndices(int el_index) {
		int[] indices = new int[2] {el_index * 2 + 1, el_index * 2 + 2};
		if (indices[0] >= Length) {
			indices[0] = -1;
			indices[1] = -1;
		} else if (indices[1] >= Length) {
			indices[1] = -1;
		};
		return indices;
	}

	public void Swap(int first, int second) {
		Node tmp = elements[first];
		elements[first] = elements[second];
		elements[second] = tmp;
	}

	public void Sift_up(int index) {
		while (Length > index && index > 0) {
			int parent_index = GetParentIndex(index);
			if (elements[index].Value < elements[parent_index].Value) {
				Swap(index, parent_index);
			} else {
				return;
			}
			index = parent_index;
		}
	}

	public void Sift_down(int index) {
		int[] children_indices = GetChildrenIndices(index);
		while (children_indices[0] != -1) {
			int max_child_index = children_indices[0];
			if (children_indices[1] != -1 && elements[children_indices[0]].Value < elements[children_indices[1]].Value) {
				max_child_index = children_indices[1];
			}

			if (elements[index].Value > elements[max_child_index].Value) {
				Swap(index, max_child_index);
			} else {
				return;
			}
			index = max_child_index;
			children_indices = GetChildrenIndices(index);
		}
	}

	public Node? ExtractMin() {
		if (Length == 0) return null;
		Node min = Elements[0];
		Delete(0);
		return min;
	}

	public void Insert(Node node) {
		elements.Add(node);
		Sift_up(Length - 1);
	}

	public void Delete(int index) {
		Swap(index, Length - 1);
		elements.RemoveAt(Length - 1);
		Sift_down(index);
		Sift_up(index);
	}
}
