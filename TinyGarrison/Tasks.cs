using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison
{
	class Tasks
	{
		public static async Task<bool> MoveTo(WoWPoint location)
		{
			var r = await CommonCoroutines.MoveTo(location);
			if (r != MoveResult.ReachedDestination) return true;

			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> LootGarrisonCache(HashSet<uint> entries)
		{
			WoWGameObject garrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => Data.GarrisonCache.Contains(o.Entry))
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (garrisonCache != null && garrisonCache.IsValid)
			{
				Helpers.Log("Looting " + garrisonCache.Name);
				garrisonCache.Interact();
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 3000);
				Jobs.NextJob();
				return true;
			}

			return true;
		}

		public static async Task<bool> LootShipment(HashSet<uint> entries)
		{
			GarrisonInfo.

			WoWGameObject shipmentCrate =
					ObjectManager.GetObjectsOfType<WoWGameObject>()
						.Where(o => Data.Shipment.Contains(o.Entry))
						.OrderBy(o => o.Distance).FirstOrDefault();

			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);

			if (shipmentCrate != null && shipmentCrate.IsValid)
			{
				Helpers.Log("Looting " + shipmentCrate.Name);
				shipmentCrate.Interact();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				return true;
			}

			return true;

			// Done
			Jobs.NextJob();
			return true;
		}
	}
}
