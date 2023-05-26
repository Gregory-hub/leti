namespace cursed;

class Program
{
    static void Main(string[] args)
    {
        Message message = new Message("Biba");

		PrimeFinder prime_finder = new PrimeFinder();
        ulong[] primes = prime_finder.FindRSAPrimes(100000000, 1000000);
        Console.WriteLine($"{primes[0]}, {primes[1]}");

        Console.ReadLine();
    }
}
