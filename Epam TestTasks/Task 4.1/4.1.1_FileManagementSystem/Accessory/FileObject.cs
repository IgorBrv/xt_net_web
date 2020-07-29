using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
