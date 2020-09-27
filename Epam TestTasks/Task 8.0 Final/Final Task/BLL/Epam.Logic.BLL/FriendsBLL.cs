using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using Epam.CommonLoggerInterface;

namespace Epam.Logic.BLL
{
    public class FriendsBLL : IFriendsBLL
    {
        private readonly ILogger logger;
        private readonly IFriendsDAL daoFriends;

        public FriendsBLL(ILogger logger, IFriendsDAL daoFriends)
		{
            this.logger = logger;
            this.daoFriends = daoFriends;
		}

		public void AcceptFrindRequest(int userId, int opponentId)
		{
			logger.Info("BLL: Accepting friend request process started");

			try
			{
				daoFriends.AcceptFrindRequest(userId, opponentId);

				logger.Info("BLL: Accepting friend request process done");
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Accepting friend request process failed!");
				throw new Exception("exeption while accepting friend request", e);
			}
		}

		public IEnumerable<UserData> GetFriendRequests(int userId)
		{
			logger.Info("BLL:  Getting friends requests list process started");

			try
			{
				IEnumerable<UserData> temp = daoFriends.GetFriendRequests(userId);

				logger.Info("BLL: Getting friends requests list process done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Getting friends requests list process failed!");
				throw new Exception("exeption while accepting friend requests list", e);
			}
		}

		public IEnumerable<UserData> GetFriends(int userId)
		{
			logger.Info("BLL: Getting friends list process started");
			try
			{
				IEnumerable<UserData> temp = daoFriends.GetFriends(userId);

				logger.Info("BLL: Getting friends list process done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Getting friends list process failed!");
				throw new Exception("exeption while recieving list of friends", e);
			}
		}

		public IEnumerable<UserData> GetInventations(int userId)
		{
			logger.Info("BLL: Getting self inventations list process started");

			try
			{
				IEnumerable<UserData> temp = daoFriends.GetInventations(userId);

				logger.Info("BLL: Getting self inventations list process done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Getting self inventations list process failed!");
				throw new Exception("exeption while recieving friends inventations list", e);
			}
		}

		public void RemoveFriend(int userId, int opponentId)
		{
			logger.Info("BLL: Person removing from friendslist process started");

			try
			{
				daoFriends.RemoveFriend(userId, opponentId);

				logger.Info("BLL: Person removing from friendslist process done");
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Person removing from friendslist process failed!");
				throw new Exception("exeption while removing friends", e);
			}
		}

		public void SendInventation(int idUser, int idOpponent)
		{
			logger.Info("BLL: Inventation sending process started");

			try
			{
				daoFriends.SendInventation(idUser, idOpponent);

				logger.Info("BLL: Inventation sending process done");
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Inventation sending process failed!");
				throw new Exception("exeption while sending inventation to friends", e);
			}
		}
	}
}
