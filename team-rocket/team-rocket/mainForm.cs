using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace team_rocket
{
    public partial class main : Form
    {
        Timer updateGraphicsTimer;
        Tile[] tilesArray;
		Bitmap[] bitmapArray;
		Level testLevel;
		Character character;
		bool keyAIsPressed, keyDIsPressed;

        public main()
        {
            InitializeComponent();

            ClientSize = new Size(1024, 768); //32*32 | 24*32

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			keyAIsPressed = keyDIsPressed = false;

			#region Initialize Frame Timer
			// The timer which determines the FPS
			updateGraphicsTimer = new Timer();
            updateGraphicsTimer.Interval = 20;
            updateGraphicsTimer.Tick += UpdateGraphichsTimer_Tick;
            updateGraphicsTimer.Start();
			#endregion

			#region Loading Textures
			// Here are the game sprites being loaded in
			bitmapArray = new Bitmap[3];
			bitmapArray[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\default.png");
			bitmapArray[1] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\background_1.png");
			bitmapArray[2] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\ground_1.png");
			#endregion

			#region Initialize the tiles
			tilesArray = new Tile[768];
			int counter = 0;
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 32; j++)
                {
					tilesArray[counter] = new Tile(j * 32, i * 32, 0, false);
					counter++;
                }
            }
			#endregion

			#region Test Level
			int[] imageIDs = new int[768];
			for (int i = 0; i < imageIDs.Length; i++)
			{
				imageIDs[i] = 1;
				if (i >= 704)
					imageIDs[i] = 2;
			}
			testLevel = new Level(imageIDs);

			loadLevel(testLevel);
			#endregion

			// Experimental
			character = new Character(new Point(50, 50));
			KeyDown += OnKeyDown;
			KeyUp += OnKeyUp;
			updateGraphicsTimer.Tick += OnTimerTick;

        }

		private void loadLevel(Level lvl)
		{
			for (int i = 0; i < tilesArray.Length; i++)
			{
				tilesArray[i].ImageID = lvl.ImageIDs[i];
			}
		}

        /// <summary>
        /// Event-Handler for the Graphic-Update-Timer-Tick
        /// </summary>
        /// <param name="sender">Contains information which object triggered the Event.</param>
        /// <param name="e">Conatins informatin about the Event.</param>
        private void UpdateGraphichsTimer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Paint-Event-Handler
        /// </summary>
        /// <param name="e">Conatins informatin about the Event.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

			foreach(Tile item in tilesArray)
			{
				e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Rect.Location);
			}
			e.Graphics.FillRectangle(Brushes.Red, new Rectangle(character.Location, character.Hitbox.Size));
        }

		/// <summary>
		/// Eventhandler for every frame tick in the mainForm, handles the movement in X dimension.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnTimerTick(object sender, EventArgs e)
		{
			// Checks if character should be moving
			if (keyAIsPressed)
				character.Location = new Point(character.Location.X - 5, character.Location.Y);
			if (keyDIsPressed)
				character.Location = new Point(character.Location.X + 5, character.Location.Y);

			//character.Location += character.Velocity;
		}

		/// <summary>
		/// Eventhandler whenever a random key is being pressed, handles the X dimension flag.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnKeyDown(object sender, KeyEventArgs e)
		{
			// The character should be moving, when either A or D is being pressed
			if (e.KeyCode == Keys.A)
				keyAIsPressed = true;
			if (e.KeyCode == Keys.D)
				keyDIsPressed = true;
			
		}

		/// <summary>
		/// Eventhandler whenever a key is being left, handles the X dimension flag.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// The character should not be moving, when A or D is stopped being pressed
			if (e.KeyCode == Keys.A)
				keyAIsPressed = false;
			if (e.KeyCode == Keys.D)
				keyDIsPressed = false;

		}
	}
}
