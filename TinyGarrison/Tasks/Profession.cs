using System.Linq;
using System.Threading.Tasks;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
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

					if (herbStack != null && herbStack.IsValid  && herbStack.StackCount >= 5)
					{
						WoWSpell.FromId(51005).Cast();
						herbStack.Interact();
						await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
						await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
						return true;
					}
				}

				// Cast daily cooldown
				if (Helpers.HasProfessionMaterial() && WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).CanCast)
				{
					// Move to Job
					if (!_alreadyMoved)
					{
						_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
						return true;
					}

					Helpers.Log("Crafting " + WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).Name);
					await CommonCoroutines.SleepForLagDuration();
					WoWSpell.FromId(Jobs.CurrentJob().ProfessionNpcEntry).Cast();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					return true;
				}
			}

			Jobs.NextJob();
			return true;
		}
	}
}
