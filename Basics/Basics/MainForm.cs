using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basics
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the painting on the MainForm.
        /// </summary>
        /// <param name="e">Contains the graphics object needed to paint.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Has always to be the first line of the overriden OnPaint-Method.
            base.OnPaint(e);
            // Get the graphics object.
            Graphics graphics = e.Graphics;
            // Prepare a Pen and a Rectagle to paint later on.
            Pen rectaglePen = Pens.Blue;
            Rectangle rectangle = new Rectangle(10, 20, 100, 50);
            // Actually draw the Rectagle using the selected Pen.
            graphics.DrawRectangle(rectaglePen, rectangle);
        }
    }
}
