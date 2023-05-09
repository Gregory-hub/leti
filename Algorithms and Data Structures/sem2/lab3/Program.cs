namespace lab3;

class Program
{
    static void PrintInfo(string text, int text_size, string encoded, string decoded, string algorithm_name)
    {
        Console.WriteLine(algorithm_name.ToUpper());
        Console.WriteLine($"Text decoded correctly: {decoded == text}");
        // Console.WriteLine($"{algorithm_name} encoded: {encoded}");
        // Console.WriteLine($"{algorithm_name} encoded and decoded: {decoded}");
        // Console.WriteLine($"Length encoded: {encoded.Length}");
        Console.WriteLine($"Comression ratio: {(double)text_size / (double)(encoded.Length)}");
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string path = Directory.GetCurrentDirectory() + "\\enwik8";
        StreamReader sr = new StreamReader(path);

        long file_size = new System.IO.FileInfo(path).Length;
        // int text_size = (int)file_size / 100;

        int len = 5000;
        int text_size = len * 16;
        string text = sr.ReadToEnd().Substring(0, len);

        // string text = "AAboba Biba";
        // text_size = text.Length * 16;

        string encoded;
        string decoded;

        // Console.WriteLine($"Text: {text}");
        Console.WriteLine($"Size: {text_size} bytes");
        Console.WriteLine();

        encoded = algorithm.EncodeHuffman(text);
        decoded = algorithm.DecodeHuffman(encoded);
        PrintInfo(text, text_size, encoded, decoded, "Huffman");

        encoded = algorithm.EncodeArithmetic(text);
        decoded = algorithm.DecodeArithmetic(encoded);
        PrintInfo(text, text_size, encoded, decoded, "Arithmetic");

        encoded = algorithm.EncodeLZ78(text);
        decoded = algorithm.DecodeLZ78(encoded);
        PrintInfo(text, text_size, encoded, decoded, "LZ78");

		BWT_MTF_Huffman BMH = new BWT_MTF_Huffman();
        encoded = BMH.Encode(text);
        decoded = BMH.Decode(encoded);
        PrintInfo(text, text_size, encoded, decoded, "BWT_MTF_Huffman");

		BWT_MTF_Arithmetic BMA = new BWT_MTF_Arithmetic();
        encoded = BMA.Encode(text);
        decoded = BMA.Decode(encoded);
        PrintInfo(text, text_size, encoded, decoded, "BWT_MTF_Arithmetic");

		RLE_BWT_MTF_RLE_Huffman RBMRH = new RLE_BWT_MTF_RLE_Huffman();
        encoded = RBMRH.Encode(text);
        decoded = RBMRH.Decode(encoded);
        PrintInfo(text, text_size, encoded, decoded, "RLE_BWT_MTF_RLE_Huffman");

		RLE_BWT_MTF_RLE_Arithmetic RBMRA = new RLE_BWT_MTF_RLE_Arithmetic();
        encoded = RBMRA.Encode(text);
        decoded = RBMRA.Decode(encoded);
        PrintInfo(text, text_size, encoded, decoded, "RLE_BWT_MTF_RLE_Arithmetic");

        int order = 2;
        encoded = algorithm.EncodePPM(text, order);
        decoded = algorithm.DecodePPM(encoded, order);
        PrintInfo(text, text_size, encoded, decoded, "PPM");

        Console.Read();
    }
}
