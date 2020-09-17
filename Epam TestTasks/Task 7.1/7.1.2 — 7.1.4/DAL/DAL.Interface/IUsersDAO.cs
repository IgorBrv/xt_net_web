using System;
using System.Collections.Generic;
using Entities;

namespace InterfaceDAL
{   // Интерфейсы DAL решения

	public interface IUsersDAO
	{   // Объект взаимодействия с сущностями пользователей в базе

		bool AddUser(User user);
		bool RemoveUser(User user);
		bool RemoveUserById(Guid id);
		bool UpdateUser(User user);
		bool IsUserInStorage(Guid id);
		User GetUserByID(Guid id);
		List<User> GetAllUsers();
	}
}
