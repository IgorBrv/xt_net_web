using System;
using System.Collections.Generic;
using Entities;

namespace DALInterface
{
	public interface IAwardsAssotiatonsDAL
	{
		Dictionary<User, List<Award>> GetAllUsersWAwards();
		Dictionary<Award, List<User>> GetAllAwardsWUsers();
		bool AddAwardToUser(User user, Award award);
		bool RemoveAwardFromUser(User user, Award award);
	}
}
