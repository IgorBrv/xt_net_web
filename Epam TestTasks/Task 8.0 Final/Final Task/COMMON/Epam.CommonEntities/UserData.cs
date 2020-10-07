using System;

namespace Epam.CommonEntities
{	// Объект пользователя, включает в себя id пользователя, имя, дату рождения, высказывание, ссылку на аватар, и флажок блокировки

	public class UserData
	{
		public int? id;
		public string name;
		public string emblem;
		public DateTime birth;
		public string statement;
		public int? blockedBy;

		public UserData(string name, DateTime birth)
		{
			this.name = name;
			this.birth = birth;

			this.id = null;
			this.emblem = null;
			this.statement = null;
		}

		public UserData(int id, string name, DateTime birth, string statement = null, string emblem = null, int? blockedBy = null)
		{
			this.id = id;
			this.name = name;
			this.birth = birth;
			this.emblem = emblem;
			this.statement = statement;
			this.blockedBy = blockedBy;
		}
	}
}
