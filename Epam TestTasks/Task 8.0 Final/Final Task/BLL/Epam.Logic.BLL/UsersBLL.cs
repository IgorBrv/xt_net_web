using System.Security.Cryptography;
using Epam.CommonLoggerInterface;
using System.Collections.Generic;
using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using System.Text;
using System.IO;
using System;


namespace Epam.Logic.BLL
{
	public class UsersBLL : IUsersBLL
	{   // DAL Users, отвечает за работу с сущностью пользователей, позволяет создавать профили пользователей, редактировать профиль пользователя, присваивать им эмблемы, а так же
		// получать профиль пользователя, получать список пользоввателей, искать пользователей

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
				int idUser = daoUsers.Create(email, user);

				if (idUser >= 0)
				{
					string pswd = ComputeSHA256Hash($"{password}{idUser}");
					daoUsers.SetPassword(idUser, pswd);

					logger.Info("BLL: Process of creating user done");
					return true;
				}

				logger.Info("BLL: Process of creating user was unsucssesseful");
				return false;
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Process of creating user failed!");
				throw new Exception("error while creating user", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of getting user by id failed!");
				throw new Exception("error while process of getting user by id", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of getting user by email failed!");
				throw new Exception("error while process of getting user by email", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of updating users data failed!");
				throw new Exception("error while process of updating users data", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of getting userlist failed!");
				throw new Exception("error while getting userlist process", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of searching of users failed!");
				throw new Exception("error while users search procecss", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Process of searching of users failed!");
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of removing emblem from user failed!");
				throw new Exception("error while removing emblem process", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Process of adding emblem to user failed!");
				throw new Exception("error while adding emblem process", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Process of adding emblem to user failed!");
				throw new Exception("error while adding emblem process", e);
			}
		}

		public bool ChangePassword(int id, string oldPassword, string password)
		{
			logger.Info("BLL: changing users pasword process started");

			try
			{
				password = ComputeSHA256Hash($"{password}{id}");
				oldPassword = ComputeSHA256Hash($"{oldPassword}{id}");
				bool result = daoUsers.ChangePassword(id, oldPassword, password);

				logger.Info("BLL: changing users pasword process done");
				return result;
			}
			catch (StorageException e)
			{
				logger.Error("BLL: changing users pasword process failed!");
				throw new Exception("error while changing password of user process", e);
			}
			catch (Exception e)
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
				int? id = daoUsers.GetId(email);

				if (id != null)
				{
					password = ComputeSHA256Hash($"{password}{id}");

					bool result = daoUsers.CheckUser(email, password);

					logger.Info("BLL: checking users password process done");
					return result;
				}

				logger.Info("BLL: checking users password process was unsucsesseful");
				return false;
			}
			catch (StorageException e)
			{
				logger.Error("BLL: checking users password process failed!");
				throw new Exception("error while checking users auth process", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: checking users password process failed!");
				throw new Exception("error while checking users auth process", e);
			}
		}

		private string ComputeSHA256Hash(string password)
		{
			try
			{
				using (var sha256 = new SHA256Managed())
				{
					return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");
				}
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Computing SHA256 Hash failed!");
				throw new Exception("error while computing SHA256 hash", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Computing SHA256 Hash failed!");
				throw new Exception("error while computing SHA256 hash", e);
			}
		}
	}
}
