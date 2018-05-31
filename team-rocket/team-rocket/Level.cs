using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace team_rocket
{
	class Level
	{
		private int[] imageIDs;
		/// <summary>
		/// An array of Integer, which represent the Images from the tiles. Orderd like the TileArray.
		/// </summary>
		public int[] ImageIDs
		{
			get { return imageIDs; }
			set { imageIDs = value; }
		}

		public Level(int[] imageIDs)
		{
			ImageIDs = imageIDs;
		}
	}
}
