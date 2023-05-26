using System;
using NewFractalCompression;
using System.Diagnostics;
using System.IO;

namespace NewFractalCompression.Code
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch watch = new Stopwatch();

            int block_size = 16;
            System.Console.WriteLine("Compression...");

            // watch.Start();
            BlockCompression.Compress("dock.jpg", block_size);
            // watch.Stop();

            System.Console.WriteLine("Decompression...");
            BlockCompression.Decompress();

            System.Console.WriteLine("Done");
            //System.Console.WriteLine(watch.Elapsed);

            Console.ReadLine();
            Console.ReadLine();
        }
    }
}

