using System.Linq;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison.Tasks
{
	class HerbGarden
	{
		private static bool _alreadyMoved;

		public static async Task<bool> Handler()
		{
			// Move to Job
			if (!_alreadyMoved)
			{
				_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
				return true;
			}

			// Loot Shipments
			await Helpers.LootShipment();

			// Gather
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
					Helpers.MoveTo(herbObj);

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

			// Start Work Orders
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);

			if (GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated !=
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity && Helpers.HasWorkOrderMaterial())
			{
				WoWUnit workOrderNpc =
				ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => o.Entry == Jobs.CurrentJob().WorkOrderNpcEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

				if (workOrderNpc != null && workOrderNpc.IsValid)
				{
					if (!workOrderNpc.WithinInteractRange)
						await Helpers.MoveTo(workOrderNpc);

					Helpers.Log("Starting " + Jobs.CurrentJob().Name + " work orders");
					workOrderNpc.Interact();
					await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_INFO", 3000);
					Lua.DoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
					Lua.DoString("GarrisonCapacitiveDisplayFrameCloseButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_CLOSED", 3000);
					return true;
				}

				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}
	}
}
