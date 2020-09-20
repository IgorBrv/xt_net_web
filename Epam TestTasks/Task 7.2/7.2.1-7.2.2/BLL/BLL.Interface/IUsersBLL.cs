using System;
using System.Collections.Generic;
using System.IO;
using Entities;

namespace InterfacesBLL
{	// Интерфейс BLL слоя (в данном случае я постарался реализовать все возможные метооды, т.к. неизвестно, какие могут потребоваться при переезде на другой UI или DAL)

	public interface IUsersBLO
	{
		void SetPath(string path);
		User AddUser(string name, int age, DateTime birth, string emblempath = null);
		bool RemoveUser(User user);
		bool UpdateUser(Guid id, string name, int age, DateTime birth, string emblempath = null);
		string AddEmblemToUser(Guid id, string ext, BinaryReader br);
		bool RemoveEmblemFromUser(Guid id);
		bool IsUserInStorage(Guid id);
		User GetUserByID(Guid id);
		List<User> GetAllUsers();
	}
}
