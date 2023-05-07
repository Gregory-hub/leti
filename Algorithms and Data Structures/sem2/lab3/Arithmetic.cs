namespace lab3;


public class Arithmetic
{
	public const int precision = 32;
	public uint Whole, Half, Quater;
	private uint a, b;
	public int Count;
	public Dictionary<int, int> Freqs = new Dictionary<int, int>();

	public Arithmetic()
	{
		Whole = uint.MaxValue;
		Half = (Whole >> 1) + 1;
		Quater = (Half >> 1);
	}

	public Dictionary<int, int[]> GetCumFreqs()
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


	public class Encoder : Arithmetic
	{
		private int s;

		public void InitEncoder(ref string text) 
		{
			text += '\0';
			InitializeFreqs(text);
			a = 0;
			b = Whole;
			s = 0;
		}

		public void Update(int sym, ref string text_encoded)
		{
			Dictionary<int, int[]> CumFreqs = GetCumFreqs();

			uint a_prev = a;
			long width = (long)b - a + 1;
			a = a_prev + (uint)(CumFreqs[sym][0] * width / Count);
			b = a_prev + (uint)(CumFreqs[sym][1] * width / Count) - 1;

			// while a and b have same first bit or a >= Quater && b < 3 * Quater
			uint bit;
			while (((a ^ b) & Half) == 0 || a >= Quater && b < 3 * Quater)
			{
				if (((a ^ b) & Half) == 0)
				{
					bit = ((a & Half) == 0) ? (uint)0 : (uint)1;
					a = (a & ~Half) << 1;
					b = ((b & ~Half) << 1) | 1;

					text_encoded += bit;
					for (; s > 0; s--) text_encoded += (bit == 0) ? '1' : '0';
				}

				if (a >= Quater && b < 3 * Quater)
				{
					a = (a & ~Half) << 1;
					b = ((b & ~Half) << 1) | 1;

					a = a ^ Half;
					b = b ^ Half;
					s++;
				}
			}
		}

		public void Finish(ref string text_encoded)
		{
			s++;
			if (a <= Quater)
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
				Update(sym, ref text_encoded);
				Freqs[sym]++;
				Count++;
			}
			Finish(ref text_encoded);

			symbols = "";
			foreach(char sym in Freqs.Keys) symbols += sym;
			return text_encoded;
		}
	}


	public class Decoder : Arithmetic
	{
		private int index;
		private uint code;

		public void InitDecoder(string text, string symbols) 
		{
			InitializeFreqs(symbols);
			a = 0;
			b = Whole;
			code = 0;
			index = Math.Min(text.Length, precision);
			if (text.Length < precision)
			{
				for (int i = index; i < precision; i++) text += '0';
			}
			index = precision;
			code = Convert.ToUInt32(text.Substring(0, index), 2);
		}

		public void ReadNextBit(string text)
		{
			if (index < text.Length)
			{
				if (text[index] == '1') code |= 1;
				index++;
			}
		}

		public char DecodeSymbol(Dictionary<int, int[]> CumFreqs)
		{
			long width = (long)b - a + 1;
			int value = (int)(((code - a + 1) * (long)Count - 1) / width);
			foreach (char sym in CumFreqs.Keys)
			{
				if (CumFreqs[sym][0] <= value && value < CumFreqs[sym][1]) return sym;
			}

			throw new InvalidDataException("Cannot decode symbol");
		}

		public void Update(char sym, string text, Dictionary<int, int[]> CumFreqs)
		{
			uint a_prev = a;
			long width = (long)b - a + 1;
			a = a_prev + (uint)(CumFreqs[sym][0] * width / Count);
			b = a_prev + (uint)(CumFreqs[sym][1] * width / Count) - 1;

			// while a and b have same first bit or a >= Quater && b < 3 * Quater
			while (((a ^ b) & Half) == 0 || a >= Quater && b < 3 * Quater)
			{
				if (((a ^ b) & Half) == 0)
				{
					a = (a & ~Half) << 1;
					b = ((b & ~Half) << 1) | 1;
					code = (code & ~Half) << 1;
					ReadNextBit(text);
				}

				if (a >= Quater && b < 3 * Quater)
				{
					a = (a & ~Half) << 1;
					b = ((b & ~Half) << 1) | 1;
					code = ((code & ~Half) << 1);

					a = a ^ Half;
					b = b ^ Half;
					code = code ^ Half;
					ReadNextBit(text);
				}
			}
		}

		public string Decode(string text, string symbols)
		{
			string text_decoded = "";
			InitDecoder(text, symbols);

			Dictionary<int, int[]> CumFreqs = GetCumFreqs();
			char sym = DecodeSymbol(CumFreqs);

			while (sym != '\0')
			{
				text_decoded += sym;
				Update(sym, text, CumFreqs);
				Freqs[sym]++;
				Count++;

				CumFreqs = GetCumFreqs();
				sym = DecodeSymbol(CumFreqs);
			}

			return text_decoded;
		}
	}
}
