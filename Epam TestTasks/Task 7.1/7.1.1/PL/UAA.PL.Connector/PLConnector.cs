using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLConsoleView;
using PLInterface;

namespace PLConnector
{
	public static class PLConnector
	{
		private static IPl pl;

		public static IPl GetPl() => pl ?? (pl = new PLConsole());

	}
}
