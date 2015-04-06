using System.Threading.Tasks;
using Styx.WoWInternals;

namespace TinyGarrison.Tasks
{
	class PrimalTrader
	{
		private static bool _alreadyMoved;

		public static async Task<bool> Handler()
		{
			if (Lua.GetReturnVal<int>("return GetItemCount('Primal Spirit')", 0) >= 50)
			{
				// Move to Job
				if (!_alreadyMoved)
				{
					_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
					return true;
				}
			}

			Jobs.NextJob();
			return true;
		}
	}
}
