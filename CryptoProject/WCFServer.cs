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

        public enum CallbackOperation { ADD, UPDATE,DELETE };

        public string AddLogEntity(LogEntity entitet) {


            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.Contains(callback) == false)
            {
                klijenti.Add(callback);
            }
            List<LogEntity> list = xh.ReturnList();
            if (list.Find(x => x.Grad.ToLower() == entitet.Grad.ToLower() && x.Godina == entitet.Godina) != null) {
                return null;
            }

            string id = xh.AddEntity(entitet);
            entitet.Id = id;
            broadcastIdMessage(id, CallbackOperation.ADD);

            Changes.ChangeList.Add(new Tuple<OperationCode, LogEntity>(OperationCode.ADD, entitet));
            return id;
        }

        public float GetAverageConsumptionForCity(string grad) {
            /////////////////////////////////////////get() klijenta koji trazi pristup
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ////////////////////////////////////////
            float ret = 0, cons = 0;
            int n = 0, i = 0;

            foreach (LogEntity item in xh.ReturnList()) {/////////////////////////////////////////ako klijentu nije dostupna dotican item -> continue
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

        public bool DeleteLogEntity(string id) {//////////////////////////////////////Ako klijentu nije dostupan doticni item -> return
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            //if (!klijenti[id].Equals(client))
            //    return false;
            ///////////////////////////////////////
            bool deletion = false;

            try {
                LogEntity le = xh.ReturnList().Find(x => x.Id.Equals(id));
                deletion = xh.DeleteEntity(id);

                broadcastIdMessage(id, CallbackOperation.DELETE);
                Changes.ChangeList.Add(new Tuple<OperationCode, LogEntity>(OperationCode.DELETE, le));
                return deletion;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }

        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni) {
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////

            List<LogEntity> ret = new List<LogEntity>();

            foreach (var item in regioni) {

                foreach (var predmet in xh.ReturnList()) {

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

        public float GetAverageConsumptionForRegion(Region reg) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;
            ///////////////////////////////////////

            float ret = 0, cons = 0;
            int n = 0, i = 0;
            List<int> godine = new List<int>();

            foreach (LogEntity item in xh.ReturnList()) {
                ///////////////////////////////////////
                //if (!klijenti[item.Id].Equals(client)) //Problem ovde!
                //    continue;
                //////////////////////////////////////
                if (item.Region.Equals(reg)) {

                    foreach (float f in item.Potrosnja) {
                        cons += item.Potrosnja[i];
                        i++;
                    }

                    if (!godine.Contains(item.Godina)) {
                        godine.Add(item.Godina);
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
            Console.WriteLine(message);
            Console.WriteLine("test");
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption) {
            //////////////////////////////////////
            IIdentity client = ServiceSecurityContext.Current.PrimaryIdentity;

            LogEntity le = new LogEntity();
            foreach (var element in xh.ReturnList()) {

                if (element.Id.Equals(id)) {
                    le = element;
                    break;
                }

            }
            le.Potrosnja[month] = consumption;
            xh.UpdateEntity(le);

            broadcastIdMessage(le.Id, CallbackOperation.UPDATE);
            Changes.ChangeList.Add(new Tuple<OperationCode, LogEntity>(OperationCode.UPDATE , xh.ReturnList().Find(x => x.Id.Equals(id))));
            return le;
        }

        void broadcastIdMessage(string id, CallbackOperation op) {

            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (IDatabaseCallback client in klijenti) {

                try 
                {
                    switch(op)
                    {
                        case CallbackOperation.ADD:
                                        LogEntity entity = GetLogEntityById(id);
                                        client.broadcastAddLogEntity(entity.Region,entity.Id);
                                        break;

                        case CallbackOperation.UPDATE:
                                        client.broadcastUpdateId(id);
                                        break;
                        case CallbackOperation.DELETE:
                                        client.broadcastDeleteId(id);
                                        break;
                    }

                }
                catch (Exception ex) {
                    Console.WriteLine("{0}", ex.Message);
                    deleteitems.Add(client);
                }

            }

            // Brisanje ugasenih klijenata..
            foreach (IDatabaseCallback client in deleteitems) {
                klijenti.Remove(client);
            }

        }

        public LogEntity GetLogEntityById(string id) {
            List<LogEntity> entiteti = xh.ReturnList();

            LogEntity updateVal = entiteti.Find(x => x.Id == id);

            return updateVal;
        }

        public List<LogEntity> GetEntitiesForRegionsString(string regioni)
        {
            throw new NotImplementedException();
        }

        public string GetAverageConsumptionForCityRetStr(string city)
        {
            throw new NotImplementedException();
        }

        public string GetAverageConsumptionForRegionRetStr(string reg)
        {
            throw new NotImplementedException();
        }
    }
}
