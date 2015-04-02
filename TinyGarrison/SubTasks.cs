using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;

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

			Jobs.NextSubTask();
			return true;
		}

		public static async Task<bool> GatherHerbs()
		{
			WoWGameObject herbObj =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.CanUse() && o.Distance < 150)
					.Where(
						o =>
							o.Entry == 235389 || o.Entry == 235391 || o.Entry == 235388 || o.Entry == 235390 ||
							o.Entry == 235376 || o.Entry == 235387)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (herbObj != null && herbObj.IsValid)
			{
				Helpers.Log("Gathering Herbs");
				if (!herbObj.WithinInteractRange)
					return await Helpers.MoveTo(herbObj);

				await CommonCoroutines.SleepForLagDuration();
				if (StyxWoW.Me.Combat)
					return true;
				herbObj.Interact();
				if (StyxWoW.Me.Combat)
					return true;
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);

				return true;
			}

			Jobs.NextSubTask();
			return true;
		}

		public static async Task<bool> GatherOre()
		{
			WoWGameObject oreObj =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.CanUse() && o.Distance < 150)
					.Where(
						o =>
							o.Entry == 232545 || o.Entry == 232544 || o.Entry == 232543 || o.Entry == 232541 ||
							o.Entry == 232542)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (oreObj != null && oreObj.IsValid)
			{
				Helpers.Log("Gathering Ore");
				if (!oreObj.WithinInteractRange)
					return await Helpers.MoveTo(oreObj);

				await CommonCoroutines.SleepForLagDuration();
				if (StyxWoW.Me.Combat)
					return true;
				oreObj.Interact();
				if (StyxWoW.Me.Combat)
					return true;
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);

				return true;
			}

			Jobs.NextSubTask();
			return true;
		}
	}
}
