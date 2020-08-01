
namespace FileManagementSystem
{
	public class FileObject
	{
		public byte[] body;
		public string path;

		public FileObject (string path, byte[] body)
		{
			this.body = body;
			this.path = path;
		}
	}
}
