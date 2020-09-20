using System;
using System.Collections.Generic;
using InterfacesBLL;
using InterfaceDAL;
using Entities;
using System.IO;

namespace CoreBLL
{	// Ядро BLL решения

	public class BLOUsers : IUsersBLO
	{	// Объект BL отвечающий за работу с сущностью пользователей

		private readonly IUsersDAO daoUsers;

		public BLOUsers(IUsersDAO daoUsers)
		{
			this.daoUsers = daoUsers;
		}

		public User AddUser(string name, int age, DateTime birth, string emblempath = null)
		{
			User user = new User(Guid.NewGuid(), age, name, birth, emblempath);

			if (daoUsers.AddUser(user))
			{
				return user;
			}

			return null;
		}

		public bool RemoveUser(User user) => daoUsers.RemoveUser(user);

		public bool UpdateUser(Guid id, string name, int age, DateTime birth, string emblempath = null) => daoUsers.UpdateUser(new User(id, age, name, birth, emblempath));

		public string AddEmblemToUser(Guid id, string ext, BinaryReader br) => daoUsers.AddEmblemToUser(id, ext, br);

		public bool RemoveEmblemFromUser(Guid id) => daoUsers.RemoveEmblemFromUser(id);

		public User GetUserByID(Guid id) => daoUsers.GetUserByID(id);

		public List<User> GetAllUsers() => daoUsers.GetAllUsers();

		public bool IsUserInStorage(Guid id) => daoUsers.IsUserInStorage(id);

		public void SetPath(string path)
		{
			daoUsers.Path = path;
		}
	}
}
