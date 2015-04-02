using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison
{
	class Job
	{
		public Job(string name, WoWPoint location, int shipmentCrateEntry, int workOrderNpcEntry, int professionNpcEntry)
		{
			Name = name;
			Location = location;
			ShipmentCrateEntry = shipmentCrateEntry;
			WorkOrderNpcEntry = workOrderNpcEntry;
			ProfessionNpcEntry = professionNpcEntry;
		}

		public string Name { get; set; }
		public WoWPoint Location { get; set; }
		public int ShipmentCrateEntry { get; set; }
		public int WorkOrderNpcEntry { get; set; }
		public int ProfessionNpcEntry { get; set; }
	}

	class Jobs
	{
		private static List<OwnedBuildingInfo> MyBuildings = new List<OwnedBuildingInfo>();
		private static List<Job> MyJobs = new List<Job>();
		private static List<string> MySubTasks = new List<string>();

		private static Dictionary<int, WoWPoint> PlotIdLocations = new Dictionary<int, WoWPoint>();
		private static int _currentJobIndex;
		private static int _currentSubTaskIndex;

		public static void Initialize()
		{
			MyBuildings = GarrisonInfo.OwnedBuildings.ToList();
			PlotIdLocations.Clear();
			PlotIdLocations.Add(18, new WoWPoint(5647.941, 4517.003, 119.2743));
			PlotIdLocations.Add(19, new WoWPoint(5653.386, 4549.211, 119.2657));
			PlotIdLocations.Add(20, new WoWPoint(5620.319, 4512.534, 120.1376));

			// Add universal buildings to job list
			MyJobs.Clear();
			MyJobs.Add(new Job("GarrisonCache", new WoWPoint(5593.86, 4586.82, 136.61), 237191, 0, 0));
			MyJobs.Add(new Job("HerbGarden", new WoWPoint(5414.47, 4573.50, 137.53), 239238, 85783, 0));
			MyJobs.Add(new Job("Mines", new WoWPoint(5475.488, 4452.166, 144.4591), 239237, 81688, 0));
			MyJobs.Add(new Job("PrimalTrader", new WoWPoint(5577.58, 4388.634, 136.4497), 0, 84967, 0));

			// Add small buildings to job list
			if (MyBuildings.Any(o => o.Type.ToString() == "Leatherworking"))
				MyJobs.Add(new Job("Leatherworking", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Leatherworking").PlotInstanceId], 237104, 79833, 79834));
			if (MyBuildings.Any(o => o.Type.ToString() == "Alchemy"))
				MyJobs.Add(new Job("Alchemy", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Alchemy").PlotInstanceId], 237120, 79814, 79813));
			if (MyBuildings.Any(o => o.Type.ToString() == "Jewelcrafting"))
				MyJobs.Add(new Job("Jewelcrafting", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Jewelcrafting").PlotInstanceId], 237069, 79830, 79832));
			if (MyBuildings.Any(o => o.Type.ToString() == "Enchanting"))
				MyJobs.Add(new Job("Enchanting", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Enchanting").PlotInstanceId], 237138, 79820, 79821));
			if (MyBuildings.Any(o => o.Type.ToString() == "Blacksmithing"))
				MyJobs.Add(new Job("Blacksmithing", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Blacksmithing").PlotInstanceId], 237131, 79817, 79867));
			if (MyBuildings.Any(o => o.Type.ToString() == "Tailoring"))
				MyJobs.Add(new Job("Tailoring", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Tailoring").PlotInstanceId], 237186, 79863, 49864));
			if (MyBuildings.Any(o => o.Type.ToString() == "Jewelcrafting"))
				MyJobs.Add(new Job("Jewelcrafting", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Jewelcrafting").PlotInstanceId], 237069, 79830, 79832));
			if (MyBuildings.Any(o => o.Type.ToString() == "Engineering"))
				MyJobs.Add(new Job("Engineering", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Engineering").PlotInstanceId], 237146, 86696, 88610));
			if (MyBuildings.Any(o => o.Type.ToString() == "Inscription"))
				MyJobs.Add(new Job("Inscription", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Inscription").PlotInstanceId], 237066, 79831, 79829));
			if (MyBuildings.Any(o => o.Type.ToString() == "SalvageYard"))
				MyJobs.Add(new Job("SalvageYard", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "SalvageYard").PlotInstanceId], 0, 79857, 0));
			
			// Add last building to job list
			MyJobs.Add(new Job("CommandTable", new WoWPoint(5559.691, 4604.524, 141.7168), 0, 0, 0));
			
			_currentJobIndex = 0;
			_currentSubTaskIndex = 0;
			MySubTasks.Clear();
			MySubTasks.Add("MoveToJob");
			Helpers.Log("Job List Created");
		}

		public static void NextJob()
		{
			_currentJobIndex++;

			TreeRoot.StatusText = "Current Job - " + CurrentJob().Name;
			_currentSubTaskIndex = 0;
			MySubTasks.Clear();
			MySubTasks.Add("MoveToJob");
		}

		public static void NextSubTask()
		{
			_currentSubTaskIndex++;
		}

		public static Job CurrentJob()
		{
			return MyJobs[_currentJobIndex];
		}

		public static string CurrentSubTask()
		{
			return MySubTasks[_currentSubTaskIndex];
		}
	}
}
