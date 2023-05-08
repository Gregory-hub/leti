namespace lab3;

class Program
{
    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string path = Directory.GetCurrentDirectory() + "\\text.txt";
        StreamReader sr = new StreamReader(path);
        string text = sr.ReadToEnd().Substring(0);
        // string text = "Aboba";

        string encoded;
        string decoded;
        // Console.WriteLine($"Text: {text}");
        Console.WriteLine($"Length: {text.Length * 8} bits ({text.Length} bytes)");
        Console.WriteLine();

        // Console.WriteLine($"BWT transformed: {algorithm.BWTTransform(text)}");
        // Console.WriteLine($"BWT transformed and detransformed: {algorithm.BWTDetransform(algorithm.BWTTransform(text))}");

        // Console.WriteLine($"MTF transformed: {algorithm.MTFTransform(text)}");
        // Console.WriteLine($"MTF transformed and detransformed: {algorithm.MTFDetransform(algorithm.MTFTransform(text))}");

        // Console.WriteLine($"RLE encoded: {algorithm.EncodeRLE(text)}");
        // Console.WriteLine($"RLE encoded and decoded: {algorithm.DecodeRLE(algorithm.EncodeRLE(text))}");

        // Console.WriteLine($"LZ78 encoded: {algorithm.EncodeLZ78(text)}");
        // Console.WriteLine($"LZ78 encoded and decoded: {algorithm.DecodeLZ78(algorithm.EncodeLZ78(text))}");

        encoded = algorithm.EncodeHuffman(text);
        decoded = algorithm.DecodeHuffman(encoded);

        Console.WriteLine("HUFFMAN");
        Console.WriteLine($"Text decoded correctly: {decoded == text}");
        // Console.WriteLine($"Huffman encoded: {encoded}");
        // Console.WriteLine($"Huffman encoded and decoded: {decoded}");
        Console.WriteLine($"Length encoded: {encoded.Length}");
        Console.WriteLine($"Comression ratio: {(double)text.Length * 8 / (double)(encoded.Length)}");
        Console.WriteLine();

        // encoded = algorithm.EncodeArithmetic(text);
        // decoded = algorithm.DecodeArithmetic(encoded);

        // Console.WriteLine("ARITHMETIC");
        // Console.WriteLine($"Text decoded correctly: {decoded == text}");
        // // Console.WriteLine($"Arithmetic encoded: {encoded}");
        // // Console.WriteLine($"Arithmetic encoded and decoded: {decoded}");
        // Console.WriteLine($"Length encoded: {encoded.Length}");
        // Console.WriteLine($"Comression ratio: {(double)text.Length * 8 / (double)(encoded.Length)}");
        // Console.WriteLine();

        // int order = 2;
        // encoded = algorithm.EncodePPM(text, order);
        // decoded = algorithm.DecodePPM(encoded, order);

        // Console.WriteLine("PPM");
        // Console.WriteLine($"Text decoded correctly: {decoded == text}");
        // // Console.WriteLine($"PPM encoded: {encoded}");
        // // Console.WriteLine($"PPM encoded and decoded: {decoded}");
        // Console.WriteLine($"Length encoded: {encoded.Length}");
        // Console.WriteLine($"Comression ratio: {(double)text.Length * 8 / (double)(encoded.Length)}");
        // Console.WriteLine();

        Console.Read();
    }
}
