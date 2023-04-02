using System;
using System.Windows.Forms;

namespace PlotDrawer
{
	static class Globals {
		public static string title = "Insert title (pass as parameter to a program)";
	}
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>

		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length > 0)
			{
				Globals.title = args[0];
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}

