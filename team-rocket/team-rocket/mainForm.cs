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
		bool bTempA, bNowA;
		bool bTempD, bNowD;
		bool bTempSpace, bNowSpace;
		float g; //Gravitational acceleration
		float velocity; //Unit px/tick

		public main()
		{
			InitializeComponent();

			ClientSize = new Size(1024, 768); //32*32 | 32*24


			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			bTempA = bNowA = bTempD = bNowD = bTempSpace = bNowSpace = false;
			velocity = 5;
			g = 0.5f;

			#region Initialize Frame Timer
			updateGraphicsTimer = new Timer
			{
				Interval = 20
			};
			updateGraphicsTimer.Tick += OnTimerTick;
			updateGraphicsTimer.Start();
			#endregion

			#region Loading Textures
			// Here are the game sprites being loaded in
			bitmapArray = new Bitmap[3];
			bitmapArray[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\default.png");
			bitmapArray[1] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\background_1.png");
			bitmapArray[2] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\ground_1.png");
			#endregion

			/* List of bitmap array with id
			 * 
			 *  = #/i =    = name =
			 *    0         default
			 *    1         background_1
			 *    2         ground_1
			 */

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
			/*	int[] imageIDs = new int[768];
				for (int i = 0; i < imageIDs.Length; i++)
				{
					imageIDs[i] = 1;
					if (i >= 704)
						imageIDs[i] = 2;
				}
				testLevel = new Level(imageIDs, new Point(0, 640), new Point(1024 - 32, 640));

				loadLevel(testLevel);*/
			#endregion

			#region Test Level 2
			int[] imageIDs = new int[768];
			for (int i = 0; i < imageIDs.Length; i++)
			{
				imageIDs[i] = 1;
				if (i >= 710)
					imageIDs[i] = 2;
			}
			testLevel = new Level(imageIDs, new Point(1024/2, 768/2), new Point(1024 - 32, 640));

			loadLevel(testLevel);
			#endregion

			character = new Character(testLevel.StartPoint);
			KeyDown += OnKeyDown;
			KeyUp += OnKeyUp;

		}

		/// <summary>
		/// Load the level to the game view.
		/// </summary>
		/// <param name="lvl">The level, which should be loaded.</param>
		private void loadLevel(Level lvl)
		{
			for (int i = 0; i < tilesArray.Length; i++)
			{
				tilesArray[i].ImageID = lvl.ImageIDs[i];
				switch (tilesArray[i].ImageID)
				{
					case 0:
					case 1:
						tilesArray[i].HitboxFlag = false;
						break;
					case 2:
						tilesArray[i].HitboxFlag = true;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Event-Handler for the Graphic-Update-Timer-Tick
		/// </summary>
		/// <param name="sender">Contains information which object triggered the Event.</param>
		/// <param name="e">Conatins informatin about the Event.</param>
		private void OnTimerTick(object sender, EventArgs e)
		{
			RectangleF player = character.RectF;
			SizeF velocity = character.Velocity;
			if (velocity.Height < 32)
				velocity.Height += g;


			RectangleF futurePlayer = new RectangleF(player.Location + velocity,player.Size);

			List<int> neededTiles = new List<int>();
			int a = Convert.ToInt32(player.Y + (Math.Round(player.X / 32)));
			neededTiles.Add(a - 33);
			neededTiles.Add(a - 32);
			neededTiles.Add(a - 31);
			neededTiles.Add(a - 1);
			neededTiles.Add(a);
			neededTiles.Add(a + 1);
			neededTiles.Add(a + 31);
			neededTiles.Add(a + 32);
			neededTiles.Add(a + 33);
			neededTiles.Add(a + 63);
			neededTiles.Add(a + 64);
			neededTiles.Add(a + 65);

			/*   Index positions
			 *   0  1  2
			 *   3  4  5
			 *   6  7  8
			 *   9  10 11
			 */

			for (int i = 0; i < neededTiles.Count; i++)
			{
				if (tilesArray[neededTiles[i]].HitboxFlag && tilesArray[neededTiles[i]].Rect.IntersectsWith(futurePlayer))
				{
					if(i == 10)
					{
						if (velocity.Height > 0)
							velocity.Height = 0;
					}
				}
			}

			#region 


			/*
			ist<int> collidedTileIndex = new List<int>();
			List<int> futureCollidedTileIndex = new List<int>();int tempX = -1;
			bool sameX = true;
			int tempY = -1;
			bool sameY = true;

			for (int i = 0; i < tilesArray.Length; i++)
			{
				if (tilesArray[i].HitboxFlag)
				{
					if (tilesArray[i].Rect.IntersectsWith(player))
					{
						collidedTileIndex.Add(i);
					}
					if (tilesArray[i].Rect.IntersectsWith(futurePlayer))
					{
						//futureCollidedTileIndex.Add(i);

						if (tempY == tilesArray[i].Rect.Y)
						{

						}

					}
				}
			}*/
			#endregion

			#region
			/*
			//foreach (int item in futureCollidedTileIndex)
			if (futureCollidedTileIndex.Count != 0)
			{
				int item = futureCollidedTileIndex[0];
				if (velocity.Height > 0) //Meaning falling
				{
					//Oberkante der kachel muss betrachtet werden
					//velocity.Height = tilesArray[item].Rect.Location.Y - (charRectF.Location.Y + charRectF.Size.Height);

					velocity.Height = tilesArray[item].Rect.Location.Y - (charRectF.Location.Y + charRectF.Size.Height);
				}
				else if (velocity.Height < 0) //Meaning jumping
				{
					//Unterkante der kachel muss betrachtet werden
				//	velocity.Height = charRectF.Location.Y - tilesArray[item].Rect.Location.Y;
				}

				if (velocity.Width < 0) //Meaning movement to the left
				{
					//rechte seite der Kachel muss betrachtet werden

				}
				else if (velocity.Width > 0) //Meaning movement to the right
				{
					//linke setite der kachel muss betrachtet werden
					//velocity.Width = tilesArray[item].Rect.Location.X - (charRectF.Location.X + charRectF.Size.Width);
				}
			}
			*/
			#endregion

			player.Location += velocity;
			character.Velocity = velocity;
			character.RectF = player;
			Invalidate();
		}

		/// <summary>
		/// Paint-Event-Handler
		/// </summary>
		/// <param name="e">Conatins informatin about the Event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			int i = 0;
			foreach (Tile item in tilesArray)
			{
				e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Rect.Location);
				e.Graphics.DrawString(i.ToString(), Font, Brushes.Black, PointF.Add(item.Rect.Location, new Size(10, 10)));
				i++;
			}
			e.Graphics.FillRectangle(Brushes.Blue, character.RectF);
			e.Graphics.DrawString(character.RectF.Location.ToString(), Font, Brushes.Black, new PointF(100, 100));
			e.Graphics.DrawString(character.RectF.X / 32 + "|||" + character.RectF.Y / 32, Font, Brushes.Blue, new PointF(100, 150));

		}

		/// <summary>
		/// Eventhandler whenever a key is being pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A)
			{
				// bTempA saves the state (pressed or not) of the Key 'A' from the last execution of the OnKeyDown-EventHandler.
				// bNpw A saves the state (pressed or not) of the Key 'A' at the moment.
				bTempA = bNowA;
				bNowA = true;
				// Comparing bNowA and bTempA to detect if the key was being pressed since the last comparison.
				if (bNowA != bTempA)
				{
					//The code which should be executed only one time if the key is being pressed.
					character.Velocity = new SizeF(character.Velocity.Width - velocity, character.Velocity.Height);
				}
			}

			if (e.KeyCode == Keys.D) // see above
			{
				bTempD = bNowD;
				bNowD = true;
				if (bNowD != bTempD)
				{
					character.Velocity = new SizeF(character.Velocity.Width + velocity, character.Velocity.Height);
				}
			}

			if (e.KeyCode == Keys.Space) // see above
			{
				bTempSpace = bNowSpace;
				bNowSpace = true;
				if (bNowSpace != bTempSpace)
				{
					character.Velocity = new SizeF(character.Velocity.Width, character.Velocity.Height - 20);
				}
			}
		}

		/// <summary>
		/// Eventhandler whenever a key is being let go.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.A)
			{
				bNowA = bTempA = false; //bNowA and bTempA will be set to false because the key isn't pressed anymore.
				character.Velocity = new SizeF(character.Velocity.Width + velocity, character.Velocity.Height);
			}
			if (e.KeyCode == Keys.D)
			{
				bNowD = bTempD = false;
				character.Velocity = new SizeF(character.Velocity.Width - velocity, character.Velocity.Height);
			}
			if (e.KeyCode == Keys.Space)
			{
				bNowSpace = bTempSpace = false;
			}
		}

		private bool detectCollision(RectangleF a, RectangleF b)
		{
			if (a.Location.X < b.Location.X + b.Size.Width && a.Location.X + a.Size.Width > b.Location.X && a.Location.Y < b.Location.Y + b.Size.Height && a.Location.Y + a.Size.Height > b.Location.Y)
			{
				return true;
			}
			return false;
		}
	}
}