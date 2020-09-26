using System.Collections.Generic;
using Epam.CommonEntities;

namespace Epam.Interfaces.DAL
{
	public interface IMessagesDAL
	{
		Message SendMessage(Message message);

		int? GetChat(int idUser, int idOpponent);

		void RemoveMessage(int idMessage);

		IEnumerable<Chat> GetAllChatsOfUser(int idUser);

		IEnumerable<Message> GetAllMessagesFromChat(int idChat);
	}
}
