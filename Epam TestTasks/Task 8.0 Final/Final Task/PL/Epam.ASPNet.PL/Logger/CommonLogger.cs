using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epam.CommonEntities;

namespace Epam.ASPNet.PL
{
	public class CommonLogger : ILogger
	{
		public CommonLogger()
		{
			Logger.InitLogger();
		}

		public void Error(string message) => Logger.Log.Error(message);

		public void Info(string message) => Logger.Log.Info(message);

		public void Warn(string message) => Logger.Log.Error(message);
	}
}