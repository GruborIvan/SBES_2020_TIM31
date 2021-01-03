using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class AuditLoggingSystem
    {

        private static EventLog customLog = null;
        const string sourcename = "SecurityManager.AuditLoggingSystem";
        const string logname = "CentralDatabaseLogFile";

        static AuditLoggingSystem() {

            try {

                if (EventLog.SourceExists(sourcename) == false) {

                    EventLog.CreateEventSource(sourcename, logname);

                }
                customLog = new EventLog(logname, Environment.MachineName, sourcename);

            }
            catch (Exception ex) {

                customLog = null;
                Console.WriteLine("Error whil trying to create log handle. Error> {0}.", ex.Message);

            }

        }

        public static void AddLogEntitySuccess(string user, string method, string id) {

           if(id == null) {

                customLog.WriteEntry(string.Format("Centralna baza: {0}, metoda: {1}. Dodavanje entiteta je neuspešno, moguće je da je baza podataka obrisana tokom rada servisa.",
                    user, method), EventLogEntryType.FailureAudit);

           }
           else {

                customLog.WriteEntry(string.Format("Centralna baza: {0} metoda: {1}. Dodavanje entiteta je uspešno. Centralna baza dodelila id: {2}.",
                    user, method, id), EventLogEntryType.SuccessAudit);

           }

        }

        public static void UpdateLogEntitySuccess(string user, string method, bool success, string id, int month, float consumption) {

            if (success == false) {

                customLog.WriteEntry(string.Format("Centralna baza: {0} metoda: {1}. Ažuriranje entiteta sa id-em: {2} je neuspešno. Pokušano menjanje entiteta koji ne postoji.",
                    user, method, id), EventLogEntryType.FailureAudit);

            }
            else {

                customLog.WriteEntry(string.Format("Centralna baza: {0} metoda: {1}. Ažuriranje entiteta sa id-em: {2} je uspešno. Potrošnja meseca {3} tog entiteta promenjena na {4} [kW/h].",
                    user, method, id, month + 1, consumption));
            
            }

        }

        public static void DeleteLogEntitySuccess(string user, string method, bool success, string id) {

            if(success == false) {

                customLog.WriteEntry(string.Format("Centralna baza: {0} metoda: {1}. Brisanje entiteta sa id-em: {2} je neuspešno. Pokušano brisanje entiteta koji ne postoji.",
                    user, method, id));

            }
            else {

                customLog.WriteEntry(string.Format("Centralna baza: {0} metoda: {1}. Brisanje entiteta sa id-em: {2} uspešno.",
                    user, method, id));

            }

        }

        public static void BroadcastLogEntitySuccess(string user, string method, CallbackOperation operation, string id, int localbasenum) {

            customLog.WriteEntry(string.Format("Centralna baza {0} izvršila metodu {1}. Kod operacije: {2}. Izvršen broadcast id-a: {3} svim({4}) lokalnim bazama.",
                user, method, operation.ToString(), id, localbasenum), EventLogEntryType.Information);

        }

        public static void ChangesTupleListSent(string user, int count) {

            customLog.WriteEntry(string.Format("Centralna baza: {0} poslala listu promena backup serveru. Veličina liste promena: {1}.", user, count), EventLogEntryType.Information);

        }

    }
}
