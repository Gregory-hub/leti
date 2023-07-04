using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0._3._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Фотогалерея";
            label1.Text = "";
            comboBox1.Text = "Список";
            comboBox1.Items.Add("haram");
            comboBox1.Items.Add("lamp");
            comboBox1.Items.Add("amogus");
            comboBox1.Items.Add("bishop");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string img_dir = Environment.CurrentDirectory + "\\img\\";
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    pictureBox1.Image = Image.FromFile(img_dir + "haram.jpg");
                    label1.Text = "Haram"; break;
                case 1:
                    pictureBox1.Image = Image.FromFile(img_dir + "lamp.jpg");
                    label1.Text = "Lamp"; break;
                case 2:
                    pictureBox1.Image = Image.FromFile(img_dir + "amogus.jpg");
                    label1.Text = "Amogus"; break;
                case 3:
                    pictureBox1.Image = Image.FromFile(img_dir + "bishop.jpg");
                    label1.Text = "Bishop"; break;
            }
        }
    }
}

