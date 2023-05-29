using System.Numerics;


namespace cursed;

class Program
{
    static void Main(string[] args)
    {
        User user1 = new User("Sussy Baka");
        User user2 = new User("Creper2021");

        string? message;
        BigInteger[] encoded;
        string decoded;

        Console.WriteLine("Messenger is ready to work. Type messages now\n");

        while (true)
        {
            Console.Write($"{user1.Name}: ");
            message = Console.ReadLine();
            if (message != "" && message is not null) 
            {
                encoded = user1.SendMessage(message, user2.publicKey);
                decoded = user2.RecieveMessage(encoded);
                Console.WriteLine($"\n{user2.Name} received: {decoded}");
            }
            Console.Write($"{user2.Name}: ");
            message = Console.ReadLine();
            if (message != "" && message is not null) 
            {
                encoded = user2.SendMessage(message, user1.publicKey);
                decoded = user1.RecieveMessage(encoded);
                Console.WriteLine($"\n{user1.Name} received: {decoded}");
            }
        }

    }
}
