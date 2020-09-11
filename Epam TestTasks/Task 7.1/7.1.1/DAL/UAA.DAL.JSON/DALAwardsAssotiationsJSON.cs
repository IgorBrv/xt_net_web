using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALInterface;
using Entities;
using Newtonsoft.Json;

namespace DALJSON
{
	public class DALAwardsAssotiationsJSON : IAwardsAssotiatonsDAL
	{
		private readonly DALJson dalJson;


		public DALAwardsAssotiationsJSON()
		{
			dalJson = JsonDAL.Get();
		}


		public List<Guid[]> GetListOfAwarded() => dalJson.awardedList;


		public Dictionary<User, List<Award>> GetAllUsersWAwards()
		{
			Dictionary<User, List<Award>> temp = new Dictionary<User, List<Award>>();

			foreach (User user in dalJson.userList.Values)
			{
				List<Award> awards = dalJson.awardedList.Where(value => value[0] == user.id).Select(value => dalJson.awardList[value[1]]).ToList();

				temp.Add(user, awards);
			}

			return temp;
		}


		public Dictionary<Award, List<User>> GetAllAwardsWUsers()
		{
			Dictionary<Award, List<User>> temp = new Dictionary<Award, List<User>>();

			foreach (Award award in dalJson.awardList.Values)
			{
				List<User> users = dalJson.awardedList.Where(value => value[1] == award.id).Select(value => dalJson.userList[value[0]]).ToList();

				temp.Add(award, users);
			}

			return temp;
		}


		public bool AddAwardToUser(User user, Award award)
		{
			if (dalJson.awardedList.Where(item => item[0] == user.id && item[1] == award.id).Count() > 0)
			{
				return false;
			}

			Data data = dalJson.LoadAll();

			Guid[] pair = new Guid[] { user.id, award.id };

			data.awardedUsers.Add(pair);
			dalJson.awardedList.Add(pair);

			if (dalJson.SaveAll(data))
			{
				return true;
			}
			else
			{
				dalJson.awardedList.Remove(pair);
				return false;
			}

		}


		public bool RemoveAwardFromUser(User user, Award award)
		{
			int pos = dalJson.awardedList.FindIndex(item => item[0] == user.id && item[1] == award.id);

			if (pos < 0)
			{
				return false;
			}
			else
			{
				Data data = dalJson.LoadAll();

				Guid[] temp = dalJson.awardedList[pos];

				dalJson.awardedList.RemoveAt(pos);
				data.awardedUsers = dalJson.awardedList;

				if (dalJson.SaveAll(data))
				{
					return true;
				}

				dalJson.awardedList.Add(temp);
			}

			return false;
		}
	}
}