using System;
using System.IO;
using System.Web.Security;
using Dependencies;
using System.Web;
using System.Web.WebPages;

namespace PL.Webpages.models
{
	public class MyRoleProvider : RoleProvider
	{
		private readonly Resolver resolver = StaticResolver.Get();

		public override string[] GetRolesForUser(string username)
		{
			return new string[] { resolver.GetBLLRoles.GetRoleOfUser(username) };
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			return resolver.GetBLLRoles.IsUserInRole(username, roleName);
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			foreach (string name in usernames)
			{
				foreach (string role in roleNames)
				{
					resolver.GetBLLRoles.AddRoleToUser(name, role);
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