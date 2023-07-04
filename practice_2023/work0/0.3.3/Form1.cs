using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0._3._3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String text = String.Format("{0}", textBox1.Text);
            Brush brush = new SolidBrush(Color.Black);
            Graphics G = pictureBox1.CreateGraphics();
            G.Clear(Color.White);
            G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            G.DrawString(text, Font, brush, 0, 0); // Координаты размещения текста
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Font = new System.Drawing.Font("Times New Roman", 12, FontStyle.Bold);
        }
    }
}
