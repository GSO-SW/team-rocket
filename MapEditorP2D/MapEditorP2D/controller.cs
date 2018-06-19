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
			set
			{
				if (value >= 0 && value < bitmapArray.Length)
				selectedIndexLeft = value;
			}
		}


		private static System.Drawing.Bitmap[] bitmapArray;
		public static System.Drawing.Bitmap[] BitmapArray
		{
			get { return bitmapArray; }
		}

		public static void loadTextures()
		{
			bitmapArray = new System.Drawing.Bitmap[3];
			bitmapArray[0] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\default.png");
			bitmapArray[1] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\background_1.png");
			bitmapArray[2] = new System.Drawing.Bitmap(Directory.GetCurrentDirectory() + @"\gfx\ground_1.png");

			/* List of bitmap array with id
			 * 
			 *  = #/i =    = name =
			 *    0         default
			 *    1         background_1
			 *    2         ground_1
			 */
		}
	}
}
