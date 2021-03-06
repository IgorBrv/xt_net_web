﻿using System;
using System.Collections.Generic;

namespace Entities
{	// Общие классы
	public class Data
	{	// Класс объекта данных (включает список пользователей, список наград, список завсимостей

		public List<User> userList;
		public List<Award> awardList;
		public Dictionary<Guid, Emblem> emblemsList;
		public List<Guid[]> awardedUsers;

		public Data(List<User> userList, List<Award> awardList, Dictionary<Guid, Emblem> emblemsList, List<Guid[]> awardedUsers)
		{
			this.userList = userList;
			this.awardList = awardList;
			this.emblemsList = emblemsList;
			this.awardedUsers = awardedUsers;
		}
	}
}
