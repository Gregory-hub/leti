using System.Drawing;
using System.Collections.Generic;
using System;
using System.Security.Policy;
using System.IO;
using System.Runtime.CompilerServices;

namespace lab4
{
	public class JPEGCompressor
	{
		private List<List<double>> Y;
		private List<List<double>> Cb;
		private List<List<double>> Cr;

		public Bitmap BitmapFromYCbCr(int width, int height, string show_param = "all")
		{
			Bitmap bitmap = new Bitmap(width, height);
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					switch (show_param)
					{
						case "all":
							bitmap.SetPixel(x, y, ColorFromYCbCr(Y[x][y], Cb[x][y], Cr[x][y]));
							break;
						case "Y":
							bitmap.SetPixel(x, y, Color.FromArgb((int)Y[x][y], (int)Y[x][y], (int)Y[x][y]));
							break;
						case "Cb":
								bitmap.SetPixel(x, y, Color.FromArgb(0, 0, (int)Cb[x][y]));
							break;
						case "Cr":
								bitmap.SetPixel(x, y, Color.FromArgb((int)Cr[x][y], 0, 0));
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
			public Bitmap Compress(Bitmap bitmap)
			{
				InitYCbCr(bitmap);

				Bitmap encoded = BitmapFromYCbCr(bitmap.Width, bitmap.Height, "Y");
				return encoded;
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
		}


		public class Decoder : JPEGCompressor
		{
			public void Decompress(string path) {}
		}
	}
}

