using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALInterface;
using DALJSON;


namespace DALConnector
{
	public static class DALGetter
	{
		private static DALUsersJSON dalUsers;
		private static DALAwardsJSON dalAwards;
		private static DALAwardsAssotiationsJSON dalAwardsAssotiations;

		public static IUsersDAL GetUsersDAL() => dalUsers ?? (dalUsers = new DALUsersJSON());
		public static IAwardsDAL GetAwardsDAL() => dalAwards ?? (dalAwards = new DALAwardsJSON());
		public static IAwardsAssotiatonsDAL GetAwardsAssotiationsDAL() => dalAwardsAssotiations ?? (dalAwardsAssotiations = new DALAwardsAssotiationsJSON());
	}
}
