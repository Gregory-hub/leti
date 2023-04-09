namespace lab3;


class BinTreeNode
{
	public BinTreeNode? Parent;
	public BinTreeNode? Left;
	public BinTreeNode? Right;

	public string Value;
	public int Frequency;

	public BinTreeNode(string value, int frequency)
	{
		Value = value;
		Frequency = frequency;
	}
}


class BinTree
{
	public BinTreeNode? Root;
}


class Algorithm
{
	public string EncodeRLE(string text)
	{
		if (text.Length == 0) return text;

		string text_encoded = "";
		char symbol = text[0];
		int i = 1;
		int count = 1;
		while (i < text.Length)
		{
			if (text[i] == symbol) count++;
			else 
			{
				text_encoded = text_encoded + count + symbol;
				symbol = text[i];
				count = 1;
			}
			i++;
		}
		text_encoded = text_encoded + count + symbol;
		return text_encoded;
	}

	public string DecodeRLE(string text)
	{
		if (text.Length == 0) return text;

		string text_decoded = "";
		string number = "";
		int i = 0;
		while (i < text.Length) 
		{
			// read number
			while (Char.IsDigit(text[i]))
			{
				number = number + text[i];
				i++;
			}
			// replace with symbol
			for (int j = 0; j < Convert.ToInt32(number); j++) text_decoded = text_decoded + text[i];
			number = "";
			i++;
		}

		return text_decoded;
	}

	public string EncodeHaffman(string text, out Dictionary<char, string> codes)
	{
		codes = new Dictionary<char, string>();
		if (text.Length == 0) return "";

		Dictionary<string, int> frequences_dict = new Dictionary<string, int>();
		for (int i = 0; i < text.Length; i++)
		{
			if (frequences_dict.ContainsKey(text[i].ToString())) frequences_dict[text[i].ToString()]++;
			else frequences_dict[text[i].ToString()] = 1;
		}

		PriorityQueue<BinTreeNode, int> queue = new PriorityQueue<BinTreeNode, int>();
		foreach(var pair in frequences_dict) queue.Enqueue(new BinTreeNode(pair.Key, pair.Value), pair.Value);

		BinTree tree = new BinTree();
		while (queue.Count > 1)
		{
			queue.TryDequeue(out BinTreeNode? first, out int priority_first);
			queue.TryDequeue(out BinTreeNode? second, out int priority_second);

			BinTreeNode new_node = new BinTreeNode(first.Value + second.Value, priority_first + priority_second);
			new_node.Left = first;
			new_node.Right = second;
			first.Parent = new_node;
			second.Parent = new_node;

			queue.Enqueue(new_node, priority_first + priority_second);
		}
		tree.Root = queue.Dequeue();

		codes = new Dictionary<char, string>();
		GetHaffmanCodes(tree.Root, ref codes);

		string text_encoded = "";
		foreach(char sym in text) text_encoded = text_encoded + codes[sym];

		return text_encoded;
	}

	private void GetHaffmanCodes(BinTreeNode? root, ref Dictionary<char, string> codes, string code = "")
	{
		if (root is null) return;
	
		if (root.Value.Length == 1)
		{
			if (code == "") code = "0";
			codes[root.Value[0]] = code;
			return;
		}

		GetHaffmanCodes(root.Left, ref codes, code + "0");
		GetHaffmanCodes(root.Right, ref codes, code + "1");
	}
}
