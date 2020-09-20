using System;
using System.Collections.Generic;
using InterfaceDAL;
using Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace JsonDAL
{   // Объекты DAL слоя

	public class AwardsAssotiationsDAO : IAwardsAssotiatonsDAO
	{   // Объект DAO отвечающий за работу с сущностью связей многие ко многим (награды : пользователи)

		private readonly IUsersDAO usersDao;
		private readonly IAwardsDAO awardsDao;
		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public AwardsAssotiationsDAO(IUsersDAO usersDao, IAwardsDAO awardsDao)
		{
			this.usersDao = usersDao;
			this.awardsDao = awardsDao;
		}

		public List<Guid[]> GetListOfAwarded()
		{
			try
			{
				List<Guid[]> ListOfAwarded = new List<Guid[]>();

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetListOfAwarded";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					connection.Open();

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						ListOfAwarded.Add(new Guid[] { Guid.Parse(reader["personid"] as string), Guid.Parse(reader["awardid"] as string) });
					}
				}

				return ListOfAwarded;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public List<Award> GetAllAwardsOfUser(User user)
		{
			try
			{
				List<Award> awardsList = new List<Award>();

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllAwardsOfUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", user.id);
					command.Parameters.Add(idParam);

					connection.Open();

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						awardsList.Add(awardsDao.GetAwardByID(Guid.Parse(reader["awardid"] as string)));
					}
				}

				return awardsList;
			}
			catch (SqlException)
			{
				return null;
			}
		}

		public List<User> GetAllUsersWithAward(Award award)
		{
			try
			{
				List<User> usersList = new List<User>();

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllPersonsWithAward";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", award.id);
					command.Parameters.Add(idParam);

					connection.Open();

					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						usersList.Add(usersDao.GetUserByID(Guid.Parse(reader["personid"] as string)));
					}
				}

				return usersList;
			}
			catch (SqlException)
			{
				return null;
			}
		}


		public Dictionary<User, List<Award>> GetAllUsersWAwards()
		{
			Dictionary<User, List<Award>> temp = new Dictionary<User, List<Award>>();

			foreach (User user in usersDao.GetAllUsers())
			{
				List<Award> awards = GetAllAwardsOfUser(user);

				if (awards != null)
				{
					temp.Add(user, awards);
				}
				else
				{
					temp.Add(user, new List<Award>());
				}

			}

			return temp;
		}


		public Dictionary<Award, List<User>> GetAllAwardsWUsers()
		{
			Dictionary<Award, List<User>> temp = new Dictionary<Award, List<User>>();

			foreach (Award award in awardsDao.GetAllAwards())
			{
				List<User> users = GetAllUsersWithAward(award);

				if (users != null)
				{
					temp.Add(award, users);
				}
				else
				{
					temp.Add(award, new List<User>());
				}

			}

			return temp;
		}


		public bool AddAwardToUser(User user, Award award)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "AddAwardToUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter personidParam = new SqlParameter("@personid", user.id);
					SqlParameter awardidParam = new SqlParameter("@awardid", award.id);

					command.Parameters.Add(personidParam);
					command.Parameters.Add(awardidParam);

					connection.Open();

					command.ExecuteScalar();

					return true;
				}

			}
			catch (SqlException)
			{
				return false;
			}
		}


		public bool RemoveAwardFromUser(User user, Award award)
		{
			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "RemoveAwardFromUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter personidParam = new SqlParameter("@personid", user.id);
					SqlParameter awardidParam = new SqlParameter("@awardid", award.id);

					command.Parameters.Add(personidParam);
					command.Parameters.Add(awardidParam);

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
	}
}