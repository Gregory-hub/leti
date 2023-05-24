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
            //System.Diagnostics.Stopwatch comSw = new Stopwatch();
            //System.Diagnostics.Stopwatch decSw = new Stopwatch();

            int block_size = 80;
            System.Console.WriteLine("Compression...");
            BlockCompression.Compress("waltuh.jpg", block_size);

            System.Console.WriteLine("Decompression...");
            BlockCompression.Decompress();

            System.Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}

