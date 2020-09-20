using InterfaceDAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JsonDAL
{
	public class RolesDAO : IRolesDAO
	{
		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public bool AddRoleToUser(string name, string role)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetNumOfUsersRoles";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter nameParam = new SqlParameter("@name", name);

					command.Parameters.Add(nameParam);

					connection.Open();

					if ((int)command.ExecuteScalar() == 0)
					{
						stProc = "CreateUser";

						command = new SqlCommand(stProc, connection)
						{
							CommandType = CommandType.StoredProcedure
						};

						nameParam = new SqlParameter("@name", name);
						SqlParameter roleParam = new SqlParameter("@role", role);

						command.Parameters.Add(nameParam);
						command.Parameters.Add(roleParam);

						command.ExecuteScalar();

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

		public string GetRoleOfUser(string name)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetRoleOfUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter nameParam = new SqlParameter("@name", name);

					command.Parameters.Add(nameParam);

					connection.Open();

					return command.ExecuteScalar() as string;
				}
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public bool IsUserInRole(string name, string role)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetRoleOfUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter nameParam = new SqlParameter("@name", name);

					command.Parameters.Add(nameParam);

					connection.Open();

					var reader = command.ExecuteReader();

					if (reader.Read())
					{
						if (reader["role"] as string == role)
						{
							return true;
						}
					}
				}
			}
			catch (SqlException)
			{
				return false;
			}

			return false;
		}
	}
}
