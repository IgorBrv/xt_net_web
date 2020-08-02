
namespace FileManagementSystem
{
	public class FileObject
	{	// Класс-контейнер для файла. Содержит в себе тело и адрес файла

		public byte[] body;
		public string path;

		public FileObject (string path, byte[] body)
		{
			this.body = body;
			this.path = path;
		}
	}
}
