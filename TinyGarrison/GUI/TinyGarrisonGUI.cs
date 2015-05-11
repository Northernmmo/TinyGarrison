using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyGarrison.GUI;

namespace TinyGarrison.GUI
{
	public partial class TinyGarrisonGUI : Form
	{
		public TinyGarrisonGUI()
		{
			InitializeComponent();
			CraftSecrets.Checked = TinyGarrisonSettings.Instance.CraftSecrets;
			TransmuteBlood.Checked = TinyGarrisonSettings.Instance.TransmuteBlood;
			OpenFollowerUpgrades.Checked = TinyGarrisonSettings.Instance.OpenFollowerUpgrades;
			OpenGearTokens.Checked = TinyGarrisonSettings.Instance.OpenGearTokens;
			BuySavageBlood.Checked = TinyGarrisonSettings.Instance.BuySavageBlood;
			UseRushOrders.Checked = TinyGarrisonSettings.Instance.UseRushOrders;
			SkipJewelcraftingWOs.Checked = TinyGarrisonSettings.Instance.SkipJewelcraftingWOs;
		}

		private void Save_Click(object sender, EventArgs e)
		{
			TinyGarrisonSettings.Instance.CraftSecrets = CraftSecrets.Checked;
			TinyGarrisonSettings.Instance.TransmuteBlood = TransmuteBlood.Checked;
			TinyGarrisonSettings.Instance.OpenFollowerUpgrades = OpenFollowerUpgrades.Checked;
			TinyGarrisonSettings.Instance.OpenGearTokens = OpenGearTokens.Checked;
			TinyGarrisonSettings.Instance.BuySavageBlood = BuySavageBlood.Checked;
			TinyGarrisonSettings.Instance.UseRushOrders = UseRushOrders.Checked;
			TinyGarrisonSettings.Instance.SkipJewelcraftingWOs = SkipJewelcraftingWOs.Checked;

			TinyGarrisonSettings.Instance.Save();
			if (ActiveForm != null) ActiveForm.Close();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
