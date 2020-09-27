using Epam.CommonEntities;
using System;
using System.Collections.Generic;
using System.IO;

namespace Epam.Interfaces.DAL
{
	public interface IUsersDAL
	{
		string Path { get; set; }

		UserData GetById(int id);

		UserData GetByEmail(string email);

		void RemoveEmblem(int id);

		string AddEmblem(int id, string ext, BinaryReader br);

		void Update(UserData user);

		IEnumerable<UserData> GetOthers(int curUserId);

		List<UserData> FindByName(int curUserId, string text);

		bool Create(string email, string password, UserData user);
	}
}
