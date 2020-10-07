using System.Collections.Generic;

namespace Epam.Interfaces.DAL
{
	public interface ISecurityDataDAL
	{   // DAL SecurityData, отвечает за работу с ролями пользователя, позволяет получить роли пользователя, добавить роли пользователю

		IEnumerable<string> GetRolesOfUser(string email);

		bool AddRoleToUser(string email, string role);
	}
}
