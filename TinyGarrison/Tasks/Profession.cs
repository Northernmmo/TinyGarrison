using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison.Tasks
{
	class Profession
	{
		private static bool _alreadyMoved;

		public static async Task<bool> Handler()
		{
			if (!WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).Cooldown)
			{
				// Move to Job
				if (!_alreadyMoved)
				{
					_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
					return true;
				}

				// Craft any needed material
				if (Jobs.CurrentJob().ProfessionNpcEntry == 169092 && Lua.GetReturnVal<bool>("return GetItemCount('Luminous Shard') < 1 and GetItemCount('Draenic Dust') >= 20", 0)) //Enchanting
				{
					await CommonCoroutines.SleepForLagDuration();
					WoWSpell.FromId(169091).Cast();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					return true;
				}

				if (Jobs.CurrentJob().ProfessionNpcEntry == 169081 && Lua.GetReturnVal<bool>("return GetItemCount('Cerulean Pigment') <= 10", 0)) //Inscription
				{
					WoWItem herbStack = ObjectManager.GetObjectsOfType<WoWItem>().Where(o =>
						o.Entry == 109124 || o.Entry == 109125 || o.Entry == 109126 || o.Entry == 109127 || o.Entry == 109128 || o.Entry == 109129)
						.OrderByDescending(o => o.StackCount).FirstOrDefault();
				
					if (herbStack != null && herbStack.IsValid  && herbStack.StackCount >= 5 )
					{
						await CommonCoroutines.SleepForLagDuration();
						WoWSpell.FromId(51005).Cast();
						await CommonCoroutines.SleepForLagDuration();
						herbStack.Interact();
						await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
						await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
						return true;
					}

					return true;
				}

				// Cast daily cooldown
				if (Helpers.HasProfessionMaterial() && WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).CanCast)
				{
					await CommonCoroutines.SleepForLagDuration();
					WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).Cast();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					return true;
				}

				return true;
			}

			Jobs.NextJob();
			return true;
		}
	}
}
