using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class WCFClient : ChannelFactory<IDatabaseService>, IDatabaseService, IDisposable
    {
        IDatabaseService factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address) {
            factory = this.CreateChannel();
            Console.WriteLine("PROVERA: Namestio sam!");
        }

        public string addLogEntity(LogEntitet entitet) {

            Database database = new Database();

            entitet.Id = factory.addLogEntity(entitet);
            database.EntityList.Add(entitet);

            return entitet.Id;
        }

        public float cityAverageConsumption(string grad) {

            float potrosnja;
            potrosnja = factory.cityAverageConsumption(grad);
            Console.WriteLine("Prosečna potrošnja za {0} je : {1} [kW/h]", grad, potrosnja);

            return potrosnja;
        }

        public bool deleteLogEntity(string id) {

            Database db = new Database();

            if (db.EntityList.Find(x => x.Id == id) == null)
            {
                Console.WriteLine("Traženi entitet ne postoji.");
                return false;
            }else
                db.EntityList.Remove(db.EntityList.Find(x => x.Id == id));

            return true;
        }

        public void Dispose() {
            if (factory != null) {
                factory = null;
            }

            this.Close();
        }

        public List<LogEntitet> readEntities(List<Region> regioni) {

            Database db = new Database();

            List <LogEntitet> entiteti = new List<LogEntitet>();
            entiteti = factory.readEntities(regioni);
            db.EntityList.AddRange(entiteti);

            return entiteti;
        }

        public float regionAverageConsumption(Region reg) {

            float potrosnja;
            potrosnja = factory.regionAverageConsumption(reg);
            Console.WriteLine("Prosečna potrošnja za {0} je : {1} [kW/h]", reg, potrosnja);

            return potrosnja;
        }

        public void testServerMessage(string message) {

            factory.testServerMessage("Hello to server from client.");
        }

        public LogEntitet updateConsumption(string id, int month, float consumption) {

            Database db = new Database();        

            foreach (LogEntitet ent in db.EntityList)
            {
                if (ent.Id == id)
                {
                    ent.Potrosnja[month] = consumption;
                    factory.updateConsumption(id, month, consumption);
                    return ent;
                }
            }

            Console.WriteLine("Traženi entitet nije pronađen.");
            return null;
        }
    }
}
