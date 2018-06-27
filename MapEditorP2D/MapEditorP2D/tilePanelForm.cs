using System;
using System.Windows.Forms;

namespace MapEditorP2D
{
	public partial class tilePanelForm : Form
	{
		public tilePanelForm()
		{
			InitializeComponent();

			listBox1.Items.Add("default"); //0
			listBox1.Items.Add("Metall Vordergrund");//1
			listBox1.Items.Add("Metall Hintergrund");//2
			listBox1.Items.Add("Ground 1");//3
			listBox1.Items.Add("Dreckiges Wasser");//4
			listBox1.Items.Add("Tür Teil: 1");//5
			listBox1.Items.Add("Tür Teil: 2");//6
			listBox1.Items.Add("Tür Teil: 3");//7
			listBox1.Items.Add("Tür Teil: 4");//8
			listBox1.Items.Add("Startpunkt");//9
			listBox1.Items.Add("Endpunkt");//10
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			controller.SelectedIndexLeft = listBox1.SelectedIndex;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			saveFileDialog1.InitialDirectory = Application.StartupPath;
			saveFileDialog1.ShowDialog();
		}

		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!e.Cancel)
				controller.saveMap(saveFileDialog1.FileName);
		}
	}
}
