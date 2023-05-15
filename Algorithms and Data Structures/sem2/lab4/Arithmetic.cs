using System.Collections.Generic;
using System;
using System.IO;


namespace lab4
{
	public class Arithmetic
	{
		public const int precision = 32;
		public const char EOF = '\uffff';
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
			for (int i = 0; i < text.Length; i++) Freqs[text[i]] = 1;
			Count = Freqs.Count;
		}


		public class Encoder : Arithmetic
		{
			private int s;

			public void InitEncoder(ref string text, out string text_encoded)
			{
				text += EOF;
				InitializeFreqs(text);
				text_encoded = "";
				EncodeAlphabet(ref text_encoded);

				a = 0;
				b = Whole;
				s = 0;
			}

			public void EncodeAlphabet(ref string text_encoded)
			{
				// Freqs must be initialized
				string symbols = "";
				string bin_sym;
				foreach (char sym in Freqs.Keys) symbols += sym;
				foreach (char sym in symbols)
				{
					bin_sym = Convert.ToString(sym, 2);
					for (int i = bin_sym.Length; i < 16; i++) bin_sym = "0" + bin_sym;
					text_encoded += bin_sym;
				}
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

			public string Encode(string text)
			{
				// init
				InitEncoder(ref text, out string text_encoded);

				// algorithm
				foreach (char sym in text)
				{
					Update(sym, ref text_encoded);
					Freqs[sym]++;
					Count++;
				}
				Finish(ref text_encoded);

				return text_encoded;
			}
		}


		public class Decoder : Arithmetic
		{
			private int index;
			private uint code;

			public string InitDecoder(ref string text)
			{
				string symbols = "";
				int k = 0;
				string bin_sym;
				char sym;
				while (k + 15 < text.Length)
				{
					bin_sym = text.Substring(k, 16);
					sym = (char)Convert.ToInt32(bin_sym, 2);

					symbols += Convert.ToString(sym);
					k += 16;
					if (sym == EOF) break;
				}
				text = text.Substring(k, text.Length - k);

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

				return symbols;
			}

			public void ReadNextBit(string text)
			{
				if (index < text.Length)
				{
					if (text[index] == '1') code |= 1;
					index++;
				}
			}

			public int DecodeSymbol(Dictionary<int, int[]> CumFreqs)
			{
				long width = (long)b - a + 1;
				int value = (int)(((code - a + 1) * (long)Count - 1) / width);
				foreach (int sym in CumFreqs.Keys)
				{
					if (CumFreqs[sym][0] <= value && value < CumFreqs[sym][1]) return sym;
				}

				throw new InvalidDataException("Cannot decode symbol");
			}

			public void Update(int sym, string text, Dictionary<int, int[]> CumFreqs)
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

			public string Decode(string text)
			{
				string text_decoded = "";
				InitDecoder(ref text);

				Dictionary<int, int[]> CumFreqs = GetCumFreqs();
				int sym = DecodeSymbol(CumFreqs);

				while (sym != EOF)
				{
					text_decoded += (char)sym;
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
}

