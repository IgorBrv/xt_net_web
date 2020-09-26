using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Epam.DependencyResolver;
using Epam.Interfaces.BLL;

namespace Epam.ASPNet.PL.Models
{
	public class MyRoleProvider : RoleProvider
	{
		private readonly Resolver resolver = StaticElements.GetResolver();

		public override string[] GetRolesForUser(string username)
		{
			return new string[] { resolver.GetBloSecurityData.GetRoleOfUser(username) };
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			return resolver.GetBloSecurityData.IsUserInRole(username, roleName);
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			foreach (string name in usernames)
			{
				foreach (string role in roleNames)
				{
					resolver.GetBloSecurityData.AddRoleToUser(name, role);
				}
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