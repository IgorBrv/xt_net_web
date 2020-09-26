using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;

namespace Epam.Logic.DAL
{
	public class UsersDAL : IUsersDAL
	{
		private readonly ILogger logger;
		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public UsersDAL(ILogger logger)
		{
			this.logger = logger;
		}

		public string Path { get; set; }

		public string AddEmblem(int id, string ext, BinaryReader br)
		{
			logger.Info("DAL: Process of adding emblem to user started");

			try
			{
				string emblemName = $"{Guid.NewGuid()}.{ext}";
				string savePath = $"{Path}\\images\\";

				if (!Directory.Exists(savePath))
				{
					Directory.CreateDirectory(savePath);
				}

				using (br)
				{
					int lenght = (int)br.BaseStream.Length;
					byte[] file = br.ReadBytes(lenght);
					File.WriteAllBytes($"{savePath}{emblemName}", file);
				}

				UserData user = Get(id);

				if (user.emblem != null)
				{
					File.Delete($"{Path}{user.emblem.Substring(1)}");
				}

				user.emblem = $"./images/{emblemName}";
				Update(user);

				logger.Info("DAL: Process of adding emblem to user done");
				return user.emblem;

			}
			catch (IOException e)
			{
				logger.Error("DAL: Process of adding emblem to user failed!");
				throw e;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of adding emblem to user failed!");
				throw e;
			}
		}

		public void RemoveEmblem(int id)
		{
			logger.Info("DAL: Process of removing emblem from user started");

			try
			{
				UserData user = Get(id);

				if (user.emblem != null)
				{
					File.Delete($"{Path}{user.emblem.Substring(1)}");
				}

				user.emblem = null;
				Update(user);

				logger.Info("DAL: Process of removing emblem from user done");
			}
			catch (IOException e)
			{
				logger.Error("DAL: Process of removing emblem from user failed!");
				throw e;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of removing emblem from user failed!");
				throw e;
			}
		}

		public void Create(string email, string password, UserData user)
		{
			logger.Info("DAL: Process of creating user started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "CreateUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@email", email),
						new SqlParameter("@password", password),
						new SqlParameter("@name", user.name),
						new SqlParameter("@birth", user.birth)
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					command.ExecuteScalar();
				}

				logger.Info("DAL: Process of creating user done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of creating user failed!");
				throw e;
			}
		}

		public List<UserData> FindByName(int curUserId, string text)
		{
			List<UserData> usersList = new List<UserData>();
			logger.Info("DAL: Process of users search started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "FindUsersByName";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter textParam = new SqlParameter("@text", $"%{text}%");
					SqlParameter curUserIdParam = new SqlParameter("@curUserId", curUserId);
					command.Parameters.Add(textParam);
					command.Parameters.Add(curUserIdParam);
					connection.Open();

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						usersList.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string));
					}
				}

				logger.Info("DAL: Process of creating user done");
				return usersList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of creating user failed!");
				throw e;
			}
		}

		public UserData Get(int id)
		{
			UserData user = null;
			logger.Info("DAL: Process of getting user by id started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetUserById";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id);
					command.Parameters.Add(idParam);
					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						user = new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string);
					}
				}

				logger.Info("DAL: Process of getting user by id done");
				return user;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of getting user by id failed!");
				throw e;
			}
		}

		public UserData Get(string email)
		{
			UserData user = null;
			logger.Info("DAL: Process of getting user by email started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetUserByEmail";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter emailParam = new SqlParameter("@email", email);
					command.Parameters.Add(emailParam);
					connection.Open();
					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						user = new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string);
					}
				}

				logger.Info("DAL: Process of getting user by email done");
				return user;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of getting user by email failed!");
				throw e;
			}
		}

		public IEnumerable<UserData> GetOthers(int curUserId)
		{
			List<UserData> usersList = new List<UserData>();
			logger.Info("DAL: Process of getting userlist started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllUsersBesides";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter curUserIdParam = new SqlParameter("@curUserId", curUserId);
					command.Parameters.Add(curUserIdParam);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						usersList.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string));
					}
				}

				logger.Info("DAL: Process of getting userlist done");
				return usersList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of getting userlist failed!");
				throw e;
			}
		}

		public void Update(UserData user)
		{
			logger.Info("DAL: Process of updating users data started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "UpdateUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] sqlParams = new SqlParameter[]
					{
						new SqlParameter("@id", user.id),
						new SqlParameter("@name", user.name),
						new SqlParameter("@birth", user.birth),
						new SqlParameter
						{
							ParameterName = "@emblem",
							Value = user.emblem,
							SqlDbType = SqlDbType.NVarChar,
							IsNullable = true
						},
						new SqlParameter
						{
							ParameterName = "@statement",
							Value = user.statement,
							SqlDbType = SqlDbType.NVarChar,
							IsNullable = true
						}
					};
					
					command.Parameters.AddRange(sqlParams);
					connection.Open();
					command.ExecuteScalar();
				}

				logger.Info("DAL: Process of updating users data done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of updating users data failed!");
				throw e;
			}
		}
	}
}
