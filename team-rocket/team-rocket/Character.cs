using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
	class Character
	{
		private Bitmap[] sprite;
		private SizeF velocity;
		private RectangleF rect;
        private int currentSpriteIndex;
        private bool isJumping;
        private bool headingRight;

		#region Properties
		/// <summary>
		/// Sets and returns the location of the character as a Point object.
		/// </summary>

		/// <summary>
		/// Returns the Sprite-Array, which will represent the walking animation of the character later on.
		/// </summary>
		public Bitmap[] Sprite
		{
			get { return sprite; }
		}

		/// <summary>
		/// Returns the velocity-vector for this Character.
		/// </summary>
		public SizeF Velocity
		{
			get { return velocity; }
			set { velocity = value; }
		}

		/// <summary>
		/// Contains information about the character position and size.
		/// </summary>
		public RectangleF RectF
		{
			get { return rect; }
			set { rect = value; }
		}

		public PointF location
		{
			get { return rect.Location; }
			set { rect.Location = value; }
		}

        /// <summary>
        /// Returns the sprite Bitmap of the characters current animation frame.
        /// </summary>
        public Bitmap CurrentSprite
        {
            get { return sprite[currentSpriteIndex]; }
        }

        /// <summary>
        /// Determines, whether the character is jumping or not.
        /// </summary>
        public bool IsJumping
        {
            get { return IsJumping; }
            set { IsJumping = value; }
        }

		#endregion

		/// <summary>
		/// Provides a class, which will represent the players character.
		/// </summary>
		/// <param name="point">Sets the initial location of the character with a PointF structure.</param>
		public Character(PointF point)
		{
			rect.Location = point; //StartPos
			rect.Size = new SizeF(32, 64); //Hitbox Size
			velocity = new SizeF(0f, 0f);
            currentSpriteIndex = 0; // Index starts at 0 for the first sprite of the animation
        }

        /// <summary>
        /// Skips to the next frame of the character animation.
        /// </summary>
        public void NextFrame()
        {
            
        }
	}
}
