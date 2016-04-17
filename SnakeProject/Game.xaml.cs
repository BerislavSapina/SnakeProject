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

namespace SnakeProject
{
	/// <summary>
	/// Interaction logic for Game.xaml
	/// </summary>
	public partial class Game : Window
	{
		#region variables

			public const int EMPTY = 0, SNAKE = 1, FRUIT = 2, SIZE = 20;
			public const int LEFT = 1, UP = 2, RIGHT = 3, DOWN = 4;
			public int SPEED =100 ,COLS, ROWS,gameWidth,gameHeight;
			private bool startGame = false;
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
			COLS = gameWidth / 20;
			ROWS = gameHeight / 20;
			rect = new Rectangle[COLS, ROWS];
			grid = new Grid(COLS, ROWS);
			this.KeyDown += MainWindow_KeyDown;
			main();
			resetButton.Click += resetButton_Click;
			
			
		}

		void resetButton_Click(object sender, RoutedEventArgs e)
		{
			canvasGame.Opacity = 1;
			gameWindow_Loaded_1(sender,e);
		}
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Left || e.Key == Key.A) && keystate != RIGHT)
            {
                keystate = LEFT;
				startGame = true;
            }
            if ((e.Key == Key.Up || e.Key == Key.W) && keystate != DOWN)
            {
                keystate = UP;
				startGame = true;
            }
            if ((e.Key == Key.Right || e.Key == Key.D) && keystate != LEFT)
            {
                keystate = RIGHT;
				startGame = true;
            }
            if ((e.Key == Key.Down || e.Key == Key.S) && keystate != UP)
            {
                keystate = DOWN;
				startGame = true;
            }

		
		}

        #region main

        public void main()
        {
            canvasGame.Background = Brushes.Black;

            for (int i = 0; i < COLS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    rect[i, j] = new Rectangle();
                    rect[i, j].Stroke = new SolidColorBrush(Colors.Black);
                    rect[i, j].Fill = new SolidColorBrush(Colors.Black);
                    rect[i, j].StrokeThickness = 2;
                    rect[i, j].Width = SIZE;
                    rect[i, j].Height = SIZE;
                    Canvas.SetLeft(rect[i, j], i *SIZE);
                    Canvas.SetTop(rect[i, j], j * SIZE);
                    canvasGame.Children.Add(rect[i, j]);
                }
            }

            init();

			disp.Tick += disp_Tick;
			disp.Interval = new TimeSpan(0, 0, 0, 0, SPEED);
			disp.Start();
        }

        void disp_Tick(object sender, EventArgs e)
        {
			if (startGame)
				update();
        }

        #endregion

        #region init

        public void init()
        {
			Random rnd = new Random();
			int x = rnd.Next(0, COLS);
			int y = rnd.Next(0, ROWS);
            grid.init(EMPTY, COLS, ROWS);
			snake.insert(x, y);
			grid.set(SNAKE, x, y);
            setFood();
			draw();
        }
        #endregion

        #region draw
        public void draw()
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
        #endregion

        #region draw grid
        public void drawGrid(int i, int j)
        {

            rect[i, j].Stroke = new SolidColorBrush(Colors.Black);
            rect[i, j].Fill = new SolidColorBrush(Colors.Black);
        }
        #endregion

        #region draw food
        public void drawFood(int i, int j)
        {
            rect[i, j].Stroke = new SolidColorBrush(Colors.Green);
            rect[i, j].Fill = new SolidColorBrush(Colors.Green);
        }
        #endregion

        #region draw snake
        public void drawSnake(int i, int j)
        {
            rect[i, j].Stroke = new SolidColorBrush(Colors.Red);
            rect[i, j].Fill = new SolidColorBrush(Colors.Red);

        }
        #endregion

        #region update grid

		public void update()
		{

			if (keystate == LEFT) { snake.direction = LEFT; }

			if (keystate == UP) { snake.direction = UP; }

			if (keystate == RIGHT) { snake.direction = RIGHT; }

			if (keystate == DOWN) { snake.direction = DOWN; }

			int nx = snake.last.x;
			int ny = snake.last.y;

			if (snake.direction != 0)
			{
				disp.Start();
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
					disp.Stop();
					startGame = false;
					snake.body.Clear();
					System.GC.Collect();
				
				}
				else
				{
					drawSnake(nx, ny);

					if (grid.get(nx, ny) == FRUIT)
					{
						SCORE++;
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
								MessageBox.Show("You win!");
								MainWindow mainWindow = new MainWindow();
								mainWindow.Show();
								this.Close();
								disp.Stop();
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
				}
			}
		}
        #endregion


        #region setFood
        public void setFood()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, COLS);
            int y = rnd.Next(0, ROWS);
            if (grid.get(x, y) == 1 || grid.get(x,y) == 2 )
            {
                setFood();
            }
            else
                grid.set(FRUIT, x, y);
			drawFood(x,y);
        }
        #endregion

    }
}
