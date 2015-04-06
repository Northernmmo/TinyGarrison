using System.Linq;
using System.Threading.Tasks;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.WoWObjects;

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

			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);
			int shipmentsReadyToStart =
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity -
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated;

			// Start Work Orders
			if (shipmentsReadyToStart > 0)
			{
				// Mill any herbs we need
				if (Jobs.CurrentJob().ProfessionNpcEntry == 79829)
				{
					WoWItem herbStack = ObjectManager.GetObjectsOfType<WoWItem>().Where(o =>
						o.Entry == 109124 || o.Entry == 109125 || o.Entry == 109126 || o.Entry == 109127 || o.Entry == 109128 ||
						o.Entry == 109129)
						.OrderByDescending(o => o.StackCount).FirstOrDefault();

					if (herbStack != null && herbStack.IsValid &&  herbStack.StackCount >= 5 && Lua.GetReturnVal<int>("return GetItemCount('Cerulean Pigment')", 0) < shipmentsReadyToStart * 2)
					{
						Helpers.Log("Milling herbs");
						if (SpellManager.HasSpell(51005)) // Has the inscription spell, mill
							WoWSpell.FromId(51005).Cast();
						else // Doesn't have inscription, use mortar
							ObjectManager.GetObjectsOfType<WoWItem>().First(o => o.Entry == 114942).Interact();
						herbStack.Interact();
						await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
						await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
						return true;
					}
				}

				if (Helpers.HasWorkOrderMaterial())
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
				}
			}

			// Done
			_alreadyMoved = false;
			Jobs.NextJob();
			return true;
		}
	}
}
