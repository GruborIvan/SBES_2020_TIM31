
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
        //komentar

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

            string id = factory.AddLogEntity(entitet);
            entitet.Id = id;
           // xh.AddEntity(entitet);
            return entitet.Id;
        }

        public float GetAverageConsumptionForCity(string grad)
        {
            
            float potrosnja;

            potrosnja = factory.GetAverageConsumptionForCity(grad);

            return potrosnja;
        }

        public bool DeleteLogEntity(string id)
        {

            try
            {
                factory.DeleteLogEntity(id);
                //deleted = xh.DeleteEntity(id);
                return true;
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

            List<LogEntity> lista = factory.GetEntitiesForRegions(regioni);
            
            foreach (LogEntity entitet in lista) {
                if (xh.ReturnList().Find(x => x.Id == entitet.Id) == null) {
                    xh.AddEntity(entitet);
                }
            }

            return lista;
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
            Console.WriteLine(message);
            factory.testServerMessage("hello from localDB :)");
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

            factory.UpdateConsumption(id, month, consumption);
            entitet.Potrosnja[month] = consumption;
            //xh.UpdateEntity(entitet);

            return entitet;
        }

        public LogEntity GetLogEntityById(string id)
        {

            LogEntity entitet = factory.GetLogEntityById(id);

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
