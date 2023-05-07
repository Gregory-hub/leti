namespace lab3;
class Program
{
    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string path = Directory.GetCurrentDirectory() + "\\text.txt";
        StreamReader sr = new StreamReader(path);
        string text = sr.ReadToEnd();
        // string text = "ba";

        string encoded;
        string decoded;
        string symbols;
        // Console.WriteLine($"Text: {text}");
        Console.WriteLine($"Length: {text.Length * 8} bits ({text.Length} bytes)");

        // Console.WriteLine($"BWT transformed: {algorithm.BWTTransform(text)}");
        // Console.WriteLine($"BWT transformed and detransformed: {algorithm.BWTDetransform(algorithm.BWTTransform(text))}");

        // Console.WriteLine($"MTF transformed: {algorithm.MTFTransform(text)}");
        // Console.WriteLine($"MTF transformed and detransformed: {algorithm.MTFDetransform(algorithm.MTFTransform(text))}");

        // Console.WriteLine($"RLE encoded: {algorithm.EncodeRLE(text)}");
        // Console.WriteLine($"RLE encoded and decoded: {algorithm.DecodeRLE(algorithm.EncodeRLE(text))}");

        // Console.WriteLine($"LZ78 encoded: {algorithm.EncodeLZ78(text)}");
        // Console.WriteLine($"LZ78 encoded and decoded: {algorithm.DecodeLZ78(algorithm.EncodeLZ78(text))}");

        // Console.WriteLine($"Huffman encoded: {algorithm.EncodeHuffman(text, out Dictionary<char, string> codes)}");
        // Console.WriteLine($"Huffman encoded and decoded: {algorithm.DecodeHuffman(algorithm.EncodeHuffman(text, out codes), codes)}");
        // // foreach (var code in codes) Console.WriteLine($"    '{code.Key}': {code.Value}");

        // encoded = algorithm.EncodeArithmetic(text, out symbols);
        // decoded = algorithm.DecodeArithmetic(encoded, symbols);

        // Console.WriteLine($"Text decoded correctly: {decoded == text}");
        // // Console.WriteLine($"Arithmetic encoded: {encoded}");
        // // Console.WriteLine($"Arithmetic encoded and decoded: {decoded}");
        // Console.WriteLine($"Length encoded: {encoded.Length + (symbols.Length) * 8}");
        // Console.WriteLine($"Comression ratio: {(double)text.Length * 8 / (double)(encoded.Length + (symbols.Length) * 8)}");
        // Console.WriteLine();

        int order = 2;
        encoded = algorithm.EncodePPM(text, out symbols, order);
        decoded = algorithm.DecodePPM(encoded, symbols, order);

        Console.WriteLine($"Text decoded correctly: {decoded == text}");
        // Console.WriteLine($"PPM encoded: {encoded}");
        // Console.WriteLine($"PPM encoded and decoded: {decoded}");
        Console.WriteLine($"Length encoded: {encoded.Length + (symbols.Length) * 8}");
        Console.WriteLine($"Comression ratio: {(double)text.Length * 8 / (double)(encoded.Length + (symbols.Length) * 8)}");
        Console.WriteLine();

        Console.Read();
    }
}
