using System.Globalization;

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

	public string EncodeHuffman(string text, out Dictionary<char, string> codes)
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
		GetHuffmanCodes(tree.Root, ref codes);

		string text_encoded = "";
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

	public string DecodeHuffman(string text, Dictionary<char, string> codes)
	{
		Dictionary<string, char> codes_reversed = new Dictionary<string, char>();
		foreach (var code in codes) codes_reversed[code.Value] = code.Key;
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

	public string EncodeArithmetic(string text, out Dictionary<char, UInt128[]> frequency_distribution)
	{
		// init
		text += '\0';
		string text_encoded = "";

		UInt128 ratio = UInt128.MaxValue / (UInt128)text.Length;
		UInt128 whole = ratio * (UInt128)text.Length;	// almost UInt128.MaxValue
		UInt128 half = whole / 2;
		UInt128 quater = whole / 4;
		UInt128 a = 0;
		UInt128 b = whole;

		// create frequency_distribution
		frequency_distribution = new Dictionary<char, UInt128[]>();
		for (int i = 0; i < text.Length; i++)
		{
			if (frequency_distribution.ContainsKey(text[i])) frequency_distribution[text[i]][0]++;
			else 
			{
				frequency_distribution[text[i]] = new UInt128[2];
				frequency_distribution[text[i]][0] = 1;
			}
		}

		UInt128 prev = 0;
		foreach (KeyValuePair<char, UInt128[]> pair in frequency_distribution)
		{
			frequency_distribution[pair.Key][1] = prev + pair.Value[0] * ratio;
			frequency_distribution[pair.Key][0] = prev;
			prev = frequency_distribution[pair.Key][1];
		}

		// algorithm
		int s = 0;
		UInt128 width;
		UInt128 a_prev;
		foreach (char sym in text)
		{
			a_prev = a;
			width = b - a;
			a = a_prev + (UInt128)((double)frequency_distribution[sym][0] / (double)whole * (double)width);
			b = a_prev + (UInt128)((double)frequency_distribution[sym][1] / (double)whole * (double)width);

			while (b < half || a > half)
			{
				if (b < half)
				{
					a = 2 * a;
					b = 2 * b;
					text_encoded += "0";
					for (int i = 0; i < s; i++) text_encoded += "1";
					s = 0;
				}
				else
				{
					a = 2 * (a - half);
					b = 2 * (b - half);
					text_encoded += "1";
					for (int i = 0; i < s; i++) text_encoded += "0";
					s = 0;
				}
			}
			while (a > quater && b < 3 * quater)
			{
				s++;
				a = 2 * (a - quater);
				b = 2 * (b - quater);
			}
		}

		s++;
		if (a <= quater)
		{
			text_encoded += "0";
			for (int i = 0; i < s; i++) text_encoded += "1";
		}
		else
		{
			text_encoded += "1";
			for (int i = 0; i < s; i++) text_encoded += "0";
		}

		return text_encoded;
	}

	public string DecodeArithmetic(string text, Dictionary<char, UInt128[]> frequency_distribution)
	{
		string text_decoded = "";
		int precision = 128;
		UInt128 ratio = UInt128.MaxValue / (UInt128)text.Length;
		UInt128 whole = ratio * (UInt128)text.Length;	// almost UInt128.MaxValue
		UInt128 half = whole / 2;
		UInt128 quater = whole / 4;
		UInt128 a = 0;
		UInt128 b = whole;
		UInt128 approximated_number = 0;

		int i = 0;
		while (i < text.Length && i < precision)
		{
			if (text[i] == '1') approximated_number += (UInt128)Math.Pow(2, precision - i - 1);
			i++;
		}

		UInt128 a_tmp = a;
		UInt128 b_tmp = b;
		UInt128 width;
		while (true)
		{
			foreach (KeyValuePair<char, UInt128[]> pair in frequency_distribution)
			{
				width = b - a;
				a_tmp = a + (UInt128)((double)width / (double)whole * (double)pair.Value[0]);
				b_tmp = a + (UInt128)((double)width / (double)whole * (double)pair.Value[1]);

				if (a_tmp <= approximated_number && approximated_number < b_tmp)
				{
					if (pair.Key == '\0') return text_decoded;
					text_decoded += pair.Key;
					a = a_tmp;
					b = b_tmp;
					break;
				}
			}

			while (b < half || a > half)
			{
				if (b < half)
				{
					a = 2 * a;
					b = 2 * b;
					approximated_number = 2 * approximated_number;
				}
				else
				{
					a = 2 * (a - half);
					b = 2 * (b - half);
					approximated_number = 2 * (approximated_number - half);
				}
				if (i < text.Length && text[i] == '1') approximated_number++;
				i++;
			}

			while (a > quater && b < 3 * quater)
			{
				a = 2 * (a - quater);
				b = 2 * (b - quater);
				approximated_number = 2 * (approximated_number - quater);
				if (i < text.Length && text[i] == '1') approximated_number++;
				i++;
			}
		}
	}
}