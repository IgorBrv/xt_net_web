﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLLInterface;
using DALInterface;
using DALConnector;
using PLInterface;
using Entities;
using System.ComponentModel;

namespace BLLCore
{
	public class BLLMain : IBll
	{
		private IPl pl;
		private readonly IUsersDAL dalUsers;
		private readonly IAwardsDAL dalAwards;
		private readonly IAwardsAssotiatonsDAL dalAwardsAssotiations;

		public BLLMain()
		{
			this.dalUsers = DALGetter.GetUsersDAL();
			this.dalAwards = DALGetter.GetAwardsDAL();
			this.dalAwardsAssotiations = DALGetter.GetAwardsAssotiationsDAL();
		}

		public bool AddUser(string name, int age, DateTime birth)
		{
			User user = new User(Guid.NewGuid(), age, name, birth);

			if (dalUsers.AddUser(user))
			{
				return true;
			}

			return false;
		}

		public bool AddAward(string title)
		{
			Award award = new Award(Guid.NewGuid(), title);

			if (dalAwards.AddAward(award))
			{
				return true;
			}

			return false;
		}



		public bool RemoveUser(User user) => dalUsers.RemoveUser(user);

		public bool RemoveAward(Award award) => dalAwards.RemoveAward(award);

		public bool UpdateUser(Guid id, string name, int age, DateTime birth) => dalUsers.UpdateUser(new User(id, age, name, birth));

		public bool UpdateAward(Guid id, string title) => dalAwards.UpdateAward(new Award(id, title));

		public User GetUserByID(Guid id) => dalUsers.GetUserByID(id);

		public Award GetAwardByID(Guid id) => dalAwards.GetAwardByID(id);

		public List<User> GetAllUsers() => dalUsers.GetAllUsers();

		public List<Award> GetAllAwards() => dalAwards.GetAllAwards();

		public Dictionary<User, List<Award>> GetAllUsersWAwards() => dalAwardsAssotiations.GetAllUsersWAwards();

		public Dictionary<Award, List<User>> GetAllAwardsWUsers() => dalAwardsAssotiations.GetAllAwardsWUsers();

		public bool AddAwardToUser(User user, Award award) => dalAwardsAssotiations.AddAwardToUser(user, award);

		public bool RemoveAwardFromUser(User user, Award award) => dalAwardsAssotiations.RemoveAwardFromUser(user, award);

		public void SetPL(IPl pl)
		{
			this.pl = pl;
		}
	}
}
