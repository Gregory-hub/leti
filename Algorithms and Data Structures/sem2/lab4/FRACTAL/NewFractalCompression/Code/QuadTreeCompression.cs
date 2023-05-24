using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NewFractalCompression.Code
{
    class QuadTreeCompression : AbstractCompression
    {
        private static int imageWidth, imageHeigth;
        public static bool CheckMonotoneBlock(Object.Block block)
        {
            int count = 0;
            Color currentColor = classImageColor[block.X, block.Y];
            for (int i = 0; i < block.BlockSize - 1; ++i)
            {
                for (int j = 1; j < block.BlockSize - 1; ++j)
                {
                    if (classImageColor[block.X + i, block.Y + j] != classImageColor[block.X + i + 1, block.Y + j + 1])
                    {
                        ++count;
                    }
                }
            }
            return (count < ((block.BlockSize)));
        }

        public static void Compression(string filename, string quality)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "images", filename);
            if (!(File.Exists(path)))
            {
                System.Console.WriteLine("File does not exist");
                return;
            }
            if (quality == "Black")
            {
                colorflag = 1;
            }
            else
            {
                if (quality == "Color")
                {
                    colorflag = 3;
                }
                else
                {
                    System.Console.WriteLine("Wrong mode");
                    return;
                }
            }
            Bitmap image = new Bitmap(path);
            imageWidth = image.Width;
            imageHeigth = image.Height;
            classImageColor = new Color[image.Width, image.Height];
            for (int i = 0; i < image.Width; ++i)
            {
                for (int j = 0; j < image.Height; ++j)
                {
                    classImageColor[i, j] = image.GetPixel(i, j);
                }
            }
            brightnessImage = new double[image.Width, image.Height];
            for (int i = 0; i < image.Width; ++i)
            {
                for (int j = 0; j < image.Height; ++j)
                {
                    brightnessImage[i, j] = image.GetPixel(i, j).GetBrightness();
                }
            }
            range_block_size = 64;
            range_num_width = image.Width / range_block_size;
            range_num_height = image.Height / range_block_size;
            rangeArray = new Object.Block[range_num_width, range_num_height];
            for (int i = 0; i < range_num_width; ++i)
            {
                for (int j = 0; j < range_num_height; ++j)
                {
                    rangeArray[i, j] = CreateBlockRange(i * range_block_size, j * range_block_size, classImageColor, range_block_size, brightnessImage);
                }
            }
            rangeTree = new BlockTree[range_num_width, range_num_height];
            for (int i = 0; i < range_num_width; ++i)
            {
                for (int j = 0; j < range_num_height; ++j)
                {
                    numLock = 1;
                    rangeTree[i, j] = new BlockTree(rangeArray[i, j], rangeArray[i, j].BlockSize);
                }
            }
            domain_num_width = range_num_width - 1;
            domain_num_height = range_num_height - 1;
            domain_block_size = range_block_size * 2;
            domainArray = new Object.Block[domain_num_width, domain_num_height];
            domainBlocks = new Object.BlockArray[PostProcessing.GetPow(domain_block_size)];
            for (int i = 0; i < domainBlocks.Length; ++i)
            {
                if (i == 0)
                    domainBlocks[i].BlockSize = domain_block_size;
                else
                    domainBlocks[i].BlockSize = domainBlocks[i - 1].BlockSize / 2;
                domainBlocks[i].num_width = image.Width / domainBlocks[i].BlockSize;
                domainBlocks[i].num_height = image.Height / domainBlocks[i].BlockSize;
                domainBlocks[i].Blocks = new Object.Block[domainBlocks[i].num_width, domainBlocks[i].num_height];
                for (int j = 0; j < domainBlocks[i].num_width; ++j)
                {
                    for (int k = 0; k < domainBlocks[i].num_height; ++k)
                    {
                        domainBlocks[i].Blocks[j, k] = CreateBlockDomain(j * domainBlocks[i].BlockSize / 2, k * domainBlocks[i].BlockSize / 2, classImageColor, domainBlocks[i].BlockSize / 2, brightnessImage);
                    }
                }
            }
            listCoeff = new List<Object.Coefficients>();
            for (int i = 0; i < range_num_width; ++i)
            {
                for (int j = 0; j < range_num_height; ++j)
                {
                    blockChecker = 0;
                    rangeTree[i, j].RoundTree(rangeTree[i, j], domainBlocks, classImageColor, rangeTree[i, j].mainBlock.BlockSize);
                }
            }

            bw = new BinaryWriter(File.Open(@"C:\Users\Admin\Documents\GitHub\Fractal-Compression\NewFractalCompression\NewFractalCompression\Quad Compression", FileMode.Create));
            bw.Write(MyConverter.Convert(BitConverter.GetBytes(image.Width), 2));
            bw.Write(MyConverter.Convert(BitConverter.GetBytes(image.Height), 2));
            bw.Write(MyConverter.Convert(BitConverter.GetBytes(range_block_size), 1));
            bw.Write(MyConverter.Convert(BitConverter.GetBytes(listCoeff.Count), 4));
            //for (int i = 0; i < range_num_width; ++i)
            //{
            //    for (int j = 0; j < range_num_height; ++j)
            //    {
            //        rangeTree[i, j].PrintTree(rangeTree[i, j], 0);
            //    }
            //}
            for (int i = 0; i < listCoeff.Count; ++i)
            {
                Byte[] D = BitConverter.GetBytes(listCoeff[i].Depth);
                Byte[] X = BitConverter.GetBytes(listCoeff[i].X);
                Byte[] Y = BitConverter.GetBytes(listCoeff[i].Y);
                Byte[] SR = BitConverter.GetBytes(listCoeff[i].shiftR);
                Byte[] SG = BitConverter.GetBytes(listCoeff[i].shiftG);
                Byte[] SB = BitConverter.GetBytes(listCoeff[i].shiftB);
                bw.Write(MyConverter.Convert(D, 1));
                bw.Write(MyConverter.Convert(X, 1));
                bw.Write(MyConverter.Convert(Y, 1));
                bw.Write(MyConverter.Convert(SR, 2));
                bw.Write(MyConverter.Convert(SG, 2));
                bw.Write(MyConverter.Convert(SB, 2));
            }
            bw.Close();
        }

        static public void FakeDecompression()
        {
            Bitmap newImage = new Bitmap(imageWidth, imageHeigth);
            Color[,] newImageColor = new Color[newImage.Width, newImage.Height];
            for (int j = 0; j < newImage.Width; ++j)
            {
                for (int k = 0; k < newImage.Height; ++k)
                {
                    newImageColor[j, k] = newImage.GetPixel(j, k);
                }
            }
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < range_num_width; ++j)
                {
                    for (int k = 0; k < range_num_height; ++k)
                    {
                        rangeTree[j, k].DrawTree(rangeTree[j, k], domainBlocks, newImageColor);
                    }
                }
            }
            for (int j = 0; j < newImage.Width; ++j)
            {
                for (int k = 0; k < newImage.Height; ++k)
                {
                    newImage.SetPixel(j, k, newImageColor[j, k]);
                }
            }
            newImage.Save(Path.Combine(Environment.CurrentDirectory, "images", "decompressed.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        static public void Decompression()
        {
            System.Console.WriteLine("Decompression dont work");
            return;

            byte[] bytesFile = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "images", "compressed"));
            int fileCount = 0;
            //Коэффициент масштабирования
            int scale = 1;

            Byte[] tmp = MyConverter.ReadByte(bytesFile, 0, 2);
            int image_width = BitConverter.ToInt16(tmp, 0) * scale;
            //+
            fileCount += 2;

            tmp = MyConverter.ReadByte(bytesFile, 2, 4);
            int image_height = BitConverter.ToInt16(tmp, 0) * scale;
            //+
            fileCount += 2;

            tmp = MyConverter.ReadByte(bytesFile, 4, 5);
            int range_block_size = tmp[0] * scale;
            //+    
            fileCount += 1;

            tmp = MyConverter.ReadByte(bytesFile, 5, 9);
            fileCount += 4;
            int listSize = BitConverter.ToInt32(tmp, 0);
            Bitmap newImage = new Bitmap(image_width, image_height);
            Color[,] newImageColor = new Color[newImage.Width, newImage.Height];
            brightnessImage = new double[newImage.Width, newImage.Height];
            for (int i = 0; i < newImage.Width; ++i)
            {
                for (int j = 0; j < newImage.Height; ++j)
                {
                    brightnessImage[i, j] = newImage.GetPixel(i, j).GetBrightness();
                }
            }

            int range_num_width = newImage.Width / range_block_size;
            int range_num_height = newImage.Height / range_block_size;
            Object.Block[,] newRangeArray = new Object.Block[range_num_width, range_num_height];
            for (int i = 0; i < range_num_width; ++i)
            {
                for (int j = 0; j < range_num_height; ++j)
                {
                    newRangeArray[i, j].X = i * range_block_size;
                    newRangeArray[i, j].Y = j * range_block_size;
                }
            }
            rangeTree = new BlockTree[range_num_width, range_num_height];
            for (int i = 0; i < range_num_width; ++i)
            {
                for (int j = 0; j < range_num_height; ++j)
                {
                    numLock = 1;
                    rangeTree[i, j] = new BlockTree(newRangeArray[i, j], newRangeArray[i, j].BlockSize);
                }
            }
            domain_num_width = range_num_width - 1;
            domain_num_height = range_num_height - 1;
            domain_block_size = range_block_size * 2;
            domainArray = new Object.Block[domain_num_width, domain_num_height];
            //System.Console.WriteLine(DomainBlocks.Length);
            domainBlocks = new Object.BlockArray[PostProcessing.GetPow(domain_block_size)];
            for (int i = 0; i < domainBlocks.Length; ++i)
            {
                if (i == 0)
                    domainBlocks[i].BlockSize = domain_block_size;
                else
                    domainBlocks[i].BlockSize = domainBlocks[i - 1].BlockSize / 2;
                domainBlocks[i].num_width = newImage.Width / domainBlocks[i].BlockSize;
                domainBlocks[i].num_height = newImage.Height / domainBlocks[i].BlockSize;
                domainBlocks[i].Blocks = new Object.Block[domainBlocks[i].num_width, domainBlocks[i].num_height];
                for (int j = 0; j < domainBlocks[i].num_width; ++j)
                {
                    for (int k = 0; k < domainBlocks[i].num_height; ++k)
                    {
                        domainBlocks[i].Blocks[j, k] = CreateBlockDomain(j * domainBlocks[i].BlockSize / 2, k * domainBlocks[i].BlockSize / 2, newImageColor, domainBlocks[i].BlockSize / 2, brightnessImage);
                    }
                }
            }
            List<Object.Coefficients> listCoeff = new List<Object.Coefficients>();
            Object.Coefficients coeff = new Object.Coefficients();
            for (int i = 0; i < listSize; ++i)
            {
                tmp = MyConverter.ReadByte(bytesFile, fileCount, fileCount + 1);
                coeff.Depth = tmp[0];
                ++fileCount;

                tmp = MyConverter.ReadByte(bytesFile, fileCount, fileCount + 1);
                coeff.X = tmp[0];
                fileCount += 1;

                tmp = MyConverter.ReadByte(bytesFile, fileCount, fileCount + 1);
                coeff.Y = tmp[0];
                fileCount += 1;

                tmp = MyConverter.ReadByte(bytesFile, fileCount, fileCount + 2);
                coeff.shiftR = BitConverter.ToInt16(tmp, 0);
                fileCount += 2;

                tmp = MyConverter.ReadByte(bytesFile, fileCount, fileCount + 2);
                coeff.shiftG = BitConverter.ToInt16(tmp, 0);
                fileCount += 2;

                tmp = MyConverter.ReadByte(bytesFile, fileCount, fileCount + 2);
                coeff.shiftB = BitConverter.ToInt16(tmp, 0);
                fileCount += 2;
                listCoeff.Add(coeff);
            }
            int blI = 0, blJ = -1;
            for (int i = 0; i < listCoeff.Count; ++i)
            {
                PrintCoefficients(listCoeff[i]);
            }
            for (int i = 0; i < listCoeff.Count; ++i)
            {
                if (listCoeff[i].Depth < 0)
                {
                    ++blJ;
                    if (blJ == range_num_height)
                    {
                        ++blI;
                        blJ = 0;
                    }
                }
                rangeTree[blI, blJ].AddCoeff(rangeTree[blI, blJ], listCoeff[i]);
            }
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < range_num_width; ++j)
                {
                    for (int k = 0; k < range_num_height; ++k)
                    {
                        rangeTree[j, k].DrawTree(rangeTree[j, k], domainBlocks, newImageColor);
                    }
                }
            }
            for (int j = 0; j < newImage.Width; ++j)
            {
                for (int k = 0; k < newImage.Height; ++k)
                {
                    newImage.SetPixel(j, k, newImageColor[j, k]);
                }
            }
            newImage.Save(Path.Combine(Environment.CurrentDirectory, "images", "decompressed.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
