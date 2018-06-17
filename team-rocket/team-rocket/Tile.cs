using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
	/// <summary>
	/// This class resembles the tiles ingame, which serves as walls and platforms for the player.
	/// </summary>
	class Tile : GameObject
	{
		private bool hitboxFlag;
		private int imageID;

		#region Properties definition
		/// <summary>
		/// Flag to determine, whether or not the tile has a hitbox.
		/// </summary>
		public bool HitboxFlag
		{
			get { return hitboxFlag; }
			set { hitboxFlag = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int ImageID
		{
			get { return imageID; }
			set { imageID = value; }
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x">The X-Coordinate of the tile.</param>
		/// <param name="y">The Y-Coordinate of the tile.</param>
		/// <param name="imageID">The ImageID of the tile. Index of the Image from the Bitmap-Array.</param>
		/// <param name="hitboxFlag">The Hitbox-Flag of the tile. true = Hitbox is enabled, false = Hitbox is disabled.</param>
		public Tile(int x, int y, int imageID, bool hitboxFlag)
		{
			Hitbox = new Rectangle();
			hitbox.Height = 32;
			hitbox.Width = 32;
			hitbox.X = x;
			hitbox.Y = y;
			ImageID = imageID;
			HitboxFlag = hitboxFlag;
            Movable = false;
		}
	}
}
