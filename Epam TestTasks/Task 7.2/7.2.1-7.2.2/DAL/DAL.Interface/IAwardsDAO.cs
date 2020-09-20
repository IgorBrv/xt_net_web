using System;
using System.Collections.Generic;
using System.IO;
using Entities;

namespace InterfaceDAL
{	// Интерфейсы DAL решения

	public interface IAwardsDAO
	{   // Интерфейс взаимодействия с сущностями наград в базе
		string Path { get; set; }
		bool AddAward(Award award);
		bool RemoveAward(Award award);
		bool RemoveAwardByID(Guid id);
		bool UpdateAward(Award award);
		bool IsAwardInStorage(Guid id);
		string AddEmblemToAward(Guid id, string ext, BinaryReader br);
		bool RemoveEmblemFromAward(Guid id);
		Award GetAwardByID(Guid id);
		List<Award> GetAllAwards();
	}
}
