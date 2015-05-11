using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx.Helpers;

namespace TinyGarrison.GUI
{
	class TinyGarrisonSettings : Settings
	{
		public static readonly TinyGarrisonSettings Instance = new TinyGarrisonSettings();

		public TinyGarrisonSettings() : base(Path.Combine(CharacterSettingsDirectory, "TinyGarrisonSettings.xml")) { }

		[Setting, DefaultValue(false)]
		public bool CraftSecrets { get; set; }

		[Setting, DefaultValue(true)]
		public bool TransmuteBlood { get; set; }

		[Setting, DefaultValue(true)]
		public bool BuySavageBlood { get; set; }

		[Setting, DefaultValue(true)]
		public bool OpenFollowerUpgrades { get; set; }

		[Setting, DefaultValue(true)]
		public bool OpenGearTokens { get; set; }

		[Setting, DefaultValue(true)]
		public bool UseRushOrders { get; set; }

		[Setting, DefaultValue(true)]
		public bool SkipJewelcraftingWOs { get; set; }
	}
}
