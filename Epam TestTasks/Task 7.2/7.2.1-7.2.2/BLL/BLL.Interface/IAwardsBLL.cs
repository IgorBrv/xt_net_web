using System;
using System.Collections.Generic;
using System.IO;
using Entities;

namespace InterfacesBLL
{   // Интерфейс BLL слоя (в данном случае я постарался реализовать все возможные метооды, т.к. неизвестно, какие могут потребоваться при переезде на другой UI или DAL)

	public interface IAwardsBLO
	{
		void SetPath(string path);
		Award AddAward(string name, string emblempath = null);
		bool RemoveAward(Award award);
		bool UpdateAward(Guid id, string title, string emblempath = null);
		string AddEmblemToAward(Guid id, string ext, BinaryReader br);
		bool RemoveEmblemFromAward(Guid id);
		bool IsAwardInStorage(Guid id);
		Award GetAwardByID(Guid id);
		List<Award> GetAllAwards();
	}
}
