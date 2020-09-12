using System;
using System.Collections.Generic;
using Entities;

namespace InterfacesBLL
{	// Интерфейс BLL слоя (в данном случае я постарался реализовать все возможные метооды, т.к. неизвестно, какие могут потребоваться при переезде на другой UI или DAL)

	public interface IUsersBLO
	{
		User AddUser(string name, int age, DateTime birth);
		bool RemoveUser(User user);
		bool UpdateUser(Guid id, string name, int age, DateTime birth);
		bool IsUserInStorage(Guid id);
		User GetUserByID(Guid id);
		List<User> GetAllUsers();
	}
}
