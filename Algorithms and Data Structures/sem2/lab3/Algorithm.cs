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
	private char[] letters_popular = new char[256];

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
			while (Char.IsDigit(text[i]))
			{
				number = number + text[i];
				i++;
			}
			for (int j = 0; j < Convert.ToInt32(number); j++) text_decoded = text_decoded + text[i];
			number = "";
			i++;
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

		for (int i = 0; i < text.Length; i++)
		{
			if (!dict.ContainsKey(buffer + text[i]))
			{
				index++;
				dict[buffer + text[i]] = new LZ78DictEntry(index, text[i]);

				if (dict.ContainsKey(buffer)) text_encoded = text_encoded + dict[buffer].Index + text[i];
				else text_encoded = text_encoded + "0" + text[i];
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
			if (dict.ContainsKey(buffer)) text_encoded = text_encoded + dict[buffer].Index + symbol;
			else text_encoded = text_encoded + "0" + symbol;
		}
		return text_encoded;
	}

	public string DecodeLZ78(string text)
	{
		if (text.Length == 0) return text;

		string text_decoded = "";
		Dictionary<int, string> dict = new Dictionary<int, string>();
		int index = 0;

		string number = "";
		for (int i = 0; i < text.Length; i++)
		{
			while (Char.IsDigit(text[i]))
			{
				number = number + text[i];
				i++;
			}
			index++;
			if (number == "0") {
				text_decoded = text_decoded + text[i];
				dict[index] = text[i].ToString();
			}
			else
			{
				text_decoded = text_decoded + dict[Convert.ToInt32(number)] + text[i];
				dict[index] = dict[Convert.ToInt32(number)] + text[i].ToString();
			}

			number = "";
		}

		return text_decoded;
	}

	public string BWTTransform(string text)
	{
		text += '\0';
		string text_transformed = "";
		string[] substrings = new string[text.Length];
		for (int i = 0; i < text.Length; i++)
		{
			substrings[i] = text.Substring(i, text.Length - i) + text.Substring(0, i);
		}
		Array.Sort(substrings, StringComparer.Ordinal);
		for (int i = 0; i < substrings.Length; i++)
		{
			text_transformed += substrings[i][substrings.Length - 1];
		}

		return text_transformed;
	}

	public string BWTDetransform(string text)
	{
		string text_detransformed = "";
		string[] substrings = new string[text.Length];
		for (int i = 0; i < text.Length; i++)
		{
			for (int j = 0; j < text.Length; j++)
			{
				substrings[j] = text[j] + substrings[j];
			}
			Array.Sort(substrings, StringComparer.Ordinal);
		}

		for (int i = 0; i < substrings.Length; i++)
		{
			if (substrings[i][substrings.Length - 1] == '\0')
			{
				text_detransformed = substrings[i].Substring(0, substrings.Length - 1);
				break;
			}
		}
		return text_detransformed;
	}

	private void InitializePopularLettersArray()
	{
		string popular = "EARIOTNSLCUDPMHGBFYWKVXZJQ";
		popular = popular.ToLower() + popular;

		for (int i = 0; i < 52; i++) letters_popular[i] = popular.ToCharArray()[i];
		for (int i = 32; i < 65; i++) letters_popular[52 + i - 32] = (char)i;
		for (int i = 91; i < 97; i++) letters_popular[85 + i - 91] = (char)i;
		for (int i = 123; i < 128; i++) letters_popular[91 + i - 123] = (char)i;
		for (int i = 0; i < 32; i++) letters_popular[123 + i] = (char)i;
		for (int i = 128; i < 256; i++) letters_popular[i] = (char)i;
	}

	public string MTFTransform(string text)
	{
		string text_transformed = "";
		char[] letters = new char[256];
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
					text_transformed += j.ToString() + ' ';
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
		char[] letters = new char[256];
		letters_popular.CopyTo(letters, 0);
		int[] nums = Array.ConvertAll(text.Trim().Split(' '), new Converter<string, int>((string s) => Int32.Parse(s)));

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
