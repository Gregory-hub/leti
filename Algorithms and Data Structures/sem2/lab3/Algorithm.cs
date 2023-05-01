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


	public class ArithmeticEncoder
	{
		private UInt128 whole, half, quater, a, b;
		private int s = 0;
		public int Count;
		public Dictionary<int, int> Freqs = new Dictionary<int, int>();	// counts, Freqs[(int)char_symbol] = frequency

		private Dictionary<int, UInt128[]> GetCumFreqs()
		{
			Dictionary<int, UInt128[]> CumFreqs = new Dictionary<int, UInt128[]>();
			UInt128 ratio = UInt128.MaxValue / (UInt128)Count;
			UInt128 prev = 0;
			foreach (KeyValuePair<int, int> pair in Freqs)
			{
				CumFreqs[pair.Key] = new UInt128[2];
				CumFreqs[pair.Key][0] = prev;
				CumFreqs[pair.Key][1] = prev + (UInt128)((double)Freqs[pair.Key] * (double)ratio);
				if (CumFreqs[pair.Key][1] < CumFreqs[pair.Key][0]) CumFreqs[pair.Key][1] = whole;
				prev = CumFreqs[pair.Key][1];
			}

			return CumFreqs;
		}

		public void InitializeFreqs(string text)
		{
			foreach (char sym in text) Freqs[sym] = 1;
			Count = Freqs.Count;
		}

		private void InitializeConstants()
		{
			UInt128 ratio = UInt128.MaxValue / (UInt128)Count;
			whole = ratio * (UInt128)Count;	// almost UInt128.MaxValue
			half = whole / 2;
			quater = whole / 4;
		}

		public void InitEncoder(ref string text) 
		{
			text += '\0';
			InitializeFreqs(text);
			InitializeConstants();
			a = 0;
			b = whole;
		}

		public void UpdateEncode(int sym, ref string text_encoded)
		{
			Dictionary<int, UInt128[]> CumFreqs = GetCumFreqs();

			UInt128 a_prev = a;
			UInt128 width = b - a;
			a = a_prev + (UInt128)((double)CumFreqs[sym][0] / (double)whole * (double)width);
			b = a_prev + (UInt128)((double)CumFreqs[sym][1] / (double)whole * (double)width);

			while (b <= half || a > half)
			{
				if (b <= half)
				{
					a = 2 * a;
					b = 2 * b;
					if (b < a) b = whole;
					text_encoded += "0";
					for (int i = 0; i < s; i++) text_encoded += "1";
					s = 0;
				}
				else
				{
					a = 2 * (a - half);
					b = 2 * (b - half);
					if (b < a) b = whole;
					text_encoded += "1";
					for (int i = 0; i < s; i++) text_encoded += "0";
					s = 0;
				}
			}

			while (a > quater && b <= 3 * quater)
			{
				s++;
				a = 2 * (a - quater);
				b = 2 * (b - quater);
			}
		}

		public void Finish(ref string text_encoded)
		{
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
		}

		public string Encode(string text, out string symbols)
		{
			// init
			InitEncoder(ref text);
			string text_encoded = "";

			// algorithm
			foreach (char sym in text)
			{
				UpdateEncode(sym, ref text_encoded);
				Freqs[sym]++;
				Count++;
			}
			Finish(ref text_encoded);

			symbols = "";
			foreach(char sym in Freqs.Keys) symbols += sym;
			return text_encoded;
		}

		public int InitDecoder(string text, string symbols, out int precision, out UInt128 approximated_number) 
		{
			precision = 128;
			InitializeFreqs(symbols);
			InitializeConstants();
			a = 0;
			b = whole;

			approximated_number = 0;
			int i = 0;
			while (i < text.Length && i < precision)
			{
				if (text[i] == '1') approximated_number += (UInt128)Math.Pow(2, precision - i - 1);
				i++;
			}
			return i;
		}

		public void UpdateDecode(string text, ref UInt128 approximated_number, ref int i)
		{
			while (b <= half || a > half)
			{
				if (b <= half)
				{
					a = 2 * a;
					b = 2 * b;
					approximated_number = 2 * approximated_number;
				}
				else
				{
					a = 2 * (a - half);
					b = 2 * (b - half);
					if (b < a) b = whole;
					approximated_number = 2 * (approximated_number - half);
				}
				if (i < text.Length && text[i] == '1') approximated_number++;
				i++;
			}

			while (a > quater && b <= 3 * quater)
			{
				a = 2 * (a - quater);
				b = 2 * (b - quater);
				if (b < a) b = whole;
				approximated_number = 2 * (approximated_number - quater);
				if (i < text.Length && text[i] == '1') approximated_number++;
				i++;
			}
		}

		public char DecodeNextSymbol(string text, UInt128 approximated_number)
		{
			UInt128 a_tmp = a;
			UInt128 b_tmp = b;
			UInt128 width;

			Dictionary<int, UInt128[]> CumFreqs = GetCumFreqs();

			foreach (KeyValuePair<int, UInt128[]> pair in CumFreqs)
			{
				width = b - a;
				a_tmp = a + (UInt128)((double)width / (double)whole * (double)pair.Value[0]);
				b_tmp = a + (UInt128)((double)width / (double)whole * (double)pair.Value[1]);

				if (a_tmp < approximated_number && approximated_number <= b_tmp)
				{
					a = a_tmp;
					b = b_tmp;
					return (char)pair.Key;
				}
			}

			throw new InvalidDataException("Cannot decode symbol");
		}

		public string Decode(string text, string symbols)
		{
			int i = InitDecoder(text, symbols, out int precision, out UInt128 approximated_number);		// i is index of next symbol in text
			string text_decoded = "";

			char sym;
			while (true)
			{
				sym = DecodeNextSymbol(text, approximated_number);

				if (sym == '\0') return text_decoded;
				text_decoded += sym;

				UpdateDecode(text, ref approximated_number, ref i);
				Freqs[sym]++;
				Count++;
			}
		}
	}


	public class PPMEncoder
	// PPMc
	{
		public const int ESC = 257;
		class Model
		{
			public int Order;
			public Dictionary<string, Context>[] Contexts;
			public Algorithm.ArithmeticEncoder Encoder;

			public Model(int order)
			{
				Order = order;
				Contexts = new Dictionary<string, Context>[Order + 1];
				for (int i = 0; i <= order; i++) Contexts[i] = new Dictionary<string, Context>();
				Encoder = new Algorithm.ArithmeticEncoder();
			}

			public class Context
			{
				public Dictionary<int, int> Frequences = new Dictionary<int, int>();	// [(int)char_symbol] = frequency
			}
		}

		private void EncodeSymbol(ref Model model, Dictionary<int, int> frequencies, int sym, ref string text_encoded)
		{
			model.Encoder.Freqs = frequencies;
			model.Encoder.Count = frequencies.Sum(freq => freq.Value);
			model.Encoder.UpdateEncode(sym, ref text_encoded);
		}

		public string Encode(string text, int model_order, out string symbols)
		{
			string text_encoded = "";
			Model model = new Model(model_order);
			model.Encoder.InitEncoder(ref text);

			Dictionary<int, int> minus_one_frequencies = new Dictionary<int, int>();
			for (int i = 0; i < text.Length; i++){
				minus_one_frequencies[text[i]] = 1;
			}
			minus_one_frequencies[ESC] = 1;

			int order;
			string context;
			for (int i = 0; i < text.Length; i++)
			{
				for (order = Math.Min(model.Order, i); order >= 0; order--)
				{
					context = text.Substring(i - order, order);
					if (model.Contexts[order].ContainsKey(context))
					{
						if (model.Contexts[order][context].Frequences.ContainsKey(text[i]))
						{
							EncodeSymbol(ref model, model.Contexts[order][context].Frequences, text[i], ref text_encoded);
							model.Contexts[order][context].Frequences[text[i]]++;
							break;
						}
						else
						{
							EncodeSymbol(ref model, model.Contexts[order][context].Frequences, ESC, ref text_encoded);
							model.Contexts[order][context].Frequences[text[i]] = 1;
							model.Contexts[order][context].Frequences[ESC]++;
						}
					}
					else
					{
						EncodeSymbol(ref model, minus_one_frequencies, ESC, ref text_encoded);
						model.Contexts[order][context] = new Model.Context();
						model.Contexts[order][context].Frequences[ESC] = 1;
						model.Contexts[order][context].Frequences[text[i]] = 1;
					}
				}

				if (order == -1)
				{
					EncodeSymbol(ref model, minus_one_frequencies, text[i], ref text_encoded);
				}
			}

			model.Encoder.Finish(ref text_encoded);

			symbols = "";
			foreach(char sym in minus_one_frequencies.Keys) symbols += sym;
			return text_encoded;
		}

		private void UpdateContexts(ref Model model, int order, string text, char sym)
		{
			if (order == -1) order = 0;
			if (text.Length == 0) throw new InvalidDataException("Text cannot be empty");
			text = text.Substring(0, text.Length - 1);
			string context;
			for (int i = order; i <= Math.Min(model.Order, text.Length); i++)
			{
				context = text.Substring(text.Length - i, i);
				if (model.Contexts[i].ContainsKey(context))
				{
					if (model.Contexts[i][context].Frequences.ContainsKey(sym))
					{
						model.Contexts[i][context].Frequences[sym]++;
					}
					else 
					{
						model.Contexts[i][context].Frequences[sym] = 1;
						model.Contexts[i][context].Frequences[ESC]++;
					}
				}
				else
				{
					model.Contexts[i][context] = new Model.Context();
					model.Contexts[i][context].Frequences[ESC] = 1;
					model.Contexts[i][context].Frequences[sym] = 1;
				}
			}
		}

		private int DecodeSymbol(ref Model model, Dictionary<int, int> frequencies, ref UInt128 approximated_number, ref int i, string text)
		{
			model.Encoder.Freqs = frequencies;
			model.Encoder.Count = frequencies.Sum(freq => freq.Value);
			char sym = model.Encoder.DecodeNextSymbol(text, approximated_number);

			if (sym == '\0') return sym;

			model.Encoder.UpdateDecode(text, ref approximated_number, ref i);
			return sym;
		}

		public string Decode(string text, int model_order, string symbols)
		{
			string text_decoded = "";
			Model model = new Model(model_order);
			int i = model.Encoder.InitDecoder(text, symbols, out int precision, out UInt128 approximated_number);

			Dictionary<int, int> minus_one_frequencies = new Dictionary<int, int>();
			for (int k = 0; k < symbols.Length; k++) {
				minus_one_frequencies[symbols[k]] = 1;
			}
			minus_one_frequencies[ESC] = 1;

			int order;
			string context;
			int sym = ESC;
			while (sym != '\0')
			{
				order = Math.Min(model.Order, text_decoded.Length);
				context = text_decoded.Substring(text_decoded.Length - order, order);
				while (order >= 0)
				{
					if (model.Contexts[order].ContainsKey(context))
					{
						sym = DecodeSymbol(ref model, model.Contexts[order][context].Frequences, ref approximated_number, ref i, text);
						if (sym == ESC)
						{
							order--;
							if (order == -1) break;
							context = context.Substring(1, order);
						}
						else
						{
							if (sym != '\0') text_decoded += (char)sym;
							break;
						}
					}

					else
					{
						sym = DecodeSymbol(ref model, minus_one_frequencies, ref approximated_number, ref i, text);
						if (sym != ESC) throw new InvalidDataException("ESC symbol expected");
						order--;
						if (order == -1) break;
						context = context.Substring(1, order);
					}
				}

				if (order == -1)
				{
					sym = DecodeSymbol(ref model, minus_one_frequencies, ref approximated_number, ref i, text);
					if (sym == ESC) throw new InvalidDataException("Order cannot be < -1");
					if (sym != '\0') text_decoded += (char)sym;
				}

				UpdateContexts(ref model, order, text_decoded, (char)sym);
			}

			return text_decoded;
		}

	}
}
