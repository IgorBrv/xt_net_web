namespace Epam.CommonEntities
{
	public class ChatMember
	{   // Объект участника чата, включает в себя id участника, аватар, флаг о покидании чата и о правах администратора чата

		public int idChat;
		public string name;
		public int idMember;
		public string emblem;
		public bool hasLeavedChat;
		public bool hasAdminRights;

		public ChatMember(int idChat, int idMember, string name, string emblem, bool hasLeavedChat, bool hasAdminRights)
		{
			this.idChat = idChat;
			this.idMember = idMember;
			this.name = name;
			this.emblem = emblem;
			this.hasLeavedChat = hasLeavedChat;
			this.hasAdminRights = hasAdminRights;
		}
	}
}