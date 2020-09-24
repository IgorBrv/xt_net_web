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
	public class SecurityDataBLL : ISecurityDataBLL
	{
		private readonly ILogger logger;
		private readonly ISecurityDataDAL daoSecurityData;

		public SecurityDataBLL(ILogger logger, ISecurityDataDAL daoSecurityData)
		{
			this.logger = logger;
			this.daoSecurityData = daoSecurityData;
		}
	}
}
