using System.Windows.Media;
using Styx.Common;

namespace TinyGarrison
{
	class Helpers
	{
		private static string _lastMsg;

		public static void Log(string msg)
		{
			if (msg == _lastMsg) return;
			Logging.Write(Colors.DarkOrange, "[TG] " + msg);
			_lastMsg = msg;
		}
	}
}
