using System.Collections.Generic;
using Styx;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.Garrison;

namespace TinyGarrison
{
	enum CustomLocationType
	{
		GarrisonCache,
		CommandTable,
		PrimalTrader,
		GardenCheck,
		MineSafeCheck
	}

	enum CustomEntryType
	{
		GarrisonCache,
		HerbNodes,
		OreNodes,
		HerbInventoryItems,
		SavageBlood,
		PrimalTrader,
		Salvage,
		FollowerUpgrades,
		GearTokens,
		ScrapsDailyNpc,
		CommandTable
	}

	class Data
	{
		// Locations
		public static Dictionary<GarrisonBuildingType, WoWPoint> ShipmentCrateLocations =
			new Dictionary<GarrisonBuildingType, WoWPoint>
		{
			{GarrisonBuildingType.HerbGarden, new WoWPoint(5414.658, 4574.515, 137.4041)},
			{GarrisonBuildingType.Mines, new WoWPoint(5476.652, 4454.104, 144.2728)}
		};

		public static Dictionary<GarrisonBuildingType, WoWPoint> WorkOrderNpcLocations =
			new Dictionary<GarrisonBuildingType, WoWPoint>
		{
			{GarrisonBuildingType.HerbGarden, new WoWPoint(5413.059, 4565.93, 138.3481)},
			{GarrisonBuildingType.Mines, new WoWPoint(5468.56, 4446.543, 144.7709)}
		};

		public static Dictionary<CustomLocationType, WoWPoint> CustomLocations = new Dictionary<CustomLocationType, WoWPoint>
		{
			{CustomLocationType.GarrisonCache, new WoWPoint(5594.231, 4587.403, 136.61)},
			{CustomLocationType.CommandTable, new WoWPoint(5560.114, 4604.042, 141.7173)},
			{CustomLocationType.PrimalTrader, new WoWPoint(5577.58, 4388.634, 136.4497)},
			{CustomLocationType.GardenCheck, new WoWPoint(5440.083, 4571.781, 135.7767)},
			{CustomLocationType.MineSafeCheck, new WoWPoint(5445.551, 4484.773, 84.06298)}
		};

		public static void InitializeBuildingLocations()
		{
			foreach (var ownedBuilding in GarrisonInfo.OwnedBuildings)
			{
				switch (ownedBuilding.PlotInstanceId)
				{
					case 18:
						ShipmentCrateLocations.Add(ownedBuilding.Type, new WoWPoint(5645.203, 4516.291, 119.2689));
						WorkOrderNpcLocations.Add(ownedBuilding.Type, new WoWPoint(5643.616, 4506.593, 120.1372));
						break;
					case 19:
						ShipmentCrateLocations.Add(ownedBuilding.Type, new WoWPoint(5654.403, 4544.771, 119.2653));
						WorkOrderNpcLocations.Add(ownedBuilding.Type, new WoWPoint(5662.758, 4548.382, 120.1351));
						break;
					case 20:
						ShipmentCrateLocations.Add(ownedBuilding.Type, new WoWPoint(5625.955, 4518.966, 119.2701));
						WorkOrderNpcLocations.Add(ownedBuilding.Type, new WoWPoint(5620.081, 4512.218, 120.1375));
						break;
					case 24:
						ShipmentCrateLocations.Add(ownedBuilding.Type, new WoWPoint(5646.772, 4452.765, 130.526));
						WorkOrderNpcLocations.Add(ownedBuilding.Type, new WoWPoint(5650.63, 4442.224, 132.8824));
						break;
				}
			}
		}

