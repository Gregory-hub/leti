using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace work1
{
    public partial class Form1 : Form
    {
        private int Shift_x;
        private int Shift_y;
        private int Scale_x;
        private int Scale_y;
        private Color Line_color;
        private int Pen_width;

        private int Left_limit;
        private int Right_limit;
        private double Step;
        private Point[] Points;

        private bool Sin_activated = false;
        private bool Hex_activated = false;

        private Point Hex_center;
        private int Hex_radius;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Sinus";
            pictureBox1.BackColor = Color.White;
            button1.Text = "Draw sin";
            button2.Text = "Draw rectangle";

            // default values
            // sin
            Line_color = Color.Black;
            Pen_width = 1;

            Left_limit = (int)(-2 * Math.PI);
            Right_limit = (int)(2 * Math.PI);
            Step = 0.1;
            Points = new Point[(int)((Right_limit - Left_limit) / Step + 1)];

            Shift_x = pictureBox1.Width / 2;
            Shift_y = pictureBox1.Height / 2;
            Scale_x = pictureBox1.Width / (Right_limit - Left_limit) * 95 / 100;
            Scale_y = 50;

            // hexagon
            Hex_center = new Point(0, 0);
            Hex_radius = 20;
        }

        private double F(double x)
        {
            return Math.Sin(x);
        }

        private void ResetCoefficients()
        {
            Shift_x = pictureBox1.Width / 2;
            Shift_y = pictureBox1.Height / 2;
            Scale_x = pictureBox1.Width / (Right_limit - Left_limit) * 95 / 100;
        }

        private void DrawSin()
        {
            ResetCoefficients();

            int i = 0;
            for (double x = Left_limit; x <= Right_limit; x += Step)
            {
                Points[i] = new Point((int)(x * Scale_x) + Shift_x, (int)(-F(x) * Scale_y) + Shift_y);
                i++;
            }

            Pen pen = new Pen(Line_color, Pen_width);
            Graphics gr = pictureBox1.CreateGraphics();
            gr.DrawLines(pen, Points);
        }

        private void DrawHexagon()
        {
            ResetCoefficients();

            Point[] points = new Point[7];
            for (int i = 0; i < 6; i++)
                points[i] = new Point(Hex_center.X + (int)(Hex_radius * (Math.Cos(Math.PI * i / 3))) + Shift_x, Hex_center.Y + (int)(Hex_radius * (Math.Sin(Math.PI * i / 3))) + Shift_y);
            points[6] = points[0];

            Pen pen = new Pen(Line_color, Pen_width);
            Graphics gr = pictureBox1.CreateGraphics();
            gr.DrawLines(pen, points);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawSin();
            Sin_activated = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawHexagon();
            Hex_activated = true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (!Sin_activated && !Hex_activated) return;

            Graphics gr = pictureBox1.CreateGraphics();
            gr.Clear(Color.White);
            if (Sin_activated) DrawSin();
            if (Hex_activated) DrawHexagon();
        }
    }
}

