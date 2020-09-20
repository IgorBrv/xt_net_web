using InterfaceDAL;
using InterfacesBLL;

namespace CoreBLL
{
	public class BLORoles : IRolesBLO
	{
		private readonly IRolesDAO daoRoles;

		public BLORoles(IRolesDAO daoRoles)
		{
			this.daoRoles = daoRoles;
		}

		public bool AddRoleToUser(string name, string role)
		{
			return daoRoles.AddRoleToUser(name, role);
		}

		public string GetRoleOfUser(string name)
		{
			string role = daoRoles.GetRoleOfUser(name);

			if (role != null)
			{
				return role;
			}
			return "guest";
		}

		public bool IsUserInRole(string name, string role)
		{
			return daoRoles.IsUserInRole(name, role);
		}
	}
}
