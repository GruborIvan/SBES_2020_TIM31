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
        EncryptionLocalDB enc = new EncryptionLocalDB();
        IDatabaseService proxy = null;
        public static Dictionary<IDatabaseCallback, List<Region>> klijenti = new Dictionary<IDatabaseCallback, List<Region>>();

        public WCFLocalDBService() {

            proxy = proxyCaller.proxy;

        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public string AddLogEntity(LogEntity entitet)
        {
            LogEntity ent = EncryptionLocalDB.decryptLogEntity(entitet.Grad);
            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();
            if (klijenti.ContainsKey(callback) == false) {
                klijenti.Add(callback, new List<Region>());
            }
            
            return Convert.ToBase64String(EncryptionLocalDB.Encrypt(proxy.AddLogEntity(ent)));
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public bool DeleteLogEntity(string id)
        {

            string ajdi = enc.decryptCall(Convert.FromBase64String(id));
            return proxy.DeleteLogEntity(ajdi);
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

            List<Region> reg = EncryptionLocalDB.decryptLogListRegion(regioni);
            List<LogEntity> lle = proxy.GetEntitiesForRegions(reg);

            if (klijenti.ContainsKey(callback) == false)
            {
                klijenti.Add(callback, reg);

            }

            int i = 0;
            foreach (LogEntity item in lle)
            {
                lle[i].Grad = EncryptionLocalDB.encryptLogEntity(item);
                lle[i].Godina = 1;
                lle[i].Id = "";
                lle[i].Region = Region.Banat;
                i++;
            }

            return lle;
        }

        public LogEntity GetLogEntityById(string id)
        {

            string decryptedid = EncryptionLocalDB.Decrypt(Convert.FromBase64String(id));
            LogEntity lent = proxy.GetLogEntityById(decryptedid);
            lent.Grad = EncryptionLocalDB.encryptLogEntity(lent);

            return lent;
        }

        public void testServerMessage(string message)
        {
            RSAKeyClass.DecryptData(message);
            message = "Hello from Local DataBase! :) xD";
            proxy.testServerMessage(message);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Modify")]
        public LogEntity UpdateConsumption(string id, int month, float consumption)
        {
            LogEntity le = EncryptionLocalDB.decryptLogEntity(id);
            LogEntity lent = new LogEntity();
            lent.Grad = EncryptionLocalDB.encryptLogEntity(proxy.UpdateConsumption(le.Id, le.Godina, le.Potrosnja[0]));
            return lent;
        }

        public float GetAverageConsumptionForRegion(Region reg)
        {
            throw new NotImplementedException();
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public string GetAverageConsumptionForCityRetStr(string city)
        {
            city = EncryptionLocalDB.Decrypt(Convert.FromBase64String(city));
            ProsPotrosnja potrosnja = new ProsPotrosnja();
            potrosnja.Potrosnja = proxy.GetAverageConsumptionForCity(city);
            return EncryptionLocalDB.encryptFloat(potrosnja);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public string GetAverageConsumptionForRegionRetStr(string reg)
        {
            List<Region> regs = EncryptionLocalDB.decryptLogListRegion(reg);
            ProsPotrosnja potrosnja = new ProsPotrosnja();
            potrosnja.Potrosnja = proxy.GetAverageConsumptionForRegion(regs[0]);
            return EncryptionLocalDB.encryptFloat(potrosnja);
        }
    }
}
