using System.IO;
using System;


namespace lab5
{
    internal class Program
    {
        static void Summarize(char[] array)
        {
            int vowel = 0, consonant = 0, line = 0;
            foreach (char sym in array)
            {
                if (char.IsLetter(sym))
                {
                    if ("AEIOUaeiou".IndexOf(sym) != -1)
                    {
                        vowel++;
                        // myCharacter is a vowel
                    }
                    else {
                        consonant++;
                        // myCharacter is not a vowel
                    }
                }
                else if (sym == '\n') line++;
            }

            Console.WriteLine("Character: {0}", array.Length);
            Console.WriteLine("Vowes: {0}", vowel);
            Console.WriteLine("Consonant: {0}", consonant);
            Console.WriteLine("New line: {0}", line);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(args.Length);
            foreach (string arg in args) Console.WriteLine(arg);
            string filename = "";
            try {
                filename = args[0];
            }
            catch (IndexOutOfRangeException) {
                Console.WriteLine("Filename not provided");
                Environment.Exit(0);
            }
            // Console.WriteLine(Environment.CurrentDirectory + "\\" + filename);

            try {
                FileStream stream = new FileStream(filename, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                int len = (int)stream.Length;
                Console.WriteLine("Length: {0}", len);

                char[] contents = new char[len];
                for (int i = 0; i < len; i++) {
                    contents[i] = (char)reader.Read();
                }

    /*            Console.WriteLine();
                foreach (char sym in contents) {
                    Console.Write(sym);
                }
                Console.WriteLine("\n");
    */            
                Summarize(contents);
                stream.Close();
            }
            catch (FileNotFoundException) {
                Console.WriteLine($"File {filename} not found");
                Environment.Exit(0);
            }
        }
    }
}

