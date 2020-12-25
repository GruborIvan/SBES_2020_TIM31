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
            List<LogEntity> list = xh.ReturnList();
            if(list.Find(x=> x.Grad.ToLower() == entitet.Grad.ToLower() && x.Godina == entitet.Godina) != null)
            {
                return null;
            }

<<<<<<< HEAD
            if (!db.EntityList.ContainsKey(entitet.Id))
            {
                db.EntityList.Add(entitet.Id, entitet);
            }

            IDatabaseCallback callback = OperationContext.Current.GetCallbackChannel<IDatabaseCallback>();

            if (klijenti.Contains(callback) == false)
            {
                klijenti.Add(callback);
            }

            broadcastIdMessage(entitet.Id,CallbackOperation.ADD);
            // POZVATI BROADCAST !!

=======
            string id = xh.AddEntity(entitet);
           
>>>>>>> c397e2a7b47d772d14e8853bf2fde23d116d81e7
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
<<<<<<< HEAD
                try
                {
                    db.EntityList.Remove(id);
                    factory.DeleteLogEntity(id);
                    broadcastIdMessage(id,CallbackOperation.DELETE);
                }
                catch(Exception e)
                {
                    Trace.TraceInformation(e.Message);
                }
                
=======
                Console.WriteLine(e);
                return false;
>>>>>>> c397e2a7b47d772d14e8853bf2fde23d116d81e7
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
<<<<<<< HEAD
                db.EntityList[id].Potrosnja[month] = consumption;
                factory.UpdateConsumption(id, month, consumption);

                // Broadcast changes to all subscribed clients..
                broadcastIdMessage(id,CallbackOperation.UPDATE);

                return db.EntityList[id];
=======
                if (en.Id.Equals(id))
                {
                    entitet = en;
                    break;
                }
>>>>>>> c397e2a7b47d772d14e8853bf2fde23d116d81e7
            }
            entitet.Potrosnja[month] = consumption;
            xh.UpdateEntity(entitet);

            return entitet;
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
