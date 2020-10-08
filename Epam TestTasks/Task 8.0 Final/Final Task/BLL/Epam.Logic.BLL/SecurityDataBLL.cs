using System.Collections.Generic;
using Epam.CommonLoggerInterface;
using System.Data.SqlClient;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using System.Linq;
using System;


namespace Epam.Logic.BLL
{
	public class SecurityDataBLL : ISecurityDataBLL
	{   // BLL SecurityData, отвечает за работу с ролями пользователя, позволяет получить роли пользователя, добавить роли пользователю, проверить принадлежность роли пользователю

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
			catch (Exception e)
			{
				logger.Error("BLL: process of adding role to user failed!");
				throw new Exception("error while adding role to user process", e);
			}
		}

		public IEnumerable<string> GetRolesOfUser(string email)
		{
			logger.Info("BLL: getting users role process started");

			try
			{
				IEnumerable<string> result = daoSecurityData.GetRolesOfUser(email);

				logger.Info("BLL: getting users role process done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: getting users role process failed!");
				throw new Exception("error while getting role of user", e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("BLL: getting users role process failed!");
				throw new Exception("error while getting role of user", e);
			}
			catch (Exception e)
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
				if (daoSecurityData.GetRolesOfUser(email).Contains(role))
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
			catch (Exception e)
			{
				logger.Info("BLL: checking users role process failed");
				throw new Exception("error while checking users role", e);
			}
		}
	}
}
