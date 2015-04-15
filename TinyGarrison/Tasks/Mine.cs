using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison.Tasks
{
	class Mine
	{
		public static void Check()
		{
			if (!GUI.TinyGarrisonSettings.Instance.GardenMine) return;
			Jobs.Add(JobType.Move, new WoWPoint(5475.488, 4452.166, 144.4591));
			Jobs.Add(JobType.Mine);
			Helpers.Log("Added Job: Mine");
		}
	}
}
