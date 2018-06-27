using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEditorP2D
{
	public partial class main : Form
	{
		
		
		public main()
		{
			InitializeComponent();

			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			ClientSize = new Size(1024, 768);

			controller.loadTextures();

			tilePanelForm tpF = new tilePanelForm();
			tpF.Show();

			controller.tilesArray = new Tile[768];
			int counter = 0;
			for (int i = 0; i < 24; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					controller.tilesArray[counter] = new Tile(j * 32, i * 32, 0);
					counter++;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			#region Tiles
			foreach (Tile item in controller.tilesArray)
			{
				e.Graphics.DrawImage(controller.BitmapArray[item.ImageID], item.Location);
			}
			#endregion

			#region Grid
			for (int i = 0; i < ClientSize.Width / 32; i++)
			{
				e.Graphics.DrawLine(Pens.Yellow, new Point(32 * i, 0), new Point(32 * i, ClientSize.Height));
			}
			for (int i = 0; i < ClientSize.Height / 32; i++)
			{
				e.Graphics.DrawLine(Pens.Yellow, new Point(0, 32 * i), new Point(ClientSize.Width, 32 * i));
			}
			#endregion

		}
		

		private void main_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ClientRectangle.Contains(e.Location))
			{
				int tileX = Convert.ToInt32(((double)e.Location.X / 32).ToString().Split(',')[0]);
				int tileY = Convert.ToInt32(((double)e.Location.Y / 32).ToString().Split(',')[0]);
				int i = tileX + tileY * 32;
				if (controller.SelectedIndexLeft < controller.BitmapArray.Length)
				{
					controller.tilesArray[i].ImageID = controller.SelectedIndexLeft;
				}
				else
				{
					if (controller.SelectedIndexLeft == controller.BitmapArray.Length)
						controller.startPoint = controller.tilesArray[i].Location;
					else if (controller.SelectedIndexLeft == controller.BitmapArray.Length + 1)
						controller.endPoint = controller.tilesArray[i].Location;
				}
			}
			Invalidate();
		}
	}
}