		// Entries (Crates, NPCs, Nodes, etc)
		public static Dictionary<CustomEntryType, HashSet<uint>> CustomEntries =
			new Dictionary<CustomEntryType, HashSet<uint>>
			{
				{CustomEntryType.GarrisonCache, new HashSet<uint>{237191, 235389, 237720}},
				{CustomEntryType.HerbNodes, new HashSet<uint>{235389, 235391, 235388, 235390, 235376, 235387}},
				{CustomEntryType.OreNodes, new HashSet<uint>{232545, 232544, 232543, 232541,232542}},
				{CustomEntryType.HerbInventoryItems, new HashSet<uint>{109124,109125,109126,109127,109128,109129}},
				{CustomEntryType.PrimalTrader, new HashSet<uint>{84967}},
				{CustomEntryType.Salvage, new HashSet<uint>{114120, 114116}},
				{CustomEntryType.FollowerUpgrades, new HashSet<uint>{120302, 120301}},
				{CustomEntryType.GearTokens, new HashSet<uint>{114082,114084,114085,114086,114087,114112,114083,127822,127819,
				127818,127810,127809,127806,127805,127797,127796,127793,127792,127784,127783,127780,127779,127823,
				114070,114071,114075,114078,114080,114110,114069,114066,114057,114109,114068,114058,114063,114059,
				114100,114105,114097,114099,114094,114108,114096,114098,114101,114052,114053}},
				{CustomEntryType.ScrapsDailyNpc, new HashSet<uint>{79815}},
				{CustomEntryType.CommandTable, new HashSet<uint>{00000000}}
			};

		public static Dictionary<GarrisonBuildingType, HashSet<uint>> ShipmentCrates =
			new Dictionary<GarrisonBuildingType, HashSet<uint>>
			{
				{GarrisonBuildingType.HerbGarden, new HashSet<uint>{239238}},
				{GarrisonBuildingType.Mines, new HashSet<uint>{239237}},
				{GarrisonBuildingType.Leatherworking, new HashSet<uint>{237104}},
				{GarrisonBuildingType.Alchemy, new HashSet<uint>{237120}},
				{GarrisonBuildingType.Jewelcrafting, new HashSet<uint>{237069}},
				{GarrisonBuildingType.Enchanting, new HashSet<uint>{237138}},
				{GarrisonBuildingType.Blacksmithing, new HashSet<uint>{237131}},
				{GarrisonBuildingType.Tailoring, new HashSet<uint>{237186, 237190}},
				{GarrisonBuildingType.Inscription, new HashSet<uint>{237066, 237064}},
				{GarrisonBuildingType.Engineering, new HashSet<uint>{237146}}
			};

		public static Dictionary<GarrisonBuildingType, HashSet<uint>> WorkOrderNpcs =
			new Dictionary<GarrisonBuildingType, HashSet<uint>>
			{
				{GarrisonBuildingType.HerbGarden, new HashSet<uint>{85783}},
				{GarrisonBuildingType.Mines, new HashSet<uint>{81688}},
				{GarrisonBuildingType.Leatherworking, new HashSet<uint>{79833}},
				{GarrisonBuildingType.Alchemy, new HashSet<uint>{79814}},
				{GarrisonBuildingType.Jewelcrafting, new HashSet<uint>{79830}},
				{GarrisonBuildingType.Enchanting, new HashSet<uint>{79820}},
				{GarrisonBuildingType.Blacksmithing, new HashSet<uint>{79817}},
				{GarrisonBuildingType.Tailoring, new HashSet<uint>{79863}},
				{GarrisonBuildingType.Inscription, new HashSet<uint>{79831}},
				{GarrisonBuildingType.Engineering, new HashSet<uint>{86696}},
				{GarrisonBuildingType.SalvageYard, new HashSet<uint>{79857}}
			};

		public static HashSet<uint> DailyProfessionSpells = new HashSet<uint>
		{
			171391, 156587, 170700, 169092, 171690, 168835, 169080, 169081
		};

		public static HashSet<uint> DailyProfessionSecrets = new HashSet<uint>
		{
			177043, 176090, 177054, 176087, 176058, 176089, 175880, 177045
		};

		public static HashSet<uint> TransmuteBlood = new HashSet<uint>
		{
			181643
		};
	}
}