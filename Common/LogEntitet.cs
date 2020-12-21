using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public enum Region { Srem, Banat, Bačka, Beograd, Šumadija, Zapadna_Srbija, Južna_Srbija, Istočna_Srbija, Kosovo_i_Metohija, Nazad };

    public class LogEntitet
    {

        string id;
        Region region;
        string grad;
        int year;
        List<float> potrosnja;

        public LogEntitet() {

            this.Id = "";
            Region = Region.Bačka;
            Grad = "";
            Year = 0;
            Potrosnja = new List<float> ();

        }

        public LogEntitet(string id, Region reg, string municipality, int godina) {

            this.Id = id;
            Region = reg;
            Grad = municipality;
            Year = godina;
            Potrosnja = new List<float> ();
            
        }

        public string Id { get => id; set => id = value; }
        public Region Region { get => region; set => region = value; }
        public string Grad { get => grad; set => grad = value; }
        public int Year { get => year; set => year = value; }
        public List<float> Potrosnja { get => potrosnja; set => potrosnja = value; }
    }
}
