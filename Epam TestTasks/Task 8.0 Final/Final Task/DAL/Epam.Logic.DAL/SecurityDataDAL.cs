using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;

namespace Epam.Logic.DAL
{
	public class SecurityDataDAL : ISecurityDataDAL
	{
		private readonly ILogger logger;
		public SecurityDataDAL(ILogger logger)
		{
			this.logger = logger;
		}
	}
}
