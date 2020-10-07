using System;
using System.Collections.Generic;
using System.IO;
using Epam.CommonEntities;

namespace Epam.Interfaces.BLL
{
	public interface IUsersBLL
	{   // DAL Users, отвечает за работу с сущностью пользователей, позволяет создавать профили пользователей, редактировать профиль пользователя, присваивать им эмблемы, а так же
		// получать профиль пользователя, получать список пользоввателей, искать пользователей

		void SetPath(string path);

		void RemoveEmblem(int id);

		UserData GetById(int id);

		void Update(UserData user);

		UserData GetByEmail(string email);

		bool CheckUser(string email, string password);

		IEnumerable<UserData> GetOthers(int curUserId);

		string AddEmblem(int id, string ext, BinaryReader br);

		List<UserData> FindByName(int curUserId, string text);

		bool ChangePassword(int id, string oldPassword, string password);

		bool Create(string email, string password, string name, DateTime birth);
	}
}
