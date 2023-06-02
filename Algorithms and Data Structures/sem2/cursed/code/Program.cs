using System.Diagnostics;
using System.Numerics;
using System.Text;


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

    static long TestRSAPerfomance(short key_length)
    {
        PrimeFinder pm = new PrimeFinder();
        BigInteger data128bit = pm.GetRandomBigInt(128);

        BigInteger encoded;
        User.PublicKey public_key = new User.PublicKey();
        User.PrivateKey private_key = new User.PrivateKey();
        RSA.InitRSA(ref public_key, ref private_key, key_length);

        int count = 5;
        long sum = 0;

        for (int i = 0; i < count; i++)
        {
            Stopwatch watch = Stopwatch.StartNew();

            encoded = RSA.Encrypt(data128bit, public_key);
            RSA.Decrypt(encoded, private_key);

            watch.Stop();
            sum += watch.ElapsedMilliseconds;
        }
        return sum / count;
    }

    static long TestAESPerfomance(int text_len)
    {
        Random rand = new Random();

        byte[,] encoded;
        BigInteger key = AES.GenerateKey();
        StringBuilder text = new StringBuilder(text_len);
        for (int i = 0; i < text_len; i++) text.Append((char)rand.NextInt64((int)Math.Pow(2, 16) - 2));

        Stopwatch watch = Stopwatch.StartNew();

        encoded = AES.Encrypt(text.ToString(), key);
        AES.Decrypt(encoded, key);

        watch.Stop();
        return watch.ElapsedMilliseconds;
    }

    static void RunTests()
    {
    	string path = Environment.CurrentDirectory + "\\rsa.txt";
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write("RSA:");
        }

        long time;
        using (StreamWriter sw = File.AppendText(path))
        {
            for (short key = 128; key <= 2048; key *= 2)
            {
                time = TestRSAPerfomance(key);
                Console.WriteLine($"Average time for key_length = {key}: {time} ms");
                sw.Write($" {time}");
            }
        }

    	path = Environment.CurrentDirectory + "\\aes.txt";
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write("AES:");
        }

        using (StreamWriter sw = File.AppendText(path))
        {
            for (short len = 0; len <= 10000; len++)
            {
                time = TestAESPerfomance(len);
                Console.WriteLine($"Average time for text len = {len}: {time} ms");
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

        // StartMessenger(user1, user2);
        RunTests();
    }
}
