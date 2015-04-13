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
		}

		private void GarrisonCache_CheckedChanged(object sender, EventArgs e)
		{
			 GarrisonCache.Checked;
		}
	}
}
