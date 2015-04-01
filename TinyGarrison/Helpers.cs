﻿using System.Threading.Tasks;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot.Coroutines;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;

namespace TinyGarrison
{
	class Helpers
	{
		private static string _lastMsg;
		public static readonly LocalPlayer Me = StyxWoW.Me;

		public static void Log(string msg)
		{
			if (msg == _lastMsg) return;
			Logging.Write(Colors.DarkOrange, "[TG] " + msg);
			_lastMsg = msg;
		}

		public static async Task<bool> MoveTo(WoWPoint destination)
		{
			MoveResult r = await CommonCoroutines.MoveTo(destination);

			if (r == MoveResult.ReachedDestination)
			{
				Jobs.CurrentJob().AlreadyMoved = true;
				return true;
			}
			return false;
		}
	}
}