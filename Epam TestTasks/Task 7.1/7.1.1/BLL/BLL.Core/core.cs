using System;
using System.Collections.Generic;
using InterfacesBLL;
using InterfaceDAL;
using Entities;

namespace CoreBLL
{	// Ядро BLL решения

	public class BLLMain : IBll
	{
		private readonly IUsersDAO daoUsers;
		private readonly IAwardsDAO daoAwards;
		private readonly IAwardsAssotiatonsDAO daoAwardsAssotiations;

		public BLLMain(IUsersDAO daoUsers, IAwardsDAO daoAwards, IAwardsAssotiatonsDAO daoAwardsAssotiations)
		{
			this.daoUsers = daoUsers;
			this.daoAwards = daoAwards;
			this.daoAwardsAssotiations = daoAwardsAssotiations;
		}

		public bool AddUser(string name, int age, DateTime birth)
		{
			User user = new User(Guid.NewGuid(), age, name, birth);

			if (daoUsers.AddUser(user))
			{
				return true;
			}

			return false;
		}

		public bool RemoveUser(User user) => daoUsers.RemoveUser(user);

		public bool UpdateUser(Guid id, string name, int age, DateTime birth) => daoUsers.UpdateUser(new User(id, age, name, birth));

		public User GetUserByID(Guid id) => daoUsers.GetUserByID(id);

		public List<User> GetAllUsers() => daoUsers.GetAllUsers();

		public bool IsUserInStorage(Guid id) => daoUsers.IsUserInStorage(id);



		public bool AddAward(string title)
		{
			Award award = new Award(Guid.NewGuid(), title);

			if (daoAwards.AddAward(award))
			{
				return true;
			}

			return false;
		}

		public bool RemoveAward(Award award) => daoAwards.RemoveAward(award);

		public bool UpdateAward(Guid id, string title) => daoAwards.UpdateAward(new Award(id, title));

		public Award GetAwardByID(Guid id) => daoAwards.GetAwardByID(id);

		public List<Award> GetAllAwards() => daoAwards.GetAllAwards();

		public bool IsAwardInStorage(Guid id) => daoAwards.IsAwardInStorage(id);



		public bool AddAwardToUser(User user, Award award) => daoAwardsAssotiations.AddAwardToUser(user, award);

		public bool RemoveAwardFromUser(User user, Award award) => daoAwardsAssotiations.RemoveAwardFromUser(user, award);

		public Dictionary<User, List<Award>> GetAllUsersWAwards() => daoAwardsAssotiations.GetAllUsersWAwards();

		public Dictionary<Award, List<User>> GetAllAwardsWUsers() => daoAwardsAssotiations.GetAllAwardsWUsers();

		public List<Award> GetAllAwardsOfUser(User user) => daoAwardsAssotiations.GetAllAwardsOfUser(user);

		public List<User> GetAllUsersWithAward(Award award) => daoAwardsAssotiations.GetAllUsersWithAward(award);


	}
}
