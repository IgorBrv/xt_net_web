using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLLInterface;
using BLLCore;


namespace BLLConnector
{
    public static class BLLGetter
    {
        private static IBll _bll;

        public static IBll GetBll() => _bll ?? (_bll = new BLLMain());
    }
}
