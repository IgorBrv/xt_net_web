using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLInterface;
using Entities;

namespace BLLInterface
{
	public interface IBll
	{
		bool AddUser(string name, int age, DateTime birth);
		bool AddAward(string name);
		bool RemoveUser(User user);
		bool RemoveAward(Award award);

		bool UpdateUser(Guid id, string name, int age, DateTime birth);
		bool UpdateAward(Guid id, string title);
		

		bool AddAwardToUser(User user, Award award);
		bool RemoveAwardFromUser(User user, Award award);

		User GetUserByID(Guid id);
		Award GetAwardByID(Guid id);

		List<User> GetAllUsers();
		List<Award> GetAllAwards();
		Dictionary<User, List<Award>> GetAllUsersWAwards();
		Dictionary<Award, List<User>> GetAllAwardsWUsers();

		void SetPL(IPl pl);
	}
}
