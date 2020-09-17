using System.Collections.Generic;
using InterfacesBLL;
using InterfaceDAL;
using Entities;

namespace CoreBLL
{   // Ядро BLL решения

	public class BLOAwardsAssotiations : IAwardsAssotiationsBLO
	{   // Объект BL отвечающий за работу с сущностью связей многие ко многим (награды : пользователи)

		private readonly IAwardsAssotiatonsDAO daoAwardsAssotiations;

		public BLOAwardsAssotiations(IAwardsAssotiatonsDAO daoAwardsAssotiations)
		{
			this.daoAwardsAssotiations = daoAwardsAssotiations;
		}

		public bool AddAwardToUser(User user, Award award) => daoAwardsAssotiations.AddAwardToUser(user, award);

		public bool RemoveAwardFromUser(User user, Award award) => daoAwardsAssotiations.RemoveAwardFromUser(user, award);

		public bool RemoveAwardFromAllUsers(Award award) => daoAwardsAssotiations.RemoveUserAwardFromAssotiations(award.id, false);

		public Dictionary<User, List<Award>> GetAllUsersWAwards() => daoAwardsAssotiations.GetAllUsersWAwards();

		public Dictionary<Award, List<User>> GetAllAwardsWUsers() => daoAwardsAssotiations.GetAllAwardsWUsers();

		public List<Award> GetAllAwardsOfUser(User user) => daoAwardsAssotiations.GetAllAwardsOfUser(user);

		public List<User> GetAllUsersWithAward(Award award) => daoAwardsAssotiations.GetAllUsersWithAward(award);
	}
}
