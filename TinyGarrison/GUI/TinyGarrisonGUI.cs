using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyGarrison.GUI
{
	public partial class TinyGarrisonGUI : Form
	{
		public TinyGarrisonGUI()
		{
			InitializeComponent();
			GarrisonCache.Checked = TinyGarrisonSettings.Instance.GarrisonCache;
			GardenMine.Checked = TinyGarrisonSettings.Instance.GardenMine;
		}

		private void Save_Click(object sender, EventArgs e)
		{
			TinyGarrisonSettings.Instance.GarrisonCache = GarrisonCache.Checked;
			TinyGarrisonSettings.Instance.GardenMine = GardenMine.Checked;
			TinyGarrisonGUI.ActiveForm.Close();
		}
	}
}
