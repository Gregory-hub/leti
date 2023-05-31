using System.Numerics;


namespace cursed;
class Program
{
    static bool OK(User user1, User user2)
    {
        try
        {
            BigInteger[] encoded = user1.SendMessage("test", user2.publicKey);
            string decoded = user2.RecieveMessage(encoded);
        }
        catch (System.OverflowException)    // means that generated primes are not prime
        {
            return false;
        }
        return true;
    }

    static void StartMessenger(User user1, User user2)
    {
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

    static void Main(string[] args)
    {
        short key_length = 1024;

        string name1 = "Mike";
        string name2 = "Waltuh";

        User user1 = new User(name1, key_length);
        User user2 = new User(name2, key_length);
        while (!OK(user1, user2))
        {
            user1 = new User(name1, key_length);
            user2 = new User(name2, key_length);
        }

        StartMessenger(user1, user2);
    }
}
