using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using InterfaceDAL;
using Entities;
using System.IO;
using System.Data;

namespace JsonDAL
{	// Объекты DAL слоя

	public class UsersDAO : IUsersDAO
	{   // Объект DAO отвечающий за работу с сущностью пользователей

		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public string Path { get; set; }

		public List<User> GetAllUsers()
		{
			List<User> personsList = new List<User>();

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllPersons";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					connection.Open();

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						personsList.Add(new User(Guid.Parse(reader["id"] as string), (int)reader["age"], reader["name"] as string, (DateTime)reader["birth"], reader["emblempath"] as string));
					}

				}

				return personsList;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public bool IsUserInStorage(Guid id)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "PersonsNumAtStorage";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id.ToString());

					command.Parameters.Add(idParam);

					connection.Open();

					if ((int)command.ExecuteScalar() > 0)
					{
						return true;
					}

					return false;
				}
			}
			catch (SqlException)
			{
				return false;
			}
		}

		public bool AddUser(User user)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "AddPerson";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", user.id.ToString());
					SqlParameter nameParam = new SqlParameter("@name", user.name);
					SqlParameter ageParam = new SqlParameter("@age", user.age);
					SqlParameter birthParam = new SqlParameter("@birth", user.birth);
					SqlParameter emblempathParam = new SqlParameter
					{
						ParameterName = "@emblempath",
						Value = user.emblempath,
						SqlDbType = SqlDbType.NVarChar,
						IsNullable = true
					};

					command.Parameters.Add(idParam);
					command.Parameters.Add(nameParam);
					command.Parameters.Add(ageParam);
					command.Parameters.Add(birthParam);
					command.Parameters.Add(emblempathParam);

					connection.Open();
					command.ExecuteScalar();
				}

				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}


		public bool RemoveUser(User user) => RemoveUserById(user.id);


		public bool RemoveUserById(Guid id)
		{
			try
			{
				User user = GetUserByID(id);

				if (user.emblempath != null)
				{
					File.Delete($"{Path}{user.emblempath.Substring(1)}");
				}

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "RemovePerson";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id);

					command.Parameters.Add(idParam);

					connection.Open();

					command.ExecuteScalar();
				}

				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}


		public bool UpdateUser(User user)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "UpdatePerson";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", user.id.ToString());
					SqlParameter nameParam = new SqlParameter("@name", user.name);
					SqlParameter ageParam = new SqlParameter("@age", user.age);
					SqlParameter birthParam = new SqlParameter("@birth", user.birth);

					SqlParameter emblempathParam = new SqlParameter
					{
						ParameterName = "@emblempath",
						Value = user.emblempath,
						SqlDbType = SqlDbType.NVarChar,
						IsNullable = true
					};

					command.Parameters.Add(idParam);
					command.Parameters.Add(nameParam);
					command.Parameters.Add(ageParam);
					command.Parameters.Add(birthParam);
					command.Parameters.Add(emblempathParam);

					connection.Open();

					command.ExecuteScalar();
				}

				return true;
			}
			catch (SqlException)
			{
				return false;
			}
		}


		public User GetUserByID(Guid id)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetPerson";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", id.ToString());

					command.Parameters.Add(idParam);

					connection.Open();

					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						return new User(Guid.Parse(reader["id"] as string), (int)reader["age"], reader["name"] as string, (DateTime)reader["birth"], reader["emblempath"] as string);
					}
				}

				return null;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public string AddEmblemToUser(Guid id, string ext, BinaryReader br)
		{
			string emblemName = $"{Guid.NewGuid()}.{ext}";
			string savePath = $"{Path}\\images\\avatars\\";

			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}

			try
			{
				using (br)
				{
					int lenght = (int)br.BaseStream.Length;
					byte[] file = br.ReadBytes(lenght);
					File.WriteAllBytes($"{savePath}{emblemName}", file);
				}

				User user = GetUserByID(id);

				if (user.emblempath != null && user.emblempath != "")
				{
					File.Delete($"{Path}{user.emblempath.Substring(1)}");
				}

				user.emblempath = $"./images/avatars/{emblemName}";

				UpdateUser(user);

				return user.emblempath;

			}
			catch (IOException)
			{
				return null;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public bool RemoveEmblemFromUser(Guid id)
		{
			try
			{
				User user = GetUserByID(id);

				if (user.emblempath != null)
				{
					File.Delete($"{Path}{user.emblempath.Substring(1)}");
				}

				user.emblempath = null;

				UpdateUser(user);

				return true;
			}
			catch (IOException)
			{
				return false;
			}

		}


}
}
