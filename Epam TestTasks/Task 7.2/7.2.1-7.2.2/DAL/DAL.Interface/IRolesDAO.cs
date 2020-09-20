
namespace InterfaceDAL
{
	public interface IRolesDAO
	{
		string GetRoleOfUser(string name);
		bool IsUserInRole(string name, string role);
		bool AddRoleToUser(string name, string role);
	}
}
