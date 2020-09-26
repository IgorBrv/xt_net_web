using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;

namespace Epam.Logic.DAL
{
	public class SecurityDataDAL : ISecurityDataDAL
	{
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

					SqlParameter nameParam = new SqlParameter("@email", email);

					command.Parameters.Add(nameParam);

					connection.Open();

					if ((int)command.ExecuteScalar() == 0)
					{
						stProc = "CreateRoleForUser";

						command = new SqlCommand(stProc, connection)
						{
							CommandType = CommandType.StoredProcedure
						};

						nameParam = new SqlParameter("@email", email);
						SqlParameter roleParam = new SqlParameter("@role", role);
						command.Parameters.Add(nameParam);
						command.Parameters.Add(roleParam);
						command.ExecuteScalar();


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
				throw e;
			}
		}

		public bool CheckUser(string email, string password)
		{
			logger.Info("DAL: checking users password process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetPassword";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter nameParam = new SqlParameter("@email", email);
					command.Parameters.Add(nameParam);
					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						if (reader["password"] as string == password)
						{
							logger.Info("DAL: checking users password process done");
							return true;
						}
					}
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: checking users password process failed!");
				throw e;
			}

			logger.Info("DAL: checking users password process was unsucsesseful");
			return false;
		}

		public string GetRoleOfUser(string email)
		{
			string result = null;
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

					result = command.ExecuteScalar() as string;
				}

				logger.Info("DAL: getting users role process done");
				return result;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: getting users role process failed!");
				throw e;
			}
		}

		public bool ChangePassword(int id, string oldPassword, string password)
		{
			logger.Info("DAL: changing users pasword process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "ChangePasswordOfUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@id", id),
						new SqlParameter("@oldPassword", oldPassword),
						new SqlParameter("@password", password)
					};

					command.Parameters.AddRange(parameters);
					connection.Open();

					if ((int)command.ExecuteScalar() == 1)
					{
						logger.Info("DAL: changing users pasword process done");
						return true;
					}
				}

				logger.Info("DAL: changing users pasword process was unsucsessefull");
				return false;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: changing users pasword process failed!");
				throw e;
			}
		}
	}
}
