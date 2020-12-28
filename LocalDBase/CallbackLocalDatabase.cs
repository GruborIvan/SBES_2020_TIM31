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
          

            foreach (KeyValuePair<IDatabaseCallback, List<Region>> kwp in WCFLocalDBService.klijenti) {

                try {

                    if (kwp.Value.Contains(region)) {

                        Console.WriteLine("Odradjen add za id: {0}.", id);

                        if (handler.ReturnList().Find(x => x.Id == id) == null) {
                            proxy = proxyCaller.proxy;
                            LogEntity entitet = proxy.GetLogEntityById(id);
                            handler.AddEntity(entitet);
                        }

                        kwp.Key.broadcastAddLogEntity(region, id);
                    }

                }
                catch (Exception ex) {
                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(kwp.Key);
                }

            }

            // Brisanje ugasenih klijenata..
            foreach (IDatabaseCallback client in deleteitems) {
                WCFLocalDBService.klijenti.Remove(client);
            }

        }

        public void broadcastDeleteId(string id) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (KeyValuePair<IDatabaseCallback, List<Region>> kwp in WCFLocalDBService.klijenti) {

                try {

                    if (handler.ReturnList().Find(x => x.Id == id) != null) {
                        Console.WriteLine("Odradjen delete za id: {0}.\n", id);
                        handler.DeleteEntity(id);
                    }

                    kwp.Key.broadcastDeleteId(id);
                }
                catch (Exception ex) {
                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(kwp.Key);
                }

            }

            // Brisanje ugasenih klijenata..
            foreach (IDatabaseCallback client in deleteitems) {
                WCFLocalDBService.klijenti.Remove(client);
            }

        }

        public void broadcastUpdateId(string id) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (KeyValuePair<IDatabaseCallback, List<Region>> kwp in WCFLocalDBService.klijenti) {

                try {

                    if (handler.ReturnList().Find(x => x.Id == id) != null) {
                        Console.WriteLine("Odradjen update za id: {0}.", id);
                        proxy = proxyCaller.proxy;
                        LogEntity entitet = proxy.GetLogEntityById(id);
                        handler.UpdateEntity(entitet);
                    }

                    kwp.Key.broadcastUpdateId(id);
                }
                catch (Exception ex) {

                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(kwp.Key);
                }
            }

            foreach (IDatabaseCallback klijent in deleteitems) {
                WCFLocalDBService.klijenti.Remove(klijent);
            }

        }
    
    }
}
