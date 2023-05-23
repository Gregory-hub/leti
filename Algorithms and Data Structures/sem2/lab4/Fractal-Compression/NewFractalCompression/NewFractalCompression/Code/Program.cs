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

            System.Console.WriteLine("Compression...");
            BlockCompression.Compress("kitcat.jpg", "Color", 16);    // block size is about 16

            System.Console.WriteLine("Decompression...");
            BlockCompression.Decompress();

            System.Console.WriteLine("Done");

            Console.ReadKey();
        }
    }
}

