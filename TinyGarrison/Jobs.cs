using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison
{
	enum JobType
	{
		Done,
		Move,
		GarrisonCache,
		Gather,
		PickupShipments,
		StartWorkOrders,
		Crafting,
		PrimalTrader,
		ScrapsDaily,
		Disenchant,
		Check,
		Vendor,
		Salvage,
		JewelcraftingDailyQuest
	}

	class Job
	{
		public Job(JobType type, WoWPoint location, HashSet<uint> entries, GarrisonBuildingType building)
		{
			Type = type;
			Location = location;
			Entries = entries;
			Building = building;
		}

		public JobType Type { get; set; }
		public WoWPoint Location { get; set; }
		public HashSet<uint> Entries { get; set; }
		public GarrisonBuildingType Building { get; set; }

	}

	class Jobs
	{
		private static readonly List<Job> MyJobs = new List<Job>();
		private static int _currentJobIndex;
		public static bool MineSafetyChecked;

		public static Job CurrentJob
		{
			get { return MyJobs[_currentJobIndex]; }
		}

		public static void NextJob()
		{
			++_currentJobIndex;
		}

		public static void Initialize()
		{
			Lua.DoString("LoadAddOn('Blizzard_GarrisonUI')");
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			Data.InitializeBuildingLocations();

			// Garrison Cache
			AddJob(JobType.GarrisonCache, Data.CustomLocations[CustomLocationType.GarrisonCache], 
				Data.CustomEntries[CustomEntryType.GarrisonCache]);
			Helpers.Log("Added: Garrison Cache");

			// Missions
			AddJob(JobType.Move, Data.CustomLocations[CustomLocationType.CommandTable],
				Data.CustomEntries[CustomEntryType.CommandTable]);
			// TODO: Complete Missions
			// TODO: Upgrade Followers
			// TODO: Start Missions
			Helpers.Log("Added: Missions");

			// Garden (Always move there to check nodes)
			AddJob(JobType.Move, Data.CustomLocations[CustomLocationType.GardenCheck]);
			AddJob(JobType.PickupShipments, Data.ShipmentCrateLocations[GarrisonBuildingType.HerbGarden], 
				Data.ShipmentCrates[GarrisonBuildingType.HerbGarden], GarrisonBuildingType.HerbGarden);
			AddJob(JobType.Gather, Data.CustomEntries[CustomEntryType.HerbNodes]);
			AddJob(JobType.StartWorkOrders, Data.WorkOrderNpcLocations[GarrisonBuildingType.HerbGarden], 
				Data.WorkOrderNpcs[GarrisonBuildingType.HerbGarden], GarrisonBuildingType.HerbGarden);
			Helpers.Log("Added: Herb Garden");

			// Mine
			AddJob(JobType.PickupShipments, Data.ShipmentCrateLocations[GarrisonBuildingType.Mines], 
				Data.ShipmentCrates[GarrisonBuildingType.Mines], GarrisonBuildingType.Mines);
			AddJob(JobType.Gather, Data.CustomEntries[CustomEntryType.OreNodes], GarrisonBuildingType.Mines);
			AddJob(JobType.StartWorkOrders, Data.WorkOrderNpcLocations[GarrisonBuildingType.Mines], 
				Data.WorkOrderNpcs[GarrisonBuildingType.Mines], GarrisonBuildingType.Mines);
			Helpers.Log("Added: Mines");

			// Crafting
			foreach (var spell in Data.DailyProfessionSpells.Where(spell =>
				StyxWoW.Me.KnowsSpell((int)spell) && !WoWSpell.FromId((int)spell).Cooldown))
			{
				AddJob(JobType.Crafting, Data.WorkOrderNpcLocations[GarrisonBuildingType.Mines], new HashSet<uint> { spell });
				Helpers.Log("Added: Crafting " + WoWSpell.FromId((int)spell).Name);
			}

			// Crafting Secrets
			foreach (var spell in Data.DailyProfessionSecrets.Where(spell =>
				StyxWoW.Me.KnowsSpell((int)spell) && !WoWSpell.FromId((int)spell).Cooldown))
			{
				AddJob(JobType.Crafting, Data.WorkOrderNpcLocations[GarrisonBuildingType.Mines], new HashSet<uint> { spell });
				Helpers.Log("Added: Crafting " + WoWSpell.FromId((int)spell).Name);
			}

			// Crafting Transmute Blood
			if (StyxWoW.Me.KnowsSpell((int)Data.TransmuteBlood.First()) && 
				!WoWSpell.FromId((int)Data.TransmuteBlood.First()).Cooldown)
			{
				AddJob(JobType.Crafting, Data.WorkOrderNpcLocations[GarrisonBuildingType.Mines], 
					new HashSet<uint> { Data.TransmuteBlood.First() });
				Helpers.Log("Added: Crafting Transmute Savage Blood");
			}

			// Primal Trader
			AddJob(JobType.PrimalTrader, Data.CustomLocations[CustomLocationType.PrimalTrader], 
				Data.CustomEntries[CustomEntryType.PrimalTrader]);
			Helpers.Log("Added: Buy Savage Blood");

			// Scraps Daily (Always move there to check quest npc)
			if (GarrisonInfo.GetBuildingByType(GarrisonBuildingType.WarMill).Rank == 3)
			{
				AddJob(JobType.Move, Data.ShipmentCrateLocations[GarrisonBuildingType.WarMill]);
				AddJob(JobType.ScrapsDaily, Data.CustomEntries[CustomEntryType.ScrapsDailyNpc]);
				Helpers.Log("Added: Scraps Daily");
			}

			// Small Profession Buildings
			foreach (var ownedBuilding in GarrisonInfo.OwnedBuildings.Where(ownedBuilding =>
				(ownedBuilding.PlotInstanceId == 18 || ownedBuilding.PlotInstanceId == 19 || ownedBuilding.PlotInstanceId == 20) &&
				ownedBuilding.Type != GarrisonBuildingType.SalvageYard && ownedBuilding.Type != GarrisonBuildingType.Storehouse))
			{
				AddJob(JobType.PickupShipments, Data.ShipmentCrateLocations[ownedBuilding.Type],
					Data.ShipmentCrates[ownedBuilding.Type], ownedBuilding.Type);
				AddJob(JobType.StartWorkOrders, Data.WorkOrderNpcLocations[ownedBuilding.Type],
					Data.WorkOrderNpcs[ownedBuilding.Type], ownedBuilding.Type);
				Helpers.Log("Added: " + ownedBuilding.Type + " building");

				/*if (ownedBuilding.Type == GarrisonBuildingType.Jewelcrafting &&
				    GarrisonInfo.GetBuildingByType(GarrisonBuildingType.Jewelcrafting).Rank >= 2)
				{
					var jewelcrafingQuestGiver = GarrisonInfo.Followers.FirstOrDefault(o =>
						o.InsideBuildingId == GarrisonInfo.GetBuildingByType(GarrisonBuildingType.Jewelcrafting).BuildingId);
					if (jewelcrafingQuestGiver != null)
					{
						AddJob(JobType.Move, Data.ShipmentCrateLocations[GarrisonBuildingType.Jewelcrafting]);
						//AddJob(JobType.JewelcraftingDailyQuest);
						Helpers.Log("Added: Jewelcrafting daily quest");
					}
				}*/
			}

			// Salvage (Always move there because... reasons)
			AddJob(JobType.Move, Data.ShipmentCrateLocations[GarrisonBuildingType.SalvageYard]);
			AddJob(JobType.Move, Data.WorkOrderNpcLocations[GarrisonBuildingType.SalvageYard]);
			AddJob(JobType.Salvage, Data.ShipmentCrateLocations[GarrisonBuildingType.SalvageYard],
				Data.CustomEntries[CustomEntryType.FollowerUpgrades]);
			AddJob(JobType.Salvage, Data.ShipmentCrateLocations[GarrisonBuildingType.SalvageYard], 
				Data.CustomEntries[CustomEntryType.GearTokens]);
			AddJob(JobType.Salvage, Data.ShipmentCrateLocations[GarrisonBuildingType.SalvageYard], 
				Data.CustomEntries[CustomEntryType.Salvage]);
			AddJob(JobType.Vendor);
			Helpers.Log("Added: Open Salvage");

			// TODO: Trading Post

			// Garrison Cache
			AddJob(JobType.GarrisonCache, Data.CustomLocations[CustomLocationType.GarrisonCache], 
				Data.CustomEntries[CustomEntryType.GarrisonCache]);
			Helpers.Log("Added: Garrison Cache");

			// Done
			AddJob(JobType.Move, Data.CustomLocations[CustomLocationType.CommandTable]);
			AddJob(JobType.Done);

			Helpers.Log("(v" + TinyGarrison.Version + ") Initialized");
		}

		#region AddJob Overrides
		public static void AddJob(JobType type, WoWPoint destination, HashSet<uint> entries, GarrisonBuildingType buildingType)
		{
			MyJobs.Add(new Job(type, destination, entries, buildingType));
		}

		public static void AddJob(JobType type, WoWPoint destination)
		{
			AddJob(type, destination, new HashSet<uint>(), GarrisonBuildingType.Unknown);
		}

		public static void AddJob(JobType type, HashSet<uint> entries)
		{
			AddJob(type, new WoWPoint(), entries, GarrisonBuildingType.Unknown);
		}

		public static void AddJob(JobType type, GarrisonBuildingType buildingType)
		{
			AddJob(type, new WoWPoint(), new HashSet<uint>(), buildingType);
		}

		public static void AddJob(JobType type, HashSet<uint> entries, GarrisonBuildingType buildingType)
		{
			AddJob(type, new WoWPoint(), entries, buildingType);
		}

		public static void AddJob(JobType type, WoWPoint destination, HashSet<uint> entries)
		{
			AddJob(type, destination, entries, GarrisonBuildingType.Unknown);
		}

		public static void AddJob(JobType type)
		{
			AddJob(type, new WoWPoint(), new HashSet<uint>(), GarrisonBuildingType.Unknown);
		}
		#endregion

		#region InsertMoveJob Overrides

		public static void InsertMoveJob(WoWPoint destination)
		{
			MyJobs.Insert(_currentJobIndex, new Job(JobType.Move, destination, new HashSet<uint>(), GarrisonBuildingType.Unknown));
		}

		public static bool AlreadyMoved
		{
			get { return MyJobs[_currentJobIndex - 1].Type == JobType.Move; }
		}

		public static void AddSafetyMineCheckJob()
		{
			MyJobs.Insert(_currentJobIndex + 1, new Job(JobType.Gather, new WoWPoint(),
				Data.CustomEntries[CustomEntryType.OreNodes], GarrisonBuildingType.Mines));
			MyJobs.Insert(_currentJobIndex + 1,
				new Job(JobType.Move, Data.CustomLocations[CustomLocationType.MineSafeCheck], new HashSet<uint>(),
					GarrisonBuildingType.Unknown));
			MineSafetyChecked = true;
		}
		#endregion
	}
}
