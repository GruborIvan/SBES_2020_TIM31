using Client;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocalDBase
{
    public class WCFLocalDB : DuplexClientBase<IDatabaseService>, IDatabaseService, IDisposable 
    {

        IDatabaseService factory;
        public static List<IDatabaseCallback> klijenti = new List<IDatabaseCallback>();
        public enum CallbackOperation { ADD, UPDATE, DELETE };

        XmlHandler xh = new XmlHandler();

        public WCFLocalDB(object callbackInstance, NetTcpBinding binding, EndpointAddress address) : base(callbackInstance, binding, address)
        {
            factory = this.CreateChannel();
        }

        public string AddLogEntity(LogEntity entitet)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();

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
            throw new NotImplementedException();

        }

        public LogEntity GetLogEntityById(string id)
        {
            throw new NotImplementedException();
        }

        void broadcastIdMessage(string id, CallbackOperation op)
        {
            List<IDatabaseCallback> deleteitems = new List<IDatabaseCallback>();

            foreach (IDatabaseCallback client in klijenti)
            {

                    try
                    {
                        switch (op)
                        {
                            case CallbackOperation.ADD:
                                LogEntity entity = GetLogEntityById(id);
                                client.broadcastAddLogEntity(entity.Region, entity.Id);
                                break;

                            case CallbackOperation.UPDATE:
                                client.broadcastUpdateId(id);
                                break;
                            case CallbackOperation.DELETE:
                                client.broadcastDeleteId(id);
                                break;
                        }
                        Console.WriteLine("Hi from thread!");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}", ex.Message);
                        deleteitems.Add(client);
                    }
            }

            // Brisanje ugasenih klijenata..
            foreach (IDatabaseCallback client in deleteitems)
            {
                klijenti.Remove(client);
            }

        }

    }
}
