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
		private Size hitbox;
		private Size velocity;
		
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
		public Size Hitbox
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
		/// Returns the velocity-vector for this Character.
		/// </summary>
		public Size Velocity
		{
			get { return velocity; }
			set { velocity = value; }
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
			velocity = new Size(0, 0);
		}
	}
}
