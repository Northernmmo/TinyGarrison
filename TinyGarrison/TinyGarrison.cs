using System;
using System.Linq;
using System.Threading.Tasks;
using Bots.Grind;
using CommonBehaviors.Actions;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Buddy.Coroutines;
using Styx.WoWInternals.DB;

namespace TinyGarrison
{
    public class TinyGarrison : BotBase
    {
		#region Declerations & Overrides
		public static readonly LocalPlayer Me = StyxWoW.Me;
		private Composite _root;
		public override string Name { get { return "TinyGarrison"; } }
		public override PulseFlags PulseFlags { get { return PulseFlags.All; } }
		public override Composite Root { get { return _root ?? (_root = new ActionRunCoroutine(ret => RootLogic())); } }
		public override void OnSelected() { Jobs.Initialize(); }
		#endregion

	    public static async Task<bool> RootLogic()
	    {
		    #region DefaultBotBehaviors
		    if (await DeathBehavior.ExecuteCoroutine()) return true;
		    if (StyxWoW.Me.Combat && await CombatBehavior.ExecuteCoroutine()) return true;
		    if (await VendorBehavior.ExecuteCoroutine()) return true;
		    if (await Loot()) return true;
		    #endregion

			switch (Jobs.CurrentJob().Type)
			{
				case GarrisonBuildingType.Unknown:
					switch (Jobs.CurrentJob().Name)
					{
						case "GarrisonCache":
							return await Tasks.GarrisonCache.Handler();
						case "Profession":
							return await Tasks.Profession.Handler();
						case "PrimalTrader":
							return await Tasks.PrimalTrader.Handler();
						case "CommandTable":
							return await Tasks.CommandTable.Handler();
						case "Done":
							Helpers.Log("Done");
							return true;
					}
					return true;
				case GarrisonBuildingType.HerbGarden:
					return await Tasks.HerbGarden.Handler();
				case GarrisonBuildingType.Mines:
					return await Tasks.Mines.Handler();
				case GarrisonBuildingType.SalvageYard:
					return await Tasks.SalvageYard.Handler();
			}
			if (Jobs.CurrentJob().ProfessionNpcEntry != 0 && Jobs.CurrentJob().WorkOrderNpcEntry != 0)
				return await Tasks.ProfessionBuilding.Handler();
			return true;
	    }

	    #region DefaultBehaviorDeclerations
		private static Composite _deathBehavior;
		private static Composite _combatBehavior;
		private static Composite _vendorBehavior;

		private static Composite DeathBehavior { get { return _deathBehavior ?? (_deathBehavior = LevelBot.CreateDeathBehavior()); } }
		private static Composite CombatBehavior { get { return _combatBehavior ?? (_combatBehavior = LevelBot.CreateCombatBehavior()); } }
		private static Composite VendorBehavior { get { return _vendorBehavior ?? (_vendorBehavior = LevelBot.CreateVendorBehavior()); } }

		public static async Task<bool> Loot()
		{
			if (StyxWoW.Me.Combat)
				return false;

			try
			{
				WoWUnit lootTarget = ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => o.IsDead && o.CanLoot && o.Distance < 100 && o.KilledByMe)
					.OrderBy(o => o.Distance)
					.First();
				if (lootTarget != null)
				{
					if (!lootTarget.WithinInteractRange)
						return await Helpers.MoveTo(lootTarget.Location);

					await CommonCoroutines.SleepForLagDuration();
					lootTarget.Interact();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
					await CommonCoroutines.SleepForLagDuration();
					return true;
				}
			}
			catch (Exception)
			{
				// ignored
			}
			return false;
		}
		#endregion
    }
}
