using System;
using System.Collections.Generic;
using System.Linq;
using InterfaceDAL;
using Entities;

namespace JsonDAL
{	// Объекты DAL слоя

	public class DALUsersJSON : IUsersDAO
	{	// Объект DAO отвечающий за работу с сущностью пользователей

		private readonly DALJson dalJson;
		private readonly IAwardsAssotiatonsDAO awardsAssotiatons;

		public DALUsersJSON(IAwardsAssotiatonsDAO awardsAssotiatons)
		{
			dalJson = JsonDAL.Get();
			this.awardsAssotiatons = awardsAssotiatons;
		}

		public List<User> GetAllUsers() => dalJson.userList.Select(KeyValuePair => KeyValuePair.Value).ToList();

		public bool IsUserInStorage(Guid id) => dalJson.userList.ContainsKey(id);

		public bool AddUser(User user)
		{
			Data data = dalJson.LoadAll();

			if (data != null && dalJson.userList.Where(item => item.Value.name == user.name && item.Value.age == user.age && item.Value.birth == user.birth).Count() == 0)
			{

				data.userList.Add(user);

				if (dalJson.SaveAll(data))
				{
					dalJson.userList.Add(user.id, user);
					return true;
				}
			}

			return false;
		}


		public bool RemoveUser(User user)
		{
			Data data = dalJson.LoadAll();

			if (data != null && dalJson.userList.ContainsKey(user.id))
			{
				data.userList = data.userList.Where(item => item.id != user.id).ToList();

				if (dalJson.SaveAll(data))
				{
					if (awardsAssotiatons.RemoveUserAwardFromAssotiations(user.id, true))
					{
						dalJson.userList.Remove(user.id);
						return true;
					}
				}
			}

			return false;
		}


		public bool RemoveUserById(Guid id)
		{
			Data data = dalJson.LoadAll();

			if (data != null && dalJson.userList.ContainsKey(id))
			{
				data.userList = data.userList.Where(item => item.id != id).ToList();

				if (dalJson.SaveAll(data))
				{
					dalJson.userList.Remove(id);
					return true;
				}
			}

			return false;
		}


		public bool UpdateUser(User user)
		{
			Data data = dalJson.LoadAll();
			User temp = dalJson.userList[user.id];

			if (data != null && dalJson.userList.ContainsKey(user.id))
			{
				int index = data.userList.FindIndex(item => item.id == user.id);

				data.userList[index].age = user.age;
				data.userList[index].name = user.name;
				data.userList[index].birth = user.birth;
				dalJson.userList[user.id].age = user.age;
				dalJson.userList[user.id].name = user.name;
				dalJson.userList[user.id].birth = user.birth;


				if (dalJson.SaveAll(data))
				{
					return true;
				}

				dalJson.userList[user.id] = temp;
			}

			return false;
		}


		public User GetUserByID(Guid id)
		{
			if (dalJson.userList.ContainsKey(id))
			{
				return dalJson.userList[id];
			}
			return null;
		}
	}
}
