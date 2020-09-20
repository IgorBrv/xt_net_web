using System;
using System.Collections.Generic;

namespace Entities
{	// Общие классы
	public class Data
	{	// Класс объекта данных (включает список пользователей, список наград, список завсимостей

		public List<User> userList;
		public List<Award> awardList;
		public Dictionary<Guid, Emblem> emblemsList;
		public readonly Dictionary<string, string> authList;
		public readonly Dictionary<string, string> rolesList;
		public List<Guid[]> awardedUsers;

		public Data(List<User> userList, List<Award> awardList, Dictionary<Guid, Emblem> emblemsList, List<Guid[]> awardedUsers, Dictionary<string, string> authList, Dictionary<string, string> rolesList)
		{
			this.userList = userList;
			this.authList = authList;
			this.rolesList = rolesList;
			this.awardList = awardList;
			this.emblemsList = emblemsList;
			this.awardedUsers = awardedUsers;
		}
	}
}
