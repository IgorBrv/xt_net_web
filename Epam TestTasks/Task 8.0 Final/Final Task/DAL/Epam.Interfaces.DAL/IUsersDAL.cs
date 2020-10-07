using Epam.CommonEntities;
using System.Collections.Generic;
using System.IO;

namespace Epam.Interfaces.DAL
{
	public interface IUsersDAL
	{   // DAL Users, отвечает за работу с сущностью пользователей, позволяет создавать профили пользователей, назначать им пароли, редактировать профиль пользователя, присваивать им эмблемы, а так же
		// получать профиль пользователя, получать список пользоввателей, искать пользователей

		string Path { get; set; }

		int? GetId(string email);

		UserData GetById(int id);

		void RemoveEmblem(int id);

		void Update(UserData user);

		UserData GetByEmail(string email);

		void SetPassword(int id, string password);

		string AddEmblem(int id, string ext, BinaryReader br);

		IEnumerable<UserData> GetOthers(int curUserId);

		List<UserData> FindByName(int curUserId, string text);

		int Create(string email, UserData user);

		bool CheckUser(string email, string password);

		bool ChangePassword(int id, string oldPassword, string password);
	}
}
