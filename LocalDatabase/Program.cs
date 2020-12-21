using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    class Program
    {

        static void Main(string[] args) {

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/wcfserver";

            CallbackClient callbackclient = new CallbackClient();
            WCFClient proxy = new WCFClient(callbackclient, binding, new EndpointAddress(new Uri(address)));
            callbackclient.Proxy = proxy;

            int opt = 0;

            while (true) {

                opt = meni();

                switch (opt) {

                    case 1:
                        List<Region> regioni = odaberiregione();
                        if (regioni != null) {
                            proxy.readEntities(regioni);
                        }
                        break;
                    case 2:
                        Region ch = odaberiRegion();
                        if (ch != Region.Nazad) {
                            proxy.regionAverageConsumption(ch);
                        }
                        break;
                    case 3:
                        string grad = odaberiGrad();
                        if (grad != null) {
                            proxy.cityAverageConsumption(grad);
                        }
                        break;
                    case 4:
                        string package = odaberiEntitet();
                        if (package != null) {
                            string[] input = package.Split(',');
                            proxy.updateConsumption(input[0], int.Parse(input[1]), float.Parse(input[2]));
                        }
                        break;
                    case 5:
                        LogEntitet kreiranEntitet = dodajEntitet();
                        if (kreiranEntitet != null) {
                            proxy.addLogEntity(kreiranEntitet);
                        }
                        break;
                    case 6:
                        string identification = izbrisiEntitet();
                        if (identification != null) {
                            proxy.deleteLogEntity(identification);
                        }
                        break;
                    case 7:
                        izlistajEntitete();
                        break;
                    case 8:
                        goto labela;
                    default:
                        Console.WriteLine("Nepostojeća opcija je odabrana, molimo pokušajte ponovo.");
                        break;
                }

            }

        labela:
            proxy.Close();

            Console.ReadLine();
        }

        static int meni() {

            int ch;

            do {
                Console.WriteLine("1. Prikaži odgovarajuće entitete(odaberi regione).\n2. Izračunaj srednju vrednost potrošnju za region.\n" +
                "3. Izračunaj srednju vrednost potrošnje za grad.\n4. Ažuriraj mesečnu potrošnju grada.\n5. Dodaj nov entitet.\n6. Obriši postojeći entitet.\n7. Izlistaj entitete.\n8. Izlaz iz programa.");

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 1 || ch > 8);


            return ch;
        }

        static List<Region> odaberiregione() {

            List<Region> regioni = new List<Region>();
            string input;

            int i = 1;
            foreach (Region reg in Enum.GetValues(typeof(Region))) {

                string val = reg.ToString();
                val = val.Replace('_', ' ');
                Console.WriteLine("{0}. {1}", i, val);
                i++;
            }

            input = Console.ReadLine();
            regioni = validateInput(input);

            return regioni;
        }

        static List<Region> validateInput(string input) {

            List<Region> regioni = new List<Region>();

            string[] splitinput = input.Split(',');
            int ch;
            bool success;

            foreach (string s in splitinput) {

                success = int.TryParse(s.Replace(" ", ""), out ch);
                if (success == false || ch < 1 || ch > 10) {
                    Console.WriteLine("Neispravan format unosa. Regione odaberite brojevima odvojenim zapetom. Brojevi su od 1 do 9.\n");
                    return null;
                }
                else if (success == true && ch == 10) {
                    return null;
                }

                regioni.Add((Region)(ch - 1));
            }

            return regioni;
        }

        static Region odaberiRegion() {

            int ch;
            int i;

            do {

                i = 1;
                foreach (Region reg in Enum.GetValues(typeof(Region))) {

                    string val = reg.ToString();
                    val = val.Replace('_', ' ');
                    Console.WriteLine("{0}. {1}", i, val);
                    i++;
                }
            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

            return (Region)ch - 1;
        }

        static string odaberiGrad() {

            Database database = new Database();
            int ch;
            int i;

            List<string> gradovi = database.EntityList.Values.Select(x => x.Grad).Distinct().ToList();

            do {

                i = 1;
                foreach (string grad in gradovi) {
                    Console.WriteLine("{0}. {1}.\n", i, grad);
                    i++;
                }
                Console.WriteLine("{0}. Nazad na glavni meni.", i);

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

            if (ch == i) {
                return null;
            }

            return database.EntityList.Values.ToList()[ch - 1].Grad;
        }

        static string odaberiEntitet() {

            Database db = new Database();
            string res = "";
            int ch;
            int defaultval;
            float consumption;
            int i;

            do {
                i = 1;
                foreach (LogEntitet entitet in db.EntityList.Values.ToList()) {

                    Console.WriteLine("{0}. {1}, {2}, prosečna potrošnja: {3}.\n", i, entitet.Region, entitet.Grad, entitet.Potrosnja.Average());
                    i++;
                }
                Console.WriteLine("{0}. Nazad na glavni meni.", i);

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

            if (ch == i) {
                return null;
            }

            res += db.EntityList.Values.ToList()[ch - 1].Id + ',';
            defaultval = ch;

            do {
                i = 1;
                foreach (float potrosnja in db.EntityList.Values.ToList()[defaultval - 1].Potrosnja) {

                    Console.WriteLine("{0}. Mesec: {1}, potrošnja: {2}[kW/h].", i, i, potrosnja);
                    i++;
                }

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch >= i);

            res += (ch - 1).ToString() + ',';

            do {
                Console.WriteLine("Unesite postrošnju: ");

            } while (float.TryParse(Console.ReadLine(), out consumption) == false);

            res += consumption.ToString();

            return res;
        }

        static LogEntitet dodajEntitet() {

            LogEntitet entitet = new LogEntitet();
            int godina;

            Console.WriteLine("Unesite region> ");
            entitet.Region = odaberiRegion();

            if (entitet.Region == Region.Nazad) {
                return null;
            }

            Console.WriteLine("Unesite grad> ");
            entitet.Grad = Console.ReadLine();

            do {
                Console.WriteLine("Unesite godinu> ");

            } while (int.TryParse(Console.ReadLine(), out godina) == false || godina < 0);

            entitet.Year = godina;

            int i = 1;
            do {

                try {

                    Console.WriteLine("Unesite potrošnju za mesec {0}: ", i);
                    entitet.Potrosnja.Add(float.Parse(Console.ReadLine()));
                    i++;
                }
                catch (Exception ex) {

                    Console.WriteLine("Unet neispravan format potrosnje.");
                    if (i != 1) {
                        i--;
                    }
                }

            } while (i < 13);

            return entitet;
        }

        static string izbrisiEntitet() {

            Database database = new Database();
            int i;
            int ch;

            do {
                i = 1;
                foreach (LogEntitet entitet in database.EntityList.Values.ToList()) {

                    Console.WriteLine("{0}. {1}, {2}, Godina: {3}.\n", i, entitet.Region, entitet.Grad, entitet.Year);
                    i++;
                }
                Console.WriteLine("{0}. Nazad na glavni meni.", i);

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

            if (ch == i) {
                return null;
            }

            return database.EntityList.Values.ToList()[ch - 1].Id;
        }

        static void izlistajEntitete() {

            Database database = new Database();
            int i = 1;

            foreach (LogEntitet entitet in database.EntityList.Values.ToList()) {

                Console.WriteLine();

                Console.WriteLine("{0}. {1}, {2}, Godina: {3}, Prosečna potrošnja: {4}.\n", i, entitet.Region, entitet.Grad, entitet.Year, entitet.Potrosnja.Average());
                Console.Write("Mesečne potrošnje (januar - decembar) {0}: ", entitet.Year);
                foreach (float monthlyconsumption in entitet.Potrosnja) {
                    Console.Write("{0}[kW/h], ", monthlyconsumption.ToString());
                }
                Console.WriteLine();
                i++;
            }

        }
    }
}