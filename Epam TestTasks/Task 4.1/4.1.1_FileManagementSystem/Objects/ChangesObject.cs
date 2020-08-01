
namespace FileManagementSystem
{
	public class ChangesObject
	{
		public string action;
		public string path;
		public string newPath;

		public ChangesObject(string action, string path, string newPath)
		{
			this.action = action;
			this.path = path;
			this.newPath = newPath;
		}

		[Newtonsoft.Json.JsonConstructor]
		public ChangesObject(string action, string path)
		{
			this.action = action;
			this.path = path;
			this.newPath = null;
		}
	}
}
