using System;
using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.Garrison;

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
		StartWorkOrders
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
				Add(JobType.Move, new WoWPoint(5414.47, 4573.50, 137.53));
				Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.HerbGarden]);
			}

			// Mine
			if (GUI.TinyGarrisonSettings.Instance.GardenMine)
			{
				Helpers.Log("Adding Job: Mine");
				Add(JobType.Move, new WoWPoint(5475.488, 4452.166, 144.4591));
				Add(JobType.LootShipment, Data.MineShipment, GarrisonBuildingType.Mines);
				Add(JobType.GatherResources);
				Add(JobType.Move, new WoWPoint(5475.488, 4452.166, 144.4591));
				Add(JobType.StartWorkOrders, Data.WorkOrderNPCs[GarrisonBuildingType.Mines]);
			}

			// PrimalTrader

			// Professions

			// ProfessionBuildings

			// SalvageYard

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
