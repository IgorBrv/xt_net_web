using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.CommonEntities;
using Epam.Interfaces.BLL;
using Epam.Interfaces.DAL;

namespace Epam.Logic.BLL
{
    public class FriendsBLL : IFriendsBLL
    {
        private readonly ILogger logger;
        private readonly IFriendsDAL daoFriends;

        public FriendsBLL(ILogger logger, IFriendsDAL daoFriends)
		{
            this.logger = logger;
            this.daoFriends = daoFriends;
		}
    }
}
