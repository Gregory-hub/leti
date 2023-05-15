using System.Drawing;
using System.Collections.Generic;
using System;
using System.Security.Policy;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace lab4
{
	public class JPEGCompressor
	{
		private List<List<double>> Y;
		private List<List<double>> Cb;
		private List<List<double>> Cr;

		protected int[,] BaseChromaQuantizationMatrix = {
			{ 17, 18, 24, 47, 99, 99, 99, 99 },
			{ 18, 21, 26, 66, 99, 99, 99, 99 },
			{ 24, 26, 56, 99, 99, 99, 99, 99 },
			{ 47, 66, 99, 99, 99, 99, 99, 99 },
			{ 99, 99, 99, 99, 99, 99, 99, 99 },
			{ 99, 99, 99, 99, 99, 99, 99, 99 },
			{ 99, 99, 99, 99, 99, 99, 99, 99 },
			{ 99, 99, 99, 99, 99, 99, 99, 99 },
		};

		protected int[,] BaseLuminanceQuantizationMatrix = {
			{ 16,  11,  10,  16,  24,  40,  51,  61  },
			{ 12,  12,  14,  19,  26,  58,  60,  55  },
			{ 14,  13,  16,  24,  40,  57,  69,  56  },
			{ 14,  17,  22,  29,  51,  87,  80,  62  },
			{ 18,  22,  37,  56,  68,  109, 103, 77  },
			{ 24,  35,  55,  64,  81,  104, 113, 92  },
			{ 49,  64,  78,  87,  103, 121, 120, 101 },
			{ 72,  92,  95,  98,  112, 100, 103, 99  }
		};


		protected struct Metadata
		{
			public int Width { get; set; }
			public int Height { get; set; }
			public int PixelsAddedBottom { get; set; }
			public int PixelsAddedRight { get; set; }
			public int Quality { get; set; }
		}


		public Bitmap BitmapFromYCbCr(int width, int height, string show_param = "All")
		{
			int chroma_ratio = 1;
			if (Y.Count - Cb.Count > 15) chroma_ratio = 2;

			Bitmap bitmap = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					switch (show_param.ToLower())
					{
						case "all":
							bitmap.SetPixel(x, y, ColorFromYCbCr(Y[y][x], Cb[y / chroma_ratio][x / chroma_ratio], Cr[y / chroma_ratio][x / chroma_ratio]));
							break;
						case "y":
							bitmap.SetPixel(x, y, Color.FromArgb((int)Y[y][x], (int)Y[y][x], (int)Y[y][x]));
							break;
						case "cb":
							bitmap.SetPixel(x, y, Color.FromArgb(0, 0, (int)Cb[y / chroma_ratio][x / chroma_ratio]));
							break;
						case "cr":
							bitmap.SetPixel(x, y, Color.FromArgb((int)Cr[y / chroma_ratio][x / chroma_ratio], 0, 0));
							break;
						default:
							throw new InvalidDataException("Invalid param");
					}
				}
			}
			return bitmap;
		}

		public Color ColorFromYCbCr(double y, double cb, double cr)
		{
			int R = (int)(y + 1.402 * (cr - 128));
			int G = (int)(y - 0.34414 * (cb - 128) - 0.71414 * (cr - 128));
			int B = (int)(y + 1.772 * (cb - 128));

			R = (R < 256) ? R : 255;
			G = (G < 256) ? G : 255;
			B = (B < 256) ? B : 255;

			R = (R > 0) ? R : 0;
			G = (G > 0) ? G : 0;
			B = (B > 0) ? B : 0;

			return Color.FromArgb(R, G, B);
		}

		public double[] YCbCrFromColor(Color px)
		{
			double[] ycbcr = new double[3];
			ycbcr[0] = 0.299 * px.R + 0.587 * px.G + 0.114 * px.B;
			ycbcr[1] = 128 - 0.168736 * px.R - 0.331264 * px.G + 0.5 * px.B;
			ycbcr[2] = 128 + 0.5 * px.R - 0.418688 * px.G - 0.081312 * px.B;
			return ycbcr;
		}


		public class Encoder : JPEGCompressor
		{
			private Metadata metadata = new Metadata();

			public void Compress(Bitmap bitmap, string out_filename, int quality = 50)
			{
				Init(bitmap, quality);
				if (metadata.Width == 0 || metadata.Height == 0) WriteToFile("", out_filename);

				ExtendToFitIntoBlocks();

				//DowngradeChroma();

				PerformDCT(ref Y);
				PerformDCT(ref Cb);
				PerformDCT(ref Cr);

				Quantize(ref Y, quality, BaseLuminanceQuantizationMatrix);
				Quantize(ref Cb, quality, BaseChromaQuantizationMatrix);
				Quantize(ref Cr, quality, BaseChromaQuantizationMatrix);

				string text_encoded = Encode();
				WriteToFile(text_encoded, out_filename);
			}

			public void Init(Bitmap bitmap, int quality)
			{
				metadata.Width = bitmap.Width;
				metadata.Height = bitmap.Height;
				metadata.Quality = quality;
				InitYCbCr(bitmap);
			}

			private void InitYCbCr(Bitmap bitmap)
			{
				Y = new List<List<double>>();
				Cb = new List<List<double>>();
				Cr = new List<List<double>>();

				for (int y = 0; y < bitmap.Height; y++)
				{
					Y.Add(new List<double>());
					Cb.Add(new List<double>());
					Cr.Add(new List<double>());

					for (int x = 0; x < bitmap.Width; x++)
					{
						double[] ycbcr = YCbCrFromColor(bitmap.GetPixel(x, y));
						Y[y].Add(ycbcr[0]);
						Cb[y].Add(ycbcr[1]);
						Cr[y].Add(ycbcr[2]);
					}
				}
			}

			public void ExtendToFitIntoBlocks()
			{
				if (Y.Count != Cb.Count && Y.Count != Cr.Count) throw new InvalidDataException("Components must be of one size");
				while (Y.Count % 16 != 0)	// 16 because chroma components width and height will be divided by 2
				{
					Y.Add(new List<double>());
					Cb.Add(new List<double>());
					Cr.Add(new List<double>());
					for (int x = 0; x < Y[0].Count; x++)
					{
						Y.Last().Add(Y[Y.Count - 2][x]);
						Cb.Last().Add(Cb[Y.Count - 2][x]);
						Cr.Last().Add(Cr[Y.Count - 2][x]);
					}
				}

				while (Y[0].Count % 16 != 0)
				{
					for (int y = 0; y < Y.Count; y++)
					{
						Y[y].Add(Y[y].Last());
						Cb[y].Add(Cb[y].Last());
						Cr[y].Add(Cr[y].Last());
					}
				}

				if (Y.Count > metadata.Height) metadata.PixelsAddedBottom = Y.Count - metadata.Height;
				if (Y[0].Count > metadata.Width) metadata.PixelsAddedRight = Y[0].Count - metadata.Width;
				metadata.Height = Y.Count;
				metadata.Width = Y[0].Count;
			}

			public void DowngradeChroma()
			{
				List<List<double>> NewCb = new List<List<double>>();
				List<List<double>> NewCr = new List<List<double>>();

				double[] current_pixels = new double[4];

				for (int x = 0; x < metadata.Height; x += 2)
				{
					NewCb.Add(new List<double>());
					NewCr.Add(new List<double>());

					for (int y = 0; y < metadata.Width; y += 2)
					{
						current_pixels[0] = Cb[x][y];
						current_pixels[1] = Cb[x][y + 1];
						current_pixels[2] = Cb[x + 1][y];
						current_pixels[3] = Cb[x + 1][y + 1];

						NewCb[x / 2].Add(current_pixels.Average());

						current_pixels[0] = Cr[x][y];
						current_pixels[1] = Cr[x][y + 1];
						current_pixels[2] = Cr[x + 1][y];
						current_pixels[3] = Cr[x + 1][y + 1];

						NewCr[x / 2].Add(current_pixels.Average());
					}
				}
				Cb = NewCb;
				Cr = NewCr;
			}

			public void PerformDCT(ref List<List<double>> component)
			{
				for (int y_start = 0; y_start < component.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < component[y_start].Count; x_start += 8)
					{
						PerformBlockDCT(ref component, y_start, x_start);
					}
				}
			}

			private void PerformBlockDCT(ref List<List<double>> component, int y_start, int x_start)
			{
				for (int y = y_start; y < y_start + 8; y++)
					for (int x = x_start; x < x_start + 8; x++) component[y][x] -= 128;

				double[,] transformed = new double[8, 8];
				for (int u = 0; u < 8; u++)
				{
					for (int v = 0; v < 8; v++)
					{
						transformed[u, v] = 0;
						for (int y = y_start; y < y_start + 8; y++)
						{
							for (int x = x_start; x < x_start + 8; x++)
								transformed[u, v] += component[y][x] * Math.Cos(((2 * x + 1) * u * Math.PI) / 16) * Math.Cos(((2 * y + 1) * v * Math.PI) / 16);
						}
						if (u == 0) transformed[u, v] *= 1 / Math.Sqrt(2);
						if (v == 0) transformed[u, v] *= 1 / Math.Sqrt(2);
						transformed[u, v] /= 4;
					}
				}
				for (int y = y_start; y < y_start + 8; y++)
					for (int x = x_start; x < x_start + 8; x++) component[y][x] = transformed[y - y_start, x - x_start];
			}

			public void Quantize(ref List<List<double>> component, int quality, int[,] base_quantization_matrix)
			{
				if (quality <= 0 || quality > 100) throw new InvalidDataException("Quality must be a number from 1 to 100");
				int[,] q_matrix = base_quantization_matrix;

				int s;
				if (quality < 50) s = 5000 / quality;
				else s = 200 - quality * 2;

				for (int y = 0; y < 8; y++)
				{
					for (int x = 0; x < 8; x++)
					{
						q_matrix[y,x] = (q_matrix[y,x] * s + 50) / 100;
						if (q_matrix[y, x] == 0) q_matrix[y, x] = 1;
					}
				}

				for (int y_start = 0; y_start < component.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < component[y_start].Count; x_start += 8)
					{
						QuantizeBlock(ref component, y_start, x_start, q_matrix);
					}
				}
			}

			private void QuantizeBlock(ref List<List<double>> component, int y_start, int x_start, int[,] q_matrix)
			{
				for (int y = y_start; y < y_start + 8; y++)
				{
					for (int x = x_start; x < x_start + 8; x++)
					{
						component[y][x] /= q_matrix[y - y_start, x - x_start];
						component[y][x] = (int)component[y][x];
					}
				}
			}

			public string Encode()
			{
				List<int[]> coefficients = GetZigzagedCoefficients(Y);
				coefficients.AddRange(GetZigzagedCoefficients(Cb));
				coefficients.AddRange(GetZigzagedCoefficients(Cr));

				Arithmetic.Encoder arithmetic = new Arithmetic.Encoder();

				string text_encoded = "" + (char)metadata.Width + (char)metadata.Height + (char)metadata.Quality + (char)metadata.PixelsAddedRight + (char)metadata.PixelsAddedBottom;
				text_encoded += EncodeRLE(coefficients);

				text_encoded = arithmetic.Encode(text_encoded);

				return text_encoded;
			}

			private List<int[]> GetZigzagedCoefficients(List<List<double>> component)
			{
				List<int[]> coefficients = new List<int[]>();

				for (int y_start = 0; y_start < component.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < component[y_start].Count; x_start += 8)
					{
						coefficients.Add(GetZigzagedBlockCoefficients(component, y_start, x_start));
					}
				}
				return coefficients;
			}
			
			private int[] GetZigzagedBlockCoefficients(List<List<double>> component, int y_start, int x_start)
			{
				int[] coefficients = new int[64];
				int x = 0, y = 0;
				int x_up = 0, y_up = 0;
				int current_index = 0;

				while (!(x_up >= 7 && y_up >= 7))
				{
					x = x_up;
					y = y_up;
					while (x >= 0 && y < 8)
					{
						coefficients[current_index] = (int)component[y_start + y][x_start + x];
						x--;
						y++;
						current_index++;
					}

					if (x_up < 7) x_up += 1;
					else y_up += 1;
				}

				return coefficients;
			}

			private string EncodeRLE(List<int[]> data)
			{
				if (data.Count == 0) return "";

				string text_encoded = "";
				int number = data[0][0];
				int count = 0;

				for (int i = 0; i < data.Count; i++)
				{
					for (int j = 0; j < data[i].Length; j++)
					{
						if (data[i][j] == number && count < Int16.MaxValue - 1) count++;
						else
						{
							if (number + 1024 < 0) throw new InvalidDataException("damn, it was too low");
							text_encoded += (char)count;
							text_encoded += (char)(number + 1024);

							count = 1;
							number = data[i][j];
						}
					}
				}
				text_encoded += (char)count;
				text_encoded += (char)(number + 1024);

				return text_encoded;
			}

			public void WriteToFile(string text, string filename)
			{
				string path = System.Environment.CurrentDirectory + "\\" + filename;
				using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
				{
					writer.Write(text.Length);
					while (text.Length % 8 != 0) text += '0';
					byte b;
					for (int i = 0; i < text.Length; i += 8)
					{
						b = Convert.ToByte(text.Substring(i, 8), 2);
						writer.Write(b);
					}
				}
			}
		}


		public class Decoder : JPEGCompressor
		{
			private Metadata metadata = new Metadata();

			public Bitmap Decompress(string path)
			{
				Bitmap bitmap = null;
				string text_encoded = ReadFromfile(path);

				Decode(text_encoded);

				Unquantize(ref Y, metadata.Quality, BaseLuminanceQuantizationMatrix);
				Unquantize(ref Cb, metadata.Quality, BaseChromaQuantizationMatrix);
				Unquantize(ref Cr, metadata.Quality, BaseChromaQuantizationMatrix);

				UnperformDCT(ref Y);
				UnperformDCT(ref Cb);
				UnperformDCT(ref Cr);

				ClipY();

				bitmap = BitmapFromYCbCr(metadata.Width, metadata.Height, "All");
				//bitmap = BitmapFromYCbCr(metadata.Width, metadata.Height, "Y");
				return bitmap;
			}

			public string ReadFromfile(string path)
			{
				string text = "";
				int len;
				string sym;
				using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
				{
					len = reader.ReadInt32();
					while (reader.BaseStream.Position < reader.BaseStream.Length)
					{
						sym = Convert.ToString(reader.ReadByte(), 2);
						while (sym.Length < 8) sym = '0' + sym;
						text += sym;
					}
				}
				return text.Substring(0, len);
			}

			public void Decode(string text_encoded)
			{
				Arithmetic.Decoder decoder = new Arithmetic.Decoder();
				text_encoded = decoder.Decode(text_encoded);

				metadata.Width = (int)text_encoded[0];
				metadata.Height = (int)text_encoded[1];
				metadata.Quality = (int)text_encoded[2];
				metadata.PixelsAddedRight = (int)text_encoded[3];
				metadata.PixelsAddedBottom = (int)text_encoded[4];
				text_encoded = text_encoded.Substring(5);

				List<int[]> coefficients = DecodeRLE(text_encoded);
				CoefficientsToYCbCr(coefficients);
			}

			private List<int[]> DecodeRLE(string text)
			{
				List<int[]> coefficients = new List<int[]>();
				if (text.Length == 0) return coefficients;

				int count;
				int number;
				int[] block = new int[64];
				int block_index = 0;
				for (int i = 0; i < text.Length; i += 2)
				{
					count = (int)text[i];
					number = (int)text[i + 1] - 1024;

					for (int j = 0; j < count; j++)
					{
						block[block_index] = number;
						block_index++;
						if (block_index == 64)
						{
							coefficients.Add(block);
							block = new int[64];
							block_index = 0;
						}
					}
				}

				return coefficients;
			}

			private void CoefficientsToYCbCr(List<int[]> coefficients)
			{
				Y = new List<List<double>>();
				Cb = new List<List<double>>();
				Cr = new List<List<double>>();

				for (int y = 0; y < metadata.Height; y++)
				{
					Y.Add(new List<double>());
					for (int x = 0; x < metadata.Width; x++) Y[y].Add(0);
				}
				for (int y = 0; y < metadata.Height / 2; y++)
				{
					Cb.Add(new List<double>());
					for (int x = 0; x < metadata.Width / 2; x++) Cb[y].Add(0);
				}
				for (int y = 0; y < metadata.Height / 2; y++)
				{
					Cr.Add(new List<double>());
					for (int x = 0; x < metadata.Width / 2; x++) Cr[y].Add(0);
				}

				for (int y_start = 0; y_start < Y.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < Y[y_start].Count; x_start += 8)
					{
						UnzigzagBlock(coefficients[(y_start * Y.Count / 8 + x_start) / 8], y_start, x_start, ref Y);
					}
				}
				for (int y_start = 0; y_start < Cb.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < Cb[y_start].Count; x_start += 8)
					{
						UnzigzagBlock(coefficients[(y_start * Cb.Count / 8 + x_start) / 8], y_start, x_start, ref Cb);
					}
				}
				for (int y_start = 0; y_start < Cr.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < Cr[y_start].Count; x_start += 8)
					{
						UnzigzagBlock(coefficients[(y_start * Cr.Count / 8 + x_start) / 8], y_start, x_start, ref Cr);
					}
				}
			}

			private List<double> UnzigzagBlock(int[] block, int y_start, int x_start, ref List<List<double>> component)
			{
				List<double> unzigzaged = new List<double>();
				int x = 0, y = 0;
				int x_up = 0, y_up = 0;
				int current_index = 0;

				while (!(x_up >= 7 && y_up >= 7))
				{
					x = x_up;
					y = y_up;
					while (x >= 0 && y < 8)
					{
						component[y_start + y][x_start + x] = block[current_index];
						x--;
						y++;
						current_index++;
					}

					if (x_up < 7) x_up += 1;
					else y_up += 1;
				}

				return unzigzaged;
			}

			public void Unquantize(ref List<List<double>> component, int quality, int[,] base_quantization_matrix)
			{
				if (quality <= 0 || quality > 100) throw new InvalidDataException("Quality must be a number from 1 to 100");
				int[,] q_matrix = base_quantization_matrix;

				int s;
				if (quality < 50) s = 5000 / quality;
				else s = 200 - quality * 2;

				for (int y = 0; y < 8; y++)
				{
					for (int x = 0; x < 8; x++)
					{
						q_matrix[y,x] = (q_matrix[y,x] * s + 50) / 100;
					}
				}

				for (int y_start = 0; y_start < component.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < component[y_start].Count; x_start += 8)
					{
						UnquantizeBlock(ref component, y_start, x_start, q_matrix);
					}
				}
			}

			private void UnquantizeBlock(ref List<List<double>> component, int y_start, int x_start, int[,] q_matrix)
			{
				for (int y = y_start; y < y_start + 8; y++)
				{
					for (int x = x_start; x < x_start + 8; x++)
					{
						component[y][x] *= q_matrix[y - y_start, x - x_start];
					}
				}
			}

			public void UnperformDCT(ref List<List<double>> component)
			{
				for (int y_start = 0; y_start < component.Count; y_start += 8)
				{
					for (int x_start = 0; x_start < component[y_start].Count; x_start += 8)
					{
						UnperformBlockDCT(ref component, y_start, x_start);
					}
				}
			}

			private void UnperformBlockDCT(ref List<List<double>> component, int y_start, int x_start)
			{
				double[,] detransformed = new double[8, 8];
				double value;
				for (int y = 0; y < 8; y++)
				{
					for (int x = 0; x < 8; x++)
					{
						detransformed[y, x] = 0;
						for (int u = 0; u < 8; u++)
						{
							for (int v =  0; v < 8; v++)
							{
								value = component[y_start + u][x_start + v] * Math.Cos(((2 * x + 1) * u * Math.PI) / 16) * Math.Cos(((2 * y + 1) * v * Math.PI) / 16);
								if (u == 0) value *= 1 / Math.Sqrt(2);
								if (v == 0) value *= 1 / Math.Sqrt(2);
								detransformed[y, x] += value;
							}
						}
						detransformed[y, x] /= 4;
					}
				}

				for (int y = y_start; y < y_start + 8; y++)
					for (int x = x_start; x < x_start + 8; x++)
					{
						component[y][x] = detransformed[y - y_start, x - x_start] + 128;
						if (component[y][x] < 0) component[y][x] = 0;
						if (component[y][x] > 255) component[y][x] = 255;
					}
			}

			public void ClipY()
			{
				while (Y.Count > metadata.Height - metadata.PixelsAddedBottom) Y.RemoveAt(Y.Count - 1);

				while (Y[0].Count > metadata.Width - metadata.PixelsAddedRight)
				{
					for (int y = 0; y < Y.Count; y++)
					{
						Y[y].RemoveAt(Y[y].Count - 1);
					}
				}

				metadata.Height = Y.Count;
				metadata.Width = Y[0].Count;
				metadata.PixelsAddedRight = 0;
				metadata.PixelsAddedBottom = 0;
			}
		}

	}
}
 