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
    public class MessagesBLL : IMessagesBLL
    {
        private readonly ILogger logger;
        private readonly IMessagesDAL daoMessages;

        public MessagesBLL(ILogger logger, IMessagesDAL daoMessages)
        {
            this.logger = logger;
            this.daoMessages = daoMessages;
        }
    }
}
