using System;
using System.Collections.Generic;
using Epam.CommonEntities;


namespace Epam.Interfaces.BLL
{
	public interface IMessagesBLL
	{
		Message SendMessage(int chatId, int senderId, string text, DateTime date);

		int? GetChat(int idUser, int idOpponent);

		void RemoveMessage(int idMessage);

		IEnumerable<Chat> GetAllChatsOfUser(int idUser);

		IEnumerable<Message> GetAllMessagesFromChat(int idChat);
	}
}
