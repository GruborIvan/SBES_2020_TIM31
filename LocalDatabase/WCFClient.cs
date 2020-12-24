using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class WCFClient : DuplexClientBase<IDatabaseService>, IDatabaseService, IDisposable
    {
        IDatabaseService factory;
        Encryption encryptor = new Encryption();
        public WCFClient(object callbackInstance, NetTcpBinding binding, EndpointAddress address) : base(callbackInstance, binding, address) 
        {
            factory = this.CreateChannel();
        }

        public string AddLogEntity(LogEntity entitet) {

            Database db = new Database();
            entitet.Id = factory.AddLogEntity(entitet);

            if (entitet.Id == null) {
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

            float potrosnja;
            
            potrosnja = factory.GetAverageConsumptionForCity(grad);
            using(AesManaged aes = new AesManaged())
            {
                byte[] encrypted = encryptor.encryptCall(grad, aes.Key, aes.IV);
                byte[] encrypted1 = encryptor.encryptCall(potrosnja.ToString(), aes.Key, aes.IV);
                Console.WriteLine($"Prosečna godišnja potrošnja za {System.Text.Encoding.UTF8.GetString(encrypted)}" +
                                $" je : {System.Text.Encoding.UTF8.GetString(encrypted1)} [kW/h]");
               
                string decrypted = encryptor.decryptCall(encrypted, aes.Key, aes.IV);
                string decrypted1 = encryptor.decryptCall(encrypted1, aes.Key, aes.IV);
                Console.WriteLine($"Prosečna godišnja potrošnja za {decrypted}" +
                                $" je : {decrypted1} [kW/h]");
            }

            return potrosnja;
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
            entiteti = factory.GetEntitiesForRegions(regioni);

            foreach (LogEntity ent in entiteti) {
                if (db.EntityList.ContainsKey(ent.Id) == false) {
                    db.EntityList.Add(ent.Id, ent);
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

            Database db = new Database();
            LogEntity entitet = factory.GetLogEntityById(id);

            return entitet;
        }
    }
}
