using System;
using System.Linq;
using System.Threading.Tasks;
using Bots.Grind;
using CommonBehaviors.Actions;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Buddy.Coroutines;
using Styx.WoWInternals.DB;

namespace TinyGarrison.Tasks
{
	class GarrisonCache
	{
		private static bool _alreadyMoved = false;

		public static async Task<bool> Handler()
		{
			// Check if GarrisonCache exists when we get close
			if (!_alreadyMoved && Jobs.CurrentJob().Location.Distance(StyxWoW.Me.Location) < 100)
			{
				bool PossibleGarrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Any(o => o.Entry == Jobs.CurrentJob().ShipmentCrateEntry);
				if (!PossibleGarrisonCache) _alreadyMoved = true;
			}

			// Move to Job
			if (!_alreadyMoved)
			{
				_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
				return true;
			}

			// Loot GarrisonCache
			WoWGameObject GarrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.Entry == Jobs.CurrentJob().ShipmentCrateEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (GarrisonCache != null && GarrisonCache.IsValid)
			{
				Helpers.Log("Looting " + GarrisonCache.Name);
				GarrisonCache.Interact();
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 6000);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}
	}
}
