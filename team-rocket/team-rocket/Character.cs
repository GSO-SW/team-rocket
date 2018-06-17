﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
	class Character : GameObject
	{
		private Bitmap[] sprite;
        
		#region Properties
		/// <summary>
		/// Returns the Sprite-Array, which will represent the walking animation of the character later on.
		/// </summary>
		public Bitmap[] Sprite
		{
			get { return sprite; }
		}
		#endregion

		/// <summary>
		/// Provides a class, which will represent the players character.
		/// </summary>
		/// <param name="point">Sets the initial location of the character with a Point object.</param>
		public Character(PointF point)
		{
			Location = point;
			hitbox.Height = 64;
			hitbox.Width = 32;
			Velocity = new SizeF(0f, 0f);
            Movable = true;
		}
	}
}
