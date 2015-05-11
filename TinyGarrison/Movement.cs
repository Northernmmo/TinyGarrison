using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals.DB;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison
{
	class Movement
	{
		private static readonly LocalPlayer Me = StyxWoW.Me;

		public static async Task<bool> Move()
		{
			if (Jobs.CurrentJob.Location == Data.CustomLocations[CustomLocationType.GardenCheck])
				Helpers.Log("Moving to check Garden/Mine");
			else if (Jobs.CurrentJob.Location == Data.ShipmentCrateLocations[GarrisonBuildingType.SalvageYard])
				Helpers.Log("Moving to Salvage");
			else if (Jobs.CurrentJob.Location == Data.ShipmentCrateLocations[GarrisonBuildingType.WarMill])
				Helpers.Log("Moving to check Scraps Daily");
			else if (Jobs.CurrentJob.Location == Data.CustomLocations[CustomLocationType.CommandTable])
				Helpers.Log("Done, moving to command table");

			var r = await CommonCoroutines.MoveTo(Jobs.CurrentJob.Location);

			if (r != MoveResult.ReachedDestination) return true;

			Jobs.NextJob();
			return true;
		}

		public static async Task<bool> MoveTo(WoWGameObject destinationObject)
		{
			var pointFromTarget = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, destinationObject.Location, destinationObject.InteractRange - 2);

			if (StyxWoW.Me.SubZoneId == 7329)
			{
				// Coffee & Pickaxe
				if (Me.BagItems.Any(o => o.Entry == 118897) && (!Me.HasAura(176049) || Me.GetAuraById(176049).StackCount < 2)) Me.BagItems.First(o => o.Entry == 118897).Interact();
				if (Me.BagItems.Any(o => o.Entry == 118903) && !Me.HasAura(176061)) Me.BagItems.First(o => o.Entry == 118903).Interact();

				// Self buffs (Druid form, hunter aspect etc) coming soon! (tm)
				if (Me.Class == WoWClass.Druid && !Me.HasAura("Cat Form")) SpellManager.Cast("Cat Form");
				if (Me.Class == WoWClass.Shaman && !Me.HasAura("Ghost Wolf")) SpellManager.Cast("Ghost Wolf");
			}

			await CommonCoroutines.MoveTo(pointFromTarget);
			return true;
		}

		public static async Task<bool> MoveTo(WoWUnit destination)
		{
			var pointFromTarget = WoWMathHelper.CalculatePointFrom(StyxWoW.Me.Location, destination.Location, destination.InteractRange - 2);

			await CommonCoroutines.MoveTo(pointFromTarget);
			return true;
		}
	}
}
