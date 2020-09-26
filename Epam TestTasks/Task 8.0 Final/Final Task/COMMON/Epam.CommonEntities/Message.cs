using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.CommonEntities
{
	public class Message
	{
		public int chatId;
		public string text;
		public int senderId;
		public DateTime date;
		public int? messageId;

		public Message(int chatId, int senderId, string text, DateTime date, int? messageId = null)
		{
			this.text = text;
			this.date = date;
			this.chatId = chatId;
			this.senderId = senderId;
			this.messageId = messageId;
		}
	}
}
