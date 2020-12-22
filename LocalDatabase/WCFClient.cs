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

        public WCFClient(object callbackInstance, NetTcpBinding binding, EndpointAddress address)
            : base(callbackInstance, binding, address) {
            factory = this.CreateChannel();
            Console.WriteLine("PROVERA: Namestio sam!");
        }

        public string AddLogEntity(LogEntity entitet) {

            Database database = new Database();

            entitet.Id = factory.AddLogEntity(entitet);

            if (entitet.Id == null) {
                Console.WriteLine("Entitet za grad: {0} i godinu: {1} već postoji!", entitet.Grad, entitet.Year);
                return null;
            }
            database.EntityList.Add(entitet.Id, entitet);

            return entitet.Id;
        }

        public float GetAverageConsumptionForCity(string grad) {

            float potrosnja;
            potrosnja = factory.GetAverageConsumptionForCity(grad);
            Console.WriteLine("Prosečna godišnja potrošnja za {0} je : {1} [kW/h]", grad, potrosnja);

            return potrosnja;
        }

        public bool DeleteLogEntity(string id) {

            Database db = new Database();

            if (db.EntityList.ContainsKey(id) == false) {
                Console.WriteLine("Traženi entitet ne postoji.");
                return false;
            }
            else
                db.EntityList.Remove(id);

            factory.DeleteLogEntity(id);

            return true;
        }

        public void Dispose() {
            if (factory != null) {
                factory = null;
            }

            this.Close();
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni) {

            Database database = new Database();

            List<LogEntity> entiteti = new List<LogEntity>();
            entiteti = factory.GetEntitiesForRegions(regioni);

            foreach (LogEntity ent in entiteti) {
                if (database.EntityList.ContainsKey(ent.Id) == false) {
                    database.EntityList.Add(ent.Id, ent);
                }
            }
            Console.WriteLine("Dobavljeni su entiteti, molimo odaberite opciju izlistaj entitete za detalje.\n");

            return entiteti;
        }

        public float GetAverageConsumptionForRegion(Region reg) {

            float potrosnja;
            potrosnja = factory.GetAverageConsumptionForRegion(reg);
            Console.WriteLine("Prosečna godišnja potrošnja za {0} je : {1} [kW/h]", reg, potrosnja);

            return potrosnja;
        }

        public void testServerMessage(string message) {

            factory.testServerMessage("Hello to server from client.");
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption) {

            Database db = new Database();

            if (db.EntityList.ContainsKey(id)) {
                db.EntityList[id].Potrosnja[month] = consumption;
                factory.UpdateConsumption(id, month, consumption);
                return db.EntityList[id];
            }

            Console.WriteLine("Traženi entitet nije pronađen.");
            return null;
        }

        public LogEntity GetLogEntityById(string id) {

            Database database = new Database();

            LogEntity entitet = factory.GetLogEntityById(id);
            if (entitet != null) {
                database.EntityList[id] = entitet;
                return entitet;
            }

            return null;
        }
    }
}
