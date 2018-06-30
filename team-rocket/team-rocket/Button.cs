using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace team_rocket
{
    /// <summary>
    /// Configurable buttons for the user to press in menus for example.
    /// </summary>
    class Button
    {
        RectangleF body;
        string text;
        Color bodyColor;

        #region Properties
        /// <summary>
        /// Represents the body of the button as a RectangleF.
        /// </summary>
        public RectangleF Body
        {
            get { return body; }
            set { body = value; }
        }

        /// <summary>
        /// The text, which will be shown in the center of the button.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// The color of the buttons body.
        /// </summary>
        public Color BodyColor
        {
            get { return bodyColor; }
            set { bodyColor = value; }
        }

        /// <summary>
        /// The Position of the upper left corner of the button.
        /// </summary>
        public PointF Location
        {
            get { return Body.Location; }
            set { body.Location = value; }
        }
        #endregion

        /// <summary>
        /// A standard button with 64*32 px, a gray body and no text.
        /// </summary>
        /// <param name="position"></param>
        public Button(PointF position)
        {
            RectangleF rect = new RectangleF();
            rect.Width = 64;
            rect.Height = 32;
            rect.Location = position;
            Body = rect;
            Text = "";
            BodyColor = Color.Gray;
        }
    }
}
