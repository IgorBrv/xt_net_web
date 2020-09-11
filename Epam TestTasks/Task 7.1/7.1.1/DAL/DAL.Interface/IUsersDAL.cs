using System;
using System.Collections.Generic;
using Entities;

namespace InterfaceDAL
{
	public interface IUsersDAL
	{

		bool AddUser(User user);
		bool RemoveUser(User user);
		bool RemoveUserById(Guid id);
		bool UpdateUser(User user);
		User GetUserByID(Guid id);
		List<User> GetAllUsers();
	}
}
