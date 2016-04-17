using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;


namespace SnakeProject
{
	public class Snake
	{

		public int direction = 0;
		public List<Point> body = new List<Point>();
		public Point last = new Point(0, 0);

		public void insert(int x, int y)
		{

			this.body.Add(new Point(x, y));
			this.last = this.body[this.body.Count - 1];

		}
		public Point remove()
		{
			Point p = new Point(0, 0);
			p = this.body[0];
			this.body.RemoveAt(0);
			return p;
		}
	}
}