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
			BuySavageBlood.Checked = TinyGarrisonSettings.Instance.BuySavageBlood;
			ProfessionDailies.Checked = TinyGarrisonSettings.Instance.ProfessionDailies;
			ProfessionBuildings.Checked = TinyGarrisonSettings.Instance.ProfessionBuildings;
			Salvage.Checked = TinyGarrisonSettings.Instance.Salvage;
		}

		private void Save_Click(object sender, EventArgs e)
		{
			TinyGarrisonSettings.Instance.GarrisonCache = GarrisonCache.Checked;
			TinyGarrisonSettings.Instance.GardenMine = GardenMine.Checked;
			TinyGarrisonSettings.Instance.BuySavageBlood = BuySavageBlood.Checked;
			TinyGarrisonSettings.Instance.ProfessionDailies = ProfessionDailies.Checked;
			TinyGarrisonSettings.Instance.ProfessionBuildings = ProfessionBuildings.Checked;
			TinyGarrisonSettings.Instance.Salvage = Salvage.Checked;
			TinyGarrisonGUI.ActiveForm.Close();
		}
	}
}
