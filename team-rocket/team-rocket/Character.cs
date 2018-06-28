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
		private Bitmap[] sprite_right;
        private Bitmap[] sprite_left;
		private SizeF velocity;
		private RectangleF rect;
        private int currentSpriteIndex;
        private bool isJumping;
        private bool isHeadingRight;
        private bool isMovingLR;

		#region Properties
		/// <summary>
		/// Sets and returns the location of the character as a Point object.
		/// </summary>

		/// <summary>
		/// Returns the Sprite-Array, which will represent the walking animation of the character later on.
		/// </summary>
		public Bitmap[] Sprite_right
		{
			get { return sprite_right; }
		}

        public Bitmap[] Sprite_left
        {
            get { return sprite_left; }
        }

		/// <summary>
		/// Returns the velocity-vector for this Character.
		/// </summary>
		public SizeF Velocity
		{
			get { return velocity; }
			set
            {
                velocity = value;
                
                if(velocity.Width > 0)
                {
                    IsHeadingRight = true;
                    IsMovingLR = true;
                }
                if(velocity.Width < 0)
                {
                    IsHeadingRight = false;
                    IsMovingLR = true;
                }
                if(velocity.Width == 0)
                {
                    IsMovingLR = false;
                }

                if(velocity.Height != 0)
                {
                    IsJumping = true;
                }
                else
                {
                    IsJumping = false;
                }
            }
		}

		/// <summary>
		/// Contains information about the character position and size.
		/// </summary>
		public RectangleF RectF
		{
			get { return rect; }
			set { rect = value; }
		}

		public PointF Location
		{
			get { return rect.Location; }
			set { rect.Location = value; }
		}

        /// <summary>
        /// Returns the sprite Bitmap of the characters current animation frame.
        /// </summary>
        public Bitmap CurrentSprite
        {
            get
            {
                // Flip the sprite when the character is facing left
                if(!IsHeadingRight)
                {
                    return sprite_left[currentSpriteIndex];
                }
                else
                {
                    return sprite_right[currentSpriteIndex];
                }
            }
        }

        /// <summary>
        /// Determines, whether the character is jumping or not.
        /// </summary>
        public bool IsJumping
        {
            get { return isJumping; }
            set { isJumping = value; }
        }

        /// <summary>
        /// Determines, if the character sprite is looking right or left.
        /// Left = false | Right = true
        /// </summary>
        public bool IsHeadingRight
        {
            get { return isHeadingRight; }
            set { isHeadingRight = value; }
        }

        /// <summary>
        /// Determines, whether the character is moving left or right at all.
        /// </summary>
        public bool IsMovingLR
        {
            get { return isMovingLR; }
            set
            {
                isMovingLR = value;

                if(!isMovingLR)
                {
                    currentSpriteIndex = 0;
                }
            }
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
            IsMovingLR = false;
            IsJumping = false;
            IsHeadingRight = true;

            sprite_right = new Bitmap[3];
            sprite_right[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_right.png");
            sprite_right[1] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\default.png");
            sprite_right[2] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\tile_01.png");

            sprite_left = new Bitmap[3];
            sprite_left[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_left.png");
            sprite_left[1] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\default.png");
            sprite_left[2] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\tile_01.png");
        }

        /// <summary>
        /// Skips to the next frame of the character animation.
        /// </summary>
        public void NextFrame()
        {
            if (!IsJumping && IsMovingLR)
            {
                // If the whole running animation was being played, start again at 0
                if (currentSpriteIndex == 2)
                {
                    currentSpriteIndex = 0;
                }
                currentSpriteIndex++;
            }
            else
            {
                currentSpriteIndex = 0;
            }
        }
	}
}
