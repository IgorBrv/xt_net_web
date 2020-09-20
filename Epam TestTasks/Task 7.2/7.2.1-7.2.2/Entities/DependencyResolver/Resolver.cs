using InterfacesBLL;
using InterfaceDAL;
using CoreBLL;
using JsonDAL;

namespace Dependencies
{
    public class Resolver
    {   // Разрешитель зависимостей

		private readonly IAuthDAO daoAuth;
		private readonly IRolesDAO daoRoles;
		private readonly IUsersDAO daoUsers;
		private readonly IAwardsDAO daoAwards;
		private readonly IAwardsAssotiatonsDAO daoAwardsAssotiations;

        public Resolver()
        {
            daoUsers = new UsersDAO();
            daoAwards = new AwardsDAO();
            daoAwardsAssotiations = new AwardsAssotiationsDAO(daoUsers, daoAwards);
            daoRoles = new RolesDAO();
            daoAuth = new AuthDAO();
            GetBLLAuth = new BLOAuth(daoAuth);
            GetBLLRoles = new BLORoles(daoRoles);
            GetBLLUsers = new BLOUsers(daoUsers);
            GetBLLAwards = new BLOAwards(daoAwards);
            GetBLLAwardsAssotiations = new BLOAwardsAssotiations(daoAwardsAssotiations);
        }

		public IAuthBLO GetBLLAuth { get; }
		public IRolesBLO GetBLLRoles { get; }
		public IUsersBLO GetBLLUsers { get; }
		public IAwardsBLO GetBLLAwards { get; }
		public IAwardsAssotiationsBLO GetBLLAwardsAssotiations { get; }
	}
}
