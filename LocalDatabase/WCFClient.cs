using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : DuplexClientBase<IDatabaseService>, IDatabaseService, IDisposable
    {

        IDatabaseService factory;
        Encryption enc = new Encryption();
        public WCFClient(object callbackInstance, NetTcpBinding binding, EndpointAddress address) : base(callbackInstance, binding, address) 
        {
            factory = this.CreateChannel();
        }

        public string AddLogEntity(LogEntity entitet) {

            Database db = new Database();
            string ent = Encryption.encryptLogEntity(entitet);
            entitet.Grad = ent;

            string Id = factory.AddLogEntity(entitet);

            entitet = Encryption.decryptLogEntity(ent);
            entitet.Id = Encryption.Decrypt(Convert.FromBase64String( Id));

            if (entitet.Id == null)
            {
                Console.WriteLine("Entitet za grad: {0} i godinu: {1} već postoji!", entitet.Grad, entitet.Godina);
                return null;
            }
            if (!db.EntityList.ContainsKey(entitet.Id))
            {
                db.EntityList.Add(entitet.Id, entitet);
            }
            
            return entitet.Id;
        }

        public float GetAverageConsumptionForCity(string grad) {

            ProsPotrosnja potrosnja = new ProsPotrosnja();
            
            string encGrad = Convert.ToBase64String( enc.encryptCall(grad));
            string ret = factory.GetAverageConsumptionForCityRetStr(encGrad);

            potrosnja = Encryption.decryptFloat(ret);

            Console.WriteLine($"Prosečna godišnja potrošnja za {grad}" +
                            $" je : {potrosnja.Potrosnja} [kW/h]");
            
            return potrosnja.Potrosnja;
        }

        public bool DeleteLogEntity(string id) {

            Database db = new Database();

            if (db.EntityList.ContainsKey(id) == false) {
                Console.WriteLine("Traženi entitet ne postoji.");
                return false;
            }
            else
            {
                db.EntityList.Remove(id);
                id = Convert.ToBase64String( enc.encryptCall(id));
                factory.DeleteLogEntity(id);
            }

            return true;
        }

        public void Dispose() {
            if (factory != null) {
                factory = null;
            }

            this.Close();
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni) {

            List<LogEntity> entiteti = new List<LogEntity>();
            Database db = new Database();

            string reg = Encryption.encryptListRegion(regioni);

            entiteti = factory.GetEntitiesForRegionsString(reg);

            int i = 0;
            List<LogEntity> lle = new List<LogEntity>();
            foreach(LogEntity item in entiteti)
            {
                lle.Add(Encryption.decryptLogEntity(entiteti[i].Grad));
                i++;
            }

            foreach (LogEntity ent in lle) {
                if (db.EntityList.ContainsKey(ent.Id) == false) {
                    db.EntityList.Add(ent.Id, ent);
                }
            }
            Console.WriteLine("Dobavljeni su entiteti, molimo odaberite opciju izlistaj entitete za detalje.\n");

            return lle;
        }

        public float GetAverageConsumptionForRegion(Region reg) {

            ProsPotrosnja potrosnja;
            List<Region> r = new List<Region>();
            r.Add(reg);
            string send = Encryption.encryptListRegion(r);

            string ret = factory.GetAverageConsumptionForRegionRetStr(send);

            potrosnja = Encryption.decryptFloat(ret);

            Console.WriteLine($"Prosečna godišnja potrošnja za {reg} je : {potrosnja.Potrosnja} [kW/h]");

            return potrosnja.Potrosnja;
        }
        public void testServerMessage(string message) {
            factory.testServerMessage(message);    
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption) {

            Database db = new Database();
            if (db.EntityList.ContainsKey(id)) {
                LogEntity le = new LogEntity();
                le.Id = id;
                le.Godina = month;
                le.Potrosnja.Add(consumption);
                string logent = Encryption.encryptLogEntity(le);

                month = 0;
                consumption = 0;
                le = Encryption.decryptLogEntity( factory.UpdateConsumption(logent, month, consumption).Grad);
                db.EntityList[id] = le;
                return db.EntityList[id];
            }

            Console.WriteLine("Traženi entitet nije pronađen.");
            return null;
        }

        public LogEntity GetLogEntityById(string id) {

            Database db = new Database();
            
            LogEntity entitet = factory.GetLogEntityById(Convert.ToBase64String( enc.encryptCall(id)));
            entitet = Encryption.decryptLogEntity(entitet.Grad);

            return entitet;
        }

        public List<LogEntity> GetEntitiesForRegionsString(string regioni)
        {
            throw new NotImplementedException();
        }

        public float GetAverageConsumptionForRegionList(string reg)
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
