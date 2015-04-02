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

			ShipmentCrate.Interact();
			await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
			await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
			return true;
		}
	}
}
