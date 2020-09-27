using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Epam.CommonLoggerInterface;

namespace Epam.Logic.BLL
{
    public class MessagesBLL : IMessagesBLL
    {
        private readonly ILogger logger;
        private readonly IMessagesDAL daoMessages;

        public MessagesBLL(ILogger logger, IMessagesDAL daoMessages)
        {
            this.logger = logger;
            this.daoMessages = daoMessages;
        }

		public IEnumerable<Chat> GetAllChatsOfUser(int idUser)
		{
			logger.Info("BLL: Getting list of users chats process started");

			try
			{
				IEnumerable<Chat> temp = daoMessages.GetAllChatsOfUser(idUser);

				logger.Info("BLL: Getting list of users chats process done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Getting list of users chats process failed!");
				throw new Exception("error while getting list of users chats", e);
			}
		}

		public IEnumerable<Message> GetAllMessagesFromChat(int idChat)
		{
			logger.Info("BLL: Getting chats list of messages process started");

			try
			{
				IEnumerable<Message> temp = daoMessages.GetAllMessagesFromChat(idChat);

				logger.Info("BLL: Getting chats list of messages process done");
				return temp;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Getting chats list of messages process failed!");
				throw new Exception("error while getting all messages from chat", e);
			}
		}

		public int? GetChat(int idUser, int idOpponent)
		{
			logger.Info("BLL: Getting chat with user process started");

			try
			{
				int? temp = daoMessages.GetChat(idUser, idOpponent);

				logger.Info("BLL: Getting chat with user process done");
				return temp;

			}
			catch (SqlException e)
			{
				logger.Error("BLL: Getting chat with user process failed!");
				throw new Exception("error while getting chat with user", e);
			}
		}

		public void RemoveMessage(int idMessage)
		{
			logger.Info("BLL: Message removing process started");

			try
			{
				daoMessages.RemoveMessage(idMessage);
				logger.Info("BLL: Message removing process done");
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Message removing process failed!");
				throw new Exception("error while process of message removing", e);
			}
		}

		public Message SendMessage(int chatId, int senderId, string text, DateTime date)
        {
			logger.Info("BLL: Message sendding process started");

			try
			{
				Message message = new Message(chatId, senderId, text, date);
				message = daoMessages.SendMessage(message);

				logger.Info("BLL: Message sendding process done");
				return message;
			}
			catch (SqlException e)
			{
				logger.Error("BLL: Message sendding process failed!");
				throw new Exception("error while process of message sending", e);
			}
        }
    }
}
