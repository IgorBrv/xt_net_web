using System;
using System.Collections.Generic;
using Entities;

namespace InterfaceDAL
{	// Интерфейсы DAL решения

	public interface IAwardsAssotiatonsDAO
	{   // Интерфейс взаимодействия с сущностями связей в базе

		List<Award> GetAllAwardsOfUser(User user);
		List<User> GetAllUsersWithAward(Award award);
		Dictionary<User, List<Award>> GetAllUsersWAwards();
		Dictionary<Award, List<User>> GetAllAwardsWUsers();
		bool AddAwardToUser(User user, Award award);
		bool RemoveAwardFromUser(User user, Award award);
	}
}
