using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagementSystem
{
	public class CMapObject
	{
		public string path;
		public string action;
		public string rawAdress;
		public int[] changesAdress;

		public CMapObject(string path, string action, int[] changesAdress)
		{
			this.path = path;
			this.action = action;
			this.rawAdress = null;
			this.changesAdress = changesAdress;
		}

		[Newtonsoft.Json.JsonConstructor]
		public CMapObject(string path, string action, int[] changesAdress, string rawAdress)
		{
			this.path = path;
			this.action = action;
			this.rawAdress = rawAdress;
			this.changesAdress = changesAdress;
		}

	}
}
