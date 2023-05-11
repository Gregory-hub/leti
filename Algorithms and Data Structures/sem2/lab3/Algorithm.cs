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


public class Algorithm
{
	private char[] letters_popular = new char[(int)Math.Pow(2, 16)];

	public Algorithm()
	{
		InitializePopularLettersArray();
	}

	struct LZ78DictEntry
	{
		public int Index;
		public char Symbol;

		public LZ78DictEntry(int index, char symbol)
		{
			Index = index;
			Symbol = symbol;
		}
	}

	public string EncodeRLE(string text)
	{
		if (text.Length == 0) return text;

		string text_encoded = "";
		char symbol = text[0];
		string bin_num;
		string bin_sym;
		int count = 1;
		for (int i = 1; i <= text.Length; i++)
		{
			if (i < text.Length && text[i] == symbol) count++;
			else 
			{
				bin_num = Convert.ToString(count, 2);
				for (int k = bin_num.Length; k < 16; k++) bin_num = "0" + bin_num;

				bin_sym = Convert.ToString((int)symbol, 2);
				for (int k = bin_sym.Length; k < 16; k++) bin_sym = "0" + bin_sym;

				text_encoded += bin_num;
				text_encoded += bin_sym;

				count = 1;
				if (i < text.Length) symbol = text[i];
			}
		}

		return text_encoded;
	}

	public string DecodeRLE(string text)
	{
		if (text.Length == 0) return text;

		string text_decoded = "";
		int count;
		char sym;
		int i = 0;
		while (i < text.Length)
		{
			count = Convert.ToInt32(text.Substring(i, 16), 2);
			i += 16;

			sym = (char)Convert.ToInt32(text.Substring(i, 16), 2);
			i += 16;

			for (int j = 0; j < count; j++) text_decoded = text_decoded + sym;
		}

		return text_decoded;
	}

	public string EncodeHuffman(string text)
	{
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

		string text_encoded = "";

		Dictionary<char, string> codes = new Dictionary<char, string>();
		GetHuffmanCodes(tree.Root, ref codes);

		string bin_sym;
		string len;
		foreach(char sym in codes.Keys)
		{
			bin_sym = Convert.ToString(sym, 2);
			for (int i = bin_sym.Length; i < 16; i++) bin_sym = "0" + bin_sym;

			len = Convert.ToString(codes[sym].Length, 2);
			for (int i = len.Length; i < 16; i++) len = "0" + len;

			text_encoded += bin_sym;
			text_encoded += len;
			text_encoded += codes[sym];
		}
		text_encoded += "0000000000000000";

		foreach(char sym in text) text_encoded = text_encoded + codes[sym];

		return text_encoded;
	}

	private void GetHuffmanCodes(BinTreeNode? root, ref Dictionary<char, string> codes, string code = "")
	{
		if (root is null) return;
	
		if (root.Value.Length == 1)
		{
			if (code == "") code = "0";
			codes[root.Value[0]] = code;
			return;
		}

		GetHuffmanCodes(root.Left, ref codes, code + "0");
		GetHuffmanCodes(root.Right, ref codes, code + "1");
	}

	public string DecodeHuffman(string text)
	{
		Dictionary<string, char> codes_reversed = new Dictionary<string, char>();

		int k = 0;
		string bin_sym;
		char sym;
		int len;
		string bin_code;
		while (k + 15 < text.Length) 
		{
			bin_sym = text.Substring(k, 16);
			sym = (char)Convert.ToInt32(bin_sym, 2);

			k += 16;
			if (sym == 0) break;
			sym = (char)Convert.ToInt32(bin_sym, 2);

			len = Convert.ToInt32(text.Substring(k, 16), 2);
			k += 16;

			bin_code = text.Substring(k, len);
			k += len;

			codes_reversed[bin_code] = sym;
		}
		text = text.Substring(k, text.Length - k);

		string text_decoded = "";
		int i = 0;
		string current_code = "";
		while (i < text.Length)
		{
			current_code = current_code + text[i];
			if (codes_reversed.ContainsKey(current_code))
			{
				text_decoded = text_decoded + codes_reversed[current_code];
				current_code = "";
			}
			i++;
		}
		return text_decoded;
	}

