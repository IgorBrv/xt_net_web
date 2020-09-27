using System;
using System.Collections.Generic;
using System.IO;
using Epam.CommonEntities;

namespace Epam.Interfaces.BLL
{
	public interface IUsersBLL
	{
		void SetPath(string path);

		void RemoveEmblem(int id);

		string AddEmblem(int id, string ext, BinaryReader br);
		
		UserData GetById(int id);

		UserData GetByEmail(string email);

		void Update(UserData user);

		IEnumerable<UserData> GetOthers(int curUserId);

		List<UserData> FindByName(int curUserId, string text);

		bool Create(string email, string password, string name, DateTime birth);
	}
}
