using System;
using System.Collections.Generic;
using System.Linq;
using InterfaceDAL;
using Entities;

namespace JsonDAL
{   // Объекты DAL слоя

	public class DALAwardsJSON : IAwardsDAO
	{	// Объект DAO отвечающий за работу с сущностью наград

		private readonly DALJson dalJson;
		private readonly IAwardsAssotiatonsDAO awardsAssotiatons;

		public DALAwardsJSON(IAwardsAssotiatonsDAO awardsAssotiatons)
		{
			dalJson = JsonDAL.Get();
			this.awardsAssotiatons = awardsAssotiatons;
		}

		public List<Award> GetAllAwards() => dalJson.awardList.Select(KeyValuePair => KeyValuePair.Value).ToList();

		public bool IsAwardInStorage(Guid id) => dalJson.awardList.ContainsKey(id);

		public bool AddAward(Award award)
		{
			Data data = dalJson.LoadAll();

			if (data != null && dalJson.awardList.Where(item => item.Value.title == award.title).Count() == 0)
			{
				data.awardList.Add(award);

				if (dalJson.SaveAll(data))
				{
					dalJson.awardList.Add(award.id, award);
					return true;
				}
			}

			return false;
		}


		public bool RemoveAward(Award award)
		{
			Data data = dalJson.LoadAll();

			if (data != null && dalJson.awardList.ContainsKey(award.id))
			{
				data.awardList = data.awardList.Where(item => item.id != award.id).ToList();

				if (dalJson.SaveAll(data))
				{
					if (awardsAssotiatons.RemoveUserAwardFromAssotiations(award.id, false))
					{
						dalJson.awardList.Remove(award.id);
						return true;
					}
				}
			}

			return false;
		}


		public bool RemoveAwardByID(Guid id)
		{
			Data data = dalJson.LoadAll();

			if (data != null && dalJson.awardList.ContainsKey(id))
			{
				data.awardList = data.awardList.Where(item => item.id != id).ToList();

				if (dalJson.SaveAll(data))
				{
					dalJson.awardList.Remove(id);
					return true;
				}
			}

			return false;
		}


		public bool UpdateAward(Award award)
		{
			Data data = dalJson.LoadAll();
			Award temp = dalJson.awardList[award.id];

			if (data != null && dalJson.awardList.ContainsKey(award.id))
			{
				int index = data.awardList.FindIndex(item => item.id == award.id);

				data.awardList[index].title = award.title;
				dalJson.awardList[award.id].title = award.title;

				if (dalJson.SaveAll(data))
				{
					return true;
				}

				dalJson.awardList[award.id] = temp;
			}

			return false;
		}


		public Award GetAwardByID(Guid id)
		{
			if (dalJson.awardList.ContainsKey(id))
			{
				return dalJson.awardList[id];
			}
			return null;
		}
	}
}