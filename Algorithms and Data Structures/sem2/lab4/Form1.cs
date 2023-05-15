using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
			string img_name = "img";
			string path = Environment.CurrentDirectory + "//" + img_name + ".jpg";

			Bitmap img = new Bitmap(path);

			const int Quality = 50;
			JPEGCompressor.Encoder encoder = new JPEGCompressor.Encoder();
			encoder.Compress(img, img_name + "_compressed.bin", Quality);

			pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			pictureBox1.Image = img;

			path = Environment.CurrentDirectory + "//" + img_name + "_compressed.bin";
			JPEGCompressor.Decoder decoder = new JPEGCompressor.Decoder();
			Bitmap img_decoded = decoder.Decompress(path);
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox2.BorderStyle = BorderStyle.FixedSingle;
			pictureBox2.Image = img_decoded;
		}
	}
}
 