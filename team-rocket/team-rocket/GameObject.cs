using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
    /// <summary>
    /// Every Object that is a game element derives from this class
    /// </summary>
    abstract class GameObject
    {
        protected bool movable;
        protected RectangleF hitbox;

        /// <summary>
        /// Depicts the hitbox of an object as a rectangle. Sets and returns the whole Rectangle object.
        /// </summary>
        virtual public RectangleF Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        /// <summary>
        /// Sets and returns the location of the GameObject as a Point object.
        /// </summary>
        virtual public PointF Location
        {
            get { return hitbox.Location; }
            set { hitbox.Location = value; }
        }

        virtual public bool Movable
        {
            get { return movable; }
            set { movable = value; }
        }
    }
}
