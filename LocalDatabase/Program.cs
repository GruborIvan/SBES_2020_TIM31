﻿using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    class Program
    {
        static void Main(string[] args) {

            //List<Region> Regioni = new List<Region>();
            //Dictionary<string, LogEntity> LokalniLogEntiteti = new Dictionary<string, LogEntity>();

            Database database = new Database();

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/wcfserver";

            CallbackClient callbackclient = new CallbackClient(ref database);
            WCFClient proxy = new WCFClient(callbackclient, binding, new EndpointAddress(new Uri(address)),ref database);
            callbackclient.Proxy = proxy;

            int opt;

            while (true) 
            {
                // Ispis menija i unos zeljene opcije..
                opt = Meni();

                switch (opt) {

                    case 1:
                        database.RegioniOdInteresa = Odaberiregione();
                        if (database.RegioniOdInteresa != null) {
                            proxy.GetEntitiesForRegions(database.RegioniOdInteresa);
                        }
                        break;
                    case 2:
                        Region ch = OdaberiRegion();
                        if (ch != Region.Nazad) {
                            proxy.GetAverageConsumptionForRegion(ch);
                        }
                        break;
                    case 3:
                        string grad = OdaberiGrad();
                        if (grad != null) {
                            proxy.GetAverageConsumptionForCity(grad);
                        }
                        break;
                    case 4:
                        string package = OdaberiEntitet();
                        if (package != null) {
                            string[] input = package.Split(',');
                            proxy.UpdateConsumption(input[0], int.Parse(input[1]), float.Parse(input[2]));
                        }
                        break;
                    case 5:
                        LogEntity kreiranEntitet = DodajEntitet();
                        if (kreiranEntitet != null) {
                            proxy.AddLogEntity(kreiranEntitet);
                        }
                        break;
                    case 6:
                        string identification = IzbrisiEntitet();
                        if (identification != null) {
                            proxy.DeleteLogEntity(identification);
                        }
                        break;
                    case 7:
                        foreach(LogEntity lent in database.LogEntities.Values)
                        {
                            Console.WriteLine($"{lent.Id}. Region: {lent.Region} Grad: {lent.Grad} Godina: {lent.Godina}");
                        }
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

        static int Meni() {

            int ch;

            do {
                Console.WriteLine("1. Prikaži odgovarajuće entitete(odaberi regione).\n2. Izračunaj srednju vrednost potrošnju za region.\n" +
                "3. Izračunaj srednju vrednost potrošnje za grad.\n4. Ažuriraj mesečnu potrošnju grada.\n5. Dodaj nov entitet.\n6. Obriši postojeći entitet.\n7. Izlistaj entitete.\n8. Izlaz iz programa.");

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 1 || ch > 8);


            return ch;
        }

        static List<Region> Odaberiregione() {

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
            regioni = ValidateInput(input);

            return regioni;
        }

        static List<Region> ValidateInput(string input) {

            List<Region> regioni = new List<Region>();

            string[] splitinput = input.Split(',');
            bool success;

            foreach (string s in splitinput) {

                success = int.TryParse(s.Replace(" ", ""), out int ch);
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

        static Region OdaberiRegion() {

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

        static string OdaberiGrad() {

            Database database = new Database();
            int ch;
            int i;

            List<string> gradovi = database.LogEntities.Values.Select(x => x.Grad).Distinct().ToList();

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

            return database.LogEntities.Values.ToList()[ch - 1].Grad;
        }

        static string OdaberiEntitet() {

            Database db = new Database();
            string res = "";
            int ch;
            int defaultval;
            float consumption;
            int i;

            do {
                i = 1;
                foreach (LogEntity entitet in db.LogEntities.Values.ToList()) {

                    Console.WriteLine("{0}. {1}, {2}, prosečna potrošnja: {3}.\n", i, entitet.Region, entitet.Grad, entitet.Potrosnja.Average());
                    i++;
                }
                Console.WriteLine("{0}. Nazad na glavni meni.", i);

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

            if (ch == i) {
                return null;
            }

            res += db.LogEntities.Values.ToList()[ch - 1].Id + ',';
            defaultval = ch;

            do {
                i = 1;
                foreach (float potrosnja in db.LogEntities.Values.ToList()[defaultval - 1].Potrosnja) {

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

        static LogEntity DodajEntitet() {

            LogEntity entitet = new LogEntity();
            int godina;

            Console.WriteLine("Unesite region> ");
            entitet.Region = OdaberiRegion();

            if (entitet.Region == Region.Nazad) {
                return null;
            }

            Console.WriteLine("Unesite grad> ");
            entitet.Grad = Console.ReadLine();

            do {
                Console.WriteLine("Unesite godinu> ");

            } while (int.TryParse(Console.ReadLine(), out godina) == false || godina < 0);

            entitet.Godina = godina;

            int i = 1;
            do {

                try {

                    Console.WriteLine("Unesite potrošnju za mesec {0}: ", i);
                    entitet.Potrosnja.Add(float.Parse(Console.ReadLine()));
                    i++;
                }
                catch (Exception ex) {
                    Trace.TraceInformation(ex.Message);
                    Console.WriteLine("Unet neispravan format potrosnje.");
                    if (i != 1) {
                        i--;
                    }
                }

            } while (i < 13);

            return entitet;
        }

        static string IzbrisiEntitet() {

            Database database = new Database();
            int i;
            int ch;

            do {
                i = 1;
                foreach (LogEntity entitet in database.LogEntities.Values.ToList()) {

                    Console.WriteLine("{0}. {1}, {2}, Godina: {3}.\n", i, entitet.Region, entitet.Grad, entitet.Godina);
                    i++;
                }
                Console.WriteLine("{0}. Nazad na glavni meni.", i);

            } while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

            if (ch == i) {
                return null;
            }

            return database.LogEntities.Values.ToList()[ch - 1].Id;
        }

        static void IzlistajEntitete() {

            Database database = new Database();
            int i = 1;

            foreach (LogEntity entitet in database.LogEntities.Values.ToList()) {

                Console.WriteLine();

                Console.WriteLine("{0}. {1}, {2}, Godina: {3}, Prosečna potrošnja: {4}.\n", i, entitet.Region, entitet.Grad, entitet.Godina, entitet.Potrosnja.Average());
                Console.Write("Mesečne potrošnje (januar - decembar) {0}: ", entitet.Godina);
                foreach (float monthlyconsumption in entitet.Potrosnja) {
                    Console.Write("{0}[kW/h], ", monthlyconsumption.ToString());
                }
                Console.WriteLine();
                i++;
            }

        }
    }
}