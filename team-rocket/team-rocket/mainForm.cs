using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace team_rocket
{
	public partial class main : Form
	{
		/// BUGS:
		/// - jumping is spamable
		/// 

		/// TODO
		/// onPaint modifizieren, 1. hintergrund, 2.characters, 3. vordergrund

		Timer updateGraphicsTimer;
		Tile[] tilesArray;
		Bitmap[] bitmapArray;
		Level loadedLevel;
		Character[] chars;
		bool bTempA, bNowA;
		bool bTempD, bNowD;
		bool bTempSpace, bNowSpace;
		float g; //Gravitational acceleration
		float velocityLR, jumpVelocity; //Unit: px/tick for the left or right and for the jumpspeed
		OpenFileDialog ofd;
		Portal bluePortal;
		Portal orangePortal;

		public main()
		{
			InitializeComponent();

			#region Configure the window
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			ClientSize = new Size(1024, 768); //32*32 | 32*24
			#endregion

			#region initialize vars
			bTempA = bNowA = bTempD = bNowD = bTempSpace = bNowSpace = false;
			velocityLR = 5;
			jumpVelocity = 15;
			g = 1f;
			bluePortal = new Portal();
			orangePortal = new Portal();
			#endregion

			#region Subscribe events
			KeyDown += OnKeyDown;
			KeyUp += OnKeyUp;
			MouseClick += onMouseClick;
			#endregion

			#region Loading Textures
			try
			{
				bitmapArray = new Bitmap[9];
				bitmapArray[0] = new Bitmap(Application.StartupPath + @"\gfx\default.png");
				bitmapArray[1] = new Bitmap(Application.StartupPath + @"\gfx\metall_foreground.png");
				bitmapArray[2] = new Bitmap(Application.StartupPath + @"\gfx\metall_background.png");
				bitmapArray[3] = new Bitmap(Application.StartupPath + @"\gfx\ground_1.png");
				bitmapArray[4] = new Bitmap(Application.StartupPath + @"\gfx\dirty_water.png");
				bitmapArray[5] = new Bitmap(Application.StartupPath + @"\gfx\door_1.png");
				bitmapArray[6] = new Bitmap(Application.StartupPath + @"\gfx\door_2.png");
				bitmapArray[7] = new Bitmap(Application.StartupPath + @"\gfx\door_3.png");
				bitmapArray[8] = new Bitmap(Application.StartupPath + @"\gfx\door_4.png");

				bluePortal.Image = new Bitmap(Application.StartupPath + @"\gfx\blue_portal.png");
				orangePortal.Image = new Bitmap(Application.StartupPath + @"\gfx\orange_portal.png");
			}
			catch (Exception)
			{
				MessageBox.Show("Missing files: " + Application.StartupPath + @"\gfx\", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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

			#region Startup Level
			int[] imageIDs = new int[768];
			for (int i = 0; i < imageIDs.Length; i++)
			{
				imageIDs[i] = 2;
				if (i >= 704)
					imageIDs[i] = 3;
			}
			loadedLevel = new Level(imageIDs, new Point(1024 / 2, 768 / 2), new Point(1024 - 32, 640));

			loadLevel(loadedLevel);
			#endregion

			#region Initialize Frame Timer
			updateGraphicsTimer = new Timer
			{
				Interval = 20
			};
			updateGraphicsTimer.Tick += OnTimerTick;
			updateGraphicsTimer.Start();
			#endregion

			#region OpenFileDialog
			ofd = new OpenFileDialog();
			if (!Directory.Exists(Application.StartupPath + @"\maps\"))
				Directory.CreateDirectory(Application.StartupPath + @"\maps\");
			ofd.InitialDirectory = Application.StartupPath + @"\maps\";
			ofd.Filter = "*.map |*.map";
			ofd.FileOk += OnFileOKofd;
			#endregion

			// Spawn Character
			chars = new Character[1];
			chars[0] = new Character(loadedLevel.StartPoint);
		}

		/// <summary>
		/// Event-Handler for Mouse Clicks. Used for the protal shooting.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void onMouseClick(object sender, MouseEventArgs e)
		{
			//pos = e.Location.ToString();
			//f(x) = m*x+b
			//mouse.y = mouse.x
			//player.y = player.x
			//m = deltaY/deltaX
			float playerX = chars[0].RectF.X + chars[0].RectF.Width / 2;
			float playerY = chars[0].RectF.Y + chars[0].RectF.Height / 2;
			float m = (playerY - e.Location.Y) / (playerX - e.Location.X);
			if (!float.IsInfinity(m) && !float.IsNaN(m))
			{
				float b = playerY - m * playerX;
				PointF pointToTest = new PointF(playerX, playerY);
				int x = Convert.ToInt32(playerX);
				int j = 0;
				bool hit = false;

				if (playerX < e.Location.X) //rechts vom spieler
					j = 1;
				else if (playerX > e.Location.X)//links vom spieler
					j = -1;
				else if (playerX == e.Location.X)//genau über oder unterm spieler
					return;

				while ((x < ClientSize.Width && x > 0) && !hit)
				{
					pointToTest = new PointF(x, m * x + b);
					foreach (Tile item in tilesArray)
					{
						if (item.HitboxFlag && item.Rect.Contains(Point.Round(pointToTest)))
						{
							hit = true;
						}
					}
					x += j;
				}

				if (x != 0 && x != ClientSize.Width)
				{
					if (e.Button == MouseButtons.Left)
					{
						bluePortal.Rect = new Rectangle(new Point(x, Convert.ToInt32(m * x + b)), new Size(0, 0));
					}
					else if (e.Button == MouseButtons.Right)
					{
						orangePortal.Rect = new Rectangle(new Point(x, Convert.ToInt32(m * x + b)), new Size(0, 0));
					}
				}
			}
		}

		/// <summary>
		/// OpenFileDialog FileOK-EventHandler. Used to load the chosen map.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFileOKofd(object sender, CancelEventArgs e)
		{
			if (File.Exists(ofd.FileName))
			{
				StreamReader strR = new StreamReader(ofd.FileName);
				string s = strR.ReadLine();
				Point startPoint = new Point
				{
					X = Convert.ToInt32(s.Split(',')[0]),
					Y = Convert.ToInt32(s.Split(',')[1])
				};
				s = strR.ReadLine();
				Point endPoint = new Point
				{
					X = Convert.ToInt32(s.Split(',')[0]),
					Y = Convert.ToInt32(s.Split(',')[1])
				};
				int[] ImageIDs = new int[768];
				for (int i = 0; i < ImageIDs.Length; i++)
				{
					ImageIDs[i] = Convert.ToInt32(strR.ReadLine());
				}
				strR.Close();
				loadedLevel = new Level(ImageIDs, startPoint, endPoint);
				loadLevel(new Level(ImageIDs, startPoint, endPoint));
			}
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
					case 2:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
						tilesArray[i].HitboxFlag = false;
						break;
					case 1:
					case 3:
						tilesArray[i].HitboxFlag = true;
						break;
					default:
						break;
				}
			}
			if (chars != null)
				chars[0].RectF = new RectangleF(lvl.StartPoint, chars[0].RectF.Size);
		}

		/// <summary>
		/// Event-Handler for the Graphic-Update-Timer-Tick
		/// </summary>
		/// <param name="sender">Contains information which object triggered the Event.</param>
		/// <param name="e">Conatins informatin about the Event.</param>
		private void OnTimerTick(object sender, EventArgs e)
		{
			for (int j = 0; j < chars.Length; j++)
			{
				RectangleF character = chars[j].RectF;
				SizeF velocity = chars[j].Velocity;
				if (velocity.Height < 30) //the player shouldn't fall faster than 30 px/tick, it can cause miss calculation
					velocity.Height += g;

				if (!ClientRectangle.IntersectsWith(Rectangle.Round(character)))
					character.Location = loadedLevel.StartPoint;

				RectangleF futureCharacter = new RectangleF(character.Location + velocity, character.Size);

				#region Detect the collision in the next tick
				List<int> futureCollidedTileIndex = new List<int>();

				for (int i = 0; i < tilesArray.Length; i++)
				{
					if (tilesArray[i].HitboxFlag)
					{
						if (tilesArray[i].Rect.IntersectsWith(Rectangle.Round(futureCharacter)))
						{
							futureCollidedTileIndex.Add(i);
						}
					}
				}
				#endregion

				#region Detect in which direction the collision will occure and calculating the borders
				// player position in tiles
				Point playerTile = new Point(Convert.ToInt32(Math.Round(character.X / 32)), Convert.ToInt32(Math.Round(character.Y / 32)));
				// vars for saving the "border" where the player has to hit
				float heightBorder = -1;
				float widthBorder = -1;

				foreach (int i in futureCollidedTileIndex)
				{
					// detecting in which direction the future hitted tile is
					Point p = Point.Subtract(new Point(Convert.ToInt32(tilesArray[i].Rect.X / 32), Convert.ToInt32(tilesArray[i].Rect.Y / 32)), (Size)playerTile);

					if (p.X == -1 || p.X == 0 || p.X == 1) //If the tile is above or under the player
					{
						if (p.Y == 2) //under the player
							heightBorder = tilesArray[i].Rect.Y; //set the height border at the top edge of the tile
						else if (p.Y == -1) // above the player
							heightBorder = tilesArray[i].Rect.Y + tilesArray[i].Rect.Height; //set the height border at the bottom edge of the tile
					}

					if (p.Y == 0 || p.Y == 1) //If the tile is left or right
					{
						if (p.X == 1) //right of the player
							widthBorder = tilesArray[i].Rect.X; // set the left edge as width border
						else if (p.X == -1) //left of the player
							widthBorder = tilesArray[i].Rect.X + tilesArray[i].Rect.Width;//set the right edge as width border
					}

				}
				#endregion

				#region Apply the borders to the velocity of the player
				if (futureCollidedTileIndex.Count > 0)
				{
					if (velocity.Height > 0 && heightBorder != -1) //Meaning falling
					{
						//Oberkante der kachel muss betrachtet werden
						if (heightBorder < (character.Y + character.Height))
							character.Y = character.Y - ((character.Y + character.Height) - heightBorder);
						velocity.Height = heightBorder - (character.Y + character.Height);
					}
					else if (velocity.Height < 0 && heightBorder != -1) //Meaning jumping
					{
						//Unterkante der kachel muss betrachtet werden
						velocity.Height = heightBorder - character.Y;
					}

					if (velocity.Width > 0 && widthBorder != -1) //Meaning movement to the right
					{
						//linke seite der kachel muss betrachtet werden
						velocity.Width = widthBorder - (character.X + character.Width);
					}
					else if (velocity.Width < 0 && widthBorder != -1) //Meaning movement to the left
					{
						//rechte seite der Kachel muss betrachtet werden
						velocity.Width = widthBorder - character.X;
					}
				}
				#endregion

				character.Location += velocity;
				chars[j].Velocity = velocity;
				chars[j].RectF = character;
			}
			Invalidate();
		}

		/// <summary>
		/// Paint-Event-Handler
		/// </summary>
		/// <param name="e">Conatins informatin about the Event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			foreach (Tile item in tilesArray)
			{
				e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Rect.Location);
			}
			foreach (Character item in chars)
			{
				e.Graphics.FillRectangle(Brushes.Blue, item.RectF);
			}
			e.Graphics.DrawLine(Pens.Red, new Point(Convert.ToInt32(chars[0].RectF.Location.X + chars[0].RectF.Size.Width / 2), Convert.ToInt32(chars[0].RectF.Location.Y + chars[0].RectF.Size.Height / 2)), PointToClient(MousePosition));
			e.Graphics.DrawImage(bluePortal.Image, bluePortal.Rect.Location);
			e.Graphics.DrawImage(orangePortal.Image, orangePortal.Rect.Location);
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
					//The code which should be executed only one time if the key is being pressed. like edge control
					chars[0].Velocity = new SizeF(chars[0].Velocity.Width - velocityLR, chars[0].Velocity.Height);
				}
			}

			if (e.KeyCode == Keys.D) // see above
			{
				bTempD = bNowD;
				bNowD = true;
				if (bNowD != bTempD)
				{
					chars[0].Velocity = new SizeF(chars[0].Velocity.Width + velocityLR, chars[0].Velocity.Height);
				}
			}

			if (e.KeyCode == Keys.Space) // see above
			{
				bTempSpace = bNowSpace;
				bNowSpace = true;
				if (bNowSpace != bTempSpace)
				{
					chars[0].Velocity = new SizeF(chars[0].Velocity.Width, chars[0].Velocity.Height - jumpVelocity);
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
				if (bNowD)
					chars[0].Velocity = new SizeF(chars[0].Velocity.Width + velocityLR, chars[0].Velocity.Height);
				else
					chars[0].Velocity = new SizeF(0, chars[0].Velocity.Height);
			}
			if (e.KeyCode == Keys.D)
			{
				bNowD = bTempD = false;
				if (bNowA)
					chars[0].Velocity = new SizeF(chars[0].Velocity.Width - velocityLR, chars[0].Velocity.Height);
				else
					chars[0].Velocity = new SizeF(0, chars[0].Velocity.Height);
			}
			if (e.KeyCode == Keys.Space)
			{
				bNowSpace = bTempSpace = false;
			}
			if (e.KeyCode == Keys.O)
			{
				ofd.ShowDialog();
			}
		}
	}
}