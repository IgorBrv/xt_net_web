using InterfaceDAL;
using InterfacesBLL;

namespace CoreBLL
{
	public class BLOAuth : IAuthBLO
	{
		private readonly IAuthDAO daoAuth;
		public BLOAuth(IAuthDAO daoAuth)
		{
			this.daoAuth = daoAuth;
		}

		public bool CheckUser(string name, string password)
		{
			return daoAuth.CheckUser(name, password);
		}

		public bool CreateUser(string name, string password)
		{
			return daoAuth.CreateUser(name, password);
		}
	}
}
