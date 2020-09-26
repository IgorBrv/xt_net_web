using System.Collections.Generic;
using Epam.CommonEntities;

namespace Epam.Interfaces.DAL
{
	public interface IFriendsDAL
	{
		IEnumerable<UserData> GetFriends(int userId);

		IEnumerable<UserData> GetInventations(int userId);

		IEnumerable<UserData> GetFriendRequests(int userId);

		void RemoveFriend(int userId, int opponentId);

		void SendInventation(int idUser, int idOpponent);

		void AcceptFrindRequest(int userId, int opponentId);
	}
}
