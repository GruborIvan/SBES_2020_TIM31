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

			WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address)));

			int opt = 0;

            while (true) {

				opt = meni();

                switch (opt) {

					case 1:
						List<Region> regioni = odaberiregione();
						proxy.readEntities(regioni);
						break;
					case 2:

						Region ch = odaberiRegion();
						proxy.regionAverageConsumption(ch);
						break;
					case 3:
						string grad = odaberiGrad();
						proxy.cityAverageConsumption(grad);
						break;
					case 4:
						string package = odaberiEntitet();
						string[] input = package.Split(',');
						proxy.updateConsumption(input[0], int.Parse(input[1]), float.Parse(input[2]));
						break;
					case 5:
						LogEntitet kreiranEntitet = dodajEntitet();
						proxy.addLogEntity(kreiranEntitet);
						break;
					case 6:
						string identification = izbrisiEntitet();
						proxy.deleteLogEntity(identification);
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

			do {

				int i = 1;
                foreach (Region reg in Enum.GetValues(typeof(Region))) {

					string val = reg.ToString();
					val = val.Replace('_', ' ');
					Console.WriteLine("{0}. {1}", i, val);
					i++;
				}

				input = Console.ReadLine();
				regioni = validateInput(input);

			} while (regioni == null);

			return regioni;
        }

		static List<Region> validateInput(string input) {

			List<Region> regioni = new List<Region>();

			string[] splitinput = input.Split(',');
			int ch;
			bool success;

            foreach (string s in splitinput) {

				success = int.TryParse(s.Replace(" ", ""), out ch);
                if (success == false || ch < 1 || ch > 9) {
					Console.WriteLine("Neispravan format unosa. Regione odaberite brojevima odvojenim zapetom. Brojevi su od 1 do 9.\n");
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

			return (Region)ch-1;
		}

		static string odaberiGrad() {

			Database database = new Database();
			int ch;
			int i;

			do {

				i = 1;
				foreach (LogEntitet s in database.EntityList) {
					Console.WriteLine("{0}. {1}.\n", i, s.Grad);
					i++;
				}

			} while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

			return database.EntityList[ch - 1].Grad;
        }

		static string odaberiEntitet() {

			Database db = new Database();
			string res = "";
			int ch;
			float consumption;
			int i;

			do {
				i = 1;
				foreach (LogEntitet entitet in db.EntityList) {

					Console.WriteLine("{0}. {1}, {2}, prosečna potrošnja: {3}.\n", i, entitet.Region, entitet.Grad, entitet.Potrosnja.Average());
					i++;
				}

			} while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

			res += db.EntityList[ch - 1].Id + ',';

			do {
				i = 1;
				foreach (float potrosnja in db.EntityList[ch - 1].Potrosnja) {

					Console.WriteLine("{0}. Mesec: {1}, potrošnja: {2}.\n", i, i, potrosnja);
					i++;
				}

			} while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

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
			float potrosnja;

			Console.WriteLine("Unesite region> ");
			entitet.Region = odaberiRegion();
			Console.WriteLine("Unesite grad> ");
			entitet.Grad = Console.ReadLine();

			do {
				Console.WriteLine("Unesite godinu> ");

			}while(int.TryParse(Console.ReadLine(), out godina) == false || godina < 0);

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
						i --;
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
				foreach (LogEntitet entitet in database.EntityList) {

					Console.WriteLine("{0}. {1}, {2}, Godina: {3}.\n", i, entitet.Region, entitet.Grad, entitet.Year);
					i++;
				}

			} while (int.TryParse(Console.ReadLine(), out ch) == false || ch < 0 || ch > i);

			return database.EntityList[ch - 1].Id;
		}

		static void izlistajEntitete() {

			Database database = new Database();
			int i = 1;

			foreach (LogEntitet entitet in database.EntityList) {

				Console.WriteLine("{0}. {1}, {2}, Godina: {3}, Prosečna potrošnja: {4}.\n", i, entitet.Region, entitet.Grad, entitet.Year, entitet.Potrosnja.Average());
				i++;
			}

		}
    }
}