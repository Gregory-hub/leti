using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace work2
{
    public partial class Form1 : Form
    {
        private Tree FractalTree;
        private int Level = -1;
        private const int NodeDiameter = 10;
        private Color[] Colors = new Color[] { Color.Blue, Color.Red, Color.Green, Color.Khaki, Color.Indigo };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBoxFractal.Width = pictureBoxFractal.Height;
            pictureBoxTree.Width = pictureBoxTree.Height;

            FractalTree = new Tree();
            FractalTree.Build(pictureBoxFractal.Width);

            object[] levels = new object[FractalTree.MaxLevel + 1];
            for (int i = 0; i <= FractalTree.MaxLevel; i++) levels[i] = i;
            comboBox1.Items.AddRange(levels);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void DrawFractal()
        {
            Graphics gr = pictureBoxFractal.CreateGraphics();
            Brush brush = new SolidBrush(GetLevelColor(Level));
            gr.Clear(pictureBoxFractal.BackColor);

            List<Node> nodes = new List<Node>();
            FractalTree.GetLevelNodes(FractalTree.Root, Level, 0, ref nodes);

            foreach (Node node in nodes)
            {
                gr.FillRectangle(brush, node.Coords.X, node.Coords.Y, node.Width, node.Width);
            }
        }

        private Color GetLevelColor(int level)
        {
            return Colors[level % Colors.Length];
        }

        private void DrawTree()
        {
            Graphics gr = pictureBoxTree.CreateGraphics();
            gr.Clear(pictureBoxTree.BackColor);

            for (int level = 0; level <= FractalTree.MaxLevel; level++)
            {
                if (level == Level) DrawLevel(level, gr, big: true);
                else DrawLevel(level, gr);
            }
        }

        private void DrawLevel(int level, Graphics gr, bool big = false)
        {
            List<Node> nodes = new List<Node>();
            FractalTree.GetLevelNodes(FractalTree.Root, level, 0, ref nodes);

            double distance_x = (double)pictureBoxTree.Width / (nodes.Count + 1);
            int top = (pictureBoxTree.Height / (FractalTree.MaxLevel + 2)) * (level + 1) - NodeDiameter / 2;
            int left;

            Point point1;
            Point point2;
            
            Brush brush = new SolidBrush(GetLevelColor(level));
            Pen pen = new Pen(Color.DarkSlateGray);

            int node_diameter = (big) ? NodeDiameter * 2 : NodeDiameter;
            for (int i = 0; i < nodes.Count; i++)
            {
                left = (int)((i + 1) * distance_x) - node_diameter / 2;
                nodes[i].TreeNodeCoords = new Point(left +  node_diameter / 2, top + node_diameter / 2);
                if (nodes[i].Father != null)
                {
                    point1 = nodes[i].Father.TreeNodeCoords;
                    point2 = nodes[i].TreeNodeCoords;
                    gr.DrawLine(pen, point1, point2);
                }
                gr.FillEllipse(brush, left, top, node_diameter, node_diameter);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Level == -1) return;
            DrawFractal();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Level = comboBox1.SelectedIndex;
            DrawTree();
        }
    }
}