	public string EncodeLZ78(string text)
	{
		string text_encoded = "";
		Dictionary<string, LZ78DictEntry> dict = new Dictionary<string, LZ78DictEntry>();
		string buffer = "";
		int index = 0;
		string bin_index;
		string bin_sym;

		for (int i = 0; i < text.Length; i++)
		{
			if (!dict.ContainsKey(buffer + text[i]))
			{
				index++;
				dict[buffer + text[i]] = new LZ78DictEntry(index, text[i]);

				bin_sym = Convert.ToString((int)text[i], 2);
				for (int k = bin_sym.Length; k < 16; k++) bin_sym = "0" + bin_sym;

				if (dict.ContainsKey(buffer)){
					bin_index = Convert.ToString(dict[buffer].Index, 2);
					for (int k = bin_index.Length; k < 16; k++) bin_index = "0" + bin_index;

					text_encoded += bin_index;
					text_encoded += bin_sym;
				}
				else text_encoded = text_encoded + "0000000000000000" + bin_sym;
				buffer = "";
			}
			else
			{
				buffer = buffer + text[i];
			}
		}

		if (buffer.Length != 0)
		{
			char symbol = buffer[buffer.Length - 1];
			buffer = buffer.Substring(0, buffer.Length - 1);

			bin_sym = Convert.ToString((int)symbol, 2);
			for (int k = bin_sym.Length; k < 16; k++) bin_sym = "0" + bin_sym;
			if (dict.ContainsKey(buffer))
			{
				bin_index = Convert.ToString(dict[buffer].Index, 2);
				for (int k = bin_index.Length; k < 16; k++) bin_index = "0" + bin_index;

				text_encoded += bin_index;
				text_encoded += bin_sym;
			}
			else text_encoded = text_encoded + "0000000000000000" + bin_sym;
		}
		return text_encoded;
	}

	public string DecodeLZ78(string text)
	{
		if (text.Length == 0) return text;

		string text_decoded = "";
		Dictionary<int, string> dict = new Dictionary<int, string>();
		int index = 0;

		int number;
		char sym;
		int i = 0;
		while (i < text.Length)
		{
			number = Convert.ToInt32(text.Substring(i, 16), 2);
			i += 16;

			sym = (char)Convert.ToInt32(text.Substring(i, 16), 2);
			i += 16;

			index++;
			if (number == 0) {
				text_decoded = text_decoded + sym;
				dict[index] = sym.ToString();
			}
			else
			{
				text_decoded = text_decoded + dict[number] + sym;
				dict[index] = dict[number] + sym;
			}
		}

		return text_decoded;
	}

	private void SortBWTStrings(string text, ref int[] indices) 
	{
		// quicksort for text permutations
		if (indices.Length == 0) return;
		int pivot_i = indices[indices.Length - 1];
		string pivot_str = text.Substring(pivot_i) + text.Substring(0, pivot_i);

		int wall = 0;
		int tmp;
		string str;

		for (int i = 0; i < indices.Length - 1; i++)
		{
			str = text.Substring(indices[i]) + text.Substring(0, indices[i]);
			if (String.Compare(str, pivot_str, comparisonType: StringComparison.Ordinal) < 0) {
				tmp = indices[i];
				indices[i] = indices[wall];
				indices[wall] = tmp;
				wall++;
			}
		}

		tmp = indices[indices.Length - 1];
		indices[indices.Length - 1] = indices[wall];
		indices[wall] = tmp;

		int[] left = new int[wall];
		for (int i = 0; i < left.Length; i++) left[i] = indices[i];

		int[] right = new int[indices.Length - wall - 1];
		for (int i = 0; i < right.Length; i++) right[i] = indices[wall + 1 + i];

		SortBWTStrings(text, ref left);
		SortBWTStrings(text, ref right);

		for (int i = 0; i < left.Length; i++) indices[i] = left[i];
		for (int i = 0; i < right.Length; i++) indices[wall + 1 + i] = right[i];
	}

