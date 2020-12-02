using Epam.CommonLoggerInterface;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;
using System.Data;
using System;

namespace Epam.Logic.DAL
{
	public class SecurityDataDAL : ISecurityDataDAL
	{	// DAL SecurityData, отвечает за работу с ролями пользователя, позволяет получить роли пользователя, добавить роли пользователю


		private readonly ILogger logger;
		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public SecurityDataDAL(ILogger logger)
		{
			this.logger = logger;
		}

		public bool AddRoleToUser(string email, string role)
		{
			logger.Info("DAL: process of adding role to user started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetNumOfUsersRoles";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@email", email),
						new SqlParameter("@role", role)
					};

					command.Parameters.AddRange(parameters);
					connection.Open();

					if ((int)command.ExecuteScalar() == 0)
					{
						stProc = "AddRoleToUser";

						command = new SqlCommand(stProc, connection)
						{
							CommandType = CommandType.StoredProcedure
						};

						parameters = new SqlParameter[]
						{
							new SqlParameter("@email", email),
							new SqlParameter("@role", role)
						};

						command.Parameters.AddRange(parameters);
						command.ExecuteNonQuery();

						logger.Info("DAL: process of adding role to user done");
						return true;
					}


					logger.Info("DAL: process of adding role to user was unsucsesseful");
					return false;
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: process of adding role to user failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: process of adding role to user failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public IEnumerable<string> GetRolesOfUser(string email)
		{
			List<string> result = new List<string>();
			logger.Info("DAL: getting users role process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetRoleOfUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter nameParam = new SqlParameter("@email", email);
					command.Parameters.Add(nameParam);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						result.Add(reader["name"] as string);
					}
				}

				logger.Info("DAL: getting users role process done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: getting users role process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: getting users role process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: getting users role process failed!");
				throw new StorageException(e.Message, e);
			}
		}
	}
}
