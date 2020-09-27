using System;
using System.Data.SqlClient;
using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using Epam.CommonLoggerInterface;

namespace Epam.Logic.BLL
{
	public class SecurityDataBLL : ISecurityDataBLL
	{
		private readonly ILogger logger;
		private readonly ISecurityDataDAL daoSecurityData;

		public SecurityDataBLL(ILogger logger, ISecurityDataDAL daoSecurityData)
		{
			this.logger = logger;
			this.daoSecurityData = daoSecurityData;
		}

		public bool AddRoleToUser(string email, string role)
		{
			logger.Info("BLL: process of adding role to user started");

			try
			{
				bool result = daoSecurityData.AddRoleToUser(email, role);

				logger.Info("BLL: process of adding role to user done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: process of adding role to user failed!");
				throw new Exception("error while adding role to user process", e);
			}
		}

		public bool ChangePassword(int id, string oldPassword, string password)
		{
			logger.Info("BLL: changing users pasword process started");

			try
			{
				bool result = daoSecurityData.ChangePassword(id, oldPassword, password);

				logger.Info("BLL: changing users pasword process done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: changing users pasword process failed!");
				throw new Exception("error while changing password of user process", e);
			}
		}

		public bool CheckUser(string email, string password)
		{
			logger.Info("BLL: checking users password process started");

			try
			{
				bool result = daoSecurityData.CheckUser(email, password);

				logger.Info("BLL: checking users password process done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: checking users password process failed!");
				throw new Exception("error while checking users auth process", e);
			}
		}

		public string GetRoleOfUser(string email)
		{
			logger.Info("BLL: getting users role process started");

			try
			{
				string result = daoSecurityData.GetRoleOfUser(email);

				logger.Info("BLL: getting users role process done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: getting users role process failed!");
				throw new Exception("error while getting role of user", e);
			}
		}

		public bool IsUserInRole(string email, string role) 
		{
			logger.Info("BLL: checking users role process started");

			try
			{
				if (daoSecurityData.GetRoleOfUser(email) == role)
				{
					logger.Info("BLL: checking users role process done");
					return true;
				}

				logger.Info("BLL: checking users role process was unsucsesseful");
				return false;
			}
			catch (SqlException e)
			{
				logger.Info("BLL: checking users role process failed");
				throw new Exception("error while checking users role", e);
			}
		}
	}
}
