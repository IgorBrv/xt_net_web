using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Data
	{
		public List<User> userList;
		public List<Award> awardList;
		public List<Guid[]> awardedUsers;

		public Data(List<User> userList, List<Award> awardList, List<Guid[]> awardedUsers)
		{
			this.userList = userList;
			this.awardList = awardList;
			this.awardedUsers = awardedUsers;
		}
	}
}
