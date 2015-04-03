using System.Threading.Tasks;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals.DB;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison
{
	class Helpers
	{
		private static string _lastMsg;
		public static readonly LocalPlayer Me = StyxWoW.Me;

		public static void Log(string msg)
		{
			if (msg == _lastMsg) return;
			Logging.Write(Colors.DarkOrange, "[TG] " + msg);
			_lastMsg = msg;
		}

		public static async Task<bool> MoveToJob(WoWPoint destination)
		{
			TreeRoot.StatusText = "Current Job - " + Jobs.CurrentJob().Name;
			Log("Moving to Job: " + Jobs.CurrentJob().Name);
			MoveResult r = await CommonCoroutines.MoveTo(destination);

			if (r == MoveResult.ReachedDestination)
			{
				return true;
			}
			return false;
		}

		public static async Task<bool> MoveTo(WoWPoint destination)
		{
			MoveResult r = await CommonCoroutines.MoveTo(destination);

			if (r == MoveResult.ReachedDestination)
			{
				return true;
			}
			return true;
		}

		public static async Task<bool> MoveTo(WoWGameObject destinationObject)
		{
			WoWPoint pointFromTarget = WoWMathHelper.CalculatePointFrom(Me.Location, destinationObject.Location, destinationObject.InteractRange - 2);

			return await MoveTo(pointFromTarget);
		}

		public static async Task<bool> MoveTo(WoWUnit destinationUnit)
		{
			WoWPoint pointFromTarget = WoWMathHelper.CalculatePointFrom(Me.Location, destinationUnit.Location, destinationUnit.InteractRange - 2);

			return await MoveTo(pointFromTarget);
		}

		public static async Task<bool> LootShipment()
		{
			WoWGameObject ShipmentCrate =
					ObjectManager.GetObjectsOfType<WoWGameObject>()
						.Where(o => o.Entry == Jobs.CurrentJob().ShipmentCrateEntry)
						.OrderBy(o => o.Distance).FirstOrDefault();

			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);

			if (ShipmentCrate != null && ShipmentCrate.IsValid && ShipmentCrate.IsValid && GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsReady > 0)
			{
				Log("Looting " + ShipmentCrate.Name);
				ShipmentCrate.Interact();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				return true;
			}

			return true;
		}

		public static async Task<bool> StartWorkOrders()
		{
			WoWUnit WorkOrderNpc =
				ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => o.Entry == Jobs.CurrentJob().WorkOrderNpcEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

			if (WorkOrderNpc != null && WorkOrderNpc.IsValid)
			{
				if (!WorkOrderNpc.WithinInteractRange)
					return await Helpers.MoveTo(WorkOrderNpc);

				Log("Starting " + Jobs.CurrentJob().Name + " work orders");
				WorkOrderNpc.Interact();
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

		public static bool HasWorkOrderMaterial()
		{
			switch (Jobs.CurrentJob().Type)
			{
				case GarrisonBuildingType.HerbGarden:
					return Lua.GetReturnVal<int>("return GetItemCount('Draenic Seeds')", 0) >= 5;
				case GarrisonBuildingType.Mines:
					return Lua.GetReturnVal<int>("return GetItemCount('Draenic Stone')", 0) >= 5;
				case GarrisonBuildingType.Enchanting:
					return Lua.GetReturnVal<int>("return GetItemCount('Draenic Dust')", 0) >= 5;
				case GarrisonBuildingType.Alchemy:
					return Lua.GetReturnVal<int>("return GetItemCount('Frostweed')", 0) >= 5;
				case GarrisonBuildingType.Leatherworking:
					return Lua.GetReturnVal<int>("return GetItemCount('Raw Beast Hide')", 0) >= 5;
				case GarrisonBuildingType.Jewelcrafting:
					return Lua.GetReturnVal<int>("return GetItemCount('Blackrock Ore')", 0) >= 5;
				case GarrisonBuildingType.Blacksmithing:
					return Lua.GetReturnVal<int>("return GetItemCount('True Iron Ore')", 0) >= 5;
				case GarrisonBuildingType.Tailoring:
					return Lua.GetReturnVal<int>("return GetItemCount('Sumptuous Fur')", 0) >= 5;
				case GarrisonBuildingType.Engineering:
					return Lua.GetReturnVal<int>("return GetItemCount('Blackrock Ore')", 0) >= 2 &&
						Lua.GetReturnVal<int>("return GetItemCount('True Iron Ore')", 0) >= 2;
				case GarrisonBuildingType.Inscription:
					return Lua.GetReturnVal<int>("return GetItemCount('Cerulean Pigment')", 0) >= 2;
			}

			return false;
		}
	}
}
