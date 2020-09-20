using System;
using System.Collections.Generic;
using System.IO;
using Entities;

namespace InterfaceDAL
{   // Интерфейсы DAL решения

	public interface IUsersDAO
	{   // Объект взаимодействия с сущностями пользователей в базе

		string Path { get; set; }
		bool AddUser(User user);
		bool RemoveUser(User user);
		bool RemoveUserById(Guid id);
		bool UpdateUser(User user);
		bool IsUserInStorage(Guid id);
		string AddEmblemToUser(Guid id, string ext, BinaryReader br);
		bool RemoveEmblemFromUser(Guid id);
		User GetUserByID(Guid id);
		List<User> GetAllUsers();
	}
}
