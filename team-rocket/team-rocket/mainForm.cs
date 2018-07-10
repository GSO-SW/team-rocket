using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace team_rocket
{
	public partial class main : Form
	{
		bool bTempA, bNowA;
		bool bTempD, bNowD;
		bool bTempSpace, bNowSpace;
		bool currentlyInMenu;
		int horizontalMovementFrameCounter; // Counts the frames where the character moves horizontally.
		int animationSpeed;
		int levelIndex;
		float g; //Gravitational acceleration
		float velocityLR;
		float jumpVelocity; //Unit: px/tick for the left or right and for the jumpspeed
		float verticalVelocityLastTick;
		OpenFileDialog ofd;
		Button startGameButton;
		Button quitGameButton;
		Portal bluePortal;
		Portal orangePortal;
		Timer updateGraphicsTimer;
		Tile[] tilesArray;
		Bitmap[] bitmapArray;
		Level loadedLevel;
		Character[] chars;

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
			levelIndex = 1;
			bTempA = bNowA = bTempD = bNowD = bTempSpace = bNowSpace = false;
			velocityLR = 5;
			jumpVelocity = 15;
			verticalVelocityLastTick = 0;
			horizontalMovementFrameCounter = 0;
			animationSpeed = 7;
			currentlyInMenu = true;
			g = 1f;
			bluePortal = new Portal();
			orangePortal = new Portal();
			startGameButton = new Button(new PointF(ClientSize.Width / 2 - 100, 200)); // -50 because it's half of the SizeF's X value, so it is centered
			quitGameButton = new Button(new PointF(ClientSize.Width / 2 - 100, 300));
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
				bitmapArray[0] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\default.png"), 32, 32);
				bitmapArray[1] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\metall_foreground.png"), 32, 32);
				bitmapArray[2] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\metall_background.png"), 32, 32);
				bitmapArray[3] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\ground_1.png"), 32, 32);
				bitmapArray[4] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\dirty_water.png"), 32, 32);
				bitmapArray[5] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\door_1.png"), 32, 32);
				bitmapArray[6] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\door_2.png"), 32, 32);
				bitmapArray[7] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\door_3.png"), 32, 32);
				bitmapArray[8] = new Bitmap(Image.FromFile(Application.StartupPath + @"\..\..\gfx\door_4.png"), 32, 32);

				bluePortal.Image = new Bitmap(Application.StartupPath + @"\..\..\gfx\blue_portal.png");
				orangePortal.Image = new Bitmap(Application.StartupPath + @"\..\..\gfx\orange_portal.png");
			}
			catch (Exception)
			{
				MessageBox.Show("Missing files: " + Application.StartupPath + @"\..\..\gfx\", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
			if (!Directory.Exists(Application.StartupPath + @"\..\..\maps\"))
				Directory.CreateDirectory(Application.StartupPath + @"\..\..\maps\");
			ofd.InitialDirectory = Application.StartupPath + @"\..\..\maps\";
			ofd.Filter = "*.map |*.map";
			ofd.FileOk += OnFileOKofd;
			#endregion

			// Spawn Character
			chars = new Character[1];
			chars[0] = new Character(new PointF(0, 0));
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
			if (!currentlyInMenu)
			{
				float playerX = chars[0].RectF.X + chars[0].RectF.Width / 2;
				float playerY = chars[0].RectF.Y + chars[0].RectF.Height / 2;
				float m = (playerY - e.Location.Y) / (playerX - e.Location.X);
				if (!float.IsInfinity(m) && !float.IsNaN(m))
				{
					float b = playerY - m * playerX;
					Point pointToTest = Point.Round(new PointF(playerX, playerY));
					int x = Convert.ToInt32(playerX);
					int y = Convert.ToInt32(playerY);
					int jx = 0;
					int jy = 0;
					bool hitX = false;
					bool hitY = false;
					int firstHittedTileIndexX = -1;
					int firstHittedTileIndexY = -1;

					#region Get the x-Value of the intercept
					if (playerX < e.Location.X) //rechts vom spieler
						jx = 1;
					else if (playerX > e.Location.X)//links vom spieler
						jx = -1;
					else if (playerX == e.Location.X) // auf der gleichen breite wie der spieler
					{
						x = e.Location.X;
						hitX = true;
					}
					while ((x < ClientSize.Width && x > 0) && !hitX)
					{
						x += jx;
						pointToTest = Point.Round(new PointF(x, m * x + b));
						for (int i = 0; i < tilesArray.Length; i++)
						{
							if (tilesArray[i].HitboxFlag && tilesArray[i].Rect.Contains(Point.Round(pointToTest)))
							{
								if (!hitX)
								{
									hitX = true;
									firstHittedTileIndexX = i;
								}
							}
						}
					}
					#endregion

					#region Get the y-Value of the intercept
					if (playerY < e.Location.Y) //unter dem spieler
						jy = 1;
					else if (playerY > e.Location.Y)//über dem spieler
						jy = -1;
					if (playerY == e.Location.Y) //auf der gleichen höhe wie der spieler
					{
						y = e.Location.Y;
						hitY = true;
					}
					while ((y < ClientSize.Height && y > 0) && !hitY)
					{
						y += jy;
						pointToTest = Point.Round(new PointF((y - b) / m, y));
						for (int i = 0; i < tilesArray.Length; i++)
						{
							if (tilesArray[i].HitboxFlag && tilesArray[i].Rect.Contains(Point.Round(pointToTest)))
							{
								if (!hitY)
								{
									hitY = true;
									firstHittedTileIndexY = i;
								}
							}
						}
					}
					#endregion

					#region Calculating the alignment of the portal
					if (firstHittedTileIndexX != -1 && firstHittedTileIndexY != -1 && tilesArray[firstHittedTileIndexX].ImageID != 3) //Disable portals on Ground_1
					{
						if (firstHittedTileIndexX == firstHittedTileIndexY && x >= 0 && x <= ClientSize.Width && y >= 0 && y <= ClientSize.Height)
						{
							Point newPortalPosition = tilesArray[firstHittedTileIndexX].Rect.Location;
							Point newPortalHitboxPosition = tilesArray[firstHittedTileIndexX].Rect.Location;
							Size newPortalHitboxSize = new Size(32, 64);
							Point diffMousePositionHittedTile = Point.Subtract(new Point(x, y), (Size)tilesArray[firstHittedTileIndexX].Rect.Location);
							bool flipImage = false;
							int alignment = -1;

							if (diffMousePositionHittedTile.X == 0) //At the left side of a block
							{
								newPortalPosition.X -= 4;
								alignment = 0;
								if (!tilesArray[firstHittedTileIndexX + 32].HitboxFlag
									|| tilesArray[firstHittedTileIndexX + 31].HitboxFlag
									|| tilesArray[firstHittedTileIndexX - 1].HitboxFlag)
									return;
							}
							else if (diffMousePositionHittedTile.X == 31) //At the right side of a block
							{
								newPortalPosition.X += 32;
								alignment = 1;
								if (!tilesArray[firstHittedTileIndexX + 32].HitboxFlag
									|| tilesArray[firstHittedTileIndexX + 33].HitboxFlag
									|| tilesArray[firstHittedTileIndexX + 1].HitboxFlag)
									return;
							}
							else if (diffMousePositionHittedTile.Y == 0) // At the top side of a block
							{
								newPortalPosition.Y -= 4;
								newPortalHitboxSize = new Size(64, 32);
								flipImage = true;
								alignment = 2;
								if (!tilesArray[firstHittedTileIndexX + 1].HitboxFlag
									|| tilesArray[firstHittedTileIndexX - 31].HitboxFlag)
									return;
							}
							else if (diffMousePositionHittedTile.Y == 31) // At the bottom side of a block
							{
								newPortalHitboxSize = new Size(64, 32);
								newPortalPosition.Y += 32;
								flipImage = true;
								alignment = 3;
								if (!tilesArray[firstHittedTileIndexX + 1].HitboxFlag
									|| tilesArray[firstHittedTileIndexX + 33].HitboxFlag)
									return;
							}
							if (e.Button == MouseButtons.Left)
							{
								if (orangePortal.Hitbox != null && !orangePortal.Hitbox.IntersectsWith(new Rectangle(newPortalHitboxPosition, newPortalHitboxSize)))
								{
									bluePortal.ImageRotated = flipImage;
									bluePortal.Alignment = alignment;
									bluePortal.ImageLocation = newPortalPosition;
									bluePortal.Hitbox = new Rectangle(newPortalHitboxPosition, newPortalHitboxSize);
								}
							}
							else if (e.Button == MouseButtons.Right)
							{
								if (bluePortal.Hitbox != null && !bluePortal.Hitbox.IntersectsWith(new Rectangle(newPortalHitboxPosition, newPortalHitboxSize)))
								{
									orangePortal.ImageRotated = flipImage;
									orangePortal.Alignment = alignment;
									orangePortal.ImageLocation = newPortalPosition;
									orangePortal.Hitbox = new Rectangle(newPortalHitboxPosition, newPortalHitboxSize);
								}
							}
						}
					}
					#endregion
				}

			}
			if (currentlyInMenu && startGameButton.Body.Contains(e.Location))
			{
				loadMapFile(Application.StartupPath + @"\maps\level_1.map");
				updateGraphicsTimer.Start();
				currentlyInMenu = false;
			}

			if (currentlyInMenu && quitGameButton.Body.Contains(e.Location))
			{
				Close();
			}
		}

		/// <summary>
		/// OpenFileDialog FileOK-EventHandler. Used to load the chosen map.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFileOKofd(object sender, CancelEventArgs e)
		{
			loadMapFile(ofd.FileName);
		}

		private void loadMapFile(string path)
		{
			if (File.Exists(path))
			{
				StreamReader strR = new StreamReader(path);
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
			else
				MessageBox.Show(path + " konnte nicht geladen werden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void resetPortals()
		{
			bluePortal.Alignment = -1;
			bluePortal.Hitbox = Rectangle.Empty;
			bluePortal.ImageRotated = false;
			bluePortal.ImageLocation = Point.Empty;

			orangePortal.Alignment = -1;
			orangePortal.Hitbox = Rectangle.Empty;
			orangePortal.ImageRotated = false;
			orangePortal.ImageLocation = Point.Empty;
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

			resetPortals();

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

				Portal sourcePortal = null;
				Portal destPortal = null;
				bool portalHitted = false;
				if (bluePortal.Hitbox.IntersectsWith(Rectangle.Round(character)) && orangePortal.Alignment != -1)
				{
					destPortal = orangePortal;
					sourcePortal = bluePortal;
					portalHitted = true;
				}
				else if (orangePortal.Hitbox.IntersectsWith(Rectangle.Round(character)) && bluePortal.Alignment != -1)
				{
					destPortal = bluePortal;
					sourcePortal = orangePortal;
					portalHitted = true;
				}

				if (portalHitted)
				{
					switch (destPortal.Alignment)
					{
						case 0:
							character.Location = Point.Subtract(destPortal.ImageLocation, new Size(Convert.ToInt32(character.Width), 0));
							break;
						case 1:
							character.Location = Point.Add(destPortal.ImageLocation, new Size(4, 0));
							break;
						case 2:
							character.Location = Point.Subtract(destPortal.ImageLocation, new Size(0, Convert.ToInt32(character.Height)));
							break;
						case 3:
							character.Location = Point.Add(destPortal.ImageLocation, new Size(32 - Convert.ToInt32(character.Width / 2), 4));
							break;
					}

					#region Velocity change
					int sa = sourcePortal.Alignment;
					int da = destPortal.Alignment;
					//source left
					if (sa == 0 && da == 0) // dest left
					{
						velocity.Width = -velocity.Width;
					}
					else if (sa == 0 && da == 1) // dest right
					{
						//nothing to change
					}
					else if (sa == 0 && da == 2) // dest up
					{
						float temp = -velocity.Width;
						velocity.Width = velocity.Height;
						velocity.Height = temp;
					}
					else if (sa == 0 && da == 3) // dest bottom
					{
						float temp = velocity.Width;
						velocity.Width = velocity.Height;
						velocity.Height = temp;
					}
					//source right
					else if (sa == 1 && da == 0) //dest left
					{
						//nothing to change
					}
					else if (sa == 1 && da == 1)  // dest right
					{
						velocity.Width = -velocity.Width;
					}
					else if (sa == 1 && da == 2) // dest up
					{
						float temp = velocity.Width;
						velocity.Width = velocity.Height;
						velocity.Height = temp;
					}
					else if (sa == 1 && da == 3) // dest bottom
					{
						float temp = -velocity.Width;
						velocity.Width = velocity.Height;
						velocity.Height = temp;
					}
					//source up
					else if (sa == 2 && da == 0) // dest left
					{
						float temp = -velocity.Height;
						velocity.Height = velocity.Width;
						velocity.Width = temp;
					}
					else if (sa == 2 && da == 1) // dest right
					{
						float temp = velocity.Height;
						velocity.Height = velocity.Width;
						velocity.Width = temp;
					}
					else if (sa == 2 && da == 2) // dest up
					{
						velocity.Height = -velocity.Height;
					}
					else if (sa == 2 && da == 3) // dest bottom
					{
						//nothing to change
					}
					// source bottom
					else if (sa == 3 && da == 0) // dest left
					{
						float temp = -velocity.Height;
						velocity.Height = velocity.Width;
						velocity.Width = temp;
					}
					else if (sa == 3 && da == 1) // dest right
					{
						float temp = velocity.Height;
						velocity.Height = velocity.Width;
						velocity.Width = temp;
					}
					else if (sa == 3 && da == 2) // dest up
					{
						// nothing to change
					}
					else if (sa == 3 && da == 3) // dest bottom
					{
						velocity.Height = -velocity.Height;
					}
					#endregion
				}

				RectangleF futureCharacter = new RectangleF(character.Location + velocity, character.Size);

				if (!bluePortal.Hitbox.IntersectsWith(Rectangle.Round(futureCharacter))
					&& !orangePortal.Hitbox.IntersectsWith(Rectangle.Round(futureCharacter))
					|| bluePortal.Alignment == -1 || orangePortal.Alignment == -1)
				{
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
				}
				character.Location += velocity;
				chars[j].Velocity = velocity;
				verticalVelocityLastTick = velocity.Height;
				chars[j].RectF = character;
			}

			#region Animation
			// Count the times where the character is moving left or right in a frame
			if (chars[0].IsMovingLR)
			{
				horizontalMovementFrameCounter++;
			}
			// When it reaches its maximum, play the next frame of the animation
			// Maximum is defined as the animationSpeed
			if (horizontalMovementFrameCounter == animationSpeed)
			{
				horizontalMovementFrameCounter = 0;
				chars[0].NextFrame();
			}
			#endregion

			Invalidate();
		}

		/// <summary>
		/// Paint-Event-Handler
		/// </summary>
		/// <param name="e">Conatins informatin about the Event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!currentlyInMenu)
			{
				foreach (Tile item in tilesArray)
				{
					if (!item.HitboxFlag)
						e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Rect.Location);
				}
				foreach (Character item in chars)
				{
					e.Graphics.DrawImage(item.CurrentSprite, item.Location);
				}
				foreach (Tile item in tilesArray)
				{
					if (item.HitboxFlag)
						e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Rect.Location);
				}
				e.Graphics.DrawLine(Pens.Red, new Point(Convert.ToInt32(chars[0].RectF.Location.X + chars[0].RectF.Size.Width / 2), Convert.ToInt32(chars[0].RectF.Location.Y + chars[0].RectF.Size.Height / 2)), PointToClient(MousePosition));
				if (bluePortal.Hitbox != Rectangle.Empty)
					e.Graphics.DrawImage(bluePortal.Image, bluePortal.ImageLocation);
				if (orangePortal.Hitbox != Rectangle.Empty)
					e.Graphics.DrawImage(orangePortal.Image, orangePortal.ImageLocation);
			}
			else
			{
				startGameButton.Text = "Start Game";
				startGameButton.Size = new SizeF(200, 40);
				quitGameButton.Text = "Quit Game";
				quitGameButton.Size = new SizeF(200, 40);

				e.Graphics.FillRectangle(Brushes.LightSkyBlue, ClientRectangle);
				e.Graphics.FillRectangle(Brushes.DimGray, startGameButton.Body);
				e.Graphics.FillRectangle(Brushes.DimGray, quitGameButton.Body);

				StringFormat format = new StringFormat();
				format.Alignment = StringAlignment.Center;
				format.LineAlignment = StringAlignment.Center;
				Font titleFont = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold);

				e.Graphics.DrawString("PORTAL 2D", titleFont, Brushes.WhiteSmoke, new PointF(ClientSize.Width / 2 - 150, 60));
				e.Graphics.DrawString(startGameButton.Text, new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold), Brushes.WhiteSmoke, new PointF(startGameButton.Location.X + startGameButton.Body.Width / 2, startGameButton.Location.Y + startGameButton.Body.Height / 2), format);
				e.Graphics.DrawString(quitGameButton.Text, new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold), Brushes.WhiteSmoke, new PointF(quitGameButton.Location.X + quitGameButton.Body.Width / 2, quitGameButton.Location.Y + quitGameButton.Body.Height / 2), format);
			}
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
					if (chars[0].Velocity.Height == 0 && verticalVelocityLastTick == 0) // Disable multi jump
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
			if (e.KeyCode == Keys.O && !currentlyInMenu)
			{
				ofd.ShowDialog();
			}
			if (e.KeyCode == Keys.R)
			{
				resetPortals();
			}
		}
	}
}