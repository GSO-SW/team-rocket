using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MapEditorP2D
{
	static class controller
	{
		private static int selectedIndexLeft = 0;
		public static int SelectedIndexLeft
		{
			get { return selectedIndexLeft; }
			set { selectedIndexLeft = value; }
		}

		public static Tile[] tilesArray;

		private static System.Drawing.Bitmap[] bitmapArray;
		public static System.Drawing.Bitmap[] BitmapArray
		{
			get { return bitmapArray; }
		}

		public static System.Drawing.Point startPoint = new System.Drawing.Point(0, 0);
		public static System.Drawing.Point endPoint = new System.Drawing.Point(0, 0);

		public static void loadTextures()
		{
			bitmapArray = new System.Drawing.Bitmap[9];
			bitmapArray[0] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\default.png");
			bitmapArray[1] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\metall_foreground.png");
			bitmapArray[2] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\metall_background.png");
			bitmapArray[3] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\ground_1.png");
			bitmapArray[4] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\dirty_water.png");
			bitmapArray[5] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\door_1.png");
			bitmapArray[6] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\door_2.png");
			bitmapArray[7] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\door_3.png");
			bitmapArray[8] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\door_4.png");
			/* List of bitmap array with id
			 * 
			 *  = #/i =    = name =
			 *    0         default
			 *    1			metall_foregrounde
			 *    2			metall_background
			 *    3			ground 1
			 *    4			drity water
			 *    5			door 1
			 *    6			door 2
			 *    7			door 3
			 *    8			door 4
			 * 
			 */
		}

		public static void saveMap(string path)
		{
			if (startPoint != new System.Drawing.Point(0,0) && endPoint != new System.Drawing.Point(0,0))
			{
				StreamWriter strW = new StreamWriter(path);
				strW.WriteLine(startPoint.X + "," + startPoint.Y);
				strW.WriteLine(endPoint.X + "," + endPoint.Y);
				for (int i = 0; i < tilesArray.Length; i++)
				{
					strW.WriteLine(tilesArray[i].ImageID);
				}
				strW.Close();
			}
		}
	}
}
