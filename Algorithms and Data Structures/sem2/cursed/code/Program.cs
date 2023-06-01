using System.Diagnostics;
using System.Numerics;


namespace cursed;
class Program
{
    static bool TryExchangeAESKeys(ref User user1, ref User user2)
    {
        BigInteger key_encrypted;
        try
        {
            key_encrypted = user1.SendAESKey(user2.publicKey);
            user2.ReceiveAESKey(key_encrypted);

            key_encrypted = user2.SendAESKey(user1.publicKey);
            user1.ReceiveAESKey(key_encrypted);
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
                encoded = user1.SendMessage(message);
                decoded = user2.RecieveMessage(encoded);
                Console.WriteLine($"\n{user2.Name} received: {decoded}");
            }
            Console.Write($"{user2.Name}: ");
            message = Console.ReadLine();
            if (message != "" && message is not null) 
            {
                encoded = user2.SendMessage(message);
                decoded = user1.RecieveMessage(encoded);
                Console.WriteLine($"\n{user1.Name} received: {decoded}");
            }
        }
    }

    static int TestPerfomance(short key_length)
    {
        string name1 = "Mike";
        string name2 = "Waltuh";

        // User user1 = new User(name1, key_length);
        // User user2 = new User(name2, key_length);
        // ExchangeAESKeys(ref user1, ref user2);
        // while (!OK(user1, user2))
        // {
        //     user1 = new User(name1, key_length);
        //     user2 = new User(name2, key_length);
        //     ExchangeAESKeys(ref user1, ref user2);
        // }

        // string? message = "test";
        // // BigInteger[] encoded;
        // byte[,] encoded;
        // // string decoded;
        int count = 5;
        long sum = 0;

        // for (int i = 0; i < count; i++)
        {
            // BigInteger[] rsa_encoded = RSA.Encode(message, user2.publicKey);

            // Stopwatch watch = Stopwatch.StartNew();
            // encoded = AES.Encode(rsa_encoded, key_length, user2.AESEncryptKey);
            // AES.Decode(encoded, key_length, user2.AESDecryptKey);

            // encoded = RSA.Encode(message, user2.publicKey);
            // decoded = RSA.Decode(encoded, user2.privateKey);

            // encoded = user1.SendMessage(message, user2.publicKey);
            // decoded = user2.RecieveMessage(encoded);

            // watch.Stop();
            // sum += watch.ElapsedMilliseconds;
        }
        return (int)(sum / count);
    }

    static void RunTests()
    {
    	string path = Environment.CurrentDirectory + "\\data.txt";
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write("AES:");
        }	

        int time;
        using (StreamWriter sw = File.AppendText(path))
        {
            for (short key = 128; key <= 2048; key *= 2)
            {
                time = TestPerfomance(key);
                Console.WriteLine($"Average time for key_length = {key}: {time} ms");
                sw.Write($" {time}");
            }
        }	
        Console.ReadLine();
    }

    static void Main(string[] args)
    {
        short key_length = 1024;

        string name1 = "Mike";
        string name2 = "Waltuh";

        User user1 = new User(name1, key_length);
        User user2 = new User(name2, key_length);
        bool ok = TryExchangeAESKeys(ref user1, ref user2);
        while (!ok)
        {
            user1 = new User(name1, key_length);
            user2 = new User(name2, key_length);
            ok = TryExchangeAESKeys(ref user1, ref user2);
        }

        StartMessenger(user1, user2);
    }
}
