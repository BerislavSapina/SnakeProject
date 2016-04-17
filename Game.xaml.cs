using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;
using System.IO;
using System.Threading;

namespace SnakeProject
{
	/// <summary>
	/// Interaction logic for Game.xaml
	/// </summary>
	public partial class Game : Window
	{
		#region variables
			public const int EMPTY = 0, SNAKE = 1, FRUIT = 2, SIZE = 30;
			public const int LEFT = 1, UP = 2, RIGHT = 3, DOWN = 4;
			public int SPEED,COLS, ROWS,gameWidth,gameHeight;
			private bool startGame, delay;
			private bool restartOn = false;
			int keystate = 0;

			public Rectangle[,] rect = new Rectangle[0,0];
			public Grid grid = new Grid(0,0);
			public Snake snake = new Snake();
			DispatcherTimer disp = new DispatcherTimer();
		#endregion

		#region dependencyPropertiesVariables
			public int SCORE
			{
				get { return (int)GetValue(ScoreProperty); }
				set { SetValue(ScoreProperty, value); }
			}
			public static readonly DependencyProperty ScoreProperty =
				DependencyProperty.Register("SCORE", typeof(int), typeof(Game), new PropertyMetadata(0));
		
			public int LEVEL
			{
				get { return (int)GetValue(LevelProperty); }
				set { SetValue(LevelProperty, value); }
			}
			public static readonly DependencyProperty LevelProperty =
				DependencyProperty.Register("LEVEL", typeof(int), typeof(Game), new PropertyMetadata(1));
		#endregion
			
		public Game()
        {
            InitializeComponent();
        }
		
        private void gameWindow_Loaded_1(object sender, RoutedEventArgs e)
		{
			gameWidth = (int)canvasBorder.ActualWidth;
			gameHeight = (int)canvasBorder.ActualHeight;
			
            COLS = gameWidth / SIZE;
            ROWS = gameHeight / SIZE;
			
            rect = new Rectangle[COLS, ROWS];
			grid = new Grid(COLS, ROWS);
            
            this.KeyDown += MainWindow_KeyDown;
			saveButton.Click += saveButton_Click;
			restartButton.Click += restartButton_Click;
			menuButton.Click += menuButton_Click;
            main();            
		}

		void menuButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
			clearTimer();	
		}

		void restartButton_Click(object sender, RoutedEventArgs e)
		{
			canvasGame.Opacity = 1;
			init();
			restartOn = true;
		}

		void saveButton_Click(object sender, RoutedEventArgs e)
		{
			if (!startGame)
			{
				saveHighScore();
				HighScores.Children.Clear();
				readHighScore();
			}
		}

