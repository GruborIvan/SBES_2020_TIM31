using LocalDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/wcfserver";

            CallbackClient callbackclient = new CallbackClient();
            WCFClient proxy = new WCFClient(callbackclient, binding, new EndpointAddress(new Uri(address)));
            callbackclient.Proxy = proxy;
        }
    }
}
