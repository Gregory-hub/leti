namespace lab1;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.Title = "Titulus";
        Console.ForegroundColor = ConsoleColor.Cyan;
        while (true) {
            string word = Console.ReadLine();
            if (word == "cls") Console.Clear();
            if (word == "exit") break;
            Console.WriteLine(word);
        }
    }
}
