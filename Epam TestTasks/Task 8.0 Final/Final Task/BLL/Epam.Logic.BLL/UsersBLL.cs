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
	public class UsersBLL : IUsersBLL
	{
		private readonly ILogger logger;
		private readonly IUsersDAL daoUsers;

		public UsersBLL(ILogger logger, IUsersDAL daoUsers)
		{
			this.logger = logger;
			this.daoUsers = daoUsers;
		}
	}
}
