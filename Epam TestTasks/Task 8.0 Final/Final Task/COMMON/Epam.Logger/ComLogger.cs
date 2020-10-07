using Epam.CommonLoggerInterface;

namespace Epam.CommonLogger
{
	public class ComLogger : ILogger
	{	// Общий фасад к классу логгера

		public ComLogger()
		{
			Logger.InitLogger();
		}

		public void Error(string message) => Logger.Log.Error(message);

		public void Info(string message) => Logger.Log.Info(message);

		public void Warn(string message) => Logger.Log.Warn(message);
	}
}