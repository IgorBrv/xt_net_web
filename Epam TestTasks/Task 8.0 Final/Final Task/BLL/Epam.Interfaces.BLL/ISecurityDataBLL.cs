
using System.Collections.Generic;

namespace Epam.Interfaces.BLL
{
	public interface ISecurityDataBLL
	{   // BLL SecurityData, отвечает за работу с ролями пользователя, позволяет получить роли пользователя, добавить роли пользователю, проверить принадлежность роли пользователю

		IEnumerable<string> GetRolesOfUser(string email);

		bool IsUserInRole(string email, string role);

		bool AddRoleToUser(string email, string role);

	}
}
