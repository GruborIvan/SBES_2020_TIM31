using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class WCFClient : ChannelFactory<IDatabaseService>, IDatabaseService, IDisposable
    {
        IDatabaseService factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address) {
            factory = this.CreateChannel();
        }

        public void Dispose() {
            if (factory != null) {
                factory = null;
            }

            this.Close();
        }

        public void testservermessage(string message) {
            factory.testservermessage(message);
        }
    }
}
