using System.Collections.Generic;
using Entities;

namespace InterfacesBLL
{   // Интерфейс BLL слоя (в данном случае я постарался реализовать все возможные метооды, т.к. неизвестно, какие могут потребоваться при переезде на другой UI или DAL)

	public interface IAwardsAssotiationsBLO
	{
		bool AddAwardToUser(User user, Award award);
		bool RemoveAwardFromUser(User user, Award award);
		List<Award> GetAllAwardsOfUser(User user);
		List<User> GetAllUsersWithAward(Award award);
		Dictionary<User, List<Award>> GetAllUsersWAwards();
		Dictionary<Award, List<User>> GetAllAwardsWUsers();
	}
}
