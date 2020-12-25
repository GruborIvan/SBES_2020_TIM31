
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


        XmlHandler xh = new XmlHandler();

        public WCFLocalDB(object callbackInstance, NetTcpBinding binding, EndpointAddress address) : base(callbackInstance, binding, address)
        {
            factory = this.CreateChannel();
        }

        public string AddLogEntity(LogEntity entitet)
        {
            List<LogEntity> list = xh.ReturnList();
            if(list.Find(x=> x.Grad.ToLower() == entitet.Grad.ToLower() && x.Godina == entitet.Godina) != null)
            {
                return null;
            }

            string id = xh.AddEntity(entitet);
           
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
            bool deleted = false;

            try
            {
                deleted = xh.DeleteEntity(id);
                return deleted;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
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
            List<LogEntity> entiteti= new List<LogEntity>();

            foreach (var item in regioni)
            {
                foreach (var reg in xh.ReturnList())
                {
                    if (item.Equals(reg.Region))
                    {
                        entiteti.Add(reg);
                    }
                }
            }

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
            LogEntity entitet = new LogEntity();

            foreach (var en in xh.ReturnList())
            {
                if (en.Id.Equals(id))
                {
                    entitet = en;
                    break;
                }
            }
            entitet.Potrosnja[month] = consumption;
            xh.UpdateEntity(entitet);

            return entitet;
        }

        public LogEntity GetLogEntityById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
