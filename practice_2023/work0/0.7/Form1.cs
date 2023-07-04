using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0._7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double InitT = 0, LastT = 6.28; // оборот в 360 градусов (6,28 радиан)
            double Step = 0.02, angle = InitT;
            double x, y, x1, y1;
            int cX = 120, cY = 120; // центр большой окружности
            int R2 = 90; // радиус большой окружности
            int k = Convert.ToInt32(textBox1.Text); // число областей на траектории
            int R1 = (int)(R2 / k); // радиус меньшей (движущейся) окружности
            int i = 0; // количество точек прорисовки
            int delay = 10;
            Point[] p = new Point[(int)((LastT - InitT) / Step) + 1]; // точки для прорисовки (LastT/Step)

            while (angle <= LastT)
            {
                x = R1 * (k - 1) * (Math.Cos(angle) + Math.Cos((k - 1) * angle) / (k - 1));
                y = R1 * (k - 1) * (Math.Sin(angle) - Math.Sin((k - 1) * angle) / (k - 1));
                p[i] = new Point(cX + (int)x, cY + (int)y); // расчет очередной точки траектории
                PaintGraphic(cX, cY, R2, (int)x, (int)y, p);
                x1 = (R2 - R1) * Math.Sin(angle + 1.57);
                y1 = (R2 - R1) * Math.Cos(angle + 1.57);
                PaintCircle(cX, cY, (int)x1, (int)y1, R1, (int)x, (int)y);
                angle += Step;
                System.Threading.Thread.Sleep(delay); //время приостановки прорисовки
                i++;
            }
        }

        private void PaintGraphic(int cX, int cY, int r2, int x, int y, Point[] p)
        {
            Graphics Графика = pictureBox1.CreateGraphics();
            Графика.Clear(BackColor);
            PaintCircle(cX, cY, 0, 0, r2, x, y);
            Графика.DrawLines(Pens.Red, p); // траектория
        }

        private void PaintCircle(int cX, int cY, int centX, int centY, int radius, int x, int y)
        {
            Graphics Графика = pictureBox1.CreateGraphics();
            Графика.DrawEllipse(Pens.Black, centX + cX - radius, cY - radius - centY, radius * 2, radius * 2);
            Графика.DrawLine(Pens.Black, centX + cX, cY - centY, cX + x, cY + y);
            // прорисовка радиуса большей окружности
        }
    }
}
