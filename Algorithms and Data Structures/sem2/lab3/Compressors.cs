namespace lab3;


class Compressor
{
	public string BinToString(string bin_text)
	{
		string text = "";
		for (int i = 0; i < bin_text.Length; i += 16)
		{
			text += (char)Convert.ToInt32(bin_text.Substring(i, 16), 2);
		}

		return text;
	}

	public string StringToBin(string text)
	{
		string bin_text = "";
		string bin_sym;
		for (int i = 0; i < text.Length; i++)
		{
			bin_sym = Convert.ToString(text[i], 2);
			for (int k = bin_sym.Length; k < 16; k++) bin_sym = "0" + bin_sym;
			bin_text += bin_sym;
		}

		return bin_text;
	}
}


class BWT_MTF_Huffman : Compressor
{
	private Algorithm algorithm = new Algorithm();

	public string Encode(string text)
	{
		text = algorithm.BWTTransform(text);
		text = algorithm.MTFTransform(text);
		text = algorithm.EncodeHuffman(text);
		return text;
	}

	public string Decode(string text)
	{
		text = algorithm.DecodeHuffman(text);
		text = algorithm.MTFDetransform(text);
		text = algorithm.BWTDetransform(text);
		return text;
	}
}


class BWT_MTF_Arithmetic : Compressor
{
	private Algorithm algorithm = new Algorithm();

	public string Encode(string text)
	{
		text = algorithm.BWTTransform(text);
		text = algorithm.MTFTransform(text);
		text = algorithm.EncodeArithmetic(text);
		return text;
	}

	public string Decode(string text)
	{
		text = algorithm.DecodeArithmetic(text);
		text = algorithm.MTFDetransform(text);
		text = algorithm.BWTDetransform(text);
		return text;
	}
}


class RLE_BWT_MTF_RLE_Huffman : Compressor
{
	private Algorithm algorithm = new Algorithm();

	public string Encode(string text)
	{
		text = algorithm.EncodeRLE(text);
		text = BinToString(text);
		text = algorithm.BWTTransform(text);
		text = algorithm.MTFTransform(text);
		text = algorithm.EncodeRLE(text);
		text = BinToString(text);
		text = algorithm.EncodeHuffman(text);
		return text;
	}

	public string Decode(string text)
	{
		text = algorithm.DecodeHuffman(text);
		text = StringToBin(text);
		text = algorithm.DecodeRLE(text);
		text = algorithm.MTFDetransform(text);
		text = algorithm.BWTDetransform(text);
		text = StringToBin(text);
		text = algorithm.DecodeRLE(text);
		return text;
	}
}


class RLE_BWT_MTF_RLE_Arithmetic : Compressor
{
	private Algorithm algorithm = new Algorithm();

	public string Encode(string text)
	{
		text = algorithm.EncodeRLE(text);
		text = BinToString(text);
		text = algorithm.BWTTransform(text);
		text = algorithm.MTFTransform(text);
		text = algorithm.EncodeRLE(text);
		text = BinToString(text);
		text = algorithm.EncodeArithmetic(text);
		return text;
	}

	public string Decode(string text)
	{
		text = algorithm.DecodeArithmetic(text);
		text = StringToBin(text);
		text = algorithm.DecodeRLE(text);
		text = algorithm.MTFDetransform(text);
		text = algorithm.BWTDetransform(text);
		text = StringToBin(text);
		text = algorithm.DecodeRLE(text);
		return text;
	}
}
