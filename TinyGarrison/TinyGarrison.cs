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

			if (Jobs.CurrentJob() != null)
			{
				switch (Jobs.CurrentSubTask())
				{
					case "MoveToJob":
						return await Helpers.MoveToJob(Jobs.CurrentJob().Location);
					case "LootShipments":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
					case "Harvest":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
					case "StartWorkOrders":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
					case "DoProfession":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
					case "BuySavageBlood":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
					case "Salvage":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
					case "Vendor":
						await Coroutine.Sleep(10000);
						Jobs.NextSubTask();
						return true;
				}
			}

			Jobs.NextJob();
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
