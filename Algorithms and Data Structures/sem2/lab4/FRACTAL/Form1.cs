using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            string img_name = "kitcat";
            const int BlockSize = 64;    // block size sould be about 16

            string compressed_name = img_name + "_compressed";
            //string path = Environment.CurrentDirectory + "\\images\\" + img_name + ".jpg";
            string path = Path.Combine(Environment.CurrentDirectory, "images", img_name + ".jpg");

            string path_compressed = Path.Combine(Environment.CurrentDirectory, "images", compressed_name);

            Bitmap img = new Bitmap(path);

            BlockCompression.Compress(img, path_compressed, "color", BlockSize);

            BlockCompression.Decompress(path_compressed);

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Image = img;

            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            pictureBox2.Image = new Bitmap(path_compressed + ".jpg");

        }
    }
}

