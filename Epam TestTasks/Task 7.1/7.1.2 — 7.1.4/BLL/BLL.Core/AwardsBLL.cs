using System;
using System.Collections.Generic;
using InterfacesBLL;
using InterfaceDAL;
using Entities;

namespace CoreBLL
{   // Ядро BLL решения

	public class BLOAwards : IAwardsBLO
	{   // Объект BL отвечающий за работу с сущностью наград

		private readonly IAwardsDAO daoAwards;

		public BLOAwards(IAwardsDAO daoAwards)
		{
			this.daoAwards = daoAwards;
		}

		public Award AddAward(string title)
		{
			Award award = new Award(Guid.NewGuid(), title);

			if (daoAwards.AddAward(award))
			{
				return award;
			}

			return null;
		}

		public bool RemoveAward(Award award) => daoAwards.RemoveAward(award);

		public bool UpdateAward(Guid id, string title) => daoAwards.UpdateAward(new Award(id, title));

		public Award GetAwardByID(Guid id) => daoAwards.GetAwardByID(id);

		public List<Award> GetAllAwards() => daoAwards.GetAllAwards();

		public bool IsAwardInStorage(Guid id) => daoAwards.IsAwardInStorage(id);
	}
}