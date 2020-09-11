using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBLL;
using InterfaceDAL;
using CoreBLL;
using JsonDAL;


namespace Dependencies
{
    public static class Resolver
    {
		private static readonly IBll bll;
        private static readonly IUsersDAL dalUsers;
        private static readonly IAwardsDAL dalAwards;
        private static readonly IAwardsAssotiatonsDAL dalAwardsAssotiations;

        static Resolver()
        {
            dalUsers = new DALUsersJSON();
            dalAwards = new DALAwardsJSON();
            dalAwardsAssotiations = new DALAwardsAssotiationsJSON();
            bll = new BLLMain(dalUsers, dalAwards, dalAwardsAssotiations);
        }

        public static IBll GetBLL => bll;
    }
}
