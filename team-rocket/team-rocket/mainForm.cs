using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace team_rocket
{
    public partial class main : Form
    {
        /// BUGS:
        /// - when the player hits a tile from the side and is a little bit under the upper edge, he will bounch up
        /// 
        /// - jumping is spamable

        Timer updateGraphicsTimer;
        Tile[] tilesArray;
        Bitmap[] bitmapArray;
        Level testLevel;
        Character character;
        bool bTempA, bNowA;
        bool bTempD, bNowD;
        bool bTempSpace, bNowSpace;
        float g; //Gravitational acceleration
        float velocityLR, jumpVelocity; //Unit px/tick for the left or right and for the jumpspeed

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
            #endregion

            #region Subscribe events
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            #endregion

            #region Initialize Frame Timer
            updateGraphicsTimer = new Timer
            {
                Interval = 20
            };
            updateGraphicsTimer.Tick += OnTimerTick;
            updateGraphicsTimer.Start();
            #endregion

            #region Loading Textures
            bitmapArray = new Bitmap[3];
            bitmapArray[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\default.png");
            bitmapArray[1] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\background_1.png");
            bitmapArray[2] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\ground_1.png");

            /* List of bitmap array with id
			 * 
			 *  = #/i =    = name =
			 *    0         default
			 *    1         background_1
			 *    2         ground_1
			 */
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
                if (i >= 710 || i == 300)
                    imageIDs[i] = 2;
            }
            testLevel = new Level(imageIDs, new Point(1024 / 2, 768 / 2), new Point(1024 - 32, 640));

            loadLevel(testLevel);
            #endregion

            // Spawn Character
            character = new Character(testLevel.StartPoint);
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
            if (velocity.Height < 30)
            {
                velocity.Height += g;
            }

            RectangleF futurePlayer = new RectangleF(player.Location + velocity, player.Size);

            //List<int> collidedTileIndex = new List<int>();
            List<int> futureCollidedTileIndex = new List<int>();

            for (int i = 0; i < tilesArray.Length; i++)
            {
                if (tilesArray[i].HitboxFlag)
                {
                    /*if (tilesArray[i].Rect.IntersectsWith(player))
					{
						collidedTileIndex.Add(i);
					}*/
                    if (tilesArray[i].Rect.IntersectsWith(futurePlayer))
                    {
                        futureCollidedTileIndex.Add(i);
                    }
                }
            }

            // player position in tiles
            Point playerTile = new Point(Convert.ToInt32(Math.Round(player.X / 32)), Convert.ToInt32(Math.Round(player.Y / 32)));
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

            if (futureCollidedTileIndex.Count > 0)
            {
                if (velocity.Height > 0 && heightBorder != -1) //Meaning falling
                {
                    //Oberkante der kachel muss betrachtet werden
                    if (heightBorder < (player.Y + player.Height))
                        player.Y = player.Y - ((player.Y + player.Height) + heightBorder);
                    velocity.Height = heightBorder - (player.Y + player.Height);
                }
                else if (velocity.Height < 0 && heightBorder != -1) //Meaning jumping
                {
                    //Unterkante der kachel muss betrachtet werden
                    velocity.Height = heightBorder - player.Y;
                }

                if (velocity.Width > 0 && widthBorder != -1) //Meaning movement to the right
                {
                    //linke seite der kachel muss betrachtet werden
                    velocity.Width = widthBorder - (player.X + player.Width);
                }
                else if (velocity.Width < 0 && widthBorder != -1) //Meaning movement to the left
                {
                    //rechte seite der Kachel muss betrachtet werden
                    velocity.Width = widthBorder - player.X;
                }
            }

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
            foreach (Tile item in tilesArray)
            {
                e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Rect.Location);
            }
            e.Graphics.FillRectangle(Brushes.Blue, character.RectF);
            e.Graphics.DrawString(character.RectF.Location.ToString(), Font, Brushes.Black, new Point(100, 100));
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
                    character.Velocity = new SizeF(character.Velocity.Width - velocityLR, character.Velocity.Height);
                }
            }

            if (e.KeyCode == Keys.D) // see above
            {
                bTempD = bNowD;
                bNowD = true;
                if (bNowD != bTempD)
                {
                    character.Velocity = new SizeF(character.Velocity.Width + velocityLR, character.Velocity.Height);
                }
            }

            if (e.KeyCode == Keys.Space) // see above
            {
                bTempSpace = bNowSpace;
                bNowSpace = true;
                if (bNowSpace != bTempSpace)
                {
                    character.Velocity = new SizeF(character.Velocity.Width, character.Velocity.Height - jumpVelocity);
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
                {
                    character.Velocity = new SizeF(character.Velocity.Width + velocityLR, character.Velocity.Height);
                }
                else
                {
                    character.Velocity = new SizeF(0, character.Velocity.Height);
                }
            }
            if (e.KeyCode == Keys.D)
            {
                bNowD = bTempD = false;
                if (bNowA)
                {
                    character.Velocity = new SizeF(character.Velocity.Width - velocityLR, character.Velocity.Height);
                }
                else
                {
                    character.Velocity = new SizeF(0, character.Velocity.Height);
                }
            }
            if (e.KeyCode == Keys.Space)
            {
                bNowSpace = bTempSpace = false;
            }
        }
    }
}