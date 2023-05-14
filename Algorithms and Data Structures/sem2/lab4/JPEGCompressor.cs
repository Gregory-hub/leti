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

namespace lab4
{
	public class JPEGCompressor
	{
		private List<List<double>> Y;
		private List<List<double>> Cb;
		private List<List<double>> Cr;

		protected int[,] BaseQuantizationMatrix = {
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
		}

		protected class BinTreeNode
		{
			public BinTreeNode Parent = null;
			public BinTreeNode Left = null;
			public BinTreeNode Right = null;

			public string Value;
			public int Frequency;

			public BinTreeNode(string value, int frequency)
			{
				Value = value;
				Frequency = frequency;
			}
		}


		public Bitmap BitmapFromYCbCr(int width, int height, string show_param = "all")
		{
			Bitmap bitmap = new Bitmap(width, height);
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					switch (show_param)
					{
						case "all":
							bitmap.SetPixel(y, x, ColorFromYCbCr(Y[y][x], Cb[y][x], Cr[y][x]));
							break;
						case "Y":
							bitmap.SetPixel(y, x, Color.FromArgb((int)Y[y][x], (int)Y[y][x], (int)Y[y][x]));
							break;
						case "Cb":
							bitmap.SetPixel(y, x, Color.FromArgb(0, 0, (int)Cb[y][x]));
							break;
						case "Cr":
							bitmap.SetPixel(y, x, Color.FromArgb((int)Cr[y][x], 0, 0));
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
				Init(bitmap);
				if (metadata.Width == 0 || metadata.Height == 0) return;

				DowngradeChroma();
				ExtendToFitIntoBlocks(ref Y);
				ExtendToFitIntoBlocks(ref Cb);
				ExtendToFitIntoBlocks(ref Cr);

				if (Y.Count > metadata.Height) metadata.PixelsAddedBottom = Y.Count - metadata.Height;
				if (Y[0].Count > metadata.Width) metadata.PixelsAddedRight = Y[0].Count - metadata.Width;
				metadata.Height = Y.Count;
				metadata.Width = Y[0].Count;

				Bitmap encoded = BitmapFromYCbCr(metadata.Width / 2, metadata.Height / 2, "Y");

				PerformDCT(ref Y);
				PerformDCT(ref Cb);
				PerformDCT(ref Cr);

				quality = 80;
				Quantize(ref Y, quality);
				Quantize(ref Cb, quality);
				Quantize(ref Cr, quality);

				string text_encoded = Encode();
			}

			public void Init(Bitmap bitmap)
			{
				metadata.Width = bitmap.Width;
				metadata.Height = bitmap.Height;
				InitYCbCr(bitmap);
			}

			private void InitYCbCr(Bitmap bitmap)
			{
				Y = new List<List<double>>();
				Cb = new List<List<double>>();
				Cr = new List<List<double>>();

				for (int x = 0; x < bitmap.Width; x++)
				{
					Y.Add(new List<double>());
					Cb.Add(new List<double>());
					Cr.Add(new List<double>());

					for (int y = 0; y < bitmap.Height; y++)
					{
						double[] ycbcr = YCbCrFromColor(bitmap.GetPixel(x, y));
						Y[x].Add(ycbcr[0]);
						Cb[x].Add(ycbcr[1]);
						Cr[x].Add(ycbcr[2]);
					}
				}
			}

			public void ExtendToFitIntoBlocks(ref List<List<double>> component)
			{
				while (component.Count % 8 != 0)
				{
					component.Add(new List<double>());
					for (int x = 0; x < component[0].Count; x++)
					{
						component.Last().Add(component[component.Count - 2][x]);
					}
				}

				while (component[0].Count % 8 != 0)
				{
					for (int y = 0; y < component.Count; y++)
					{
						component[y].Add(component[y].Last());
					}
				}
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

			public void Quantize(ref List<List<double>> component, int quality)
			{
				if (quality <= 0 || quality > 100) throw new InvalidDataException("Quality must be a number from 1 to 100");
				int[,] q_matrix = BaseQuantizationMatrix;

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

				string text_encoded = EncodeRLE(coefficients);

				text_encoded = (char)metadata.Width + text_encoded;
				text_encoded = (char)metadata.Height + text_encoded;
				text_encoded = (char)metadata.PixelsAddedRight + text_encoded;
				text_encoded = (char)metadata.PixelsAddedBottom + text_encoded;

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
						if (data[i][j] == number || count < Int16.MaxValue - 1) count++;
						else
						{
							text_encoded += (char)count;
							text_encoded += (char)number;

							count = 1;
							number = data[i][j];
						}
					}
				}
				text_encoded += (char)count;
				text_encoded += (char)number;

				return text_encoded;
			}
		}


		public class Decoder : JPEGCompressor
		{
			public void Decompress(string path) {}
		}
	}
}

