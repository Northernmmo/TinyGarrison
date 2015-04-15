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

		public static Dictionary<GarrisonBuildingType, HashSet<uint>> WorkOrderNPCs = new Dictionary<GarrisonBuildingType, HashSet<uint>>
		{
			{GarrisonBuildingType.HerbGarden, new HashSet<uint>{ 85783}},
			{GarrisonBuildingType.Mines, new HashSet<uint>{ 81688}}
		};
	}
}
