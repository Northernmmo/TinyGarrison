using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.DB;

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
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 6000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 6000);
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
				await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 6000);
				return true;
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

		public static async Task<bool> StartWorkOrders()
		{
			WoWUnit WorkOrderNpc =
				ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => o.Entry == Jobs.CurrentJob().WorkOrderNpcEntry)
					.OrderBy(o => o.Distance).FirstOrDefault();

			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.SleepForLagDuration();

			if (WorkOrderNpc != null && WorkOrderNpc.IsValid &&
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentCapacity -
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob().Type).ShipmentsCreated > 0 &&
				Helpers.HasWorkOrderMaterial())
			{
				if (!WorkOrderNpc.WithinInteractRange)
					return await Helpers.MoveTo(WorkOrderNpc);

				WorkOrderNpc.Interact();
				await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_INFO", 3000);
				Lua.DoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
				await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
				Lua.DoString("GarrisonCapacitiveDisplayFrameCloseButton:Click()");
				await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_CLOSED", 3000);
				return true;
			}

			Jobs.NextSubTask();
			return true;
		}

		public static async Task<bool> Profession()
		{
			switch (Jobs.CurrentJob().Type)
			{
				case GarrisonBuildingType.Enchanting:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(169092).CanCast && !WoWSpell.FromId(169092).Cooldown)
					{
						WoWSpell.FromId(169092).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Alchemy:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(156587).CanCast && !WoWSpell.FromId(156587).Cooldown)
					{
						WoWSpell.FromId(156587).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Leatherworking:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(171391).CanCast && !WoWSpell.FromId(171391).Cooldown)
					{
						WoWSpell.FromId(171391).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Jewelcrafting:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(170700).CanCast && !WoWSpell.FromId(170700).Cooldown)
					{
						WoWSpell.FromId(170700).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Blacksmithing:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(171690).CanCast && !WoWSpell.FromId(171690).Cooldown)
					{
						WoWSpell.FromId(171690).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Tailoring:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(168835).CanCast && !WoWSpell.FromId(168835).Cooldown)
					{
						WoWSpell.FromId(168835).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Engineering:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(169080).CanCast && !WoWSpell.FromId(169080).Cooldown)
					{
						WoWSpell.FromId(169080).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
				case GarrisonBuildingType.Inscription:
					if (SpellManager.HasSpell(Jobs.CurrentJob().Type.ToString()) && WoWSpell.FromId(169081).CanCast && !WoWSpell.FromId(169081).Cooldown)
					{
						WoWSpell.FromId(169081).Cast();
						await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
						return true;
					}
					break;
			}

			Jobs.NextSubTask();
			return true;
		}
	}
}
