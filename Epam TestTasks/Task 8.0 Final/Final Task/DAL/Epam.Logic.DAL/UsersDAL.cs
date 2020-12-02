using Epam.CommonLoggerInterface;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;
using System.Data;
using System.IO;
using System;


namespace Epam.Logic.DAL
{
	public class UsersDAL : IUsersDAL
	{	// DAL Users, отвечает за работу с сущностью пользователей, позволяет создавать профили пользователей, назначать им пароли, редактировать профиль пользователя, присваивать им эмблемы, а так же
		// получать профиль пользователя, получать список пользоввателей, искать пользователей

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
				string savePath = $"{Path}\\avatars\\";

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

				UserData user = GetById(id);

				if (user.emblem != null && File.Exists($"{Path}{user.emblem}"))
				{
					File.Delete($"{Path}{user.emblem}");
				}

				user.emblem = $"/avatars/{emblemName}";
				Update(user);

				logger.Info("DAL: Process of adding emblem to user done");
				return user.emblem;

			}
			catch (IOException e)
			{
				logger.Error("DAL: Process of adding emblem to user failed!");
				throw new StorageException(e.Message, e);
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of adding emblem to user failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of adding emblem to user failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public void RemoveEmblem(int id)
		{
			logger.Info("DAL: Process of removing emblem from user started");

			try
			{
				UserData user = GetById(id);

				if (user.emblem != null && File.Exists($"{Path}{user.emblem}"))
				{
					File.Delete($"{Path}{user.emblem}");
				}

				user.emblem = null;
				Update(user);

				logger.Info("DAL: Process of removing emblem from user done");
			}
			catch (IOException e)
			{
				logger.Error("DAL: Process of removing emblem from user failed!");
				throw new StorageException(e.Message, e);
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of removing emblem from user failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of removing emblem from user failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public int Create(string email, UserData user)
		{
			logger.Info("DAL: Process of creating user started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetEmailCount";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter emailParam = new SqlParameter("@email", email);

					command.Parameters.Add(emailParam);
					connection.Open();

					if ((int)command.ExecuteScalar() == 0)
					{
						stProc = "CreateUser";

						command = new SqlCommand(stProc, connection)
						{
							CommandType = CommandType.StoredProcedure
						};

						SqlParameter[] parameters = new SqlParameter[]
						{
							new SqlParameter("@email", email),
							new SqlParameter("@name", user.name),
							new SqlParameter("@birth", user.birth),
						};

						SqlParameter output = new SqlParameter
						{   // выходной параметр
							ParameterName = "@id",
							SqlDbType = SqlDbType.Int,
							Direction = ParameterDirection.Output
						};

						command.Parameters.AddRange(parameters);
						command.Parameters.Add(output);

						command.ExecuteScalar();

						logger.Info("DAL: Process of creating user done");
						return (int)output.Value;
					}
					else
					{
						logger.Info("DAL: Process of creating user was unsucsesseful");
						return -1;
					}
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of creating user failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of creating user failed!");
				throw new StorageException(e.Message, e);
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
						int? blockedBy;
						SqlInt32 temp = reader.GetSqlInt32(reader.GetOrdinal("blockedBy"));
						blockedBy = temp.IsNull ? (int?)null : temp.Value;
						usersList.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string, blockedBy));
					}
				}

				logger.Info("DAL: Process of searching of users done");
				return usersList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of searching of users failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Process of searching of users failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of searching of users failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public UserData GetById(int id)
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
						int? blockedBy;
						SqlInt32 temp = reader.GetSqlInt32(reader.GetOrdinal("blockedBy"));
						blockedBy = temp.IsNull ? (int?)null : temp.Value;
						user = new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string, blockedBy);
					}
				}

				logger.Info("DAL: Process of getting user by id done");
				return user;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of getting user by id failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Process of getting user by id failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of getting user by id failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public UserData GetByEmail(string email)
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
						int? blockedBy;
						SqlInt32 temp = reader.GetSqlInt32(reader.GetOrdinal("blockedBy"));
						blockedBy = temp.IsNull ? (int?)null : temp.Value;
						user = new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string, blockedBy);
					}
				}

				logger.Info("DAL: Process of getting user by email done");
				return user;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of getting user by email failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Process of getting user by email failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of getting user by email failed!");
				throw new StorageException(e.Message, e);
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
						int? blockedBy;
						SqlInt32 temp = reader.GetSqlInt32(reader.GetOrdinal("blockedBy"));
						blockedBy = temp.IsNull ? (int?)null : temp.Value;
						usersList.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string, blockedBy));
					}
				}

				logger.Info("DAL: Process of getting userlist done");
				return usersList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of getting userlist failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Process of getting userlist failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of getting userlist failed!");
				throw new StorageException(e.Message, e);
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
							ParameterName = "@blockedBy",
							Value = user.blockedBy,
							SqlDbType = SqlDbType.Int,
							IsNullable = true
						},
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
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Process of updating users data failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of updating users data failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public int? GetId(string email)
		{
			logger.Info("DAL: Getting id of user process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetUserId";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter sqlParam = new SqlParameter("@email", email);

					command.Parameters.Add(sqlParam);
					connection.Open();

					int? result = null;

					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						SqlInt32 temp = reader.GetSqlInt32(reader.GetOrdinal("id"));
						result = temp.IsNull ? (int?)null : temp.Value;
					}

					logger.Info("DAL: Getting id of user process  done");
					return result;
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting id of user process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting id of user process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting id of user process failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public void SetPassword(int id, string password)
		{
			logger.Info("DAL: Process of password setting started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "SetPasswordById";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] sqlParams = new SqlParameter[]
					{
						new SqlParameter("@id", id),
						new SqlParameter("@password", password),
					};

					command.Parameters.AddRange(sqlParams);
					connection.Open();
					command.ExecuteNonQuery();
				}

				logger.Info("DAL: Process of password setting  done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Process of password setting  failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Process of password setting  failed!");
				throw new StorageException(e.Message, e);
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

					logger.Info("DAL: checking users password process was unsucsesseful");
					return false;
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: checking users password process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: checking users password process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: checking users password process failed!");
				throw new StorageException(e.Message, e);
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

					int count = (int)command.ExecuteScalar();

					if (count == 1)
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
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: changing users pasword process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: changing users pasword process failed!");
				throw new StorageException(e.Message, e);
			}
		}
	}
}
