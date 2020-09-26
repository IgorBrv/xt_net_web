using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epam.CommonEntities
{
	public class Chat
	{
		public int? id;
		public string opponentName;
		public string lastMessage;
		public DateTime date;

		public Chat(int? id, string opponentName, string lastMessage, DateTime date)
		{
			this.id = id;
			this.opponentName = opponentName;
			this.lastMessage = lastMessage;
			this.date = date;
		}
	}
}
