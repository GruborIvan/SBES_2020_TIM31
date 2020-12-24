using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    public class proxyCaller
    {

        public static IDatabaseService proxy = null;

        public IDatabaseService ProxySetter {

            get { return proxy; }
            set { proxy = value;}
        }

    }
}
