namespace example
{
    class Example
    {
        private int Value;
        public Example(int value)
        {
            Value = value;
        }

        public static int operator- (Example a, Example b)
        {
            return b.Value - a.Value;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Example a = new Example(10);
            Example b = new Example(4);
            Console.WriteLine(a - b);
        }
    }
}