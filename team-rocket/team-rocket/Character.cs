using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
	class Character
	{
		private int x, y;

		/// <summary>
		/// Sets and returns the X coordinate of the character.
		/// </summary>
		public int X
		{
			get { return x; }
			set { x = value; }
		}

		/// <summary>
		/// Sets and returns the Y coordinate of the character.
		/// </summary>
		public int Y
		{
			get { return y; }
			set { y = value; }
		}

		public Character(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
