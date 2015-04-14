using System.Linq;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System.Collections.Generic;

namespace TinyGarrison.Tasks
{
	class GarrisonCache
	{
		public static void AddJob()
		{
			Jobs.Add(JobType.GarrisonCache, new WoWPoint);
		}

		public static async Task<bool> Execute()
		{
			// Loot GarrisonCache
			WoWGameObject garrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => Data.GarrisonCache.Contains(o.Entry))
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (garrisonCache != null && garrisonCache.IsValid)
			{
				Helpers.Log("Looting " + garrisonCache.Name);
				garrisonCache.Interact();
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 3000);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}
	}
}

/*private static bool _alreadyMoved;

		public static async Task<bool> Handler()
		{
			// Check if GarrisonCache exists when we get close
			if (!_alreadyMoved && Jobs.CurrentJob().Location.Distance(StyxWoW.Me.Location) < 100)
			{
				bool possibleGarrisonCache =
					ObjectManager.GetObjectsOfType<WoWGameObject>()
						.Any(o => o.Entry == 235389 || o.Entry == 237191 || o.Entry == 237720);
				if (!possibleGarrisonCache) _alreadyMoved = true;
			}

			// Move to Job
			if (!_alreadyMoved)
			{
				_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
				return true;
			}

			// Loot GarrisonCache
			WoWGameObject garrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.Entry == 235389 || o.Entry == 237191 || o.Entry == 237720)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (garrisonCache != null && garrisonCache.IsValid)
			{
				Helpers.Log("Looting " + garrisonCache.Name);
				garrisonCache.Interact();
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 6000);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}*/