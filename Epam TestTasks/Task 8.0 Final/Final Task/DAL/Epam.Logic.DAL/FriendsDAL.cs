﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;

namespace Epam.Logic.DAL
{
    public class FriendsDAL : IFriendsDAL
    {
        private readonly ILogger logger;
		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public FriendsDAL(ILogger logger)
		{
            this.logger = logger;
		}

		public void AcceptFrindRequest(int userId, int opponentId)
		{
			logger.Info("DAL: Accepting friend request process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "AcceptFriendRequest";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@idUser", userId),
						new SqlParameter("@idOpponent", opponentId),
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					command.ExecuteScalar();
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Accepting friend request process failed!");
				throw e;
			}

			logger.Info("DAL: Accepting friend request process done");
		}

		public IEnumerable<UserData> GetFriendRequests(int userId)
		{
			List<UserData> users = new List<UserData>();

			logger.Info("DAL: Getting friends requests list process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetFriendRequests";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter parameter = new SqlParameter("@idUser", userId);
					command.Parameters.Add(parameter);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						users.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string));
					}
				}

				logger.Info("DAL: Getting friends requests list process done");

				return users;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting friends requests list process failed!");
				throw e;
			}
		}

		public IEnumerable<UserData> GetFriends(int userId)
		{
			List<UserData> users = new List<UserData>();
			logger.Info("DAL: Getting friends list process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetFriends";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter parameter = new SqlParameter("@idUser", userId);
					command.Parameters.Add(parameter);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						users.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string));
					}
				}

				logger.Info("DAL: Getting friends list process done");

				return users;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting friends list process failed!");
				throw e;
			}
		}

		public IEnumerable<UserData> GetInventations(int userId)
		{
			List<UserData> users = new List<UserData>();
			logger.Info("DAL: Getting self inventations list process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetUsersInventations";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter parameter = new SqlParameter("@idUser", userId);
					command.Parameters.Add(parameter);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						users.Add(new UserData((int)reader["id"], reader["name"] as string, (DateTime)reader["birth"], reader["statement"] as string, reader["emblem"] as string));
					}
				}

				logger.Info("DAL: Getting self inventations list process done");
				return users;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting self inventations list process failed!");
				throw e;
			}
		}

		public void RemoveFriend(int userId, int opponentId)
		{
			logger.Info("DAL: Person removing from friendslist process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "RemoveFriend";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@idUser", userId),
						new SqlParameter("@idOpponent", opponentId),
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					command.ExecuteScalar();
				}
			}
			catch (SqlException e)
			{
				logger.Info("DAL: Person removing from friendslist process failed!");
				throw e;
			}

			logger.Info("DAL: Person removing from friendslist process done");
		}

		public void SendInventation(int idUser, int idOpponent)
		{
			logger.Info("DAL: Inventation sending process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "SendInventation";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@idUser", idUser),
						new SqlParameter("@idOpponent", idOpponent),
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					command.ExecuteScalar();
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Inventation sending process failed!");
				throw e;
			}

			logger.Info("DAL: Inventation sending process done");
		}
	}
}
