﻿using System.Windows.Media;
using Styx.Common;
using Styx.WoWInternals.DB;
using Styx.WoWInternals;

namespace TinyGarrison
{
	class Helpers
	{
		private static string _lastMsg;

		public static void Log(string msg)
		{
			if (msg == _lastMsg) return;
			Logging.Write(Colors.DarkOrange, "[TG] " + msg);
			_lastMsg = msg;
		}

		public static bool HasWorkOrderMaterial
		{
			get
			{
				switch (Jobs.CurrentJob.Building)
				{
					case GarrisonBuildingType.HerbGarden:
						return Lua.GetReturnVal<int>("return GetItemCount('Draenic Seeds')", 0) >= 5;
					case GarrisonBuildingType.Mines:
						return Lua.GetReturnVal<int>("return GetItemCount('Draenic Stone')", 0) >= 5;
					case GarrisonBuildingType.Enchanting:
						return Lua.GetReturnVal<int>("return GetItemCount('Draenic Dust')", 0) >= 5;
					case GarrisonBuildingType.Alchemy:
						return Lua.GetReturnVal<int>("return GetItemCount('Frostweed')", 0) >= 5;
					case GarrisonBuildingType.Leatherworking:
						return Lua.GetReturnVal<int>("return GetItemCount('Raw Beast Hide')", 0) >= 5;
					case GarrisonBuildingType.Jewelcrafting:
						return Lua.GetReturnVal<int>("return GetItemCount('Blackrock Ore')", 0) >= 5;
					case GarrisonBuildingType.Blacksmithing:
						return Lua.GetReturnVal<int>("return GetItemCount('True Iron Ore')", 0) >= 5;
					case GarrisonBuildingType.Tailoring:
						return Lua.GetReturnVal<int>("return GetItemCount('Sumptuous Fur')", 0) >= 5;
					case GarrisonBuildingType.Engineering:
						return Lua.GetReturnVal<int>("return GetItemCount('Blackrock Ore')", 0) >= 2 &&
							Lua.GetReturnVal<int>("return GetItemCount('True Iron Ore')", 0) >= 2;
					case GarrisonBuildingType.Inscription:
						return Lua.GetReturnVal<int>("return GetItemCount('Cerulean Pigment')", 0) >= 2;
				}

				return false;
			}
		}

		public static bool HasProfessionMaterial
		{
			get
			{
				switch (Jobs.CurrentJob.Building)
				{
					case GarrisonBuildingType.Leatherworking: //Leatherworking
						return Lua.GetReturnVal<bool>("return GetItemCount('Raw Beast Hide') >= 20 and GetItemCount('Gorgrond Flytrap') >= 10", 0);
					case GarrisonBuildingType.Alchemy: //Alchemy
						return Lua.GetReturnVal<bool>("return GetItemCount('Frostweed') >= 20 and GetItemCount('Blackrock Ore') >= 10", 0);
					case GarrisonBuildingType.Jewelcrafting: //Jewelcrafting
						return Lua.GetReturnVal<bool>("return GetItemCount('Blackrock Ore') >= 20 and GetItemCount('True Iron Ore') >= 10", 0);
					case GarrisonBuildingType.Enchanting: //Enchanting
						return Lua.GetReturnVal<bool>("return GetItemCount('Luminous Shard') >= 1", 0);
					case GarrisonBuildingType.Blacksmithing: //Blacksmithing
						return Lua.GetReturnVal<bool>("return GetItemCount('True Iron Ore') >= 20 and GetItemCount('Blackrock Ore') >= 10", 0);
					case GarrisonBuildingType.Tailoring: //Tailoring
						return Lua.GetReturnVal<bool>("return GetItemCount('Sumptuous Fur') >= 20 and GetItemCount('Gorgrond Flytrap') >= 10", 0);
					case GarrisonBuildingType.Engineering: //Engineering
						return Lua.GetReturnVal<bool>("return GetItemCount('True Iron Ore') >= 15 and GetItemCount('Blackrock Ore') >= 15", 0);
					case GarrisonBuildingType.Inscription: //Inscription
						return Lua.GetReturnVal<bool>("return GetItemCount('Cerulean Pigment') >= 10", 0);
				}

				return false;
			}
		}
	}
}
