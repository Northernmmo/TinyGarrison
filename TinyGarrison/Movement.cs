using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.Pathing;

namespace TinyGarrison
{
	class Movement
	{
		public static async Task<bool> MoveTo(WoWPoint location)
		{
			var r = await CommonCoroutines.MoveTo(location);

			if (r != MoveResult.ReachedDestination) return true;

			Jobs.NextJob();
			return true;
		}
	}
}
