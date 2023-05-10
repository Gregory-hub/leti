namespace lab3;


public class PPM
// PPMc
{
	public const int ESC = (int)'\uffff';
	

	// model
	public class Model
	{
		public int Order;
		public Dictionary<string, Context>[] Contexts;
		public Dictionary<int, int> MinusOneFrequencies;

		public Model(int order, string text)
		{
			Order = order;
			Contexts = new Dictionary<string, Context>[Order + 1];
			for (int i = 0; i <= order; i++) Contexts[i] = new Dictionary<string, Context>();

			MinusOneFrequencies = new Dictionary<int, int>();
		
			for (int i = 0; i < text.Length; i++) MinusOneFrequencies[text[i]] = 1;
			MinusOneFrequencies[ESC] = 1;
		}

		public class Context
		{
			public Dictionary<int, int> Frequencies = new Dictionary<int, int>();	// [(int)char_symbol] = frequency
		}
	}


	// encoder
	public class Encoder : PPM
	{
		private Arithmetic.Encoder encoder;
		public Encoder()
		{
			encoder = new Arithmetic.Encoder();
		}

		public void InitEncoder(ref string text, out string text_encoded, int model_order, out Model model)
		{
			encoder.InitEncoder(ref text, out text_encoded);
			model = new Model(model_order, text);
		}

		private void EncodeSymbol(Dictionary<int, int> frequencies, int sym, ref string text_encoded)
		{
			encoder.Freqs = frequencies;
			encoder.Count = frequencies.Sum(freq => freq.Value);
			encoder.Update(sym, ref text_encoded);
		}

		public void Update(Model model, int sym_index, string text, ref string text_encoded)
		{
			int order;
			string context;
			for (order = Math.Min(model.Order, sym_index); order >= 0; order--)
			{
				context = text.Substring(sym_index - order, order);
				if (model.Contexts[order].ContainsKey(context))
				{
					if (model.Contexts[order][context].Frequencies.ContainsKey(text[sym_index]))
					{
						EncodeSymbol(model.Contexts[order][context].Frequencies, text[sym_index], ref text_encoded);
						model.Contexts[order][context].Frequencies[text[sym_index]]++;
						break;
					}
					else
					{
						EncodeSymbol(model.Contexts[order][context].Frequencies, ESC, ref text_encoded);
						model.Contexts[order][context].Frequencies[text[sym_index]] = 1;
						model.Contexts[order][context].Frequencies[ESC]++;
					}
				}
				else
				{
					EncodeSymbol(model.MinusOneFrequencies, ESC, ref text_encoded);
					model.Contexts[order][context] = new Model.Context();
					model.Contexts[order][context].Frequencies[ESC] = 1;
					model.Contexts[order][context].Frequencies[text[sym_index]] = 1;
				}
			}

			if (order == -1)
			{
				EncodeSymbol(model.MinusOneFrequencies, text[sym_index], ref text_encoded);
			}
		}

		public string Encode(string text, int model_order)
		{
			InitEncoder(ref text, out string text_encoded, model_order, out Model model);

			for (int i = 0; i < text.Length; i++)
			{
				Update(model, i, text, ref text_encoded);
			}
			encoder.Finish(ref text_encoded);

			return text_encoded;
		}
	}


	public class Decoder : PPM
	{
		private Arithmetic.Decoder decoder;
		public Decoder()
		{
			decoder = new Arithmetic.Decoder();
		}

		public void InitDecoder(ref string text, int model_order, out Model model)
		{
			string symbols = decoder.InitDecoder(ref text);
			model = new Model(model_order, symbols);
		}

		private void Update(ref Model model, int order, string text, char sym)
		{
			if (order == -1) order = 0;
			if (text.Length == 0) return;

			text = text.Substring(0, text.Length - 1);
			string context;

			for (int i = order; i <= Math.Min(model.Order, text.Length); i++)
			{
				context = text.Substring(text.Length - i, i);
				if (model.Contexts[i].ContainsKey(context))
				{
					if (model.Contexts[i][context].Frequencies.ContainsKey(sym))
					{
						model.Contexts[i][context].Frequencies[sym]++;
					}
					else 
					{
						model.Contexts[i][context].Frequencies[sym] = 1;
						model.Contexts[i][context].Frequencies[ESC]++;
					}
				}
				else
				{
					model.Contexts[i][context] = new Model.Context();
					model.Contexts[i][context].Frequencies[ESC] = 1;
					model.Contexts[i][context].Frequencies[sym] = 1;
				}
			}
		}

		private char DecodeSymbol(Dictionary<int, int> frequencies, string text)
		{
			decoder.Freqs = frequencies;
			decoder.Count = frequencies.Sum(freq => freq.Value);
			Dictionary<int, int[]> CumFreqs = decoder.GetCumFreqs();

			int sym = decoder.DecodeSymbol(CumFreqs);

			if (sym != 0) decoder.Update(sym, text, CumFreqs);

			return (char)sym;
		}

		public string Decode(string text, int model_order)
		{
			string text_decoded = "";
			InitDecoder(ref text, model_order, out Model model);

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
						sym = DecodeSymbol(model.Contexts[order][context].Frequencies, text);
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
						sym = DecodeSymbol(model.MinusOneFrequencies, text);
						if (sym != ESC) throw new InvalidDataException("ESC symbol expected");
						order--;
						if (order == -1) break;
						context = context.Substring(1, order);
					}
				}

				if (order == -1)
				{
					sym = DecodeSymbol(model.MinusOneFrequencies, text);
					if (sym == ESC) throw new InvalidDataException("Order cannot be < -1");
					if (sym != '\0') text_decoded += (char)sym;
				}

				Update(ref model, order, text_decoded, (char)sym);
			}

			return text_decoded;
		}
	}

}
