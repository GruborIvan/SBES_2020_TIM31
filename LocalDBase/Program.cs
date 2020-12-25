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

            NetTcpBinding bindingServer = new NetTcpBinding();
            string addressServer = "net.tcp://localhost:9999/wcfserver";

            CallbackClient callbackclient = new CallbackClient();
            WCFLocalDB proxy = new WCFLocalDB(callbackclient, bindingServer, new EndpointAddress(new Uri(addressServer)));
            callbackclient.Proxy = proxy;

            proxyCaller callerbase = new proxyCaller();
            callerbase.ProxySetter = proxy;

            ////////////////////////////

            int port = callerbase.openNewEndpoint();

            string baseName = "baza" + port.ToString() +".xml";

            XmlHandler xh = new XmlHandler(baseName);

            Console.WriteLine("WCFService is opened. Press <enter> to finish...");

            Console.ReadLine();
        }
        
    }
}
