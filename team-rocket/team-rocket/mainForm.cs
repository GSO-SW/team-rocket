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

        public main()
        {
            InitializeComponent();

            ClientSize = new Size(1024, 768); //32*32 | 24*32

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            updateGraphicsTimer = new Timer();
            updateGraphicsTimer.Interval = 20;
            updateGraphicsTimer.Tick += UpdateGraphichsTimer_Tick;
            updateGraphicsTimer.Start();

			bitmapArray = new Bitmap[1];
			bitmapArray[0] = new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\gfx\default.png");

            tilesArray = new Tile[768];
			int counter = 0;
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 32; j++)
                {
					tilesArray[counter] = new Tile(j * 32, i * 32, 0);
					counter++;
                }
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
        }
    }
}
