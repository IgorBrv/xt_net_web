using System;
using System.Collections.Generic;
using Entities;

namespace InterfaceDAL
{
	public interface IAwardsDAL
	{
		bool AddAward(Award award);
		bool RemoveAward(Award award);
		bool RemoveAwardByID(Guid id);
		bool UpdateAward(Award award);
		Award GetAwardByID(Guid id);
		List<Award> GetAllAwards();
	}
}
