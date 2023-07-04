using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _0._4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Текстовый редактор";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|All files (*.*)|*.*";
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName == null) return;
            try
            {
                StreamReader MyReader = new StreamReader(openFileDialog1.FileName);
                textBox1.Text = MyReader.ReadToEnd();
                MyReader.Close();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message + "\nФайл не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = openFileDialog1.FileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) Save();
        }

        void Save()
        {
            try
            {
                StreamWriter MyWriter = new StreamWriter(saveFileDialog1.FileName, false);
                MyWriter.Write(textBox1.Text);
                MyWriter.Close();
                textBox1.Modified = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Modified == false)
            {
                this.Close();
                return;
            }
            DialogResult MeBox = MessageBox.Show(
                "Текст был изменѐн. \nСохранить изменения?",
                "Простой редактор",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Exclamation
            );

            if (MeBox.Equals(DialogResult.No)) this.Close();
            if (MeBox.Equals(DialogResult.Cancel)) return;
            if (MeBox.Equals(DialogResult.Yes))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Save();
                    this.Close();
                }
            }
        }
    }
}

