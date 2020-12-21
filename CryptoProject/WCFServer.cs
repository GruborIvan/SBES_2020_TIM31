using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CryptoProject
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class WCFServer : IDatabaseService
    {
        XmlHandler xh = new XmlHandler();
        //private static Dictionary<string, IIdentity> klijenti = new Dictionary<string, IIdentity>();
        public static List<IDatabaseCallback> klijenti = new List<IDatabaseCallback>();

        public string addLogEntity(LogEntitet entitet) {

            //dictionary za klijent id vezu 
            // IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            // if (client == null)
            //    return null;
            //klijenti.Add(entitet.Id, client);
            /////////////////////////////////
            ///

            List<LogEntitet> list = xh.ReturnList();
            if (list.Find(x => x.Grad.ToLower() == entitet.Grad.ToLower() && x.Year == entitet.Year) != null) {

                return null;
            }

            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.Contains(callback) == false) {
                klijenti.Add(callback);
            }

            return xh.AddEntity(entitet);
        }

        public float cityAverageConsumption(string grad) {
            /////////////////////////////////////////get() klijenta koji trazi pristup
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ////////////////////////////////////////
            float ret = 0, cons = 0;
            int n = 0, i = 0;

            foreach (LogEntitet item in xh.ReturnList()) {/////////////////////////////////////////ako klijentu nije dostupna dotican item -> continue
                                                          //   if (!klijenti[item.Id].Equals(client))
                                                          //       continue;
                                                          /////////////////////////////////////////
                if (item.Grad.Equals(grad)) {
                    foreach (float f in item.Potrosnja) {
                        cons += item.Potrosnja[i];
                        i++;
                    }
                    ret += cons;
                    cons = 0;
                    i = 0;
                    n++;
                }
            }
            return (ret / n); //Ne!
        }

        public bool deleteLogEntity(string id) {//////////////////////////////////////Ako klijentu nije dostupan doticni item -> return
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            //if (!klijenti[id].Equals(client))
            //    return false;
            ///////////////////////////////////////
            bool deletion = false;

            try {
                deletion = xh.DeleteEntity(id);
                broadcastIdMessage(id, 1);
                return deletion;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }

        }

        public List<LogEntitet> readEntities(List<Region> regioni) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////

            List<LogEntitet> ret = new List<LogEntitet>();

            foreach (var item in regioni) {

                foreach (var predmet in xh.ReturnList()) {////////////////////////////////////////////
                                                          //if (!klijenti[predmet.Id].Equals(client))
                                                          //    continue;
                                                          ////////////////////////////////////////////
                    if (item.Equals(predmet.Region)) {
                        ret.Add(predmet);
                    }
                }

            }

            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.Contains(callback) == false) {
                klijenti.Add(callback);
            }

            return ret;
        }

        public float regionAverageConsumption(Region reg) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////

            float ret = 0, cons = 0;
            int n = 0, i = 0;
            List<int> godine = new List<int>();

            foreach (LogEntitet item in xh.ReturnList()) {
                ///////////////////////////////////////
                //if (!klijenti[item.Id].Equals(client)) //Problem ovde!
                //    continue;
                //////////////////////////////////////
                if (item.Region.Equals(reg)) {

                    foreach (float f in item.Potrosnja) {
                        cons += item.Potrosnja[i];
                        i++;
                    }

                    if (!godine.Contains(item.Year)) {
                        godine.Add(item.Year);
                        n++;
                    }
                    ret += cons;
                    cons = 0;
                    i = 0;

                }
            }
            return (ret / n);
        }

        public void testServerMessage(string message) {
            throw new NotImplementedException();
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////

            LogEntitet le = new LogEntitet();
            foreach (var element in xh.ReturnList()) {
                ///////////////////////////////////////
                // if (!klijenti[element.Id].Equals(client))
                //     continue;
                //////////////////////////////////////

                if (element.Id.Equals(id)) {
                    le = element;
                    break;
                }
            }
            le.Potrosnja[month] = consumption;
            xh.UpdateEntity(le);

            broadcastIdMessage(le.Id, 0);

            return le;

        }

        void broadcastIdMessage(string id, int type) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (IDatabaseCallback client in klijenti) {
                try {
                    if (type == 0) {
                        client.broadcastUpdateId(id);
                    }
                    else {
                        client.broadcastDeleteId(id);
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(client);
                }
            }

            foreach (IDatabaseCallback client in deleteitems) {

                klijenti.Remove(client);
            }

        }

        public LogEntitet getUpdatedEntity(string id) {
            List<LogEntitet> entiteti = xh.ReturnList();

            LogEntitet updateVal = entiteti.Find(x => x.Id == id);

            return updateVal;
        }

    }
}
