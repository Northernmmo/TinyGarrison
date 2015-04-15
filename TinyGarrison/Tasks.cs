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
using Styx.Helpers;

namespace TinyGarrison
{
	class Tasks
	{
		public static async Task<bool> MoveTo()
		{
			var r = await CommonCoroutines.MoveTo(Jobs.CurrentJob.Location);
			if (r != MoveResult.ReachedDestination) return true;

			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> MoveTo(WoWGameObject destinationObject)
		{
			var location = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, destinationObject.Location, destinationObject.InteractRange - 2);
			var r = await CommonCoroutines.MoveTo(location);

			return r == MoveResult.ReachedDestination;
		}

		public static async Task<bool> MoveTo(WoWUnit destinationUnit)
		{
			var location = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, destinationUnit.Location, destinationUnit.InteractRange - 2);
			var r = await CommonCoroutines.MoveTo(location);

			return r == MoveResult.ReachedDestination;
		}

		public static async Task<bool> LootGarrisonCache()
		{
			WoWGameObject garrisonCache =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => Jobs.CurrentJob.Entries.Contains(o.Entry))
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (garrisonCache != null && garrisonCache.IsValid)
			{
				Helpers.Log("Looting " + garrisonCache.Name);
				garrisonCache.Interact();
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 3000);
				return true;
			}

			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> LootShipment()
		{
			WoWGameObject shipmentCrate =
					ObjectManager.GetObjectsOfType<WoWGameObject>()
						.Where(o => Jobs.CurrentJob.Entries.Contains(o.Entry))
						.OrderBy(o => o.Distance).FirstOrDefault();

			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);
			var shipmentsReady = GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).LandingPageInfo.ShipmentsReady;

			if (shipmentCrate != null && shipmentCrate.IsValid && shipmentsReady > 0)
			{
				Helpers.Log("Looting " + shipmentCrate.Name);
				shipmentCrate.Interact();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> GatherResources()
		{
			WoWGameObject resourceObj =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.CanUse())
					.Where(
						o => Jobs.CurrentJob.Entries.Contains(o.Entry))
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (resourceObj != null && resourceObj.IsValid)
			{
				Helpers.Log("Gathering " + resourceObj.Name);
				if (!resourceObj.WithinInteractRange)
				{
					await MoveTo(resourceObj);
					return true;
				}

				await CommonCoroutines.SleepForLagDuration();
				if (StyxWoW.Me.Combat)
					return true;
				resourceObj.Interact();
				if (StyxWoW.Me.Combat)
					return true;
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> StartWorkOrders()
		{
			WoWUnit workOrderNpc =
				ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => Jobs.CurrentJob.Entries.Contains(o.Entry))
					.OrderBy(o => o.Distance).FirstOrDefault();

			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);
			var workOrdersCanStart = 
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).ShipmentCapacity -
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).LandingPageInfo.ShipmentsCreated;

			if (workOrderNpc != null && workOrderNpc.IsValid && workOrdersCanStart > 0)
			{
				if (!workOrderNpc.WithinInteractRange)
				{
					await MoveTo(workOrderNpc);
					return true;
				}

				Helpers.Log("Starting " + Jobs.CurrentJob.Building + " work orders");
				workOrderNpc.Interact();
				await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_INFO", 3000);
				Lua.DoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
				await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
				Lua.DoString("GarrisonCapacitiveDisplayFrameCloseButton:Click()");
				await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_CLOSED", 3000);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}
	}
}
