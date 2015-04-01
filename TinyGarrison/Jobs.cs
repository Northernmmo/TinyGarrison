using System.Collections.Generic;
using System.Linq;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison
{
	class Job
	{
		public Job(string name, WoWPoint location, int shipmentCrateEntry, int workOrderNpcEntry, int professionNpcEntry, bool alreadyMoved)
		{
			Name = name;
			Location = location;
			ShipmentCrateEntry = shipmentCrateEntry;
			WorkOrderNpcEntry = workOrderNpcEntry;
			ProfessionNpcEntry = professionNpcEntry;
			AlreadyMoved = alreadyMoved;
		}

		public string Name { get; set; }
		public WoWPoint Location { get; set; }
		public int ShipmentCrateEntry { get; set; }
		public int WorkOrderNpcEntry { get; set; }
		public int ProfessionNpcEntry { get; set; }
		public bool AlreadyMoved { get; set; }
	}

	class Jobs
	{
		public static List<OwnedBuildingInfo> MyBuildings = new List<OwnedBuildingInfo>();
		public static List<Job> MyJobs = new List<Job>();

		public static Dictionary<int, WoWPoint> PlotIdLocations = new Dictionary<int, WoWPoint>();
		private static int _currentJobIndex;

		public static void Initialize()
		{
			MyBuildings = GarrisonInfo.OwnedBuildings.ToList();
			PlotIdLocations.Clear();
			PlotIdLocations.Add(18, new WoWPoint(5647.941, 4517.003, 119.2743));
			PlotIdLocations.Add(19, new WoWPoint(5653.386, 4549.211, 119.2657));
			PlotIdLocations.Add(20, new WoWPoint(5620.319, 4512.534, 120.1376));

			// Add universal buildings to job list
			MyJobs.Clear();
			MyJobs.Add(new Job("GarrisonCache", new WoWPoint(5593.86, 4586.82, 136.61), 237191, 0, 0, false));
			MyJobs.Add(new Job("HerbGarden", new WoWPoint(5414.47, 4573.50, 137.53), 239238, 85783, 0, false));
			MyJobs.Add(new Job("Mines", new WoWPoint(5475.488, 4452.166, 144.4591), 239237, 81688, 0, false));
			MyJobs.Add(new Job("PrimalTrader", new WoWPoint(5577.58, 4388.634, 136.4497), 0, 84967, 0, false));

			// Add small buildings to job list
			if (MyBuildings.Any(o => o.Type.ToString() == "Leatherworking"))
				MyJobs.Add(new Job("Leatherworking", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Leatherworking").PlotInstanceId], 237104, 79833, 79834, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Alchemy"))
				MyJobs.Add(new Job("Alchemy", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Alchemy").PlotInstanceId], 237120, 79814, 79813, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Jewelcrafting"))
				MyJobs.Add(new Job("Jewelcrafting", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Jewelcrafting").PlotInstanceId], 237069, 79830, 79832, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Enchanting"))
				MyJobs.Add(new Job("Enchanting", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Enchanting").PlotInstanceId], 237138, 79820, 79821, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Blacksmithing"))
				MyJobs.Add(new Job("Blacksmithing", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Blacksmithing").PlotInstanceId], 237131, 79817, 79867, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Tailoring"))
				MyJobs.Add(new Job("Tailoring", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Tailoring").PlotInstanceId], 237186, 79863, 49864, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Jewelcrafting"))
				MyJobs.Add(new Job("Jewelcrafting", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Jewelcrafting").PlotInstanceId], 237069, 79830, 79832, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Engineering"))
				MyJobs.Add(new Job("Engineering", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Engineering").PlotInstanceId], 237146, 86696, 88610, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "Inscription"))
				MyJobs.Add(new Job("Inscription", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "Inscription").PlotInstanceId], 237066, 79831, 79829, false));
			if (MyBuildings.Any(o => o.Type.ToString() == "SalvageYard"))
				MyJobs.Add(new Job("SalvageYard", PlotIdLocations[MyBuildings.First(o => o.Type.ToString() == "SalvageYard").PlotInstanceId], 0, 79857, 0, false));
			
			// Add last building to job list
			MyJobs.Add(new Job("Done", new WoWPoint(5559.691, 4604.524, 141.7168), 0, 0, 0, false));
			
			_currentJobIndex = 0;
			Helpers.Log("Job List Created");
			TreeRoot.StatusText = "Current Job - " + CurrentJob().Name;
		}

		public static void NextJob()
		{
			TreeRoot.StatusText = "Current Job - " + CurrentJob().Name;
			_currentJobIndex++;
		}

		public static Job CurrentJob()
		{
			return MyJobs[_currentJobIndex];
		}
	}
}
