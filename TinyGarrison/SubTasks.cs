using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison
{
	class SubTasks
	{
		public static async Task<bool> LootShipments()
		{
			WoWGameObject ShipmentCrate =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.Entry == Jobs.CurrentJob().ShipmentCrateEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (ShipmentCrate != null && ShipmentCrate.IsValid)
			{
				Helpers.Log("Looting " + ShipmentCrate.Name);
				ShipmentCrate.Interact();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
			}

			Jobs.NextSubTask();
			return true;
		}

		public static async Task<bool> LootGarrisonCache()
		{
			Helpers.Log("checking garrisoncaceh");
			WoWGameObject GarrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.Entry == Jobs.CurrentJob().ShipmentCrateEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (GarrisonCache != null && GarrisonCache.IsValid)
			{
				Helpers.Log("Looting " + GarrisonCache.Name);
				GarrisonCache.Interact();
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 3000);
			}

			Helpers.Log("next subtask");
			Jobs.NextSubTask();
			return true;
		}
	}
}
