using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    public class proxyCaller
    {
        // MOJSI PROKSI <3 !

        public static IDatabaseService proxy = null;

        public IDatabaseService ProxySetter {

            get { return proxy; }
            set { proxy = value;}
        }

        public int openNewEndpoint()
        {
            int port = 8888;
            while (true)
            {
                try
                {
                    NetTcpBinding bindingClient = new NetTcpBinding();
                    string addressSelf = "net.tcp://localhost:" + port.ToString() + "/localdb";

                    ServiceHost host = new ServiceHost(typeof(WCFLocalDBService));
                    host.AddServiceEndpoint(typeof(IDatabaseService), bindingClient, addressSelf);
                    host.Open();
                    break;
                }
                catch (Exception e)
                {
                    port++;
                }
            }
            return port;
        }
    }
}
