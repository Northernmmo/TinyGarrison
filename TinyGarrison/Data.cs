using Styx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison
{
	class Data
	{
		// Object Entries
		public static HashSet<uint> GarrisonCache = new HashSet<uint>(){ 235389, 237191, 237720 };

		// WoWPoint Locations
		public static Dictionary<string, WoWPoint> Locations = new Dictionary<string, WoWPoint>(){
			{"GarrisonCache", new WoWPoint(5593.86, 4586.82, 136.61)}
		};
	}
}
