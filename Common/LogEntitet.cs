using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public enum Region { SREM, BANAT, BACKA, BEOGRAD, SUMADIJA, ZAPADNA_SRBIJA, JUZNA_SRBIJA, ISTOCNA_SRBIJA, KOSOVO_I_METOHIJA };

    public class LogEntitet
    {

        string id;
        Region region;
        string grad;
        int year;
        List<float> potrosnja;

        public LogEntitet() {

            this.Id = "";
            Region = Region.BACKA;
            Grad = "";
            Year = 0;
            Potrosnja = new List<float>(12);

        }

        public LogEntitet(Region reg, string municipality, int godina) {

            this.Id = "";
            Region = reg;
            Grad = municipality;
            Year = godina;
            Potrosnja = new List<float>(12);

        }

        public LogEntitet(string id, Region reg, string municipality, int godina) {

            this.Id = id;
            Region = reg;
            Grad = municipality;
            Year = godina;
            Potrosnja = new List<float>(12);
            
        }

        public string Id { get => id; set => id = value; }
        public Region Region { get => region; set => region = value; }
        public string Grad { get => grad; set => grad = value; }
        public int Year { get => year; set => year = value; }
        public List<float> Potrosnja { get => potrosnja; set => potrosnja = value; }
    }
}
