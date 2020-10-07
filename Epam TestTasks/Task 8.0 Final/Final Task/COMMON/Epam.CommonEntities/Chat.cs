using System;

namespace Epam.CommonEntities
{
	public class Chat
	{	// Объект чата, включает в себя id чата, имя отправителя последнего сообщения, последнее сообщение с его датой, эмблему оппонента

		public int? id;
		public int? unreaded;
		public string emblem;
		public DateTime date;
		public string lastMessage;
		public string opponentName;

		public Chat(int? id, string opponentName, string lastMessage, DateTime date, int? unreaded, string emblem = null)
		{
			this.id = id;
			this.date = date;
			this.emblem = emblem;
			this.unreaded = unreaded;
			this.lastMessage = lastMessage;
			this.opponentName = opponentName;
		}
	}
}
