using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison
{
	internal class Tasks
	{
		//== GarrisonCache ==//
		public static async Task<bool> GarrisonCache()
		{
			// Get
			var garrisonCache = ObjectManager.GetObjectsOfTypeFast<WoWGameObject>()
				.FirstOrDefault(o => Data.CustomEntries[CustomEntryType.GarrisonCache].Contains(o.Entry));

			// Check
			if (garrisonCache == null || !garrisonCache.IsValid || !garrisonCache.CanUse())
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Looting " + garrisonCache.Name);

			// Move
			if (!garrisonCache.WithinInteractRange)
			{
				await Movement.MoveTo(garrisonCache);
				return true;
			}
			if (StyxWoW.Me.IsMoving || StyxWoW.Me.IsCasting) return true;

			// Loot
			garrisonCache.Interact();
			await CommonCoroutines.WaitForLuaEvent("CHAT_MESSAGE_CURRENCY", 2000);

			return true;
		}

		//== PickupShipments ==//
		public static async Task<bool> PickupShipments()
		{
			// Check
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 2000);
			if (GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).LandingPageInfo.ShipmentsReady == 0)
			{
				Jobs.NextJob();
				return true;
			}

			//Log
			Helpers.Log("Looting " + Jobs.CurrentJob.Building + " shipments");

			// Move (Job)
			if (!Jobs.AlreadyMoved)
			{
				Jobs.InsertMoveJob(Jobs.CurrentJob.Location);
				return true;
			}

			// Get
			var shipmentCrates = ObjectManager.GetObjectsOfType<WoWGameObject>().First(o =>
				Jobs.CurrentJob.Entries.Contains(o.Entry));

			// Move (Interact)
			if (!shipmentCrates.WithinInteractRange)
			{
				await Movement.MoveTo(shipmentCrates);
				return true;
			}
			if (StyxWoW.Me.IsMoving || StyxWoW.Me.IsCasting) return true;

			// Loot
			shipmentCrates.Interact();
			await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 2000);

			return true;
		}

		//== Gather ==//
		public static async Task<bool> Gather()
		{
			// Get
			var node = ObjectManager.GetObjectsOfTypeFast<WoWGameObject>()
				.Where(o => o.CanUse() && o.Distance < 150 && Jobs.CurrentJob.Entries.Contains(o.Entry))
				.OrderBy(o => o.Distance).FirstOrDefault();

			// Check
			if (node == null || !node.IsValid || !node.CanUse())
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Gathering " + node.Name);

			// Move
			if (!node.WithinInteractRange)
			{
				await Movement.MoveTo(node);
				return true;
			}
			if (StyxWoW.Me.IsMoving || StyxWoW.Me.IsCasting) return true;

			// Gather
			if (StyxWoW.Me.Combat)
				return true;
			node.Interact();
			if (StyxWoW.Me.Combat)
				return true;
			await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 1000);
			await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 1000);
			
			if (!Jobs.MineSafetyChecked && Jobs.CurrentJob.Building == GarrisonBuildingType.Mines &&
				Data.CustomLocations[CustomLocationType.MineSafeCheck].Distance(StyxWoW.Me.Location) > 150)
				Jobs.AddSafetyMineCheckJob();

			return true;
		}

		//== StartWorkOrders ==//
		public static async Task<bool> StartWorkOrders()
		{
			// Check
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			await CommonCoroutines.WaitForLuaEvent("GARRISON_LANDINGPAGE_SHIPMENTS", 2000);
			var numberOfWorkOrdersReadyToStart =
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).ShipmentCapacity -
				GarrisonInfo.GetShipmentInfoByType(Jobs.CurrentJob.Building).LandingPageInfo.ShipmentsCreated;

			if (numberOfWorkOrdersReadyToStart == 0)
			{
				Jobs.NextJob();
				return true;
			}

			if (Jobs.CurrentJob.Building == GarrisonBuildingType.Jewelcrafting &&
				GUI.TinyGarrisonSettings.Instance.SkipJewelcraftingWOs && !StyxWoW.Me.KnowsSpell(170700))
			{
				Jobs.NextJob();
				return true;
			}

			// Mill as needed
			if (Jobs.CurrentJob.Building == GarrisonBuildingType.Inscription)
			{
				WoWItem herbStack = ObjectManager.GetObjectsOfType<WoWItem>().
					Where(o => Data.CustomEntries[CustomEntryType.HerbInventoryItems].Contains(o.Entry)).
					OrderByDescending(o => o.StackCount).FirstOrDefault();

				if (herbStack != null && herbStack.IsValid && herbStack.StackCount >= 5 && 
					Lua.GetReturnVal<int>("return GetItemCount('Cerulean Pigment')", 0) < numberOfWorkOrdersReadyToStart * 2)
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

			if (!Helpers.HasWorkOrderMaterial)
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Starting work orders for " + Jobs.CurrentJob.Building);

			// Move (Job)
			if (!Jobs.AlreadyMoved)
			{
				Jobs.InsertMoveJob(Jobs.CurrentJob.Location);
				return true;
			}

			// Get
			var workOderNpc = ObjectManager.GetObjectsOfType<WoWUnit>()
				.First(o => Jobs.CurrentJob.Entries.Contains(o.Entry));

			// Move (Interact)
			if (!workOderNpc.WithinInteractRange)
			{
				await Movement.MoveTo(workOderNpc);
				return true;
			}
			if (StyxWoW.Me.IsMoving || StyxWoW.Me.IsCasting) return true;
			
			// StartWorkOrders
			workOderNpc.Interact();
			await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_OPENED", 3000);
			await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_INFO", 3000);
			Lua.DoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
			Lua.DoString("GarrisonCapacitiveDisplayFrameCloseButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("SHIPMENT_CRAFTER_CLOSED", 3000);

			return true;
		}

		//== Salvage ==//
		public static async Task<bool> Salvage()
		{
			// Get
			var item = StyxWoW.Me.BagItems.FirstOrDefault(o => Jobs.CurrentJob.Entries.Contains(o.Entry));

			// Check
			if (item == null)
			{
				Jobs.NextJob();
				return true;
			}

			if (Jobs.CurrentJob.Entries == Data.CustomEntries[CustomEntryType.FollowerUpgrades] &&
				!GUI.TinyGarrisonSettings.Instance.OpenFollowerUpgrades)
			{
				Jobs.NextJob();
				return true;
			}

			if (Jobs.CurrentJob.Entries == Data.CustomEntries[CustomEntryType.GearTokens] &&
				!GUI.TinyGarrisonSettings.Instance.OpenGearTokens)
			{
				Jobs.NextJob();
				return true;
			}

			if (StyxWoW.Me.IsMoving || StyxWoW.Me.IsCasting) return true;

			// Vendor if we need to
			if (StyxWoW.Me.FreeNormalBagSlots <= 4)
			{
				await Helpers.Vendor();
				return true;
			}

			// Log
			Helpers.Log("Opening " + item.Name);

			// Open
			item.Interact();
			await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 2000);
			if (Jobs.CurrentJob.Entries != Data.CustomEntries[CustomEntryType.Salvage])
			{
				await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 2000);
				await Coroutine.Sleep(1000);
			}

			return true;
		}

		//== Crafting ==//
		public static async Task<bool> Crafting()
		{
			// Get
			var spell = (int)Jobs.CurrentJob.Entries.First();

			// Check
			if (Data.DailyProfessionSecrets.Contains((uint)spell) && !GUI.TinyGarrisonSettings.Instance.CraftSecrets)
			{
				Jobs.NextJob();
				return true;
			}

			if (Data.TransmuteBlood.Contains((uint)spell) && !GUI.TinyGarrisonSettings.Instance.TransmuteBlood)
			{
				Jobs.NextJob();
				return true;
			}

			if (WoWSpell.FromId(spell).Cooldown)
			{
				Jobs.NextJob();
				return true;
			}

			if (StyxWoW.Me.IsMoving || StyxWoW.Me.IsCasting) return true;

			// Craft Materials Needed
			// Inscription
			if ((Jobs.CurrentJob.Entries.First() == 169081 && Lua.GetReturnVal<bool>("return GetItemCount('Cerulean Pigment') < 10", 0)) ||
				Jobs.CurrentJob.Entries.First() == 120136 && Lua.GetReturnVal<bool>("return GetItemCount('Cerulean Pigment') < 2", 0))
			{
				WoWItem herbStack = ObjectManager.GetObjectsOfType<WoWItem>().
					Where(o => Data.CustomEntries[CustomEntryType.HerbInventoryItems].Contains(o.Entry)).
					OrderByDescending(o => o.StackCount).FirstOrDefault();

				if (herbStack != null && herbStack.IsValid && herbStack.StackCount >= 5)
				{
					WoWSpell.FromId(51005).Cast();
					herbStack.Interact();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					return true;
				}
			}
			// Enchanting
			if (Jobs.CurrentJob.Entries.First() == 169092 &&
				Lua.GetReturnVal<bool>("return GetItemCount('Luminous Shard') < 1 and GetItemCount('Draenic Dust') < 20", 0))
			{
				Jobs.NextJob();
				return true;
			}
			if (Jobs.CurrentJob.Entries.First() == 169092 && Lua.GetReturnVal<bool>("return GetItemCount('Luminous Shard') < 1 and GetItemCount('Draenic Dust') >= 20", 0))
			{
				WoWSpell.FromId(169091).Cast();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				return true;
			}

			if (!SpellManager.CanCast(spell))
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Crafting " + WoWSpell.FromId(spell).Name);

			// Move (Job)
			if (!Jobs.AlreadyMoved)
			{
				Jobs.InsertMoveJob(Jobs.CurrentJob.Location);
				return true;
			}

			// Craft
			WoWSpell.FromId(spell).Cast();
			await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 2000);

			return true;
		}

		//== PrimalTrader ==//
		public static async Task<bool> PrimalTrader()
		{
			// Get
			var primalSpirits = Lua.GetReturnVal<int>("return GetItemCount('Primal Spirit')", 0);

			// Check
			if (primalSpirits < 50 || !GUI.TinyGarrisonSettings.Instance.BuySavageBlood)
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Buying Savage Blood");

			// Move (Job)
			if (!Jobs.AlreadyMoved)
			{
				Jobs.InsertMoveJob(Jobs.CurrentJob.Location);
				return true;
			}

			// Buy Savage Blood
			ObjectManager.GetObjectsOfType<WoWUnit>()
					.First(o => Jobs.CurrentJob.Entries.Contains(o.Entry)).Interact();

			await CommonCoroutines.WaitForLuaEvent("MERCHANT_SHOW", 3000);
			Lua.DoString("BuyMerchantItem(5," + primalSpirits / 50 + ")");
			await CommonCoroutines.WaitForLuaEvent("BAG_UPDATE_DELAYED", 3000);
			Lua.DoString("MerchantFrameCloseButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("MERCHANT_CLOSED", 3000);

			return true;
		}

		//== ScrapsDaily ==//
		public static async Task<bool> ScrapsDaily()
		{
			// Get
			var questGiver = ObjectManager.GetObjectsOfType<WoWUnit>().First(o =>
				Jobs.CurrentJob.Entries.Contains(o.Entry));

			// Check
			if (!questGiver.HasQuestCursor ||
				Lua.GetReturnVal<int>("return GetItemCount('Iron Horde Scraps')", 0) < 25)
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Doing scraps daily");

			// Move
			if (!questGiver.WithinInteractRange)
			{
				await Movement.MoveTo(questGiver);
				return true;
			}

			// Check Gossip Window
			if (!Lua.GetReturnVal<bool>("return GossipFrame:IsShown()", 0))
			{
				questGiver.Interact();
				return true;
			}

			// Decide if we need Armor or Weapon
			var armorTotal = Lua.GetReturnVal<int>("return " +
				"GetItemCount('Braced Armor Enhancement') * 3 + " +
				"GetItemCount('Fortified Armor Enhancement') * 6 + " +
				"GetItemCount('Heavily Reinforced Armor Enhancement') * 9", 0);
			var weaponTotal = Lua.GetReturnVal<int>("return " +
				"GetItemCount('Balanced Weapon Enhancement') * 3 + " +
				"GetItemCount('Striking Weapon Enhancement') * 6 + " +
				"GetItemCount('Power Overrun Weapon Enhancement') * 9", 0);

			// Do Daily
			Lua.DoString("GossipTitleButton1:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_DETAIL", 2000);
			Lua.DoString("QuestFrameAcceptButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_LOG_UPDATE", 2000);
			questGiver.Interact();
			await CommonCoroutines.WaitForLuaEvent("GOSSIP_SHOW", 2000);
			Lua.DoString("GossipTitleButton1:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_PROGRESS", 2000);
			Lua.DoString("QuestFrameCompleteButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_COMPLETE", 2000);
			if (armorTotal > weaponTotal) Lua.DoString("QuestInfoRewardsFrameQuestInfoItem2:Click()");
			else Lua.DoString("QuestInfoRewardsFrameQuestInfoItem1:Click()");
			Lua.DoString("QuestFrameCompleteQuestButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_FINISHED", 2000);
			await Coroutine.Sleep(1000);
			return true;
		}

		//== Vendor ==//
		public static async Task<bool> Vendor()
		{
			await Helpers.Vendor();

			Jobs.NextJob();
			return true;
		}

		//== JewelcraftingDailyQuest ==//
		public static async Task<bool> JewelcraftingDailyQuest()
		{
			// Get
			var questGiverName = GarrisonInfo.Followers.First(o =>
				o.InsideBuildingId == GarrisonInfo.GetBuildingByType(GarrisonBuildingType.Jewelcrafting).BuildingId).Name;
			var questGiver = ObjectManager.GetObjectsOfType<WoWUnit>().FirstOrDefault(o =>
				o.Name == questGiverName);

			// Check
			if (questGiver == null || !questGiver.HasQuestCursor)
			{
				Jobs.NextJob();
				return true;
			}

			// Log
			Helpers.Log("Doing jewelcrafting daily quest");

			// Move
			if (!questGiver.WithinInteractRange)
			{
				await Movement.MoveTo(questGiver);
				return true;
			}

			// Check Quest Window
			if (!Lua.GetReturnVal<bool>("return QuestFrame:IsShown()", 0))
			{
				questGiver.Interact();
				return true;
			}

			// Accept Daily
			Lua.DoString("QuestFrameAcceptButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_LOG_UPDATE", 2000);

			// Craft Daily

			// Turn in Daily
			questGiver.Interact();
			await CommonCoroutines.WaitForLuaEvent("QUEST_PROGRESS", 2000);

			/*
			Lua.DoString("GossipTitleButton1:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_PROGRESS", 2000);
			Lua.DoString("QuestFrameCompleteButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_COMPLETE", 2000);
			if (armorTotal > weaponTotal) Lua.DoString("QuestInfoRewardsFrameQuestInfoItem2:Click()");
			else Lua.DoString("QuestInfoRewardsFrameQuestInfoItem1:Click()");
			Lua.DoString("QuestFrameCompleteQuestButton:Click()");
			await CommonCoroutines.WaitForLuaEvent("QUEST_FINISHED", 2000);
			*/
			return true;
		}
	}
}
