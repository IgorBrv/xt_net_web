using InterfaceDAL;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace JsonDAL
{
	public class AuthDAO : IAuthDAO
	{

		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public bool CheckUser(string name, string password)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetPassword";

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
						if (reader["password"] as string == password)
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

		public bool CreateUser(string name, string password)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetUserCount";

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
						SqlParameter passwordParam = new SqlParameter("@password", password);

						command.Parameters.Add(nameParam);
						command.Parameters.Add(passwordParam);

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
	}
}
