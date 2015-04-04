using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison.Tasks
{
	class Profession
	{
		private static bool _alreadyMoved;

		public static async Task<bool> Handler()
		{
			// Move to Job
			if (!_alreadyMoved)
			{
				_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
				return true;
			}

			if (WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).CanCast && !WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).Cooldown && Helpers.HasProfessionMaterial())
			{
				// Cast daily cooldown
				await CommonCoroutines.SleepForLagDuration();
				WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).Cast();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				return true;
			}

			Jobs.NextJob();
			return true;
		}
	}
}
