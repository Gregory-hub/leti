using System.Numerics;


namespace cursed;
class Program
{
    static void ExchangeAESKeys(ref User user1, ref User user2)
    {
        user1.ReceiveAESKey(user2.AESEncryptKey);
        user2.ReceiveAESKey(user1.AESEncryptKey);
    }

    static bool OK(User user1, User user2)
    {
        try
        {
            // byte[,] encoded = user1.SendMessage("Test", user2.publicKey);
            byte[,] encoded = user1.SendMessage("t", user2.publicKey);
            string decoded = user2.RecieveMessage(encoded);
            encoded = user2.SendMessage("t", user1.publicKey);
            decoded = user1.RecieveMessage(encoded);
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
        byte[,] encoded;
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
        ExchangeAESKeys(ref user1, ref user2);
        while (!OK(user1, user2))
        {
            user1 = new User(name1, key_length);
            user2 = new User(name2, key_length);
            ExchangeAESKeys(ref user1, ref user2);
        }

        StartMessenger(user1, user2);
    }
}
