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
		private Point location;
		private Bitmap[] sprite;
		private Rectangle hitbox;
		private bool moveXFlag;
		private bool moveYFlag;

		#region Properties
		/// <summary>
		/// Sets and returns the location of the character as a Point object.
		/// </summary>
		public Point Location
		{
			get { return location; }
			set { location = value; }
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

		/// <summary>
		/// Returns and set the flag to determine, whether or not the character is moving into the X dimension.
		/// </summary>
		public bool MoveXFlag
		{
			get { return moveXFlag; }
			set { moveXFlag = value; }
		}

		/// <summary>
		/// Returns and set the flag to determine, whether or not the character is moving into the Y dimension.
		/// </summary>
		public bool MoveYFlag
		{
			get { return moveYFlag; }
			set { moveYFlag = value; }
		}
		#endregion

		/// <summary>
		/// Provides a class, which will represent the players character.
		/// </summary>
		/// <param name="point">Sets the initial location of the character with a Point object.</param>
		public Character(Point point)
		{
			location = point;
			hitbox.Height = 64;
			hitbox.Width = 32;
		}
	}
}
