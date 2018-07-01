using System.Drawing;

namespace team_rocket
{
	class Level
	{
		private int[] imageIDs;
		private Point startPoint;
		private Point endPoint;

		#region Properties
		/// <summary>
		/// An array of Integer, which represent the Images from the tiles. Orderd like the TileArray.
		/// </summary>
		public int[] ImageIDs
		{
			get { return imageIDs; }
			set { imageIDs = value; }
		}

		/// <summary>
		/// The player should spawn at this Point.
		/// </summary>
		public Point StartPoint
		{
			get { return startPoint; }
		}

		/// <summary>
		/// The level is completed, if the player reach this point.
		/// </summary>
		public Point EndPoint
		{
			get { return endPoint; }
		}
		#endregion

		public Level(int[] imageIDs, Point startPoint, Point endPoint)
		{
			ImageIDs = imageIDs;
			this.startPoint = startPoint;
			this.endPoint = endPoint;
		}
	}
}
