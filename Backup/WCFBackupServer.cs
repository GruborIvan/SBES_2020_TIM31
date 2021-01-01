using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Backup
{
    public class WCFBackupServer : IBackupServer
    {
        public bool sendChanges(List<Tuple<OperationCode, LogEntity>> promena)
        {
            Console.WriteLine("radi");
            return true;
        }
    }
}