	public string BWTTransform(string text)
	{
		if (text.Length == 0) return "";

		string text_transformed = "";
		int[] indices = new int[text.Length];
		for (int i = 0; i < text.Length; i++) indices[i] = i;

		SortBWTStrings(text, ref indices);

		int first_i = -1;
		int index;
		for (int i = 0; i < indices.Length; i++) 
		{
			if (indices[i] > 0) index = indices[i] - 1;
			else index = indices.Length - 1;

			text_transformed += text[index];

			if (indices[i] == 0) first_i = i;
		}

		if (first_i == -1) throw new Exception("BWTTransform did not as intended");
		return (char)first_i + text_transformed;
	}

	public string BWTDetransform(string text)
	{
		if (text.Length == 0) return "";

		string text_detransformed = "";

		int index = (int)text[0];
		text = text.Substring(1);

		List<KeyValuePair<int, char>> last_col = new List<KeyValuePair<int, char>>();
		for (int i = 0; i < text.Length; i++) last_col.Add(new KeyValuePair<int, char>(i, text[i]));

		List<KeyValuePair<int, char>> first_col = new List<KeyValuePair<int, char>>();
		for (int i = 0; i < text.Length; i++) first_col.Add(last_col[i]);

		first_col = first_col.OrderBy(p => p.Value).ToList();

		KeyValuePair<int, char> symbol = first_col[index];
		text_detransformed += symbol.Value;
		for (int i = 0; i < first_col.Count - 1; i++)
		{
			symbol = first_col[last_col.FindIndex(p => p.Key == symbol.Key)];
			text_detransformed += symbol.Value;
		}

		return text_detransformed;
	}

	private void InitializePopularLettersArray()
	{
		string popular = "EARIOTNSLCUDPMHGBFYWKVXZJQ";
		popular = popular.ToLower() + popular;

		for (int i = 0; i < 52; i++) letters_popular[i] = popular.ToCharArray()[i];
		for (int i = 0; i < 65; i++) letters_popular[52 + i] = (char)i;
		for (int i = 91; i < 97; i++) letters_popular[117 + i - 91] = (char)i;
		for (int i = 123; i < (int)Math.Pow(2, 16); i++) letters_popular[i] = (char)i;
	}

	public string MTFTransform(string text)
	{
		string text_transformed = "";
		char[] letters = new char[(int)Math.Pow(2, 16)];
		letters_popular.CopyTo(letters, 0);
		char tmp;
		char tmp_2;
		for (int i = 0; i < text.Length; i++)
		{
			tmp = letters[0];
			for (int j = 0; j < letters.Length; j++)
			{
				tmp_2 = letters[j];
				letters[j] = tmp;
				tmp = tmp_2;
				if (tmp == text[i])
				{
					if (j == '\uffff') throw new InvalidDataException("invalid text");
					text_transformed += (char)(j + 1);
					break;
				}
			}
			letters[0] = text[i];
		}
		return text_transformed;
	}

	public string MTFDetransform(string text)
	{
		if (text == "") return text;
		string text_detransformed = "";
		char[] letters = new char[(int)Math.Pow(2, 16)];
		letters_popular.CopyTo(letters, 0);
		int[] nums = new int[text.Length];

		for (int i = 0; i < text.Length; i++) nums[i] = (int)text[i] - 1;

		char tmp;
		char tmp_2;
		char current_char;
		for (int i = 0; i < nums.Length; i++)
		{
			tmp = letters[0];
			current_char = letters[nums[i]];
			text_detransformed += letters[nums[i]];
			for (int j = 0; j <= nums[i]; j++)
			{
				tmp_2 = letters[j];
				letters[j] = tmp;
				tmp = tmp_2;
			}
			letters[0] = current_char;
		}
		return text_detransformed;
	}

	public string EncodeArithmetic(string text)
	{
		Arithmetic.Encoder encoder = new Arithmetic.Encoder();
		return encoder.Encode(text);
	}

	public string DecodeArithmetic(string text)
	{
		Arithmetic.Decoder encoder = new Arithmetic.Decoder();
		return encoder.Decode(text);
	}

	public string EncodePPM(string text, int order)
	{
		PPM.Encoder encoder = new PPM.Encoder();
		return encoder.Encode(text, order);
	}

	public string DecodePPM(string text, int order)
	{
		PPM.Decoder encoder = new PPM.Decoder();
		return encoder.Decode(text, order);
	}
}
