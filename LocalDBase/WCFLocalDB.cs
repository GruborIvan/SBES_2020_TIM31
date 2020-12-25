using Client;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDBase
{
    public class WCFLocalDB : DuplexClientBase<IDatabaseService>, IDatabaseService, IDisposable {
        IDatabaseService factory;

        public WCFLocalDB(object callbackInstance, NetTcpBinding binding, EndpointAddress address) : base(callbackInstance, binding, address)
        {
            factory = this.CreateChannel();
        }

        public string AddLogEntity(LogEntity entitet)
        {

            Database db = new Database();
            entitet.Id = factory.AddLogEntity(entitet);

            if (entitet.Id == null)
            {
                return null;
            }

            if (!db.EntityList.ContainsKey(entitet.Id))
            {
                db.EntityList.Add(entitet.Id, entitet);
            }

            return entitet.Id;
        }

        public float GetAverageConsumptionForCity(string grad)
        {

            float potrosnja;

            potrosnja = factory.GetAverageConsumptionForCity(grad);
         
            Console.WriteLine($"Prosečna godišnja potrošnja za {grad}" +
                            $" je : {potrosnja} [kW/h]");
            

            return potrosnja;
        }

        public bool DeleteLogEntity(string id)
        {

            Database db = new Database();

            if (db.EntityList.ContainsKey(id) == false)
            {
                Console.WriteLine("Traženi entitet ne postoji.");
                return false;
            }
            else
            {
                db.EntityList.Remove(id);
                factory.DeleteLogEntity(id);
            }

            return true;
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni)
        {

            List<LogEntity> entiteti = new List<LogEntity>();
            Database db = new Database();
            entiteti = factory.GetEntitiesForRegions(regioni);

            foreach (LogEntity ent in entiteti)
            {
                if (db.EntityList.ContainsKey(ent.Id) == false)
                {
                    db.EntityList.Add(ent.Id, ent);
                }
            }
            Console.WriteLine("Dobavljeni su entiteti, molimo odaberite opciju izlistaj entitete za detalje.\n");

            return entiteti;
        }

        public float GetAverageConsumptionForRegion(Region reg)
        {

            float potrosnja;
            potrosnja = factory.GetAverageConsumptionForRegion(reg);
            Console.WriteLine($"Prosečna godišnja potrošnja za {reg} je : {potrosnja} [kW/h]");

            return potrosnja;
        }

        public void testServerMessage(string message)
        {
            Console.WriteLine(message +" LOKALNA");
            factory.testServerMessage(message);
        }

        public LogEntity UpdateConsumption(string id, int month, float consumption)
        {

            Database db = new Database();
            if (db.EntityList.ContainsKey(id))
            {
                db.EntityList[id].Potrosnja[month] = consumption;
                factory.UpdateConsumption(id, month, consumption);
                return db.EntityList[id];
            }

            Console.WriteLine("Traženi entitet nije pronađen.");
            return null;
        }

        public LogEntity GetLogEntityById(string id)
        {

            LogEntity entitet = factory.GetLogEntityById(id);

            return entitet;
        }
    }
}
