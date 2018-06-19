using System;
using System.Windows.Forms;

namespace MapEditorP2D
{
	public partial class tilePanelForm : Form
	{
		public tilePanelForm()
		{
			InitializeComponent();

			listBox1.Items.Add("default");
			listBox1.Items.Add("backgroud_1");
			listBox1.Items.Add("ground_1");
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			controller.SelectedIndexLeft = listBox1.SelectedIndex;
		}
	}
}
