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

namespace work1
{
    public partial class Form1 : Form
    {
        private int Shift_x;
        private int Shift_y;
        private int Scale_x;
        private int Scale_y;

        // line
        private Color Line_color;
        private Color Current_line_color;
        private int Line_width;
        private int Current_line_width;
        private float[] Line_style;
        private float[] Current_line_style;

        private int Left_limit;
        private int Right_limit;
        private double Step;
        private Point[] Points;

        private bool Sin_activated = false;
        private bool Hex_activated = false;

        // hexagon
        private Point Hex_center;
        private Point Current_hex_center;
        private int Hex_radius;
        private int Current_hex_radius;
        private Color Hex_line_color;
        private int Hex_line_width;
        private int Current_hex_line_width;
        private float[] Hex_line_style;
        private float[] Current_hex_line_style;
        
        private int Speed;
        private double Move_step;
        private int Cycles;
        private bool Is_pulsating = false;
        private int Pulsation_speed;
        private int Min_size;
        private bool Is_rotating = false;
        private int Rotation_speed;
        private bool Counterclockwise = false;
        private double Angle;
        private double Current_angle;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // default values
            // sin
            Line_color = Color.Black;
            Line_width = 1;
            Line_style = null;

            Left_limit = (int)(-2 * Math.PI);
            Right_limit = (int)(2 * Math.PI);
            Step = 0.05;
            Points = new Point[(int)((Right_limit - Left_limit) / Step + 1)];

            ResetCoefficients();
            Scale_y = 50;

            // hexagon
            Hex_line_color = Color.Black;
            Hex_line_width = 1;

            Hex_radius = 20;
            Hex_center = new Point(Left_limit * Scale_x + Shift_x, (int)(-F(Left_limit) * Scale_y + Shift_y));
            Speed = 50;
            Move_step = 0.5;
            Cycles = 1;
            Pulsation_speed = 10;
            Min_size = (Hex_radius / 2);
            Rotation_speed = 10;
            Angle = 0;

            // form
            Text = "Sinus";
            pictureBox1.BackColor = Color.White;

            comboBox1.Text = "Black";
            comboBox1.Items.AddRange(new string[] { "Black", "Blue", "Red", "Cyan" });

