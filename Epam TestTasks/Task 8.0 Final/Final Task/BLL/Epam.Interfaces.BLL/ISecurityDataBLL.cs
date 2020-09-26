
namespace Epam.Interfaces.BLL
{
	public interface ISecurityDataBLL
	{
		string GetRoleOfUser(string email);

		bool IsUserInRole(string email, string role);

		bool AddRoleToUser(string email, string role);

		bool CheckUser(string email, string password);

		bool ChangePassword(int id, string oldPassword, string password);
	}
}
