using System;
using System.Collections.Generic;
using Entities;

namespace InterfacesBLL
{	// Интерфейс BLL слоя (в данном случае я постарался реализовать все возможные метооды, т.к. неизвестно, какие могут потребоваться при переезде на другой UI или DAL)
	public interface IBll
	{
		bool AddUser(string name, int age, DateTime birth);
		bool AddAward(string name);
		bool RemoveUser(User user);
		bool RemoveAward(Award award);

		bool UpdateUser(Guid id, string name, int age, DateTime birth);
		bool UpdateAward(Guid id, string title);

		bool IsUserInStorage(Guid id);
		bool IsAwardInStorage(Guid id);

		bool AddAwardToUser(User user, Award award);
		bool RemoveAwardFromUser(User user, Award award);

		User GetUserByID(Guid id);
		Award GetAwardByID(Guid id);

		List<User> GetAllUsers();
		List<Award> GetAllAwards();

		List<Award> GetAllAwardsOfUser(User user);
		List<User> GetAllUsersWithAward(Award award);
		Dictionary<User, List<Award>> GetAllUsersWAwards();
		Dictionary<Award, List<User>> GetAllAwardsWUsers();
	}
}
