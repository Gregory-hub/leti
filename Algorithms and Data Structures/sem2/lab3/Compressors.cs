namespace lab3;


public abstract class Compressor
{
	protected Algorithm algorithm = new Algorithm();

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

	public abstract string Encode(string text);
	public abstract string Decode(string text);
}


class HuffmanCompressor : Compressor
{
	public override string Encode(string text)
	{
		return algorithm.EncodeHuffman(text);
	}

	public override string Decode(string text)
	{
		return algorithm.DecodeHuffman(text);
	}
}


class ArithmeticCompressor : Compressor
{
	public override string Encode(string text)
	{
		return algorithm.EncodeArithmetic(text);
	}

	public override string Decode(string text)
	{
		return algorithm.DecodeArithmetic(text);
	}
}


class LZ78Compressor : Compressor
{
	public override string Encode(string text)
	{
		return algorithm.EncodeLZ78(text);
	}

	public override string Decode(string text)
	{
		return algorithm.DecodeLZ78(text);
	}
}


class PPMCompressor : Compressor
{
	public int Order;
	public PPMCompressor(int order) 
	{
		Order = order;
	}

	public override string Encode(string text)
	{
		return algorithm.EncodePPM(text, Order);
	}

	public override string Decode(string text)
	{
		return algorithm.DecodePPM(text, Order);
	}
}


class BWT_MTF_Huffman : Compressor
{
	public override string Encode(string text)
	{
		text = algorithm.BWTTransform(text);
		text = algorithm.MTFTransform(text);
		text = algorithm.EncodeHuffman(text);
		return text;
	}

	public override string Decode(string text)
	{
		text = algorithm.DecodeHuffman(text);
		text = algorithm.MTFDetransform(text);
		text = algorithm.BWTDetransform(text);
		return text;
	}
}


class BWT_MTF_Arithmetic : Compressor
{
	public override string Encode(string text)
	{
		text = algorithm.BWTTransform(text);
		text = algorithm.MTFTransform(text);
		text = algorithm.EncodeArithmetic(text);
		return text;
	}

	public override string Decode(string text)
	{
		text = algorithm.DecodeArithmetic(text);
		text = algorithm.MTFDetransform(text);
		text = algorithm.BWTDetransform(text);
		return text;
	}
}


class RLE_BWT_MTF_RLE_Huffman : Compressor
{
	public override string Encode(string text)
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

	public override string Decode(string text)
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
	public override string Encode(string text)
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

	public override string Decode(string text)
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