            comboBox2.Text = "1";
            comboBox2.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });

            comboBox3.Text = "Solid";
            comboBox3.Items.AddRange(new string[] { "Solid", "Dashed", "Dotted" });

            comboBox4.Text = "Black";
            comboBox4.Items.AddRange(new string[] { "Black", "Blue", "Red", "Cyan" });

            comboBox5.Text = "1";
            comboBox5.Items.AddRange(new string[] { "1", "2", "3" });

            comboBox6.Text = "Solid";
            comboBox6.Items.AddRange(new string[] { "Solid", "Dashed", "Dotted" });

            comboBox7.Text = "Left";
            comboBox7.Items.AddRange(new string[] { "Left", "Right" });

            comboBox8.Text = "Clockwise";
            comboBox8.Items.AddRange(new string[] { "Clockwise", "Counterclockwise" });

            textBox1.Text = Hex_radius.ToString();
            textBox2.Text = Pulsation_speed.ToString();
            textBox3.Text = Min_size.ToString();
            textBox4.Text = Speed.ToString();
            textBox5.Text = Move_step.ToString();
            textBox6.Text = Cycles.ToString();
            textBox7.Text = Rotation_speed.ToString();
        }

        private double F(double x)
        {
            return Math.Sin(x);
        }

        private void ResetCoefficients()
        {
            Shift_x = pictureBox1.Width / 2;
            Shift_y = pictureBox1.Height / 2;
            Scale_x = pictureBox1.Width / (Right_limit - Left_limit) * 90 / 100;
        }

        private void DrawSin(Color line_color, int line_width, float[] line_style)
        {
            ResetCoefficients();

            int i = 0;
            for (double x = Left_limit; x <= Right_limit; x += Step)
            {
                Points[i] = new Point((int)(x * Scale_x) + Shift_x, (int)(-F(x) * Scale_y) + Shift_y);
                i++;
            }

            Points[Points.Length - 1] = Points[Points.Length - 2];

            Pen pen = new Pen(line_color, line_width);
            if (line_style != null) pen.DashPattern = line_style;
            Graphics gr = pictureBox1.CreateGraphics();
            gr.DrawLines(pen, Points);

            Current_line_color = line_color;
            Current_line_width = line_width;
            Current_line_style = line_style;
        }

        private void DeleteSin()
        {
            int i = 0;
            for (double x = Left_limit; x <= Right_limit; x += Step)
            {
                Points[i] = new Point((int)(x * Scale_x) + Shift_x, (int)(-F(x) * Scale_y) + Shift_y);
                i++;
            }

            Points[Points.Length - 1] = Points[Points.Length - 2];

            Pen pen = new Pen(pictureBox1.BackColor, Current_line_width);
            Graphics gr = pictureBox1.CreateGraphics();
            if (Current_line_style != null) pen.DashPattern = Current_line_style;
            gr.DrawLines(pen, Points);
            if (Hex_activated) DrawHexagon(Hex_line_color, Hex_line_width, Hex_line_style);
        }

        private void DrawHexagon(Color line_color, int line_width, float[] line_style)
        {
            ResetCoefficients();

            Point[] points = new Point[7];
            for (int i = 0; i < 6; i++)
                points[i] = new Point(Hex_center.X + (int)(Hex_radius * (Math.Cos(Math.PI * i / 3 + Angle))), Hex_center.Y + (int)(Hex_radius * (Math.Sin(Math.PI * i / 3 + Angle))));
            points[6] = points[0];

            Pen pen = new Pen(line_color, line_width);
            Graphics gr = pictureBox1.CreateGraphics();
            if (line_style != null) pen.DashPattern = line_style;
            gr.DrawLines(pen, points);

            Current_hex_line_width = line_width;
            Current_hex_line_style = line_style;
            Current_hex_radius = Hex_radius;
            Current_hex_center = Hex_center;
            Current_angle = Angle;
        }

        private void DeleteHexagon()
        {
            Point[] points = new Point[7];
            for (int i = 0; i < 6; i++)
                points[i] = new Point(Current_hex_center.X + (int)(Current_hex_radius * (Math.Cos(Math.PI * i / 3 + Current_angle))), Current_hex_center.Y + (int)(Current_hex_radius * (Math.Sin(Math.PI * i / 3 + Current_angle))));
            points[6] = points[0];

            Pen pen = new Pen(pictureBox1.BackColor, Current_hex_line_width);
            Graphics gr = pictureBox1.CreateGraphics();
            if (Current_hex_line_style != null) pen.DashPattern = Current_hex_line_style;
            gr.DrawLines(pen, points);
            if (Sin_activated) DrawSin(Current_line_color, Current_line_width, Current_line_style);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteSin();
            DrawSin(Line_color, Line_width, Line_style);
            Sin_activated = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteHexagon();
            Angle = 0;
            switch (comboBox7.SelectedIndex)
            {
                case 0:
                    Hex_center = new Point(Left_limit * Scale_x + Shift_x, (int)(-F(Left_limit) * Scale_y + Shift_y));
                    break;
                case 1:
                    Hex_center = new Point(Right_limit * Scale_x + Shift_x, (int)(-F(Right_limit) * Scale_y + Shift_y));
                    break;
            }

            try
            {
                Hex_radius = int.Parse(textBox1.Text);
                if (Hex_radius < 0) throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Invalid size\nCannot draw hexagon", "Error");
                return;
            }

            DrawHexagon(Hex_line_color, Hex_line_width, Hex_line_style);
            Hex_activated = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Hex_activated || !Sin_activated) return;
            bool OK = InitMovement();
            if (!OK) return;

            int radius = Hex_radius;
            bool grows = false;
            int delay_time = (int)(10000 / Speed * Move_step);
            double base_x = (Hex_center.X - Shift_x) / Scale_x;

            int init_direction = (base_x <= 0) ? 1 : -1;
            int direction = init_direction;
            int right = Right_limit * Scale_x + Shift_x;
            int left = Left_limit * Scale_x + Shift_x;

            int rotation_direction = Counterclockwise ? 1 : -1;
            if (!Is_rotating) rotation_direction = 0;

            int cycle_count = 0;
            while (cycle_count < Cycles)
            {
                while (direction == 1 && Hex_center.X < right || direction == -1 && Hex_center.X > left)
                {
                    DeleteHexagon();
                    if (grows)
                    {
                        Hex_radius += (int)(Move_step * Pulsation_speed);
                        if (Hex_radius >= radius)
                        {
                            Hex_radius = radius;
                            grows = false;
                        }
                    }
                    else
                    {
                        Hex_radius -= (int)(Move_step * Pulsation_speed);
                        if (Hex_radius <= Min_size)
                        {
                            Hex_radius = Min_size;
                            grows = true;
                        }
                    }

                    base_x += Move_step * direction;
                    Hex_center.X = (int)(base_x * Scale_x) + Shift_x;
                    Hex_center.Y = (int)(-F(base_x) * Scale_y) + Shift_y;

                    Angle += Move_step * Rotation_speed * rotation_direction;

                    DrawHexagon(Hex_line_color, Hex_line_width, Hex_line_style);
                    System.Threading.Thread.Sleep(delay_time);
                }
                direction = -direction;
                if (direction == init_direction) cycle_count++;
            }
        }

        private bool InitMovement()
        {
            try
            {
                Speed = int.Parse(textBox4.Text);
                if (Speed < 0) throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Invalid speed\nSpeed must be a positive integer", "Error");
                return false;
            }

            try
            {
                Move_step = double.Parse(textBox5.Text);
                if (Move_step < 0) throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Invalid step\nStep must be a positive float number (with comma)", "Error");
                return false;
            }

            try
            {
                Cycles = int.Parse(textBox6.Text);
                if (Cycles < 0) throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: Invalid cycle number\nCycle number must be a positive integer", "Error");
                return false;
            }

            if (Is_pulsating)
            {
                try
                {
                    Pulsation_speed = int.Parse(textBox2.Text);
                    if (Pulsation_speed < 0) throw new FormatException();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Error: Invalid speed\nPulsation speed must be a positive integer", "Error");
                    return false;
                }

                try
                {
                    Min_size = int.Parse(textBox3.Text);
                    if (Min_size > Hex_radius) throw new InvalidDataException("Min size cannot be bigger than size");
                    if (Min_size < 0) throw new FormatException();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Error: Invalid min size\nMin size must be a positive integer", "Error");
                    return false;
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                    return false;
                }
            }
            else Pulsation_speed = 0;

            if (Is_rotating)
            {
                try
                {
                    Rotation_speed = int.Parse(textBox7.Text);
                    if (Rotation_speed < 0) throw new FormatException();
                }
                catch (FormatException)
                {
                    MessageBox.Show("Error: Invalid speed\nRotation speed must be a positive integer", "Error");
                    return false;
                }
            }

            return true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            int old_shift_x = Shift_x;
            int old_shift_y = Shift_y;
            int old_scale_x = Scale_x;
            ResetCoefficients();

            double base_x = (double)(Hex_center.X - old_shift_x) / old_scale_x;
            double base_y = Hex_center.Y - old_shift_y;
            Hex_center = new Point((int)(base_x * Scale_x + Shift_x), (int)(base_y + Shift_y));

            if (!Sin_activated && !Hex_activated) return;
            Graphics gr = pictureBox1.CreateGraphics();
            gr.Clear(pictureBox1.BackColor);

            if (Hex_activated) DrawHexagon(Hex_line_color, Hex_line_width, Hex_line_style);
            if (Sin_activated) DrawSin(Line_color, Line_width, Line_style);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Line_color = Color.Black;
                    return;
                case 1:
                    Line_color = Color.Blue;
                    return;
                case 2:
                    Line_color = Color.Red;
                    return;
                case 3:
                    Line_color = Color.Cyan;
                    return;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Line_width = comboBox2.SelectedIndex + 1;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    Line_style = null;
                    return;
                case 1:
                    Line_style = new float[] { 10, 5 };
                    return;
                case 2:
                    Line_style = new float[] { 2, 5 };
                    return;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox4.SelectedIndex)
            {
                case 0:
                    Hex_line_color = Color.Black;
                    return;
                case 1:
                    Hex_line_color = Color.Blue;
                    return;
                case 2:
                    Hex_line_color = Color.Red;
                    return;
                case 3:
                    Hex_line_color = Color.Cyan;
                    return;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hex_line_width = comboBox5.SelectedIndex + 1;
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox6.SelectedIndex)
            {
                case 0:
                    Hex_line_style = null;
                    return;
                case 1:
                    Hex_line_style = new float[] { 5, 2 };
                    return;
                case 2:
                    Hex_line_style = new float[] { 2, 2 };
                    return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) Is_pulsating = true;
            else Is_pulsating = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) Is_rotating = true;
            else Is_rotating = false;
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox8.SelectedIndex)
            {
                case 0:
                    Counterclockwise = false;
                    return;
                case 1:
                    Counterclockwise = true;
                    return;
            }

        }
    }
}

