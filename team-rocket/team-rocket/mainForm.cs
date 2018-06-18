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
        float velocity; //Unit px/tick
        List<GameObject> gameObjects;
        List<GameObject> movableObjects;
        List<GameObject> collidingObjects;

        public main()
        {
            InitializeComponent();

            ClientSize = new Size(1024, 768); //32*32 | 24*32

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            bTempA = bNowA = bTempD = bNowD = bTempSpace = bNowSpace = false;
            velocity = 5;
            collidingObjects = new List<GameObject>();
            gameObjects = new List<GameObject>();
            movableObjects = new List<GameObject>();

            #region Initialize Frame Timer
            // The timer which determines the FPS
            updateGraphicsTimer = new Timer();
            updateGraphicsTimer.Interval = 20;
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
            testLevel = new Level(imageIDs, new Point(0, 640), new Point(1024 - 32, 640));

            loadLevel(testLevel);
            #endregion

            character = new Character(new Point(50, 50));

            gameObjects.Add(character);
            for (int i = 0; i < tilesArray.Length; i++)
            {
                gameObjects.Add(tilesArray[i]);
            }
            foreach (GameObject obj in gameObjects)
            {
                if (obj.Movable)
                {
                    movableObjects.Add(obj);
                }
            }

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
            /*
            if (character.Location.Y < 600)
            {
                character.Velocity = new SizeF(character.Velocity.Width, character.Velocity.Height + 0.5f);
            }
            if (character.Location.Y > 600)
            {
                character.Velocity = new SizeF(character.Velocity.Width, 0);
                character.Location = new PointF(character.Location.X, 600);
            }

            character.Location += character.Velocity;*/

            CollisionDetection();
            for(int i = 0; i < movableObjects.Count; i++)
            {
                movableObjects[i].Location += movableObjects[i].Velocity;
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
                e.Graphics.DrawImage(bitmapArray[item.ImageID], item.Hitbox.Location);
            }
            e.Graphics.FillRectangle(Brushes.Red, new RectangleF(character.Location, character.Hitbox.Size));
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

        private void CollisionDetection()
        {
            bool collisionDetected = false;
            for (int i = 0; i < movableObjects.Count; i++)
            {
                // Generate an object that acts like a Timer_tick has passed
                GameObject futureObj1 = movableObjects[i];
                futureObj1.Location += futureObj1.Velocity;
                collisionDetected = false;

                for (int j = 0; j < gameObjects.Count; j++)
                {
                    // Do the same with the second object
                    GameObject futureObj2 = gameObjects[j];
                    futureObj2.Location += futureObj2.Velocity;

                    // Obj1 contains itself in gameObjects list at some point, so skip that case
                    // and check if both hitboxes intersect
                    if (gameObjects[j].HitboxFlag && movableObjects[i] != gameObjects[j] && futureObj1.Hitbox.IntersectsWith(futureObj2.Hitbox))
                    {
                        collisionDetected = true;
                        if (movableObjects[i].Movable)
                        {
                            movableObjects[i].Velocity = new SizeF(futureObj2.Hitbox.X - (futureObj2.Hitbox.X + futureObj1.Hitbox.Width), futureObj2.Hitbox.Y - (futureObj2.Hitbox.Y + futureObj1.Hitbox.Height));
                        }
                        if (gameObjects[j].Movable)
                        {
                            gameObjects[j].Velocity = new SizeF(futureObj2.Hitbox.X - (futureObj2.Hitbox.X + futureObj1.Hitbox.Width), futureObj2.Hitbox.Y - (futureObj2.Hitbox.Y + futureObj1.Hitbox.Height));
                        }
                    }
                }
                if (!collisionDetected)
                {
                    movableObjects[i].Velocity = new SizeF(movableObjects[i].Velocity.Width, movableObjects[i].Velocity.Height + 0.5f);
                }
            }
        }
    }
}