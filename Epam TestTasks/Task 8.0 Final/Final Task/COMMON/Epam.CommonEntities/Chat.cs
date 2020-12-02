using System;
using System.Collections.Generic;

namespace Epam.CommonEntities
{
	public class Chat
	{	// Объект чата, включает в себя id чата, имя отправителя последнего сообщения, последнее сообщение с его датой, эмблему оппонента

		public int id;

		public string title;
		public string emblem;
		public bool? unreaded;
		public bool abandoned;
		public string lastMessage;
		public DateTime lastMessageDate;
		public string messageSenderName;
		public List<ChatMember> members;

		public Chat(int id, bool abandoned, List<ChatMember> members, string emblem, string lastMessage, string messageSenderName, DateTime lastMessageDate, bool? unreaded, string title = null)
		{
			this.id = id;
			this.title = title;
			this.emblem = emblem;
			this.members = members;
			this.unreaded = unreaded;
			this.abandoned = abandoned;
			this.lastMessage = lastMessage;
			this.lastMessageDate = lastMessageDate;
			this.messageSenderName = messageSenderName;
		}
	}
}
