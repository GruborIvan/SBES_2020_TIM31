using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace CryptoProject
{
    public class WCFBackupClient : ChannelFactory<IBackupServer>, IBackupServer, IDisposable
    {
        IBackupServer factory;
        Encryption enc = new Encryption();
        public WCFBackupClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void Dispose()
        {

            if (factory != null)
            {
                factory = null;
            }
            this.Close();
        }

        public bool sendChanges(List<Tuple<OperationCode, LogEntity>> promena)
        {
            return factory.sendChanges(null);
        }
    }
}
