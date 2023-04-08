namespace lab3;
class Program
{
    static void Main(string[] args)
    {
        Algorithm algorithm = new Algorithm();

        string text = "1e2r1e3t1r";

        Console.WriteLine(algorithm.decode_RLE(text));
        Console.Read();
    }
}
