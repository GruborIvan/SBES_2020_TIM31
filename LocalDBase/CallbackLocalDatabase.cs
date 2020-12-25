using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{

    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class CallbackLocalDatabase : IDatabaseCallback {

        XmlHandler handler = new XmlHandler();
        IDatabaseService proxy = null;

        public void broadcastAddLogEntity(Region region, string id) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();
          

            foreach (IDatabaseCallback client in WCFLocalDBService.klijenti) {

                try {

                    if (handler.ReturnList().Find(x => x.Id == id) == null) {
                        Console.WriteLine("Odradjen add za id: {0}.", id);
                        proxy = proxyCaller.proxy;
                        LogEntity entitet = proxy.GetLogEntityById(id);
                        handler.AddEntity(entitet);
                    }

                    client.broadcastAddLogEntity(region, id);
                }
                catch (Exception ex) {
                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(client);
                }

            }

            // Brisanje ugasenih klijenata..
            foreach (IDatabaseCallback client in deleteitems) {
                WCFLocalDBService.klijenti.Remove(client);
            }

        }

        public void broadcastDeleteId(string id) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (IDatabaseCallback client in WCFLocalDBService.klijenti) {

                try {

                    if (handler.ReturnList().Find(x => x.Id == id) != null) {
                        Console.WriteLine("Odradjen delete za id: {0}.\n", id);
                        handler.DeleteEntity(id);
                    }

                    client.broadcastDeleteId(id);
                }
                catch (Exception ex) {
                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(client);
                }

            }

            // Brisanje ugasenih klijenata..
            foreach (IDatabaseCallback client in deleteitems) {
                WCFLocalDBService.klijenti.Remove(client);
            }

        }

        public void broadcastUpdateId(string id) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (IDatabaseCallback client in WCFLocalDBService.klijenti) {

                try {

                    if (handler.ReturnList().Find(x => x.Id == id) != null) {
                        Console.WriteLine("Odradjen update za id: {0}.", id);
                        proxy = proxyCaller.proxy;
                        LogEntity entitet = proxy.GetLogEntityById(id);
                        handler.UpdateEntity(entitet);
                    }

                    client.broadcastUpdateId(id);
                }
                catch (Exception ex) {

                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(client);
                }
            }

            foreach (IDatabaseCallback klijent in deleteitems) {
                WCFLocalDBService.klijenti.Remove(klijent);
            }

        }
    
    }
}
