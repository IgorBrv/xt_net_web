using Epam.Logic.BLL;
using Epam.Interfaces.BLL;
using Epam.Logic.DAL;
using Epam.Interfaces.DAL;
using Epam.CommonLogger;
using Epam.CommonLoggerInterface;

namespace Epam.DependencyResolver
{	// Распределитель зависимостей

	public class Resolver
	{
		private readonly IUsersDAL daoUsers;
		private readonly IFriendsDAL daoFriends;
		private readonly IMessagesDAL daoMessages;
		private readonly ISecurityDataDAL daoSecurityData;

		public Resolver()
		{
			GetLogger = new ComLogger();
			daoUsers = new UsersDAL(GetLogger);
			daoFriends = new FriendsDAL(GetLogger);
			daoMessages = new MessagesDAL(GetLogger);
			daoSecurityData = new SecurityDataDAL(GetLogger);
			GetBloUsers = new UsersBLL(GetLogger, daoUsers);
			GetBloFriends = new FriendsBLL(GetLogger, daoFriends);
			GetBloMessages = new MessagesBLL(GetLogger, daoMessages);
			GetBloSecurityData = new SecurityDataBLL(GetLogger, daoSecurityData);
		}

		public ILogger GetLogger { get; }
		public IUsersBLL GetBloUsers { get; }
		public IFriendsBLL GetBloFriends { get; }
		public IMessagesBLL GetBloMessages { get; }
		public ISecurityDataBLL GetBloSecurityData { get; }
	}
}
