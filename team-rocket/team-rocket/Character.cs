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
		private Bitmap[] sprite;
		private Rectangle hitbox;

		#region Properties
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

		/// <summary>
		/// Returns the hitbox of the character.
		/// </summary>
		public Rectangle Hitbox
		{
			get { return hitbox; }
		}

		/// <summary>
		/// Returns the Sprite-Array, which will represent the walking animation of the character later on.
		/// </summary>
		public Bitmap[] Sprite
		{
			get { return sprite; }
		}
		#endregion

		public Character(int x, int y)
		{
			X = x;
			Y = y;
			hitbox.Height = 64;
			hitbox.Width = 32;
		}
	}
}
