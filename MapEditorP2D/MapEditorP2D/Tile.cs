using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MapEditorP2D
{
	class Tile
	{
		private Point location;
		private int imageID;

		#region Properties definition
		/// <summary>
		/// Returns the location of the upper left and right corner of the rectangle
		/// as a Point-Structure.
		/// </summary>
		public Point Location
		{
			get { return location; }
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
		public Tile(int x, int y, int imageID)
		{
			location = new Point(x, y);
			ImageID = imageID;
		}
	}
}
