using System.Collections.Generic;
using Epam.CommonLoggerInterface;
using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;
using System;


namespace Epam.Logic.BLL
{
    public class MessagesBLL : IMessagesBLL
	{   // BLL Messages, отвечает за работу с чатами и сообщениями пользователя. Позволяет создать чат, удалить чат, отправить сообщение, удалить сообщение, получить список чатов и сообщений

		private readonly ILogger logger;
        private readonly IMessagesDAL daoMessages;

        public MessagesBLL(ILogger logger, IMessagesDAL daoMessages)
        {
            this.logger = logger;
            this.daoMessages = daoMessages;
        }

		public int CreateChat(int idUser, int idOpponent)
		{
			logger.Info("BLL: Chat creating process started");

			try
			{
				int temp = daoMessages.CreateChat(idUser, idOpponent);

				logger.Info("BLL: Chat creating  process done");
				return temp;
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Chat creating  process failed!");
				throw new Exception("error while creating chat process", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Chat creating  process failed!");
				throw new Exception("error while creating chat process", e);
			}
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
			catch (StorageException e)
			{
				logger.Error("BLL: Getting list of users chats process failed!");
				throw new Exception("error while getting list of users chats", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Getting list of users chats process failed!");
				throw new Exception("error while getting list of users chats", e);
			}
		}

		public IEnumerable<Message> GetAllMessagesFromChat(int idChat, int idReader)
		{
			logger.Info("BLL: Getting chats list of messages process started");

			try
			{
				IEnumerable<Message> temp = daoMessages.GetAllMessagesFromChat(idChat, idReader);

				logger.Info("BLL: Getting chats list of messages process done");
				return temp;
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Getting chats list of messages process failed!");
				throw new Exception("error while getting all messages from chat", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Getting chat with user process failed!");
				throw new Exception("error while getting chat with user", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Getting chat with user process failed!");
				throw new Exception("error while getting chat with user", e);
			}
		}

		public int GetUnreadedCount(int idUser)
		{
			logger.Info("BLL: Getting unreaded chats count with user process started");

			try
			{
				int temp = daoMessages.GetUnreadedCount(idUser);

				logger.Info("BLL: Getting unreaded chats count with user process done");
				return temp;
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Getting unreaded chats count with user process failed!");
				throw new Exception("error while getting count of all unreaded chats of user", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Getting unreaded chats count with user process failed!");
				throw new Exception("error while getting count of all unreaded chats of user", e);
			}
		}

		public void RemoveChat(int idChat)
		{
			logger.Info("BLL: Chat removing process started");

			try
			{
				daoMessages.RemoveChat(idChat);
				logger.Info("BLL: Chat removing process done");
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Chat removing process failed!");
				throw new Exception("error while process of chat removing", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Chat removing process failed!");
				throw new Exception("error while process of chat removing", e);
			}
		}

		public void LeaveChat(int idChat, int idUser)
		{
			logger.Info("BLL: Chat leaving process started");

			try
			{
				daoMessages.LeaveChat(idChat, idUser);
				logger.Info("BLL: Chat removing process done");
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Chat leaving process failed!");
				throw new Exception("error while process of chat leaving", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Chat leaving process failed!");
				throw new Exception("error while process of chat leaving", e);
			}
		}

		public void ReturnToChat(int idChat, int idUser)
		{
			logger.Info("BLL: Returning to chat procedure started");

			try
			{
				daoMessages.ReturnToChat(idChat, idUser);
				logger.Info("BLL: Returning to chat procedure done");
			}
			catch (StorageException e)
			{
				logger.Error("BLL: Returning to chat procedure failed!");
				throw new Exception("error while process of returning to chat", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Returning to chat procedure failed!");
				throw new Exception("error while process of returning to chat", e);
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
			catch (StorageException e)
			{
				logger.Error("BLL: Message removing process failed!");
				throw new Exception("error while process of message removing", e);
			}
			catch (Exception e)
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
			catch (StorageException e)
			{
				logger.Error("BLL: Message sendding process failed!");
				throw new Exception("error while process of message sending", e);
			}
			catch (Exception e)
			{
				logger.Error("BLL: Message sendding process failed!");
				throw new Exception("error while process of message sending", e);
			}
		}
    }
}
