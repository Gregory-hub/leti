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
        // Console.WriteLine($"Comression ratio: {(double)text_size / (double)(encoded.Length)}");
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string path = Directory.GetCurrentDirectory() + "\\enwik8";
        StreamReader sr = new StreamReader(path);

        long file_size = new System.IO.FileInfo(path).Length;
        // int text_size = (int)file_size / 100;
        int len = 2500;
        int text_size = len * 16;

        string text = sr.ReadToEnd().Substring(0, len);
        // string text = "Aboba\nBiba";

        string encoded;
        string decoded;

        // Console.WriteLine($"Text: {text}");
        Console.WriteLine($"Size: {text_size} bytes");
        Console.WriteLine();

        encoded = algorithm.BWTTransform(text);
        decoded = algorithm.BWTDetransform(encoded);
        PrintInfo(text, text_size, encoded, decoded, "BWT");

        encoded = algorithm.MTFTransform(text);
        decoded = algorithm.MTFDetransform(encoded);
        PrintInfo(text, text_size, encoded, decoded, "MTF");

        encoded = algorithm.EncodeRLE(text);
        decoded = algorithm.DecodeRLE(encoded);
        PrintInfo(text, text_size, encoded, decoded, "RLE");

        encoded = algorithm.EncodeLZ78(text);
        decoded = algorithm.DecodeLZ78(encoded);
        PrintInfo(text, text_size, encoded, decoded, "LZ78");

        encoded = algorithm.EncodeHuffman(text);
        decoded = algorithm.DecodeHuffman(encoded);
        PrintInfo(text, text_size, encoded, decoded, "Huffman");

        encoded = algorithm.EncodeArithmetic(text);
        decoded = algorithm.DecodeArithmetic(encoded);
        PrintInfo(text, text_size, encoded, decoded, "Arithmetic");

        int order = 2;
        encoded = algorithm.EncodePPM(text, order);
        decoded = algorithm.DecodePPM(encoded, order);
        PrintInfo(text, text_size, encoded, decoded, "PPM");

        Console.Read();
    }
}
