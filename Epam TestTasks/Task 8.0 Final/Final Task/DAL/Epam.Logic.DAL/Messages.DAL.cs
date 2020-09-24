using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epam.CommonEntities;
using Epam.Interfaces.DAL;

namespace Epam.Logic.DAL
{
    public class MessagesDAL : IMessagesDAL
    {
        private readonly ILogger logger;
        public MessagesDAL(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
