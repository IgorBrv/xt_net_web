using System;
using System.Collections.Generic;
using InterfacesBLL;
using InterfaceDAL;
using Entities;
using System.IO;

namespace CoreBLL
{   // Ядро BLL решения

	public class BLOAwards : IAwardsBLO
	{   // Объект BL отвечающий за работу с сущностью наград

		private readonly IAwardsDAO daoAwards;

		public BLOAwards(IAwardsDAO daoAwards)
		{
			this.daoAwards = daoAwards;
		}

		public Award AddAward(string title, string emblempath = null)
		{
			Award award = new Award(Guid.NewGuid(), title, emblempath);

			if (daoAwards.AddAward(award))
			{
				return award;
			}

			return null;
		}

		public bool RemoveAward(Award award) => daoAwards.RemoveAward(award);

		public bool UpdateAward(Guid id, string title, string emblempath = null) => daoAwards.UpdateAward(new Award(id, title, emblempath));

		public string AddEmblemToAward(Guid id, string ext, BinaryReader br) => daoAwards.AddEmblemToAward(id, ext, br);

		public bool RemoveEmblemFromAward(Guid id) => daoAwards.RemoveEmblemFromAward(id);

		public Award GetAwardByID(Guid id) => daoAwards.GetAwardByID(id);

		public List<Award> GetAllAwards() => daoAwards.GetAllAwards();

		public bool IsAwardInStorage(Guid id) => daoAwards.IsAwardInStorage(id);

		public void SetPath(string path)
		{
			daoAwards.Path = path;
		}
	}
}