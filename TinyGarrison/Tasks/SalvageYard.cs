using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyGarrison.Tasks
{
	class SalvageYard
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

			Jobs.NextJob();
			return true;
		}
	}
}
