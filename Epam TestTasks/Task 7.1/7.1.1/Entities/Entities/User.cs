using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class User
	{
		public Guid id;
		public int age;
		public string name;
		public DateTime birth;

		public User(Guid id, int age, string name, DateTime birth)
		{
			this.id = id;
			this.age = age;
			this.name = name;
			this.birth = birth;
		}
	}
}
