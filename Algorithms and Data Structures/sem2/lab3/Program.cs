namespace lab3;

using System.Diagnostics;


class Program
{
    static void AnalyseCompressor(Compressor compressor, string name, string text)
    {
        Console.WriteLine(name.ToUpper());
		Stopwatch watch = new Stopwatch();

        watch.Start();
        string encoded = compressor.Encode(text);
        watch.Stop();
        Console.WriteLine($"Size encoded: {((double)encoded.Length / 1024 / 1024):N3}Mb");
        Console.WriteLine($"Comression ratio: {((double)text.Length * 16 / (double)(encoded.Length)):N3}");
		Console.WriteLine($"Compression time: {(double)watch.ElapsedMilliseconds / 1000}s");

		watch.Start();
		string decoded = compressor.Decode(encoded);
		watch.Stop();
		Console.WriteLine($"Decompression time: {(double)watch.ElapsedMilliseconds / 1000}s");
		if (decoded != text) Console.WriteLine("ERROR: decompressed incorrectly");
		Console.WriteLine();
    }

    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string path = Directory.GetCurrentDirectory() + "\\enwik8";
        StreamReader sr = new StreamReader(path);

        long file_size = new System.IO.FileInfo(path).Length;

        // int text_size = (int)file_size / 1000;
        // int len = text_size / 16;

        // string text = "A\n";
        // int len = text.Length;
        int len = 100;
        int text_size = len * 16;

        string text = sr.ReadToEnd().Substring(0, len);

        Console.WriteLine($"Text size: {(double)text_size / 1024 / 1024:N3}Mb\n");

        AnalyseCompressor(new HuffmanCompressor(), "Huffman", text);
        AnalyseCompressor(new ArithmeticCompressor(), "Arithmetic", text);
        AnalyseCompressor(new LZ78Compressor(), "LZ78", text);
        AnalyseCompressor(new BWT_MTF_Huffman(), "BWT_MTF_Huffman", text);
        AnalyseCompressor(new BWT_MTF_Arithmetic(), "BWT_MTF_Arithmetic", text);
        AnalyseCompressor(new RLE_BWT_MTF_RLE_Huffman(), "RLE_BWT_MTF_RLE_Huffman", text);
        AnalyseCompressor(new RLE_BWT_MTF_RLE_Arithmetic(), "RLE_BWT_MTF_RLE_Arithmetic", text);

        for (int order = 2; order <= 2; order++)
        {
            PPMCompressor ppm = new PPMCompressor(order);
            AnalyseCompressor(ppm, $"PPM order {order}", text);
        }

        Console.Read();
    }
}
