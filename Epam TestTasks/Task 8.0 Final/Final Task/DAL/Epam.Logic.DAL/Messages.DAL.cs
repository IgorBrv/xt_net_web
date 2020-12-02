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
						bool? unreaded;

						SqlBoolean temp = reader.GetSqlBoolean(reader.GetOrdinal("unreaded"));
						unreaded = temp.IsNull ? (bool?)null : temp.Value;

						int id = (int)reader["idChat"];
						bool abandoned = (bool)reader["userLeft"];
						string emblem = reader["chatEmblem"] as string;
						string lastmessage = reader["mtext"] as string;
						string chatTitle = reader["chatName"] as string;
						DateTime messageDate = (DateTime)reader["mdate"];
						List<ChatMember> members = new List<ChatMember>();
						string messageSenderName = reader["sname"] as string;

						foreach (string member in (reader["members"] as string).Split(';'))
						{
							bool hasLeavedChat = false;
							bool hasAdminRights = false;
							string[] details = member.Split(',');

							if (details[4] as string == "1")
							{
								hasLeavedChat = true;
							}

							if (details[3] as string == "1")
							{
								hasAdminRights = true;
							}

							members.Add(new ChatMember(id, Int32.Parse(details[0]), details[1], details[2], hasLeavedChat, hasAdminRights));
						}

						chatsList.Add(new Chat(id, abandoned, members, emblem, lastmessage, messageSenderName, messageDate, unreaded, chatTitle));
					}
				}

				logger.Info("DAL: Getting list of users chats process done");

				return chatsList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting list of users chats process failed!");
				throw new StorageException(e.Message, e);
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
						bool? userLeft;
						bool? userJoined;
						string text = reader["mtext"] as string;

						SqlBoolean temp = reader.GetSqlBoolean(reader.GetOrdinal("userLeft"));
						userLeft = temp.IsNull ? (bool?)null : temp.Value;
						temp = reader.GetSqlBoolean(reader.GetOrdinal("userJoined"));
						userJoined = temp.IsNull ? (bool?)null : temp.Value;

						messagesList.Add(new Message((int)reader["idChat"], (int)reader["idSender"], text, (DateTime)reader["mdate"], (int)reader["idMessage"], userLeft, userJoined));
					}
				}

				logger.Info("DAL: Getting chats list of messages process done");
				return messagesList;
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Getting chats list of messages process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting chats list of messages process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting chats list of messages process failed!");
				throw new StorageException(e.Message, e);
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
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Message removing process failed!");
				throw new StorageException(e.Message, e);
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
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Message sendding process failed!");
				throw new StorageException(e.Message, e);
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
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting chat with user process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting chat with user process failed!");
				throw new StorageException(e.Message, e);
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
						new SqlParameter("@idPerson", idUser),
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

					stProc = "AddUserToChat";

					command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					parameters = new SqlParameter[]
					{
						new SqlParameter("@idChat", idChat),
						new SqlParameter("@idPerson", idOpponent)
					};

					command.Parameters.AddRange(parameters);
					command.ExecuteNonQuery();

					logger.Info("DAL: Chat creating  process done");
					return idChat;
				}
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Chat creating  process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Chat creating  process failed!");
				throw new StorageException(e.Message, e);
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

					SqlParameter parameter = new SqlParameter("@idChat", idChat);

					command.Parameters.Add(parameter);
					connection.Open();
					command.ExecuteNonQuery();
				}

				logger.Info("DAL: Chat removing process done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Chat removing process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Chat removing process failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public void LeaveChat(int idChat, int idUser)
		{
			logger.Info("DAL: Chat leaving process started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "LeaveChat";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@idChat", idChat),
						new SqlParameter("@idUser", idUser)
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					command.ExecuteNonQuery();
				}

				logger.Info("DAL: Chat leaving process done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Chat leaving process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Chat removing process failed!");
				throw new StorageException(e.Message, e);
			}
		}

		public void ReturnToChat(int idChat, int idUser)
		{
			logger.Info("DAL: Returning to chat procedure started");

			try
			{
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					var stProc = "ReturnToChat";

					var command = new SqlCommand(stProc, connection)
					{
						CommandType = CommandType.StoredProcedure
					};

					SqlParameter[] parameters = new SqlParameter[]
					{
						new SqlParameter("@idChat", idChat),
						new SqlParameter("@idUser", idUser)
					};

					command.Parameters.AddRange(parameters);
					connection.Open();
					command.ExecuteNonQuery();
				}

				logger.Info("DAL: Returning to chat procedure done");
			}
			catch (SqlException e)
			{
				logger.Error("DAL: Returning to chat procedure failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Returning to chat procedure failed!");
				throw new StorageException(e.Message, e);
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
				throw new StorageException(e.Message, e);
			}
			catch (IndexOutOfRangeException e)
			{
				logger.Error("DAL: Getting unreaded chats count with user process failed!");
				throw new StorageException(e.Message, e);
			}
			catch (Exception e)
			{
				logger.Error("DAL: Getting unreaded chats count with user process failed!");
				throw new StorageException(e.Message, e);
			}
		}
	}
}
