
namespace InterfacesBLL
{
	public interface IRolesBLO
	{
		string GetRoleOfUser(string name);
		bool IsUserInRole(string name, string role);
		bool AddRoleToUser(string name, string role);
	}
}
