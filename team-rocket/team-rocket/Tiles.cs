using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
    // This class resembles the tiles ingame, which serves as walls and platforms for the player.
    class Tiles
    {
        Rectangle rectangle;
        bool hitboxFlag;

        public Tiles()
        {
            rectangle = new Rectangle();
            rectangle.Height = 32;
            rectangle.Width = 32;
        }

        /// <summary>
        /// Returns the location of the upper left and right corner of the rectangle
        /// as a Point-Structure.
        /// </summary>
        public Point Position
        {
            get
            {
                return rectangle.Location;
            }
        }

        /// <summary>
        /// Flag to determine, whether or not the tile has a hitbox.
        /// </summary>
        public bool HitboxFlag
        {
            get
            {
                return hitboxFlag;
            }
        }
    }
}
