using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.CommonEntities
{
	public class UserData
	{
		public int? id;
		public string name;
		public string emblem;
		public DateTime birth;
		public string statement;

		public UserData(string name, DateTime birth)
		{
			this.name = name;
			this.birth = birth;

			this.id = null;
			this.emblem = null;
			this.statement = null;
		}

		public UserData(int id, string name, DateTime birth, string statement = null, string emblem = null)
		{
			this.id = id;
			this.name = name;
			this.birth = birth;
			this.emblem = emblem;
			this.statement = statement;
		}
	}
}
