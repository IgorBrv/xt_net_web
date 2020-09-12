using InterfacesBLL;
using InterfaceDAL;
using CoreBLL;
using JsonDAL;


namespace Dependencies
{
    public static class Resolver
    {   // Разрешитель зависимостей

		private static readonly IBll bll;
        private static readonly IUsersDAO daoUsers;
        private static readonly IAwardsDAO daoAwards;
        private static readonly IAwardsAssotiatonsDAO daoAwardsAssotiations;

        static Resolver()
        {
            daoAwardsAssotiations = new DALAwardsAssotiationsJSON();
            daoUsers = new DALUsersJSON(daoAwardsAssotiations);
            daoAwards = new DALAwardsJSON(daoAwardsAssotiations);
            bll = new BLLMain(daoUsers, daoAwards, daoAwardsAssotiations);
        }

        public static IBll GetBLL => bll;
    }
}
