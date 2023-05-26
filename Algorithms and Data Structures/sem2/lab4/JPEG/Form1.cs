using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			string img_name = "dock";
			const int Quality = 15;

			string compressed_name = img_name + "_compressed.bin";
			string path = Environment.CurrentDirectory + "\\images\\" + img_name + ".jpg";
			string path_compressed = Environment.CurrentDirectory + "\\images\\" + compressed_name;

			Bitmap img = new Bitmap(path);

            JPEGCompressor.Encoder encoder = new JPEGCompressor.Encoder();
			JPEGCompressor.Decoder decoder = new JPEGCompressor.Decoder();

            //System.Diagnostics.Stopwatch watch = new Stopwatch();

			//watch.Start();
			encoder.Compress(img, path_compressed, Quality);
			//watch.Stop();

			//System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\images\\data.txt", watch.Elapsed.ToString());

			Bitmap img_decoded = decoder.Decompress(path_compressed);

			//encoder.Compress(img_decoded, compressed_name, Quality);
			//img_decoded = decoder.Decompress(path_compressed);

			pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			pictureBox1.Image = img;

			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox2.BorderStyle = BorderStyle.FixedSingle;
			pictureBox2.Image = img_decoded;
		}
	}
}

