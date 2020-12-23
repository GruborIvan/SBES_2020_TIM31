using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class WCFClient : DuplexClientBase<IDatabaseService>, IDatabaseService, IDisposable
    {
        IDatabaseService factory;
        Database db;

        public WCFClient(object callbackInstance, NetTcpBinding binding, EndpointAddress address,ref Database db) : base(callbackInstance, binding, address) 
        {
            factory = this.CreateChannel();
            this.db = db;
        }

        public string AddLogEntity(LogEntity entitet) {

            entitet.Id = factory.AddLogEntity(entitet);

            if (entitet.Id == null) {
                Console.WriteLine("Entitet za grad: {0} i godinu: {1} već postoji!", entitet.Grad, entitet.Godina);
                return null;
            }

            if (!db.LogEntities.ContainsKey(entitet.Id))
            {
                db.LogEntities.Add(entitet.Id, entitet);
            }
            
            return entitet.Id;
        }

        public float GetAverageConsumptionForCity(string grad) {

            float potrosnja;
            potrosnja = factory.GetAverageConsumptionForCity(grad);
            Console.WriteLine($"Prosečna godišnja potrošnja za {grad} je : {potrosnja} [kW/h]");

            return potrosnja;
        }

        public bool DeleteLogEntity(string id) {

            if (db.LogEntities.ContainsKey(id) == false) {
                Console.WriteLine("Traženi entitet ne postoji.");
                return false;
            }
            else
            {
                db.LogEntities.Remove(id);
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
            entiteti = factory.GetEntitiesForRegions(regioni);

            foreach (LogEntity ent in entiteti) {
                if (db.LogEntities.ContainsKey(ent.Id) == false) {
                    db.LogEntities.Add(ent.Id, ent);
                }
            }
            Console.WriteLine("Dobavljeni su entiteti, molimo odaberite opciju izlistaj entitete za detalje.\n");

            return entiteti;
        }

        public float GetAverageConsumptionForRegion(Region reg) {

            float potrosnja;
            potrosnja = factory.GetAverageConsumptionForRegion(reg);
            Console.WriteLine($"Prosečna godišnja potrošnja za {reg} je : {potrosnja} [kW/h]");

            return potrosnja;
        }

        public void testServerMessage(string message) {

            factory.testServerMessage("Hello to server from client.");
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption) {

            if (db.LogEntities.ContainsKey(id)) {
                db.LogEntities[id].Potrosnja[month] = consumption;
                factory.UpdateConsumption(id, month, consumption);
                return db.LogEntities[id];
            }

            Console.WriteLine("Traženi entitet nije pronađen.");
            return null;
        }

        public LogEntity GetLogEntityById(string id) {

            LogEntity entitet = factory.GetLogEntityById(id);

            if (entitet != null) {
                db.LogEntities[id] = entitet;
                return entitet;
            }

            return null;
        }
    }
}
