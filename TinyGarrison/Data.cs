using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison
{
	class Data
	{
		public static HashSet<uint> GarrisonCache = new HashSet<uint>{ 235389, 237191, 237720 };
		public static HashSet<uint> GardenHerbs = new HashSet<uint>{ 235389, 235391, 235388, 235390, 235376, 235387};
		public static HashSet<uint> GardenShipment = new HashSet<uint> { 239238 };
		public static HashSet<uint> MineOre = new HashSet<uint> { 232545, 232544, 232543, 232541, 232542 };
		public static HashSet<uint> MineShipment = new HashSet<uint> { 239237 };
		public static HashSet<uint> Shipment = new HashSet<uint> { 237104, 237120, 237069, 237138, 237131, 237186, 237146, 237066 };
		public static HashSet<uint> HerbItems = new HashSet<uint> { 109124, 109125, 109126, 109127, 109128, 109129 };

		public static HashSet<uint> LeatherworkingShipments = new HashSet<uint> { 237104 };
		public static HashSet<uint> AlchemyShipments = new HashSet<uint> { 237120 };
		public static HashSet<uint> JewelcraftingShipments = new HashSet<uint> { 237069 };
		public static HashSet<uint> EnchantingShipments = new HashSet<uint> { 237138 };
		public static HashSet<uint> BlacksmithingShipments = new HashSet<uint> { 237131 };
		public static HashSet<uint> TailoringShipments = new HashSet<uint> { 237186 };
		public static HashSet<uint> EngineeringShipments = new HashSet<uint> { 237146 };
		public static HashSet<uint> InscriptionShipments = new HashSet<uint> { 237066 };

		public static Dictionary<GarrisonBuildingType, HashSet<uint>> WorkOrderNPCs = new Dictionary<GarrisonBuildingType, HashSet<uint>>
		{
			{GarrisonBuildingType.HerbGarden, new HashSet<uint>{ 85783 }},
			{GarrisonBuildingType.Mines, new HashSet<uint>{ 81688 }},
			{GarrisonBuildingType.Leatherworking, new HashSet<uint>{ 79833 }},
			{GarrisonBuildingType.Alchemy, new HashSet<uint>{ 79814 }},
			{GarrisonBuildingType.Jewelcrafting, new HashSet<uint>{ 79830 }},
			{GarrisonBuildingType.Enchanting, new HashSet<uint>{ 79820 }},
			{GarrisonBuildingType.Blacksmithing, new HashSet<uint>{ 79817 }},
			{GarrisonBuildingType.Tailoring, new HashSet<uint>{ 79863 }},
			{GarrisonBuildingType.Engineering, new HashSet<uint>{ 86696 }},
			{GarrisonBuildingType.Inscription, new HashSet<uint>{ 79831 }},
		};

		public static Dictionary<int, WoWPoint> PlotLocations = new Dictionary<int, WoWPoint>
		{
			{18, new WoWPoint(5647.941, 4517.003, 119.2743)},
			{19, new WoWPoint(5653.386, 4549.211, 119.2657)},
			{20, new WoWPoint(5620.319, 4512.534, 120.1376)}
		};
	}
}
