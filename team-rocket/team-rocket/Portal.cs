using System.Drawing;

namespace team_rocket
{
	class Portal
	{
		private Rectangle hitbox;
		private Point imageLocation;
		private Bitmap image;
		private int alignment;
		private bool imageRotated;

		#region Prperties
		/// <summary>
		/// Contains the hitbox from the Portal.
		/// </summary>
		public Rectangle Hitbox
		{
			get { return hitbox; }
			set { hitbox = value; }
		}

		/// <summary>
		/// The image of the portal.
		/// </summary>
		public Bitmap Image
		{
			get { return image; }
			set { image = value; }
		}

		/// <summary>
		/// Indicates in which direction the portal shows:
		/// 0 - At the left side of a block,
		/// 1 - At the right side of a block,
		/// 2 - At the top side of a block,
		/// 3 - At the bottom side of a block.
		/// </summary>
		public int Alignment
		{
			get { return alignment; }
			set { alignment = value; }
		}

		/// <summary>
		/// Determine wether the image of the portal is rotated or not.
		/// </summary>
		public bool ImageRotated
		{
			get { return imageRotated; }
			set
			{
				if (value != imageRotated)
					image.RotateFlip(RotateFlipType.Rotate90FlipNone);
				imageRotated = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Point ImageLocation
		{
			get { return imageLocation; }
			set { imageLocation = value; }
		}
		#endregion

		public Portal()
		{
			hitbox = Rectangle.Empty;
			alignment = -1;
			imageRotated = false;
			imageLocation = Point.Empty;
		}
	}
}
