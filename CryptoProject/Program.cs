using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject
{
    class Program
    {

        static void Main(string[] args) {

			NetTcpBinding binding = new NetTcpBinding();
			string address = "net.tcp://localhost:9999/wcfserver";

			ServiceHost host = new ServiceHost(typeof(WCFServer));
			host.AddServiceEndpoint(typeof(IDatabaseService), binding, address);

			host.Open();
			Console.WriteLine("WCFService is opened. Press <enter> to finish...");
			Console.ReadLine();

            LogEntitet le = new LogEntitet("1", Region.BACKA, "Senta", 1231);
            LogEntitet le1 = new LogEntitet("2", Region.BANAT, "Kikinda", 1241);
            LogEntitet le2 = new LogEntitet("3", Region.BANAT, "Kikinda", 1241);
            XmlHandler xh = new XmlHandler();

            xh.AddEntity(le);
            xh.AddEntity(le1);
            xh.AddEntity(le2);


            le.Potrosnja[0] = 200;
            le.Potrosnja[1] = 300;
            le.Potrosnja[2] = 400;
            le.Potrosnja[3] = 500;
            le.Potrosnja[4] = 600;
            le.Potrosnja[5] = 700;
            le.Potrosnja[6] = 800;
            le.Potrosnja[7] = 900;

            Console.WriteLine("potrosnja: "+le.Potrosnja[1]);
            xh.UpdateEntity(le);

            try
            {
               // xh.DeleteEntity("1");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            le2.Region = Region.BEOGRAD;
            le2.Grad = "SMECE ODVRATNO";
            xh.UpdateEntity(le2);

            Console.ReadLine();
            Console.ReadLine();

            Console.ReadLine();

        }
    }
}
