using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Coroutines;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison.Tasks
{
	class Profession
	{
		public static void Check()
		{
			if (!SpellManager.HasSpell("Leatherworking") && !SpellManager.HasSpell("Alchemy") &&
			    !SpellManager.HasSpell("Jewelcrafting") && !SpellManager.HasSpell("Jewelcrafting") &&
			    !SpellManager.HasSpell("Enchanting") && !SpellManager.HasSpell("Blacksmithing") &&
			    !SpellManager.HasSpell("Tailoring") && !SpellManager.HasSpell("Engineering") &&
			    !SpellManager.HasSpell("Inscription")) return;
			Jobs.Add(JobType.Move, new WoWPoint(5468.646, 4447.296, 144.7437));
			Jobs.Add(JobType.Profession);
		}

		public static async Task<bool> Execute()
		{
			await CommonCoroutines.SleepForLagDuration();

			Jobs.NextJob();
			return true;
		}
	}
}
