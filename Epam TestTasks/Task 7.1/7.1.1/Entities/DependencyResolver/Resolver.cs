using InterfacesBLL;
using InterfaceDAL;
using CoreBLL;
using JsonDAL;


namespace Dependencies
{
    public static class Resolver
    {   // Разрешитель зависимостей

        private static readonly IUsersBLO bllUsers;
        private static readonly IUsersDAO daoUsers;
        private static readonly IAwardsBLO bllAwards;
        private static readonly IAwardsDAO daoAwards;
        private static readonly IAwardsAssotiationsBLO bllAwardsAssotiations;
        private static readonly IAwardsAssotiatonsDAO daoAwardsAssotiations;

        static Resolver()
        {
            daoAwardsAssotiations = new DALAwardsAssotiationsJSON();
            daoUsers = new DALUsersJSON(daoAwardsAssotiations);
            daoAwards = new DALAwardsJSON(daoAwardsAssotiations);
            bllUsers = new BLOUsers(daoUsers);
            bllAwards = new BLOAwards(daoAwards);
            bllAwardsAssotiations = new BLOAwardsAssotiations(daoAwardsAssotiations);
        }

        public static IUsersBLO GetBLLUsers => bllUsers;
        public static IAwardsBLO GetBLLAwards => bllAwards;
        public static IAwardsAssotiationsBLO GetBLLAwardsAssotiations => bllAwardsAssotiations;
    }
}
