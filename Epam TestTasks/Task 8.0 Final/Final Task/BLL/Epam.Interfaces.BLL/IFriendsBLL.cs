using System.Collections.Generic;
using Epam.CommonEntities;

namespace Epam.Interfaces.BLL
{
	public interface IFriendsBLL
	{   // BLL Friends, отвечает за работу со списком друзей пользоввателя (Добавление в друзья, отправление заявки, удаление из друзей, подтверждение заявки, получение списка друзей)

		IEnumerable<UserData> GetFriends(int userId);

		IEnumerable<UserData> GetInventations(int userId);

		IEnumerable<UserData> GetFriendRequests(int userId);

		void RemoveFriend(int userId, int opponentId);

		void SendInventation(int idUser, int idOpponent);

		void AcceptFrindRequest(int userId, int opponentId);
	}
}
