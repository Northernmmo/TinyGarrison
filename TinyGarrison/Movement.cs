using Styx;
using Styx.CommonBot.Coroutines;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison
{
	class Movement
	{
		private static readonly LocalPlayer Me = StyxWoW.Me;

		public static async Task<bool> MoveTo(WoWPoint destination)
		{
			var r = await CommonCoroutines.MoveTo(destination);

			if (r == MoveResult.ReachedDestination)
			{
				Jobs.NextJob();
				return true;
			}

			return true;
		}
	}
}
