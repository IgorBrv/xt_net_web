using System;
using System.Collections.Generic;
using Entities;

namespace InterfaceDAL
{	// Интерфейсы DAL решения

	public interface IAwardsDAO
	{	// Интерфейс взаимодействия с сущностями наград в базе

		bool AddAward(Award award);
		bool RemoveAward(Award award);
		bool RemoveAwardByID(Guid id);
		bool UpdateAward(Award award);
		bool IsAwardInStorage(Guid id);
		Award GetAwardByID(Guid id);
		List<Award> GetAllAwards();
	}
}
