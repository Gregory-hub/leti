namespace lab3;
class Program
{
    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string text = "badcadbdd";

        Console.WriteLine($"Text: {text}");
        Console.WriteLine($"RLE encoded: {algorithm.EncodeRLE(text)}");
        Console.WriteLine($"RLE encoded and decoded: {algorithm.DecodeRLE(algorithm.EncodeRLE(text))}");
        Console.WriteLine($"Haffman encoded: {algorithm.EncodeHaffman(text, out Dictionary<char, string> codes)}");
        foreach (var code in codes) Console.WriteLine($"    {code.Key}: {code.Value}");
        // Console.WriteLine($"Haffman encoded and decoded: {algorithm.DecodeHaffman(algorithm.EncodeHaffman(text))}");
        Console.Read();
    }
}
