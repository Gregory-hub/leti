namespace lab3;
class Program
{
    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string text = "hamud habibi hamud";

        Console.WriteLine($"Text: {text}");
        Console.WriteLine($"RLE encoded: {algorithm.EncodeRLE(text)}");
        Console.WriteLine($"RLE encoded and decoded: {algorithm.DecodeRLE(algorithm.EncodeRLE(text))}");
        Console.WriteLine($"Huffman encoded: {algorithm.EncodeHuffman(text, out Dictionary<char, string> codes)}");
        foreach (var code in codes) Console.WriteLine($"    '{code.Key}': {code.Value}");
        Console.WriteLine($"Huffman encoded and decoded: {algorithm.DecodeHuffman(algorithm.EncodeHuffman(text, out codes), codes)}");
        Console.WriteLine($"LZ78 encoded: {algorithm.EncodeLZ78(text)}");
        Console.WriteLine($"LZ78 encoded and decoded: {algorithm.DecodeLZ78(algorithm.EncodeLZ78(text))}");
        Console.WriteLine($"BWT transformed: {algorithm.BWTTransform(text)}");
        Console.WriteLine($"BWT transformed and detransformed: {algorithm.BWTDetransform(algorithm.BWTTransform(text))}");
        Console.Read();
    }
}
