using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using Epam.CommonLoggerInterface;

namespace Epam.Logic.BLL
{
	public class UsersBLL : IUsersBLL
	{
		private readonly ILogger logger;
		private readonly IUsersDAL daoUsers;

		public UsersBLL(ILogger logger, IUsersDAL daoUsers)
		{
			this.logger = logger;
			this.daoUsers = daoUsers;
		}

		public void SetPath(string path)
		{
			daoUsers.Path = path;
		}

		public bool Create(string email, string password, string name, DateTime birth)
		{
			logger.Info("BLL: Process of creating user started");

			try
			{
				UserData user = new UserData(name, birth);
				bool result = daoUsers.Create(email, password, user);

				logger.Info("BLL: Process of creating user done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of creating user failed!");
				throw new Exception("error while creating user", e);
			}

		}

		public UserData GetById(int id)
		{
			logger.Info("BLL: Process of getting user by id started");

			try
			{
				UserData temp = daoUsers.GetById(id);

				logger.Info("BLL: Process of getting user by id done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of getting user by id failed!");
				throw new Exception("error while process of getting user by id", e);
			}
		}

		public UserData GetByEmail(string email)
		{
			logger.Info("BLL: Process of getting user by email started");

			try
			{
				UserData temp = daoUsers.GetByEmail(email);

				logger.Info("BLL: Process of getting user by email done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of getting user by email failed!");
				throw new Exception("error while process of getting user by email", e);
			}
		}

		public void Update(UserData user)
		{
			logger.Info("BLL: Process of updating users data started");

			try
			{
				daoUsers.Update(user);
				logger.Info("BLL: Process of updating users data done");
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of updating users data failed!");
				throw new Exception("error while process of updating users data", e);
			}
		}

		public IEnumerable<UserData> GetOthers(int curUserId)
		{
			logger.Info("BLL: Process of getting userlist started");

			try
			{
				IEnumerable<UserData> temp = daoUsers.GetOthers(curUserId);

				logger.Info("BLL: Process of getting userlist done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of getting userlist failed!");
				throw new Exception("error while getting userlist process", e);
			}
		}

		public List<UserData> FindByName(int curUserId, string text)
		{
			logger.Info("BLL: Process of users search started");

			try
			{
				List<UserData> temp = daoUsers.FindByName(curUserId, text);

				logger.Info("BLL: Process of creating user done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of creating user failed!");
				throw new Exception("error while users search procecss", e);
			}
		}

		public void RemoveEmblem(int id)
		{
			logger.Info("BLL: Process of removing emblem from user started");

			try
			{
				daoUsers.RemoveEmblem(id);

				logger.Info("BLL: Process of removing emblem from user done");
			}
			catch (IOException e)
			{
				logger.Error("BLL: Process of removing emblem from user failed!");
				throw new Exception("error while removing emblem process", e);
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of removing emblem from user failed!");
				throw new Exception("error while removing emblem process", e);
			}
		}

		public string AddEmblem(int id, string ext, BinaryReader br)
		{
			logger.Info("BLL: Process of adding emblem to user started");

			try
			{
				string temp = daoUsers.AddEmblem(id, ext, br);

				logger.Info("BLL: Process of adding emblem to user done");
				return temp;
			}
			catch (IOException e)
			{
				logger.Error("BLL: Process of adding emblem to user failed!");
				throw new Exception("error while adding emblem process", e);
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Process of adding emblem to user failed!");
				throw new Exception("error while adding emblem process", e);
			}
		}
	}
}
