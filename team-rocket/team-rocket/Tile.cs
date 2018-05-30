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
	class Tile
	{
		private Rectangle rect;
		private bool hitboxFlag;
		private int imageID;

		#region Properties definition
		/// <summary>
		/// Returns the location of the upper left and right corner of the rectangle
		/// as a Point-Structure.
		/// </summary>
		public Rectangle Rect
		{
			get { return rect; }
		}

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
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Tile(int x, int y, int imageID)
		{
			rect = new Rectangle();
			rect.Height = 32;
			rect.Width = 32;
			rect.X = x;
			rect.Y = y;
			ImageID = imageID;
		}
	}
}
