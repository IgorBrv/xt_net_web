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

namespace Epam.DependencyResolver
{
	public class Resolver
	{
		private readonly IUsersDAL daoUsers;
		private readonly IFriendsDAL daoFriends;
		private readonly IMessagesDAL daoMessages;
		private readonly ISecurityDataDAL daoSecurityData;


		public Resolver(ILogger logger)
		{
			daoUsers = new UsersDAL(logger);
			daoFriends = new FriendsDAL(logger);
			daoMessages = new MessagesDAL(logger);
			daoSecurityData = new SecurityDataDAL(logger);
			GetBloUsers = new UsersBLL(logger, daoUsers);
			GetBloFriends = new FriendsBLL(logger, daoFriends);
			GetBloMessages = new MessagesBLL(logger, daoMessages);
			GetBloSecurityData = new SecurityDataBLL(logger, daoSecurityData);
		}

		public IUsersBLL GetBloUsers { get; }
		public IFriendsBLL GetBloFriends { get; }
		public IMessagesBLL GetBloMessages { get; }
		public ISecurityDataBLL GetBloSecurityData { get; }
	}
}
