using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace SnakeProject.Controls
{
	/// <summary>
	/// Interaction logic for HighScoresControl.xaml
	/// </summary>
	public partial class HighScoresControl : UserControl
	{
		public HighScoresControl()
		{
			InitializeComponent();
			readHighScore();
		}
		private void readHighScore()
		{
			try
			{
				StreamReader sr = new StreamReader(@"../../Files/HighScore.txt");
				var scores = new List<string>();
				string line;

				line = sr.ReadLine();

				while (line != null)
				{
					scores.Add(line);
					line = sr.ReadLine();
				}

				var sortedScores = scores.OrderByDescending(l => int.Parse(l.Split(' ')[1])).ToList();

				foreach (var score in sortedScores)
				{

					TextBlock txtBlock = new TextBlock();
					txtBlock.Text = score;
					txtBlock.FontSize = 30;
					scoreTable.Children.Add(txtBlock);
				}
				sr.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Couldnt open high score file");
			}

		}
	}
}
