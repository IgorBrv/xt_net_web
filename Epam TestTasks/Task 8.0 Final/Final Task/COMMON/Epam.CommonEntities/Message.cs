using System;

namespace Epam.CommonEntities
{	// Объект сообщения, включает в себя id чата, текст сообщения, id отправителя, дату и id сообщения
	public class Message
	{
		public int chatId;
		public string text;
		public int senderId;
		public DateTime date;
		public int? messageId;
		public bool? userLeft;
		public bool? userJoined;


		public Message(int chatId, int senderId, string text, DateTime date, int? messageId = null, bool? userLeft = null, bool? userJoined = null)
		{
			this.text = text;
			this.date = date;
			this.chatId = chatId;
			this.senderId = senderId;
			this.messageId = messageId;
			this.userLeft = userLeft;
			this.userJoined = userJoined;
		}
	}
}
