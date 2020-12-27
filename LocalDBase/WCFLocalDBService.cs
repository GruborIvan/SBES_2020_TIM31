using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    class WCFLocalDBService : IDatabaseService
    {
        IDatabaseService proxy = null;
        public static Dictionary<IDatabaseCallback, List<Region>> klijenti = new Dictionary<IDatabaseCallback, List<Region>>();

        public WCFLocalDBService() {

            proxy = proxyCaller.proxy;

        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public string AddLogEntity(LogEntity entitet)
        {
            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.ContainsKey(callback) == false) {
                klijenti.Add(callback, new List<Region>());
            }

            LogEntity ent = Encryption.decryptLogEntity(entitet.Grad);

            return Convert.ToBase64String( Encryption.Encrypt(proxy.AddLogEntity(ent)));
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public bool DeleteLogEntity(string id)
        {
            Encryption.Decrypt(Convert.FromBase64String(id));
            return proxy.DeleteLogEntity(id);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public float GetAverageConsumptionForCity(string city)
        {
            throw new NotImplementedException();
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni)
        {

            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.ContainsKey(callback) == false) {
                klijenti.Add(callback, regioni);
            }
            
            return proxy.GetEntitiesForRegions(regioni);
        }


        public List<LogEntity> GetEntitiesForRegionsString(string regioni)
        {
            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.Contains(callback) == false)
            {
                klijenti.Add(callback);
            }

            List<Region> reg = Encryption.decryptLogListRegion(regioni);
            List<LogEntity> lle = proxy.GetEntitiesForRegions(reg);

            int i = 0;
            foreach (LogEntity item in lle)
            {
                lle[i].Godina = 0;
                lle[i].Grad = Encryption.encryptLogEntity(item);
                lle[i].Id = "";
                lle[i].Region = Region.Banat;
                i++;
            }

            return lle;
        }

        public LogEntity GetLogEntityById(string id)
        {
            Encryption.Decrypt(Convert.FromBase64String(id));
            LogEntity lent = new LogEntity();
            lent.Grad = Encryption.encryptLogEntity(proxy.GetLogEntityById(id));

            return lent;
        }

        public void testServerMessage(string message)
        {
            proxy.testServerMessage(message);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Modify")]
        public LogEntity UpdateConsumption(string id, int month, float consumption)
        {
            LogEntity le = Encryption.decryptLogEntity(id);
            LogEntity lent = new LogEntity();
            lent.Grad = Encryption.encryptLogEntity(proxy.UpdateConsumption(le.Id, le.Godina, le.Potrosnja[0]));
            return lent;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public float GetAverageConsumptionForRegion(Region reg)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public float GetAverageConsumptionForRegionList(string reg)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public string GetAverageConsumptionForCityRetStr(string city)
        {
            city = Encryption.Decrypt(Convert.FromBase64String(city));
            ProsPotrosnja potrosnja = new ProsPotrosnja();
            potrosnja.Potrosnja = proxy.GetAverageConsumptionForCity(city);
            return Encryption.encryptFloat(potrosnja);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public string GetAverageConsumptionForRegionRetStr(string reg)
        {
            List<Region> regs = Encryption.decryptLogListRegion(reg);
            ProsPotrosnja potrosnja = new ProsPotrosnja();
            potrosnja.Potrosnja = proxy.GetAverageConsumptionForRegion(regs[0]);
            return Encryption.encryptFloat(potrosnja);
        }
    }
}
