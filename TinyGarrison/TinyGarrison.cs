using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bots.Grind;
using Bots.Quest;
using CommonBehaviors.Actions;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.CommonBot.Profiles.Quest.Order;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TinyGarrison.GUI;

namespace TinyGarrison
{
    public class TinyGarrison : BotBase
    {
		public static readonly Version Version = new Version(5, 1, 3);
		#region Declerations & Overrides
		public static readonly LocalPlayer Me = StyxWoW.Me;
	    private Composite _root;
		public override string Name { get { return "TinyGarrison"; } }
	    public override PulseFlags PulseFlags { get { return PulseFlags.All; } }
		public override Composite Root { get { return _root ?? (_root = new ActionRunCoroutine(ret => RootLogic())); } }
		public override void OnSelected() { Jobs.Initialize(); }
		public override Form ConfigurationForm { get { return new TinyGarrisonGUI(); } }
		#endregion

	    public static async Task<bool> RootLogic()
	    {
		    #region DefaultBotBehaviors
		    if (await DeathBehavior.ExecuteCoroutine()) return true;
		    if (StyxWoW.Me.Combat && await CombatBehavior.ExecuteCoroutine()) return true;
		    if (await VendorBehavior.ExecuteCoroutine()) return true;
		    if (await Loot()) return true;
		    #endregion

		    switch (Jobs.CurrentJob.Type)
		    {
				case JobType.Move:
				    await Movement.Move();
				    return true;
				case JobType.GarrisonCache:
				    await Tasks.GarrisonCache();
					return true;
				case JobType.PickupShipments:
					await Tasks.PickupShipments();
					return true;
				case JobType.Gather:
					await Tasks.Gather();
					return true;
				case JobType.StartWorkOrders:
					await Tasks.StartWorkOrders();
					return true;
				case JobType.Crafting:
				    await Tasks.Crafting();
				    return true;
				case JobType.Vendor:
					await Tasks.Vendor();
					return true;
				case JobType.PrimalTrader:
					await Tasks.PrimalTrader();
					return true;
				case JobType.ScrapsDaily:
					await Tasks.ScrapsDaily();
					return true;
				case JobType.Salvage:
				    await Tasks.Salvage();
				    return true;
				case JobType.JewelcraftingDailyQuest:
				    await Tasks.JewelcraftingDailyQuest();
				    return true;
				case JobType.Done:
					TreeRoot.Stop("Done with Garrison");
				    return true;
		    }
			
			Helpers.Log("Missed JOB!: " + Jobs.CurrentJob.Type);
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
				var lootTarget = ObjectManager.GetObjectsOfType<WoWUnit>()
					.Where(o => o.IsDead && o.CanLoot && o.Distance < 100 && o.KilledByMe)
					.OrderBy(o => o.Distance)
					.First();
				if (lootTarget != null)
				{
					if (!lootTarget.WithinInteractRange)
						await CommonCoroutines.MoveTo(lootTarget.Location);

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