using System.Collections.Generic;
using Epam.CommonLoggerInterface;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;
using System.Data;
using System;

namespace Epam.Logic.DAL
{
    public class MessagesDAL : IMessagesDAL
    {	// DAL Messages, отвечает за работу с чатами и сообщениями пользователя. Позволяет создать чат, удалить чат, отправить сообщение, удалить сообщение, получить список чатов и сообщений

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
						int? unreaded;
						SqlInt32 temp = reader.GetSqlInt32(reader.GetOrdinal("unreaded"));
						unreaded = temp.IsNull ? (int?)null : temp.Value;
						string text = $"{reader["sname"] as string}: {reader["text"] as string}";
						chatsList.Add(new Chat((int)reader["idChat"], reader["name"] as string, text, (DateTime)reader["date"], unreaded, reader["emblem"] as string));
					}
				}

				logger.Info("DAL: Getting list of users chats process done");

				return chatsList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw e;
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw e;
			}

		}

		public IEnumerable<Message> GetAllMessagesFromChat(int idChat, int idReader)
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

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@id", idChat),
						new SqlParameter("@idReader", idReader),

					};

					command.Parameters.AddRange(parameters);
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
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting chats list of messages process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting chats list of messages process failed!");
				throw e;
			}
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

				logger.Info("DAL: Message removing process done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Message removing process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Message removing process failed!");
				throw e;
			}
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

					SqlParameter output = new SqlParameter
					{   // выходной параметр
						ParameterName = "@id",
						SqlDbType = SqlDbType.Int,
						Direction = ParameterDirection.Output 
					};

					command.Parameters.AddRange(parameters);
					command.Parameters.Add(output);
					connection.Open();
					command.ExecuteNonQuery();
					message.messageId = (int?)output.Value;
				}

				logger.Info("DAL: Message sendding process done");
				return message;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Message sendding process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Message sendding process failed!");
				throw e;
			}
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
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting chat with user process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting chat with user process failed!");
				throw e;
			}
		}

		public int CreateChat(int idUser, int idOpponent)
		{
			logger.Info("DAL: Chat creating process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "CreateChat";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@idPerson1", idUser),
						new SqlParameter("@idPerson2", idOpponent)
					};

					SqlParameter output = new SqlParameter
					{   // выходной параметр
						ParameterName = "@id",
						SqlDbType = SqlDbType.Int,
						Direction = ParameterDirection.Output
					};

					command.Parameters.AddRange(parameters);
					command.Parameters.Add(output);
					connection.Open();
					command.ExecuteNonQuery();

					int idChat = (int)output.Value;

					logger.Info("DAL: Chat creating  process done");
					return idChat;
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Chat creating  process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Chat creating  process failed!");
				throw e;
			}
		}

		public void RemoveChat(int idChat)
		{
			logger.Info("DAL: Chat removing process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "RemoveChat";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter parameter = new SqlParameter("@id", idChat);

					command.Parameters.Add(parameter);
					connection.Open();
					command.ExecuteNonQuery();
				}

				logger.Info("DAL: Chat removing process done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Chat removing process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Chat removing process failed!");
				throw e;
			}
		}

		public int GetUnreadedCount(int idUser)
		{
			logger.Info("DAL: Getting unreaded chats count with user process started");
			int count;

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "GetUnreadedChatsCount";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter parameter = new SqlParameter("@id", idUser);

					command.Parameters.Add(parameter);
					connection.Open();
					count = (int)command.ExecuteScalar();
				}

				logger.Info("DAL: Getting unreaded chats count with user process done");
				return count;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting unreaded chats count with user process failed!");
				throw e;
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting unreaded chats count with user process failed!");
				throw e;
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting unreaded chats count with user process failed!");
				throw e;
			}
		}
	}
}
