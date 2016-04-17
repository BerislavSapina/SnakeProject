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

namespace SnakeProject
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		
		}

		private void Window_Loaded_1(object sender, RoutedEventArgs e)
		{
			
			this.mainMenuControl.exitButton.Click += exitButton_Click;
			this.mainMenuControl.howToPlayButton.Click += howToPlayButton_Click;
			this.mainMenuControl.highScoreButton.Click += highScoreButton_Click;
			this.mainMenuControl.newGameButton.Click += newGameButton_Click;
			
			

			
		}

		void newGameButton_Click(object sender, RoutedEventArgs e)
		{
			this.playerControl.Visibility = Visibility.Visible;
			this.mainMenuControl.Visibility = Visibility.Hidden;
			this.playerControl.cancelButton.Click += cancelButton_Click;
			this.playerControl.okButton.Click += okButton_Click;
			this.playerControl.KeyDown += playerControl_KeyDown;
			this.playerControl.userNameTextBox.Focus();
			
		}

		void playerControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter) 
			{
				okButton_Click(sender, e);
			}
			else if (e.Key == Key.Escape) 
			{
				cancelButton_Click(sender, e);
			}
		}	

		void okButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.playerControl.userNameTextBox.Text == "")
			{
				MessageBox.Show("Please enter your name!");

			}
			else 
			{
				Game gameWindow = new Game();
				gameWindow.Show();
				gameWindow.gamePlayer.Text = this.playerControl.userNameTextBox.Text;
				this.Close();

			}
		}

		void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.playerControl.Visibility = Visibility.Hidden;
			this.mainMenuControl.Visibility = Visibility.Visible;
		}

		void howToPlayButton_Click(object sender, RoutedEventArgs e)
		{
			this.howToPlayControl.Visibility = Visibility.Visible;
			this.mainMenuControl.Visibility = Visibility.Hidden;
			this.howToPlayControl.closeButton.Click += closeButton_Click;
			this.KeyDown += MainWindow_KeyDown;
		}

		void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && ( this.howToPlayControl.Visibility == Visibility.Visible || this.highScoreControl.Visibility ==   Visibility.Visible))
			{
				closeButton_Click(sender, e);
			}
			
		}

		
		void highScoreButton_Click(object sender, RoutedEventArgs e)
		{
			this.highScoreControl.Visibility = Visibility.Visible;
			this.mainMenuControl.Visibility = Visibility.Hidden;
			this.highScoreControl.closeButton.Click += closeButton_Click;
			this.howToPlayControl.closeButton.Click += closeButton_Click;
			this.KeyDown += MainWindow_KeyDown;
		}

		void closeButton_Click(object sender, RoutedEventArgs e)
		{
			this.howToPlayControl.Visibility = Visibility.Hidden;
			this.highScoreControl.Visibility = Visibility.Hidden;
			this.mainMenuControl.Visibility = Visibility.Visible;
		}

		void exitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
