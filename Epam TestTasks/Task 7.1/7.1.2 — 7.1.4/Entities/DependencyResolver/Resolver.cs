using InterfacesBLL;
using InterfaceDAL;
using CoreBLL;
using JsonDAL;
using System.IO;

namespace Dependencies
{
    public class Resolver
    {   // Разрешитель зависимостей

        private readonly IUsersBLO bllUsers;
        private readonly IUsersDAO daoUsers;
        private readonly IAwardsBLO bllAwards;
        private readonly IAwardsDAO daoAwards;
        private readonly IEmblemsBLO bllEmblems;
        private readonly IEmblemsDAO daoEmblems;
        private readonly IAwardsAssotiationsBLO bllAwardsAssotiations;
        private readonly IAwardsAssotiatonsDAO daoAwardsAssotiations;

        public Resolver(string path)
        {
            daoAwardsAssotiations = new DALAwardsAssotiationsJSON(path);
            daoUsers = new DALUsersJSON(daoAwardsAssotiations, path);
            daoAwards = new DALAwardsJSON(daoAwardsAssotiations, path);
            daoEmblems = new EmblemsJSON(path);
            bllUsers = new BLOUsers(daoUsers);
            bllAwards = new BLOAwards(daoAwards);
            bllEmblems = new EmblemsBLL(daoEmblems);
            bllAwardsAssotiations = new BLOAwardsAssotiations(daoAwardsAssotiations);
        }
        public IUsersBLO GetBLLUsers => bllUsers;
        public IAwardsBLO GetBLLAwards => bllAwards;
        public IEmblemsBLO GetBLLEmblems => bllEmblems;
        public IAwardsAssotiationsBLO GetBLLAwardsAssotiations => bllAwardsAssotiations;
    }
}
