using OopLabs.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Linq
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			listBox1.DataSource = Album.GetAlbums().OrderBy(album => album.Artist).Select(album => album.Artist).Distinct().ToList();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			listBox2.DataSource = Album.GetAlbums().Where(album => album.Artist == (string)listBox1.SelectedItem).OrderByDescending(album => album.Date).ToList();
		}
	}
}

