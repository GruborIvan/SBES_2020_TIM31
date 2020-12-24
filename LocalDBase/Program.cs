using Client;
using Common;
using CryptoProject;
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
            NetTcpBinding bindingClient = new NetTcpBinding();
            string addressSelf = "net.tcp://localhost:8888/wcfserver";

            ServiceHost host = new ServiceHost(typeof(WCFServer));
            host.AddServiceEndpoint(typeof(IDatabaseService), bindingClient, addressSelf);

            host.Open();

            Console.WriteLine("WCFService is opened. Press <enter> to finish...");
            Console.ReadLine();

            NetTcpBinding bindingServer = new NetTcpBinding();
            string addressServer = "net.tcp://localhost:9999/wcfserver";

            CallbackClient callbackclient = new CallbackClient();
            WCFClient proxy = new WCFClient(callbackclient, bindingServer, new EndpointAddress(new Uri(addressServer)));
            callbackclient.Proxy = proxy;
        }
    }
}