        private void main()
        {
            canvasGame.Background = Brushes.Black;

            for (int i = 0; i < COLS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    rect[i, j] = new Rectangle();
                    rect[i, j].Stroke = Brushes.Black;
                    rect[i, j].Fill = Brushes.Black;
                    rect[i, j].StrokeThickness = 2;
                    rect[i, j].Width = SIZE;
                    rect[i, j].Height = SIZE;
                    Canvas.SetLeft(rect[i, j], i *SIZE);
                    Canvas.SetTop(rect[i, j], j * SIZE);
                    canvasGame.Children.Add(rect[i, j]);
                }
            }
			readHighScore();
            init();
        }

        private void init()
        {
            clearTimer();

			restartWindow.SetValue(Panel.ZIndexProperty, 0);
			Canvas.SetLeft(restartWindow, (gameWidth / 2) - restartWindow.Width / 2);
			Canvas.SetTop(restartWindow, (gameHeight / 2) - restartWindow.Height / 2);
            startGame = false;
            SCORE = 0;
            LEVEL = 1;
            SPEED = 100;
			keystate = 0;

            snake.body.Clear();

			Random rnd = new Random();
			int x = rnd.Next(0, COLS);
			int y = rnd.Next(0, ROWS);
            grid.init(EMPTY, COLS, ROWS);
			snake.insert(x, y);
			grid.set(SNAKE, x, y);
            setFood();
			draw();

            startTimer();
        }
        
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
			delay = false;	
            if ((e.Key == Key.Left || e.Key == Key.A) && keystate != RIGHT)
            {
                keystate = LEFT;
				startGame = true;
				canvasGame.Opacity = 1;
            }
            if ((e.Key == Key.Up || e.Key == Key.W) && keystate != DOWN)
            {
                keystate = UP;
				startGame = true;
				canvasGame.Opacity = 1;
            }
            if ((e.Key == Key.Right || e.Key == Key.D) && keystate != LEFT)
            {
                keystate = RIGHT;
				startGame = true;
				canvasGame.Opacity = 1;
            }
            if ((e.Key == Key.Down || e.Key == Key.S) && keystate != UP)
            {
                keystate = DOWN;
				startGame = true;
				canvasGame.Opacity = 1;
            }
            
            if(e.Key == Key.R) 
            {
				canvasGame.Opacity = 1;
				init();
				restartOn = true;
            }
			if (e.Key == Key.P)
			{
				startGame = false;
				canvasGame.Opacity = 0.5;
			}
			if (startGame && restartOn)
			{
				canvasGame.Opacity = 1;
				restartOn = false;
			}
		}
		
		#region Timer
        void startTimer()
        {
			Dispatcher.Invoke(()=>{
            disp.Tick += disp_Tick;
			disp.Interval = TimeSpan.FromMilliseconds(SPEED);
			disp.Start();
			});
        }

        void clearTimer()
        {
            disp.Tick -= disp_Tick;
			disp.Stop();
        }

        void disp_Tick(object sender, EventArgs e)
        {
			if (startGame)
				update();
        }
		#endregion 

		#region draw
		private void draw()
        {
            for (int i = 0; i < grid.width; i++)
            {
                for (int j = 0; j < grid.height; j++)
                {
                    switch (grid.get(i, j))
                    {
                        case 0:
                            drawGrid(i, j);
                            break;
                        case 1:
                            drawSnake(i, j);
                            break;
                        case 2:
                            drawFood(i, j);
                            break;
                    }
                }
            }
        }

        private void drawGrid(int i, int j)
        {
            rect[i, j].Stroke = Brushes.Black;
            rect[i, j].Fill = Brushes.Black;
        }

        private void drawFood(int i, int j)
        {
            rect[i, j].Stroke = Brushes.Black; 
            rect[i, j].Fill = Brushes.Red;
        }
        private void drawSnake(int i, int j)
        {
			rect[i, j].Stroke = Brushes.Black;
            rect[i, j].Fill = Brushes.Green;

        }
        #endregion

		private void update()
		{
			
			if (keystate == LEFT) { snake.direction = LEFT; }

			if (keystate == UP) { snake.direction = UP; }

			if (keystate == RIGHT) { snake.direction = RIGHT; }

			if (keystate == DOWN) { snake.direction = DOWN; }

			int nx = snake.last.x;
			int ny = snake.last.y;

			if (snake.direction != 0)
			{
				switch (snake.direction)
				{
					case LEFT:					//LEFT
						nx--;
						break;
					case UP:					//UP
						ny--;
						break;
					case RIGHT:					//RIGHT
						nx++;
						break;
					case DOWN:					//DOWN
						ny++;
						break;

				}

				if (0 > nx || nx > grid.width - 1 || 0 > ny || ny > grid.height - 1 || grid.get(nx, ny) == SNAKE)
				{
					MessageBox.Show("Game over!");
					canvasGame.Opacity = 0.5;
					startGame = false;
					restartWindow.SetValue(Panel.ZIndexProperty, 1);
                    clearTimer();					
				}
				else
				{
					drawSnake(nx, ny);

					if (grid.get(nx, ny) == FRUIT)
					{
						SCORE += 10;
						if (SCORE % 3 == 0)
						{
							if (LEVEL < 10)
							{
								LEVEL++;
								SPEED -= 10;
								disp.Interval = new TimeSpan(0, 0, 0, 0, SPEED);
							}
							else
							{
                                clearTimer();

                                MessageBox.Show("You win!");
								MainWindow mainWindow = new MainWindow();
								mainWindow.Show();
                                
								this.Close();
							}

						}
						setFood();
					}
					else
					{
						Point tail = new Point(0, 0);
						tail = snake.remove();
						grid.set(EMPTY, tail.x, tail.y);
						drawGrid(tail.x, tail.y);
					}
					grid.set(SNAKE, nx, ny);
					snake.insert(nx, ny);
					delay = true;
				}
			}
		}



        #region setFood
        public void setFood()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, COLS);
            int y = rnd.Next(0, ROWS);
            if (grid.get(x, y) == 1 || grid.get(x,y) == 2)
            {
                setFood();
            }
            else
                grid.set(FRUIT, x, y);
			drawFood(x,y);
        }
        #endregion

		#region highScore
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

				foreach (var score  in sortedScores)
				{
					
					TextBlock txtBlock = new TextBlock();
					txtBlock.Text = score;
					txtBlock.FontSize = 30;
					HighScores.Children.Add(txtBlock);
				}
				sr.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Couldnt open HighScore file");
			}

		}

		public void saveHighScore()
		{
			try
			{
				StreamWriter sw = new StreamWriter(@"../../Files/HighScore.txt", true);

				sw.WriteLine(gamePlayer.Text + " " + SCORE.ToString());

				sw.Close();
			}
			catch (Exception)
			{
				MessageBox.Show("Couldnt open gamexaml highscore file");
			}

		}
		#endregion

    }
}
