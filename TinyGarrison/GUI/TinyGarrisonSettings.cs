using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.Common;
using Styx.Helpers;

namespace TinyGarrison.GUI
{
	class TinyGarrisonSettings : Settings
	{
		public static readonly TinyGarrisonSettings Instance = new TinyGarrisonSettings();

		public TinyGarrisonSettings()
			: base(Path.Combine(System.Environment.CurrentDirectory, string.Format(@"/BotBase/TinyGarrison/Settings/TinyGarrison-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        [Setting, DefaultValue(true)]
        public bool GarrisonCache { get; set; }

		[Setting, DefaultValue(true)]
		public bool GardenMine { get; set; }
	}
}
