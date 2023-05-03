namespace lab3;


public class Arithmetic
{
	private int a, b, whole, half, quater;
	public int approximated_number;
	private int s = 0;
	public int Count;
	public int precision = 16;
	public Dictionary<int, int> Freqs = new Dictionary<int, int>();

	public Arithmetic()
	{
		InitializeConstants();
	}

	public void InitializeConstants()
	{
		whole = int.MaxValue;
		half = whole / 2;
		quater = half / 2;
	}

	private Dictionary<int, int[]> GetCumFreqs()
	{
		Dictionary<int, int[]> CumFreqs = new Dictionary<int, int[]>();
		int prev = 0;
		foreach (KeyValuePair<int, int> pair in Freqs)
		{
			CumFreqs[pair.Key] = new int[2];
			CumFreqs[pair.Key][0] = prev;
			CumFreqs[pair.Key][1] = prev + pair.Value;
			prev = CumFreqs[pair.Key][1];
		}

		return CumFreqs;
	}

	public void InitializeFreqs(string text)
	{
		foreach (char sym in text) Freqs[sym] = 1;
		Count = Freqs.Count;
	}

	public void InitEncoder(ref string text) 
	{
		text += '\0';
		InitializeFreqs(text);
		a = 0;
		b = whole;
	}

	public void UpdateEncode(int sym, ref string text_encoded)
	{
		Dictionary<int, int[]> CumFreqs = GetCumFreqs();

		int a_prev = a;
		int width = b - a;
		a = a_prev + (int)Math.Round((decimal)CumFreqs[sym][0] / (decimal)Count * width);
		b = a_prev + (int)Math.Round((decimal)CumFreqs[sym][1] / (decimal)Count * width);

		while (b <= half || a >= half)
		{
			if (b <= half)
			{
				a = 2 * a;
				b = 2 * b;
				text_encoded += "0";
				for (int i = 0; i < s; i++) text_encoded += "1";
			}
			else
			{
				a = 2 * (a - half);
				b = 2 * (b - half);
				text_encoded += "1";
				for (int i = 0; i < s; i++) text_encoded += "0";
			}
			s = 0;
		}

		while (a >= quater && b <= 3 * quater)
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


	public int InitDecoder(string text, string symbols) 
	{
		InitializeFreqs(symbols);
		a = 0;
		b = whole;

		approximated_number = 0;
		int i = 0;
		while (i < text.Length && i < precision)
		{
			if (text[i] == '1') approximated_number += (int)((double)whole / Math.Pow(2, 1 + i));
			i++;
		}
		return i;
	}

	private void ReadNextBit(string text, ref int i, ref int approximated_number)
	{
		if (i < text.Length)
		{
			if (text[i] == '1') approximated_number += 1;
			i++;
		}
	}

	public void UpdateDecode(string text, ref int i)
	{
		while (b <= half || a >= half)
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
				approximated_number = 2 * (approximated_number - half);
			}
			ReadNextBit(text, ref i, ref approximated_number);
		}

		while (a >= quater && b <= 3 * quater)
		{
			a = 2 * (a - quater);
			b = 2 * (b - quater);
			approximated_number = 2 * (approximated_number - quater);
			ReadNextBit(text, ref i, ref approximated_number);
		}
	}

	public char DecodeNextSymbol(string text)
	{
		int a_tmp = a;
		int b_tmp = b;
		int width;
		Dictionary<int, int[]> CumFreqs = GetCumFreqs();

		foreach (KeyValuePair<int, int[]> pair in CumFreqs)
		{
			width = b - a;
			a_tmp = a + (int)Math.Round((decimal)pair.Value[0] / (decimal)Count * width);
			b_tmp = a + (int)Math.Round((decimal)pair.Value[1] / (decimal)Count * width);

			if (a_tmp <= approximated_number && approximated_number <= b_tmp)
			{
				a = a_tmp;
				b = b_tmp;
				return (char)pair.Key;
			}
		}

		throw new InvalidDataException($"Cannot decode symbol(a = {a}, approximated_number = {approximated_number}, b = {b}). Approximated_number must be between a and b");
	}

	public string Decode(string text, string symbols)
	{
		int i = InitDecoder(text, symbols);		// i is index of next symbol in text
		string text_decoded = "";

		char sym;
		while (true)
		{
			sym = DecodeNextSymbol(text);

			if (sym == '\0') return text_decoded;
			text_decoded += sym;

			UpdateDecode(text, ref i);
			Freqs[sym]++;
			Count++;
		}
	}
}
