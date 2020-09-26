namespace Epam.Interfaces.DAL
{
	public interface ISecurityDataDAL
	{
		string GetRoleOfUser(string email);

		bool AddRoleToUser(string email, string role);

		bool CheckUser(string email, string password);

		bool ChangePassword(int id, string oldPassword, string password);
	}
}
