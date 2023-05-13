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
			string path = Environment.CurrentDirectory + "//img.jpg";

			Bitmap img = new Bitmap(path);

			const int Quality = 50;

			JPEGCompressor.Encoder encoder = new JPEGCompressor.Encoder();
			Bitmap encoded_img = encoder.Compress(img, Quality);

			pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			pictureBox1.Image = img;

			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox2.BorderStyle = BorderStyle.FixedSingle;
			pictureBox2.Image = encoded_img;
		}
	}
}

