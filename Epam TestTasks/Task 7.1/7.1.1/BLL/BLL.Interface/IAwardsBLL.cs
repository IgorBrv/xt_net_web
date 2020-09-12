using System;
using System.Collections.Generic;
using Entities;

namespace InterfacesBLL
{   // Интерфейс BLL слоя (в данном случае я постарался реализовать все возможные метооды, т.к. неизвестно, какие могут потребоваться при переезде на другой UI или DAL)

	public interface IAwardsBLO
	{
		Award AddAward(string name);
		bool RemoveAward(Award award);
		bool UpdateAward(Guid id, string title);
		bool IsAwardInStorage(Guid id);
		Award GetAwardByID(Guid id);
		List<Award> GetAllAwards();
	}
}
