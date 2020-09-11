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
	public class DALJson
	{
		public readonly string path = "data.sav";
		public readonly Dictionary<Guid, User> userList;
		public readonly Dictionary<Guid, Award> awardList;
		public readonly List<Guid[]> awardedList;


		public DALJson()
		{
			if (File.Exists(path))
			{
				Data data = LoadAll();

				userList = new Dictionary<Guid, User>();
				awardList = new Dictionary<Guid, Award>();

				foreach (User user in data.userList)
				{
					userList.Add(user.id, user);
				}
				foreach (Award award in data.awardList)
				{
					awardList.Add(award.id, award);
				}

				awardedList = data.awardedUsers;
			}
			else
			{
				userList = new Dictionary<Guid, User>();
				awardList = new Dictionary<Guid, Award>();
				awardedList = new List<Guid[]>();

				Data savedata = new Data(userList.Select(KeyValuePair => KeyValuePair.Value).ToList(), awardList.Select(KeyValuePair => KeyValuePair.Value).ToList(), awardedList);

				SaveAll(savedata);
			}
		}


		public Data LoadAll()
		{
			if (File.Exists(path))
			{
				try
				{
					Data data = JsonConvert.DeserializeObject<Data>(File.ReadAllText(path));
					return data;
				}
				catch (IOException)
				{
					return null;
				}
				catch (SystemException)
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}


		public bool SaveAll(Data savedata)
		{
			string json = JsonConvert.SerializeObject(savedata);

			if (json != string.Empty)
			{
				try
				{
					File.WriteAllText(path, json);
					return true;
				}
				catch (IOException)
				{
					return false;
				}
				catch (SystemException)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}