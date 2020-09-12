
namespace JsonDAL
{
	public static class JsonDAL
	{   // Геттер с синглтоном класса работающего с базой json

		private static DALJson dalJson;

		public static DALJson Get() => dalJson ?? (dalJson = new DALJson());
	}
}