using System;
using System.Collections.Generic;
using Epam.CommonEntities;


namespace Epam.Interfaces.BLL
{
	public interface IMessagesBLL
	{   // BLL Messages, отвечает за работу с чатами и сообщениями пользователя. Позволяет создать чат, удалить чат, отправить сообщение, удалить сообщение, получить список чатов и сообщений

		Message SendMessage(int chatId, int senderId, string text, DateTime date);

		int? GetChat(int idUser, int idOpponent);

		void RemoveMessage(int idMessage);

		IEnumerable<Chat> GetAllChatsOfUser(int idUser);

		IEnumerable<Message> GetAllMessagesFromChat(int idChat, int idReader);

		int GetUnreadedCount(int idUser);

		int CreateChat(int idUser, int idOpponent);

		void RemoveChat(int idChat);

		void LeaveChat(int idChat, int idUser);

		void ReturnToChat(int idChat, int idUser);
	}
}
