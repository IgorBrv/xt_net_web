using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.Logic.BLL;
using Epam.Interfaces.BLL;
using Epam.Logic.DAL;
using Epam.Interfaces.DAL;
using Epam.CommonEntities;
using System.Runtime.CompilerServices;
using Epam.ASPNet.PL;

namespace Epam.DependencyResolver
{
	public class Resolver
	{
		private readonly IUsersDAL daoUsers;
		private readonly IFriendsDAL daoFriends;
		private readonly IMessagesDAL daoMessages;
		private readonly ISecurityDataDAL daoSecurityData;


		public Resolver()
		{
			GetLogger = new CommonLogger();
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
