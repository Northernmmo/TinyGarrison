using System.Threading.Tasks;

namespace TinyGarrison.Tasks
{
	class CommandTable
	{
		private static bool _alreadyMoved;

		public static async Task<bool> Handler()
		{
			// Move to Job
			if (!_alreadyMoved)
			{
				_alreadyMoved = await Helpers.MoveToJob(Jobs.CurrentJob().Location);
				return true;
			}

			// Done
			Jobs.NextJob();
			return true;
		}
	}
}
