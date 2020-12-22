using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public enum Region { 
        [EnumMember] Srem, [EnumMember] Banat, [EnumMember] Bačka, [EnumMember] Beograd, 
        [EnumMember] Šumadija, [EnumMember] Zapadna_Srbija, [EnumMember] Južna_Srbija, 
        [EnumMember] Istočna_Srbija, [EnumMember] Kosovo_i_Metohija, [EnumMember] Nazad 
    };

    [DataContract]
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

        [DataMember]
        public string Id { get => id; set => id = value; }

        [DataMember]
        public Region Region { get => region; set => region = value; }

        [DataMember]
        public string Grad { get => grad; set => grad = value; }

        [DataMember]
        public int Year { get => year; set => year = value; }

        [DataMember]
        public List<float> Potrosnja { get => potrosnja; set => potrosnja = value; }
    }
}
