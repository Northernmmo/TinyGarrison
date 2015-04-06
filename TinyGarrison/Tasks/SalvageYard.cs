using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buddy.Coroutines;
using Styx;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison.Tasks
{
	class SalvageYard
	{
		private static bool _alreadyMoved;
		private static bool _needToVendor;
		public static readonly LocalPlayer Me = StyxWoW.Me;

		public static async Task<bool> Handler()
		{
			// Move to Job
			if (!_alreadyMoved)
			{
				_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
				await CommonCoroutines.SleepForLagDuration();
				return true;
			}

			// Vendor if we need bagspace
			if (Me.FreeNormalBagSlots <= 2) _needToVendor = true;

			// Open salvage and tokens
			List<WoWItem> items = StyxWoW.Me.BagItems.Where(o => (new HashSet<uint>()
			{
				114116, 114120, 114119, 120301, 122633, 122607, 114069, 114071, 114075, 114078, 114080, 
				114110, 122621, 122622, 122623, 122624, 122625, 122626, 122627, 122628, 122629, 122630, 
				122631, 122632, 114070, 114057, 114059, 114060, 114063, 114066, 114068, 114109, 114058, 
				114100, 114105, 114097, 114099, 114094, 114108, 114096, 114098, 114101, 114052
			}.Contains(o.Entry))).ToList();

			while (items.Count > 0 && Me.FreeNormalBagSlots > 2)
			{
				items.First().Interact();
				await CommonCoroutines.SleepForLagDuration();
				await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
				await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				_needToVendor = true;

				items = StyxWoW.Me.BagItems.Where(o => (new HashSet<uint>()
				{
				114116, 114120, 114119, 120301, 122633, 122607, 114069, 114071, 114075, 114078, 114080, 
				114110, 122621, 122622, 122623, 122624, 122625, 122626, 122627, 122628, 122629, 122630, 
				122631, 122632, 114070, 114057, 114059, 114060, 114063, 114066, 114068, 114109, 114058, 
				114100, 114105, 114097, 114099, 114094, 114108, 114096, 114098, 114101, 114052
				}.Contains(o.Entry))).ToList();
			}

			// Vendor
			if (_needToVendor)
			{
				WoWUnit vendorNpc =
					ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(o => o.Entry == Jobs.CurrentJob().WorkOrderNpcEntry)
						.OrderBy(o => o.Distance).FirstOrDefault();

				if (vendorNpc != null && vendorNpc.IsValid)
				{
					vendorNpc.Interact();
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_SHOW", 3000);
					await Coroutine.Sleep(8000);
					Lua.DoString("MerchantFrameCloseButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_CLOSED", 3000);
					_needToVendor = false;
					return true;
				}
			}

			Jobs.NextJob();
			return true;
		}
	}
}
/*



			if (items.Count > 0)
			{
				// Move to SalvageYard
				if (!_movedCheck && Me.Location.Distance(Jobs.CurrentJob().Location) > 2)
				{
					Helpers.Log("Moving to " + Jobs.CurrentJob().Name);
					return await Helpers.MoveTo(Jobs.CurrentJob().Location);
				}
				_movedCheck = true;
				_needToVendor = true;

				// Open Salvage && Tokens
				while (StyxWoW.Me.BagItems.Any(o => (new HashSet<uint>()
				{
					114116, 114120, 114119, 120301, 122633, 122607, 114069, 114071, 114075, 114078, 114080, 
					114110, 122621, 122622, 122623, 122624, 122625, 122626, 122627, 122628, 122629, 122630, 
					122631, 122632, 114070, 114057, 114059, 114060, 114063, 114066, 114068, 114109, 114058, 
					114100, 114105, 114097, 114099, 114094, 114108, 114096, 114098, 114101, 114052
				}.Contains(o.Entry))))
				{
					await CommonCoroutines.SleepForLagDuration();
					StyxWoW.Me.BagItems.FirstOrDefault(o => (new HashSet<uint>()
					{
						114116, 114120, 114119, 120301, 122633, 122607, 114069, 114071, 114075, 114078, 114080, 
						114110, 122621, 122622, 122623, 122624, 122625, 122626, 122627, 122628, 122629, 122630, 
						122631, 122632, 114070, 114057, 114059, 114060, 114063, 114066, 114068, 114109, 114058, 
						114100, 114105, 114097, 114099, 114094, 114108, 114096, 114098, 114101, 114052
					}.Contains(o.Entry))).Interact();
					await CommonCoroutines.SleepForLagDuration();
					await CommonCoroutines.WaitForLuaEvent("LOOT_OPENED", 3000);
					await CommonCoroutines.WaitForLuaEvent("LOOT_CLOSED", 3000);
				}
			}

			// Vendor
			if (_needToVendor)
			{
				WoWUnit vendorNPC =
					ObjectManager.GetObjectsOfType<WoWUnit>()
						.Where(o => o.Entry == Jobs.CurrentJob().WorkOrderNpcEntry)
						.OrderBy(o => o.Distance).FirstOrDefault();

				if (vendorNPC != null && vendorNPC.IsValid)
				{
					await CommonCoroutines.SleepForLagDuration();
					vendorNPC.Interact();
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_SHOW", 3000);
					await CommonCoroutines.SleepForLagDuration();
					await CommonCoroutines.WaitForLuaEvent("FAKE_LUA_EVENT", 8000);
					Lua.DoString("MerchantFrameCloseButton:Click()");
					await CommonCoroutines.WaitForLuaEvent("MERCHANT_CLOSED", 3000);

					_needToVendor = false;
					return true;
				}
			}

			Jobs.NextJob();
			return true;*/