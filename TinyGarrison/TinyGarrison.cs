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
using Styx.WoWInternals.DB;
using Styx.WoWInternals.WoWObjects;
using TinyGarrison.Tasks;

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

			// Movement Buffs
			if (Me.IsIndoors)
			{
				if (Me.Class == WoWClass.Shaman && !Me.HasAura("Ghost Wolf")) SpellManager.Cast("Ghost Wolf");
				if (Me.Class == WoWClass.Druid && !Me.HasAura("Cat Form")) SpellManager.Cast("Cat Form");
			}

			// Job Handler
			switch (Jobs.CurrentJob().Type)
			{
				case GarrisonBuildingType.Unknown:
					switch (Jobs.CurrentJob().Name)
					{
						case "GarrisonCache":
							return await GarrisonCache.Handler();
						case "Profession":
							return await Profession.Handler();
						case "PrimalTrader":
							return await PrimalTrader.Handler();
						case "CommandTable":
							return await CommandTable.Handler();
						case "Done":
							Helpers.Log("Done");
							return true;
					}
					return true;
				case GarrisonBuildingType.HerbGarden:
					return await HerbGarden.Handler();
				case GarrisonBuildingType.Mines:
					return await Mines.Handler();
				case GarrisonBuildingType.SalvageYard:
					return await SalvageYard.Handler();
			}
			if (Jobs.CurrentJob().ProfessionNpcEntry != 0 && Jobs.CurrentJob().WorkOrderNpcEntry != 0)
				return await ProfessionBuilding.Handler();
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
