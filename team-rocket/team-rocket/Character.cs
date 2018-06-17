using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
	class Character : GameObject
	{
		private PointF location;
		private Bitmap[] sprite;
		private SizeF velocity;
		
		#region Properties
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
		public SizeF Velocity
		{
			get { return velocity; }
			set { velocity = value; }
		}
		#endregion

		/// <summary>
		/// Provides a class, which will represent the players character.
		/// </summary>
		/// <param name="point">Sets the initial location of the character with a Point object.</param>
		public Character(PointF point)
		{
			location = point;
			hitbox.Height = 64;
			hitbox.Width = 32;
			velocity = new SizeF(0f, 0f);
            Movable = true;
		}
	}
}
