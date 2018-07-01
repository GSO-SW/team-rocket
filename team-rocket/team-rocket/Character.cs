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

        /// <summary>
        /// The characters current position saved as a PointF.
        /// </summary>
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
            IsMovingLR = false; // The character isn't moving at the start
            IsJumping = false; // The character isn't jumping at the start
            IsHeadingRight = true; // The character is facing right at the start

            // Load the sprites for movement
            sprite_right = new Bitmap[3];
            sprite_right[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_0.png");
            sprite_right[1] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_1.png");
            sprite_right[2] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_2.png");

            // Do the same here but flip the images
            sprite_left = new Bitmap[3];
            Bitmap mirrored = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_0.png");
            mirrored.RotateFlip(RotateFlipType.RotateNoneFlipX);
            sprite_left[0] = mirrored;
            mirrored = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_1.png");
            mirrored.RotateFlip(RotateFlipType.RotateNoneFlipX);
            sprite_left[1] = mirrored;
            mirrored = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\character_2.png");
            mirrored.RotateFlip(RotateFlipType.RotateNoneFlipX);
            sprite_left[2] = mirrored;
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
