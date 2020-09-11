
namespace JsonDAL
{
	public static class JsonDAL
	{
		private static DALJson dalJson;

		public static DALJson Get() => dalJson ?? (dalJson = new DALJson());
	}
}