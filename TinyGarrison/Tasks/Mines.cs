using System.Linq;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison.Tasks
{
	class Mines
	{
		private static bool _alreadyMoved;
		public static readonly LocalPlayer Me = StyxWoW.Me;
		private static WoWGameObject _oreObj;

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
			if (_oreObj == null)
				_oreObj =
				ObjectManager.GetObjectsOfType<WoWGameObject>()
					.Where(o => o.CanUse() && o.Distance < 150)
					.Where(
						o =>
							o.Entry == 232545 || o.Entry == 232544 || o.Entry == 232543 || o.Entry == 232541 ||
							o.Entry == 232542)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (_oreObj != null && _oreObj.IsValid)
			{
				// Coffee & Pickaxe
				if (Me.SubZoneId == 7329)
				{
					if (Me.BagItems.Any(o => o.Entry == 118897) && (!Me.HasAura(176049) || Me.GetAuraById(176049).StackCount < 2)) Me.BagItems.First(o => o.Entry == 118897).Interact();
					if (Me.BagItems.Any(o => o.Entry == 118903) && !Me.HasAura(176061)) Me.BagItems.First(o => o.Entry == 118903).Interact();
				}

				if (!_oreObj.WithinInteractRange)
				{
					Helpers.MoveTo(_oreObj);
					return true;
				}

				await CommonCoroutines.SleepForLagDuration();
				if (StyxWoW.Me.Combat)
					return true;
				_oreObj.Interact();
				if (StyxWoW.Me.Combat)
					return true;
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				_oreObj = null;
				return true;
			}

			// Move back to entrance
			if (StyxWoW.Me.Location.Distance(Jobs.CurrentJob().Location) > 10)
			{
				Helpers.Log("Moving back to mine entrance");
				await Helpers.MoveTo(Jobs.CurrentJob().Location);
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
