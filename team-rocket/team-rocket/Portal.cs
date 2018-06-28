using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
	class Portal
	{
		private Rectangle rect;
		private Bitmap image;
		private int alignment;

		#region Prperties
		public Rectangle Rect
		{
			get { return rect; }
			set { rect = value; }
		}

		public Bitmap Image
		{
			get { return image; }
			set { image = value; }
		}

		public int Alignment
		{
			get { return alignment; }
			set { alignment = value; }
		}
		#endregion

		public Portal()
		{

		}
	}
}
