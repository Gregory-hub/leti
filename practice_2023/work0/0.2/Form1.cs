using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0._2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Форма приветствия";
            label1.Text = "Name: ";
            label2.Text = "Напишите ваше имя.";
            button1.Text = "Ввод";
            toolTip1.SetToolTip(textBox1, "Введите\nваше имя");
            toolTip1.IsBalloon = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Здравствуй, " + textBox1.Text + "!", "Приветствие");
        }
    }
}

