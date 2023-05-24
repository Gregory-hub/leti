﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NewFractalCompression.Code
{
    abstract class AbstractCompression
    {
        protected static int range_num_width, range_num_height;
        protected static int domain_num_width, domain_num_height;
        protected static int range_block_size, domain_block_size, colorflag;
        protected static int count, block_all_num;
        protected static Object.Block[,] rangeArray;
        protected static Object.Block[,] domainArray;
        protected static Object.Coefficients[,,] compressCoeff;

        public static List<Object.Coefficients> listCoeff;
        public static double[,] brightnessImage;
        public static Color[,] classImageColor;
        protected static BinaryWriter bw;
        protected static BlockTree[,] rangeTree;
        protected static Object.BlockArray[] domainBlocks;

        public static int numLock = 0;
        public static int blockChecker = 0;
        public static double u = 0.75;

        public static void PrintCoefficients(Object.Coefficients fac)
        {
            System.Console.Write(fac.Depth);
            System.Console.Write(" ");
            System.Console.Write(fac.X);
            System.Console.Write(" ");
            System.Console.Write(fac.Y);
            System.Console.Write(" ");
            System.Console.WriteLine();
        }

        public static Object.Block CreateBlockRange(int x, int y, Color[,] imageColor, int range_block_size, double[,] brightness)
        {
            Object.Block block = new Object.Block
            {
                X = x,
                Y = y,
                SumR = 0,
                SumG = 0,
                SumB = 0,
                Px = 0,
                Py = 0,
                BlockSize = range_block_size,
                Active = false
            };
            double blockBrightness = 0;
            for (int i = 0; i < range_block_size; ++i)
            {
                for (int j = 0; j < range_block_size; ++j)
                {
                    block.SumR += imageColor[block.X + i, block.Y + j].R;
                    block.SumG += imageColor[block.X + i, block.Y + j].G;
                    block.SumB += imageColor[block.X + i, block.Y + j].B;

                    block.SumR2 += (block.SumR * block.SumR);
                    block.SumG2 += (block.SumG * block.SumG);
                    block.SumB2 += (block.SumB * block.SumB);

                    blockBrightness += brightness[block.X + i, block.Y + j];
                    block.Px += i * brightness[block.X + i, block.Y + j];
                    block.Py += j * brightness[block.X + i, block.Y + j];
                }
            }
            block.Px = (block.Px / blockBrightness) - ((range_block_size + 1) / 2);
            block.Py = (block.Py / blockBrightness) - ((range_block_size + 1) / 2);
            return block;
        }

        public static Object.Block CreateBlockDomain(int x, int y, Color[,] imageColor, int range_block_size, double[,] brightness)
        {
            Object.Block block = new Object.Block
            {
                X = x,
                Y = y,
                SumR = 0,
                SumG = 0,
                SumB = 0,
                Px = 0,
                Py = 0,
                Active = false
            };
            double blockBrightness = 0;
            for (int i = 0; i < range_block_size; ++i)
            {
                for (int j = 0; j < range_block_size; ++j)
                {
                    block.SumR += imageColor[block.X + i * 2, block.Y + j * 2].R;
                    block.SumG += imageColor[block.X + i * 2, block.Y + j * 2].G;
                    block.SumB += imageColor[block.X + i * 2, block.Y + j * 2].B;

                    block.SumR2 += (block.SumR * block.SumR);
                    block.SumG2 += (block.SumG * block.SumG);
                    block.SumB2 += (block.SumB * block.SumB);

                    blockBrightness += brightness[block.X + i * 2, block.Y + j * 2];
                    block.Px += i * brightness[block.X + i * 2, block.Y + j * 2];
                    block.Py += j * brightness[block.X + i * 2, block.Y + j * 2];
                }
            }
            block.Px = (block.Px / blockBrightness) - ((range_block_size + 1) / 2);
            block.Py = (block.Py / blockBrightness) - ((range_block_size + 1) / 2);
            return block;
        }

        public static Bitmap GetImage(string path)
        {
            Bitmap image = new Bitmap(path);
            int rgb;
            Color c;
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                {
                    c = image.GetPixel(x, y);
                    rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                    image.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            return image;
        }

        // angle between 2 centers of mass
        protected static double Angle(Object.Block RangeBlock, Object.Block DomainBlock)
        {
            double angle = 0;
            double vec1X = RangeBlock.Px;
            double vec1Y = RangeBlock.Py;
            double vec2X = DomainBlock.Px;
            double vec2Y = DomainBlock.Py;

            double sum1 = Math.Sqrt(vec1X * vec1X + vec1Y * vec1Y);
            double sum2 = Math.Sqrt(vec2X * vec2X + vec2Y * vec2Y);
            double scalar = vec1X * vec2X + vec1Y * vec2Y;
            angle = Math.Acos(scalar / (sum1 * sum2));
            //return (angle * 57.2958);
            return (angle);
        }

        // brightness shift between 2 blocks
        public static double Shift(Object.Block rangeBlock, Object.Block domainBlock, int range_block_size, int flag)
        {
            double shift = 0;
            if (flag == 0)
                shift = ((rangeBlock.SumR) - (u * domainBlock.SumR)) / (range_block_size * range_block_size);
            if (flag == 1)
                shift = ((rangeBlock.SumG) - (u * domainBlock.SumG)) / (range_block_size * range_block_size);
            if (flag == 2)
                shift = ((rangeBlock.SumB) - (u * domainBlock.SumB)) / (range_block_size * range_block_size);
            return shift;
        }
    }
}