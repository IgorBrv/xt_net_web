using Epam.CommonEntities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Epam.Interfaces.DAL
{
	public interface IUsersDAL
	{
		string Path { get; set; }

		UserData Get(int id);

		UserData Get(string email);

		void RemoveEmblem(int id);

		string AddEmblem(int id, string ext, BinaryReader br);

		void Update(UserData user);

		IEnumerable<UserData> GetOthers(int curUserId);

		List<UserData> FindByName(int curUserId, string text);

		void Create(string email, string password, UserData user);
	}
}
