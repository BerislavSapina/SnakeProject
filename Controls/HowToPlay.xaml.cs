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
	/// Interaction logic for HowToPlay.xaml
	/// </summary>
	public partial class HowToPlay : UserControl
	{
		public string text
		{
			get { return (string)GetValue(ScoreProperty); }
			set { SetValue(ScoreProperty, value); }
		}

		public static readonly DependencyProperty ScoreProperty =
		DependencyProperty.Register("text", typeof(string), typeof(HowToPlay), new PropertyMetadata(""));
		
		
		public HowToPlay()
		{
			InitializeComponent();
			readHowToPlay();
		}

		public void readHowToPlay()
		{
			try
			{
				text = File.ReadAllText(@"../../Files/HowToPlay.txt");
			}
			catch (Exception)
			{
				MessageBox.Show("Couldnt open How To Play file");
			}

		}

	}
}
