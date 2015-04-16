using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison
{
	enum JobType
	{
		Done,
		Move,
		LootGarrisonCache,
		GatherResources,
		Profession,
		LootShipment,
		StartWorkOrders,
		PrimalTrader
	};

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
		private static List<Job> MyJobs = new List<Job>();
		private static int _currentJobIndex;

		public static void Initialize()
		{
			Helpers.Log("Initializing..");

			// GarrisonCache
			if (GUI.TinyGarrisonSettings.Instance.GarrisonCache)
			{
				Helpers.Log("Adding Job: GarrisonCache");
				Add(JobType.Move, new WoWPoint(5593.86, 4586.82, 136.61));
				Add(JobType.LootGarrisonCache, Data.GarrisonCache);
			}

			// Garden
			if (GUI.TinyGarrisonSettings.Instance.GardenMine)
			{
				Helpers.Log("Adding Job: Garden");
				Add(JobType.Move, new WoWPoint(5414.47, 4573.50, 137.53));
				Add(JobType.LootShipment, Data.GardenShipment, GarrisonBuildingType.HerbGarden);
				Add(JobType.GatherResources, Data.GardenHerbs);
				Add(JobType.Move, new WoWPoint(5412.99, 4566.60, 138.29));
				Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.HerbGarden], GarrisonBuildingType.HerbGarden);
			}

			// Mine
			if (GUI.TinyGarrisonSettings.Instance.GardenMine)
			{
				Helpers.Log("Adding Job: Mine");
				Add(JobType.Move, new WoWPoint(5475.488, 4452.166, 144.4591));
				Add(JobType.LootShipment, Data.MineShipment, GarrisonBuildingType.Mines);
				Add(JobType.GatherResources, Data.MineOre);
				Add(JobType.Move, new WoWPoint(5469.95, 4447.67, 144.70));
				Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Mines], GarrisonBuildingType.Mines);
			}

			// Professions
			if (GUI.TinyGarrisonSettings.Instance.ProfessionDailies)
			{
				Helpers.Log("Adding Job: Profession");
				Add(JobType.Move, new WoWPoint(5468.646, 4447.296, 144.7437));

				if (SpellManager.HasSpell("Leatherworking")) Add(JobType.Profession, new HashSet<uint> { 171391 }, GarrisonBuildingType.Leatherworking);
				if (SpellManager.HasSpell("Alchemy")) Add(JobType.Profession, new HashSet<uint> { 156587 }, GarrisonBuildingType.Alchemy);
				if (SpellManager.HasSpell("Jewelcrafting")) Add(JobType.Profession, new HashSet<uint> { 170700 }, GarrisonBuildingType.Jewelcrafting);
				if (SpellManager.HasSpell("Enchanting")) Add(JobType.Profession, new HashSet<uint> { 169092 }, GarrisonBuildingType.Enchanting);
				if (SpellManager.HasSpell("Blacksmithing")) Add(JobType.Profession, new HashSet<uint> { 171690 }, GarrisonBuildingType.Blacksmithing);
				if (SpellManager.HasSpell("Tailoring")) Add(JobType.Profession, new HashSet<uint> { 168835 }, GarrisonBuildingType.Tailoring);
				if (SpellManager.HasSpell("Engineering")) Add(JobType.Profession, new HashSet<uint> { 169080 }, GarrisonBuildingType.Engineering);
				if (SpellManager.HasSpell("Inscription")) Add(JobType.Profession, new HashSet<uint> { 169081 }, GarrisonBuildingType.Inscription);
			}

			// PrimalTrader
			if (GUI.TinyGarrisonSettings.Instance.BuySavageBlood)
			{
				Helpers.Log("Adding Job: PrimalTrader");
				Add(JobType.Move, new WoWPoint(5577.58, 4388.634, 136.4497));
				Add(JobType.PrimalTrader);
			}

			// ProfessionBuildings
			if (GUI.TinyGarrisonSettings.Instance.ProfessionBuildings)
			{
				Helpers.Log("Adding Job: ProfessionBuildings");
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Leatherworking))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Leatherworking).PlotInstanceId]);
					Add(JobType.LootShipment, Data.LeatherworkingShipments, GarrisonBuildingType.Leatherworking);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Leatherworking).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Leatherworking], GarrisonBuildingType.Leatherworking);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Leatherworking).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Alchemy))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Alchemy).PlotInstanceId]);
					Add(JobType.LootShipment, Data.AlchemyShipments, GarrisonBuildingType.Alchemy);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Alchemy).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Alchemy], GarrisonBuildingType.Alchemy);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Alchemy).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Jewelcrafting))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Jewelcrafting).PlotInstanceId]);
					Add(JobType.LootShipment, Data.JewelcraftingShipments, GarrisonBuildingType.Jewelcrafting);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Jewelcrafting).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Jewelcrafting], GarrisonBuildingType.Jewelcrafting);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Jewelcrafting).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Enchanting))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Enchanting).PlotInstanceId]);
					Add(JobType.LootShipment, Data.EnchantingShipments, GarrisonBuildingType.Enchanting);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Enchanting).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Enchanting], GarrisonBuildingType.Enchanting);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Enchanting).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Blacksmithing))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Blacksmithing).PlotInstanceId]);
					Add(JobType.LootShipment, Data.BlacksmithingShipments, GarrisonBuildingType.Blacksmithing);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Blacksmithing).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Blacksmithing], GarrisonBuildingType.Blacksmithing);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Blacksmithing).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Tailoring))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Tailoring).PlotInstanceId]);
					Add(JobType.LootShipment, Data.TailoringShipments, GarrisonBuildingType.Tailoring);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Tailoring).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Tailoring], GarrisonBuildingType.Tailoring);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Tailoring).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Engineering))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Engineering).PlotInstanceId]);
					Add(JobType.LootShipment, Data.EngineeringShipments, GarrisonBuildingType.Engineering);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Engineering).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Engineering], GarrisonBuildingType.Engineering);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Engineering).PlotInstanceId]);
				}
				if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Inscription))
				{
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Inscription).PlotInstanceId]);
					Add(JobType.LootShipment, Data.InscriptionShipments, GarrisonBuildingType.Inscription);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Inscription).PlotInstanceId]);
					Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Inscription], GarrisonBuildingType.Inscription);
					Add(JobType.Move, Data.PlotLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Inscription).PlotInstanceId]);
				}
			}

			// SalvageYard
			if (GUI.TinyGarrisonSettings.Instance.Salvage)
			{
				Helpers.Log("Adding Job: Salvage");
			}

			// Done
			Add(JobType.Done);
		}

		public static void NextJob()
		{
			_currentJobIndex++;
			Helpers.Log("Current JobType: " + CurrentJob.Type.ToString());
		}

		public static Job CurrentJob
		{
			get
			{
				return MyJobs[_currentJobIndex];
			}
		}

		public static void Add(JobType type, WoWPoint location, HashSet<uint> entries, GarrisonBuildingType building)
		{
			MyJobs.Add(new Job(type, location, entries, building));
		}

		public static void Add(JobType type)
		{
			Add(type, new WoWPoint(0, 0, 0), new HashSet<uint>(), GarrisonBuildingType.Unknown);
		}

		public static void Add(JobType type, HashSet<uint> entries)
		{
			Add(type, new WoWPoint(0, 0, 0), entries, GarrisonBuildingType.Unknown);
		}

		public static void Add(JobType type, WoWPoint location)
		{
			Add(type, location, new HashSet<uint>(), GarrisonBuildingType.Unknown);
		}

		public static void Add(JobType type, HashSet<uint> entries, GarrisonBuildingType building)
		{
			Add(type, new WoWPoint(0, 0, 0), entries, building);
		}
	}
}
