using System;
using System.Linq;
using System.Web.Security;
using Epam.DependencyResolver;

namespace Epam.ASPNet.PL.Models
{	// Самописный RoleProvider

	public class MyRoleProvider : RoleProvider
	{
		private readonly Resolver resolver = CommonData.GetResolver();

		public override string[] GetRolesForUser(string username)
		{
			try
			{
				return resolver.GetBloSecurityData.GetRolesOfUser(username).ToArray();
			}
			catch
			{   // Перехват ошибок сгенерированных в bll

				return new string[] { "user" }; // TODO (возвращаем роль младшего пользоателя в случае ошибки)
			}
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			try
			{
				return resolver.GetBloSecurityData.IsUserInRole(username, roleName);
			}
			catch
			{   // Перехват ошибок сгенерированных в bll

				return false;  // TODO (возвращаем false, т.к. проверок на младшего пользователя в коде нет)
			}
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			try
			{
				foreach (string name in usernames)
				{
					foreach (string role in roleNames)
					{
						resolver.GetBloSecurityData.AddRoleToUser(name, role);
					}
				}
			}
			catch
			{
				// TODO
			}
		}

		#region NOT_IMPLEMENTED

		public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}