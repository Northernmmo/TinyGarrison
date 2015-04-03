using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison.Tasks
{
	class ProfessionBuilding
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

			// Start Work Orders
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);

			if (GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated !=
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity && Helpers.HasWorkOrderMaterial())
			{
				WoWUnit WorkOrderNpc =
				ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => o.Entry == Jobs.CurrentJob().WorkOrderNpcEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

				if (WorkOrderNpc != null && WorkOrderNpc.IsValid)
				{
					if (!WorkOrderNpc.WithinInteractRange)
						await Helpers.MoveTo(WorkOrderNpc);

					Helpers.Log("Starting " + Jobs.CurrentJob().Name + " work orders");
					WorkOrderNpc.Interact();
					await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_INFO", 3000);
					Lua.DoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
					Lua.DoString("GarrisonCapacitiveDisplayFrameCloseButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_CLOSED", 3000);
					return true;
				}
			}

			// Done
			_alreadyMoved = false;
			Jobs.NextJob();
			return true;
		}
	}
}
