using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;
using Epam.CommonLoggerInterface;

namespace Epam.Logic.DAL
{
    public class MessagesDAL : IMessagesDAL
    {
        private readonly ILogger logger;
		private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;

		public MessagesDAL(ILogger logger)
        {
            this.logger = logger;
        }

		public IEnumerable<Chat> GetAllChatsOfUser(int idUser)
		{
			List<Chat> chatsList = new List<Chat>();
			logger.Info("DAL: Getting list of users chats process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllChatsOfUser";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", idUser);
					command.Parameters.Add(idParam);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						chatsList.Add(new Chat((int)reader["idChat"], reader["name"] as string, reader["text"] as string, (DateTime)reader["date"]));
					}
				}

				logger.Info("DAL: Getting list of users chats process done");

				return chatsList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw e;
			};
		}

		public IEnumerable<Message> GetAllMessagesFromChat(int idChat)
		{
			List<Message> messagesList = new List<Message>();
			logger.Info("DAL: Getting chats list of messages process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetAllMessagesFromChat";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@id", idChat);
					command.Parameters.Add(idParam);
					connection.Open();
					var reader = command.ExecuteReader();

					while (reader.Read())
					{
						messagesList.Add(new Message((int)reader["idChat"], (int)reader["idSender"], reader["text"] as string, (DateTime)reader["date"], (int)reader["idMessage"]));
					}
				}

				logger.Info("DAL: Getting chats list of messages process done");
				return messagesList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting chats list of messages process failed!");
				throw e;
			};
		}

		public void RemoveMessage(int idMessage)
		{
			logger.Info("DAL: Message removing process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "RemoveMessage";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter idParam = new SqlParameter("@idMessage", idMessage);
					command.Parameters.Add(idParam);
					connection.Open();
					command.ExecuteScalar();
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Message removing process failed!");
				throw e;
			};

			logger.Info("DAL: Message removing process done");
		}

		public Message SendMessage(Message message)
		{
			logger.Info("DAL: Message sendding process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "SendMessage";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@chatId", message.chatId),
						new SqlParameter("@senderId", message.senderId),
						new SqlParameter("@text", message.text),
						new SqlParameter("@date", message.date)
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					message.messageId = (int)command.ExecuteScalar();
				}

				logger.Info("DAL: Message sendding process done");
				return message;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Message sendding process failed!");
				throw e;
			};
		}

		public int? GetChat(int idUser, int idOpponent)
		{
			logger.Info("DAL: Getting chat with user process started");
			int? chatNum = null;

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetChatId";

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
					chatNum = (int?)command.ExecuteScalar();
				}

				logger.Info("DAL: Getting chat with user process done");
				return chatNum;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting chat with user process failed!");
				throw e;
			};
		}
	}
}
