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
				Jobs.NextSubTask();
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

		public static async Task<bool> UseRushOrder()
		{
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);
			Log("checking rush orders");
			Log("Created: " + GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated + " capacity: " + GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity);
			switch (Jobs.CurrentJob().Type)
			{
				case GarrisonBuildingType.Enchanting:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Alchemy:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Leatherworking:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Jewelcrafting:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Blacksmithing:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Tailoring:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Engineering:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: Engineering Works')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: Engineering Works\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
				case GarrisonBuildingType.Inscription:
					if (Lua.GetReturnVal<int>("return GetItemCount('Rush Order: The Tannery')", 0) > 0 &&
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).LandingPageInfo.ShipmentsCreated ==
					    GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity)
					{
						Lua.DoString("RunMacroText(\"/use Rush Order: The Tannery\")");
						Jobs.ResetSubTasks();
						return true;
					}
					return false;
			}
			return false;
		}
	}
}
