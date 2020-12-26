﻿using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string Id = "";
            
            try
            {
                Id = factory.AddLogEntity(entitet);
            }
            catch(Exception e)
            {
                Trace.TraceInformation(e.Message);
                Console.WriteLine("User not authorized to Add Entities!");
                return null;
            }
            

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
            string ret = String.Empty;

            try
            {
                ret = factory.GetAverageConsumptionForCityRetStr(encGrad);
            }
            catch(Exception e)
            {
                Trace.TraceInformation(e.Message);
                Console.WriteLine("User not authorized to Calculate average consumption for city!");
                return 0;
            }

            potrosnja = Encryption.decryptFloat(ret);
            Console.WriteLine($"Prosečna godišnja potrošnja za {grad} je: {potrosnja.Potrosnja} [kW/h]");
            return potrosnja.Potrosnja;
        }

        public bool DeleteLogEntity(string id) {

            //Debugger.Launch();
            Database db = new Database();

            if (db.EntityList.ContainsKey(id) == false) {
                Console.WriteLine("Traženi entitet ne postoji.");
                return false;
            }
            else
            {
                try
                {
                    factory.DeleteLogEntity(id);
                }
                catch(Exception e)
                {
                    Trace.TraceInformation(e.Message);
                    Console.WriteLine("User not authrized to delete Log Entities!");
                    return false;
                }

                db.EntityList.Remove(id);
                id = Convert.ToBase64String( enc.encryptCall(id));
            }

            return true;
        }

        public List<LogEntity> GetEntitiesForRegions(List<Region> regioni) {

            List<LogEntity> entiteti = new List<LogEntity>();
            Database db = new Database();

            string reg = Encryption.encryptListRegion(regioni);

            try
            {
                entiteti = factory.GetEntitiesForRegionsString(reg);
            }
            catch(Exception e)
            {
                Trace.TraceInformation(e.Message);
                Console.WriteLine("User not authorized to Read entities for regions!");
                return null;
            }

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
            string ret = String.Empty;

            try
            {
                ret = factory.GetAverageConsumptionForRegionRetStr(send);
            }
            catch(Exception e)
            {
                Trace.TraceInformation(e.Message);
                Console.WriteLine("User not authorized to calculate consumption for region!");
                return 0;
            }

            potrosnja = Encryption.decryptFloat(ret);

            Console.WriteLine($"Prosečna godišnja potrošnja za {reg} je : {potrosnja.Potrosnja} [kW/h]");

            return potrosnja.Potrosnja;
        }

        public void testServerMessage(string message) {
            try {
                factory.testServerMessage(message);
            }
            catch(Exception e)
            {
                Trace.TraceInformation(e.Message);
                Console.WriteLine("User not authorized to use this service!");
                return;
            }  
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
                LogEntity logEntt = null;

                try
                {
                    logEntt = factory.UpdateConsumption(logent, month, consumption);
                }
                catch(Exception e)
                {
                    Trace.TraceInformation(e.Message);
                    Console.WriteLine("User not authorized to Update Log Entities!");
                    return null;
                }

                le = Encryption.decryptLogEntity(logEntt.Grad);
                db.EntityList[id] = le;
                return db.EntityList[id];
            }

            Console.WriteLine("Traženi entitet nije pronađen.");
            return null;
        }

        public LogEntity GetLogEntityById(string id) {

            Database db = new Database();
            LogEntity entitet = null;
            
            try
            {
                entitet = factory.GetLogEntityById(Convert.ToBase64String(enc.encryptCall(id)));
            }
            catch(Exception e)
            {
                Trace.TraceInformation(e.Message);
                Console.WriteLine("User not authorized to get elements!");
                return null;
            }
          
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

        public void Dispose()
        {

            if (factory != null)
            {
                factory = null;
            }
            this.Close();
        }
    }
}
