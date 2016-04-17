using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeProject
{
	public class Grid
	{
		public int width;
		public int height;
		public int[,] grid = new int[0,0];	

		public Grid(int i, int j)
		{ 
			grid = new int[i,j];		
		}
		
        public void init(int val, int w, int h)
		{
			this.width = w;
			this.height = h;
			
            for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					this.grid[i, j] = val;
				}
			}
		}

		public void set(int val, int x, int y)
		{
			this.grid[x, y] = val;
		}

		public int get(int x, int y)
		{
			return this.grid[x, y];
		}
	}
}
