using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals.Garrison;
using Styx.WoWInternals.DB;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison
{
	class Job
	{
		public Job(string name, GarrisonBuildingType type, WoWPoint location, int shipmentCrateEntry, int workOrderNpcEntry, int professionNpcEntry)
		{
			Name = name;
			Type = type;
			Location = location;
			ShipmentCrateEntry = shipmentCrateEntry;
			WorkOrderNpcEntry = workOrderNpcEntry;
			ProfessionNpcEntry = professionNpcEntry;
		}

		public string Name { get; set; }
		public GarrisonBuildingType Type { get; set; }
		public WoWPoint Location { get; set; }
		public int ShipmentCrateEntry { get; set; }
		public int WorkOrderNpcEntry { get; set; }
		public int ProfessionNpcEntry { get; set; }
	}

	class Jobs
	{
		private static List<Job> MyJobs = new List<Job>();

		private static Dictionary<int, WoWPoint> PlotIdLocations = new Dictionary<int, WoWPoint>();
		public static readonly LocalPlayer Me = StyxWoW.Me;
		private static int _currentJobIndex;

		public static void Initialize()
		{
			Lua.DoString("C_Garrison.RequestLandingPageShipmentInfo()");
			PlotIdLocations.Clear();
			PlotIdLocations.Add(18, new WoWPoint(5647.941, 4517.003, 119.2743));
			PlotIdLocations.Add(19, new WoWPoint(5653.386, 4549.211, 119.2657));
			PlotIdLocations.Add(20, new WoWPoint(5620.319, 4512.534, 120.1376));

			// Add universal buildings to job list
			MyJobs.Clear();
			MyJobs.Add(new Job("GarrisonCache", GarrisonBuildingType.Unknown, new WoWPoint(5593.86, 4586.82, 136.61), 237191, 0, 0));
			MyJobs.Add(new Job("HerbGarden", GarrisonBuildingType.HerbGarden, new WoWPoint(5414.47, 4573.50, 137.53), 239238, 85783, 0));
			MyJobs.Add(new Job("Mines", GarrisonBuildingType.Mines, new WoWPoint(5475.488, 4452.166, 144.4591), 239237, 81688, 0));
			MyJobs.Add(new Job("PrimalTrader", GarrisonBuildingType.Unknown, new WoWPoint(5577.58, 4388.634, 136.4497), 0, 84967, 0));

			// Add small buildings to job list
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Leatherworking))
				MyJobs.Add(new Job("Leatherworking", GarrisonBuildingType.Leatherworking, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Leatherworking).PlotInstanceId], 237104, 79833, 79834));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Alchemy))
				MyJobs.Add(new Job("Alchemy", GarrisonBuildingType.Alchemy, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Alchemy).PlotInstanceId], 237120, 79814, 79813));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Jewelcrafting))
				MyJobs.Add(new Job("Jewelcrafting", GarrisonBuildingType.Jewelcrafting, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Jewelcrafting).PlotInstanceId], 237069, 79830, 79832));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Enchanting))
				MyJobs.Add(new Job("Enchanting", GarrisonBuildingType.Enchanting, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Enchanting).PlotInstanceId], 237138, 79820, 79821));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Blacksmithing))
				MyJobs.Add(new Job("Blacksmithing", GarrisonBuildingType.Blacksmithing, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Blacksmithing).PlotInstanceId], 237131, 79817, 79867));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Tailoring))
				MyJobs.Add(new Job("Tailoring", GarrisonBuildingType.Tailoring, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Tailoring).PlotInstanceId], 237186, 79863, 49864));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Engineering))
				MyJobs.Add(new Job("Engineering", GarrisonBuildingType.Engineering, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Engineering).PlotInstanceId], 237146, 86696, 88610));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.Inscription))
				MyJobs.Add(new Job("Inscription", GarrisonBuildingType.Inscription, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.Inscription).PlotInstanceId], 237066, 79831, 79829));
			if (GarrisonInfo.OwnedBuildings.Any(o => o.Type == GarrisonBuildingType.SalvageYard))
				MyJobs.Add(new Job("SalvageYard", GarrisonBuildingType.SalvageYard, PlotIdLocations[GarrisonInfo.GetOwnedBuildingByType(GarrisonBuildingType.SalvageYard).PlotInstanceId], 0, 79857, 0));
			
			// Add last building to job list
			MyJobs.Add(new Job("CommandTable", GarrisonBuildingType.Unknown, new WoWPoint(5559.691, 4604.524, 141.7168), 0, 0, 0));
			MyJobs.Add(new Job("Done", GarrisonBuildingType.Unknown, new WoWPoint(0,0,0), 0, 0, 0));
			
			_currentJobIndex = 0;	
			Helpers.Log("Job List Created");
		}

		public static void NextJob()
		{
			_currentJobIndex++;
		}

		public static Job CurrentJob()
		{
			return MyJobs[_currentJobIndex];
		}
	}
}
