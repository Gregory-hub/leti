using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0._5._4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Закрашивание фигур";
            label1.Text = "Выберите фигуру";
            comboBox1.Text = "Фигуры";
            string[] figures = { "Прямоугольник", "Эллипс", "Окружность" };
            comboBox1.Items.AddRange(figures);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Graphics gr = pictureBox1.CreateGraphics();
            Brush fill = new SolidBrush(Color.Orange);
            gr.Clear(SystemColors.Control);
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    gr.FillRectangle(fill, 60, 60, 120, 180); break;
                case 1:
                    gr.FillEllipse(fill, 60, 60, 120, 180); break;
                case 2:
                    gr.FillEllipse(fill, 60, 60, 120, 120); break;
            }
        }
    }
}

