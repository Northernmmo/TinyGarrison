using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.WoWInternals.Garrison;
using Styx.Helpers;
using Styx.WoWInternals.DB;
using Styx.CommonBot;

namespace TinyGarrison
{
	class Tasks
	{
		public static readonly LocalPlayer Me = StyxWoW.Me;

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
				if (StyxWoW.Me.SubZoneId == 7329)
				{
					// Movement Buffs
					if (Me.Class == WoWClass.Shaman && !Me.HasAura("Ghost Wolf")) SpellManager.Cast("Ghost Wolf");
					if (Me.Class == WoWClass.Druid && !Me.HasAura("Cat Form")) SpellManager.Cast("Cat Form");

					// Coffee & Pickaxe
					if (StyxWoW.Me.BagItems.Any(o => o.Entry == 118897) && (!StyxWoW.Me.HasAura(176049) || StyxWoW.Me.GetAuraById(176049).StackCount < 2)) StyxWoW.Me.BagItems.First(o => o.Entry == 118897).Interact();
					if (StyxWoW.Me.BagItems.Any(o => o.Entry == 118903) && !StyxWoW.Me.HasAura(176061)) StyxWoW.Me.BagItems.First(o => o.Entry == 118903).Interact();
				}

				// Gather
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
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 3000);
			var workOrdersReadyToStart = 
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).ShipmentCapacity -
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).LandingPageInfo.ShipmentsCreated;

			if (workOrdersReadyToStart > 0)
			{
				// Mill any herbs we need
				if (Jobs.CurrentJob.Building == GarrisonBuildingType.Inscription)
				{
					WoWItem herbStack = ObjectManager.GetObjectsOfType<WoWItem>().
						Where(o => Data.HerbItems.Contains(o.Entry)).
						OrderByDescending(o => o.StackCount).FirstOrDefault();

					if (herbStack != null && herbStack.IsValid && herbStack.StackCount >= 5 && Lua.GetReturnVal<int>("return GetItemCount('Cerulean Pigment')", 0) < workOrdersReadyToStart * 2)
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

				if (Helpers.HasWorkOrderMaterial)
				{
					WoWUnit workOrderNpc =
					ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(o => Jobs.CurrentJob.Entries.Contains(o.Entry))
						.OrderBy(o => o.Distance).FirstOrDefault();

					if (workOrderNpc != null && workOrderNpc.IsValid)
					{
						if (!workOrderNpc.WithinInteractRange)
						{
							await MoveTo(workOrderNpc);
							return true;
						}

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
			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> PrimalTrader()
		{
			int PrimalSpirits = Lua.GetReturnVal<int>("return GetItemCount('Primal Spirit')", 0);

			if (PrimalSpirits >= 50)
			{
				WoWUnit primalTraderNPC =
					ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(o => o.Entry == 84967)
						.OrderBy(o => o.Distance).FirstOrDefault();

				if (primalTraderNPC != null && primalTraderNPC.IsValid)
				{
					primalTraderNPC.Interact();
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_SHOW", 3000);
					Lua.DoString("BuyMerchantItem(5," + PrimalSpirits/50 + ")");
					await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
					Lua.DoString("MerchantFrameCloseButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_CLOSED", 3000);
				}
			}

			// Done
			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> Profession()
		{
			int spellID = (int)Jobs.CurrentJob.Entries.First();

			// Disenchant
			if (spellID == 169092)
			{
				Helpers.Log("i'm gonna disenchant, maybe...");
				WoWItem disenchantable = ObjectManager.GetObjectsOfType<WoWItem>().
					FirstOrDefault(o =>
						o.ItemInfo.Level >= 494 && o.ItemInfo.Level <= 610 &&
						(o.ItemInfo.ItemClass == WoWItemClass.Armor || o.ItemInfo.ItemClass == WoWItemClass.Weapon));

				if (disenchantable != null)
				{
					WoWSpell.FromId(13262).Cast();
					disenchantable.Interact();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					await CommonCoroutines.SleepForLagDuration();
					return true;
				}
			}

			if (!WoWSpell.FromId(spellID).Cooldown)
			{
				// Craft any needed material
				if (spellID == 169092 && Lua.GetReturnVal<bool>("return GetItemCount('Luminous Shard') < 1 and GetItemCount('Draenic Dust') >= 20", 0)) //Enchanting
				{
					await CommonCoroutines.SleepForLagDuration();
					WoWSpell.FromId(169091).Cast();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					return true;
				}

				if (spellID == 169081 && Lua.GetReturnVal<bool>("return GetItemCount('Cerulean Pigment') <= 10", 0)) //Inscription
				{
					WoWItem herbStack = ObjectManager.GetObjectsOfType<WoWItem>().Where(o =>
						o.Entry == 109124 || o.Entry == 109125 || o.Entry == 109126 || o.Entry == 109127 || o.Entry == 109128 || o.Entry == 109129)
						.OrderByDescending(o => o.StackCount).FirstOrDefault();

					if (herbStack != null && herbStack.IsValid && herbStack.StackCount >= 5)
					{
						WoWSpell.FromId(51005).Cast();
						herbStack.Interact();
						await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
						await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
						return true;
					}
				}

				// Cast daily cooldown
				if (Helpers.HasProfessionMaterial && WoWSpell.FromId(spellID).CanCast)
				{
					Helpers.Log("Crafting " + WoWSpell.FromId(spellID).Name);
					await CommonCoroutines.SleepForLagDuration();
					WoWSpell.FromId(spellID).Cast();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					return true;
				}
			}

			// Done
			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> Salvage()
		{
			// Vendor if we need bagspace
			bool needToVendor = Me.FreeNormalBagSlots <= 2;

			// Open salvage and tokens
			List<WoWItem> items = StyxWoW.Me.BagItems.Where(o => (new HashSet<uint>()
			{
				114116, 114120, 114119, 120301, 122633, 122607, 114069, 114071, 114075, 114078, 114080, 
				114110, 122621, 122622, 122623, 122624, 122625, 122626, 122627, 122628, 122629, 122630, 
				122631, 122632, 114070, 114057, 114059, 114060, 114063, 114066, 114068, 114109, 114058, 
				114100, 114105, 114097, 114099, 114094, 114108, 114096, 114098, 114101, 114052, 120302,
				114082, 114112
			}.Contains(o.Entry))).ToList();

			while (items.Count > 0 && Me.FreeNormalBagSlots > 2)
			{
				items.First().Interact();
				await CommonCoroutines.SleepForLagDuration();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				needToVendor = true;

				items = StyxWoW.Me.BagItems.Where(o => (new HashSet<uint>()
				{
				114116, 114120, 114119, 120301, 122633, 122607, 114069, 114071, 114075, 114078, 114080, 
				114110, 122621, 122622, 122623, 122624, 122625, 122626, 122627, 122628, 122629, 122630, 
				122631, 122632, 114070, 114057, 114059, 114060, 114063, 114066, 114068, 114109, 114058, 
				114100, 114105, 114097, 114099, 114094, 114108, 114096, 114098, 114101, 114052, 120302,
				114082, 114112
				}.Contains(o.Entry))).ToList();
			}

			// Vendor
			if (needToVendor)
			{
				WoWUnit vendorNpc =
					ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(o => Data.SalvageNPC.Contains(o.Entry))
						.OrderBy(o => o.Distance).FirstOrDefault();

				if (vendorNpc != null && vendorNpc.IsValid)
				{
					vendorNpc.Interact();
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_SHOW", 3000);
					await Coroutine.Sleep(8000);
					Lua.DoString("MerchantFrameCloseButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_CLOSED", 3000);
					return true;
				}
			}

			Jobs.NextJob();
			return true;
		}
	}
}
