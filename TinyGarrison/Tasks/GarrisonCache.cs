using System.Linq;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison.Tasks
{
	class GarrisonCache
	{
		public static void Check()
		{
			if (!GUI.TinyGarrisonSettings.Instance.GarrisonCache) return;
			Jobs.Add(JobType.Move, new WoWPoint(5593.86, 4586.82, 136.61));
			Jobs.Add(JobType.GarrisonCahce);
			Helpers.Log("Added Job: GarrisonCache");
		}

		public static async Task<bool> Execute()
		{
			// Loot GarrisonCache
			var garrisonCache =
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
