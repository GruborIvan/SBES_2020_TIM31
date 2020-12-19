using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Common;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace CryptoProject
{
    public class XmlHandler
    {
        public static Dictionary<string,LogEntitet> ListaLogEntitet = new Dictionary<string, LogEntitet>();
        public XmlHandler()
        {

        }

        public List<LogEntitet> ReturnList()
        {
            List<LogEntitet> entries = new List<LogEntitet>(ListaLogEntitet.Count);
            foreach (string key in ListaLogEntitet.Keys)
            {
                entries.Add(ListaLogEntitet[key]);
            }
            return entries;
        }

        public string AddEntity(LogEntitet le)
        {
            le.Id = "0";
            if (!(ListaLogEntitet.Count()==0))
            {
                le.Id = NadjiRupu(ListaLogEntitet).ToString();
            }

            ListaLogEntitet.Add(le.Id,le);
            List<LogEntitet> entries = new List<LogEntitet>(ListaLogEntitet.Count);
            foreach(string key in ListaLogEntitet.Keys)
            {
                entries.Add(ListaLogEntitet[key]);
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<LogEntitet>));
            using (StringWriter textWriter = new StringWriter()){

                xmlSerializer.Serialize(textWriter, entries);
                File.WriteAllText("baza.xml", textWriter.ToString());
                return le.Id;

            }
            
        }

        public bool DeleteEntity(string id)
        {
            
            string xmlEntitet = "";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogEntitet));
            
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ListaLogEntitet[id]);
                xmlEntitet = textWriter.ToString();
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText("baza.xml"));
            XmlNode xmlNode = doc.SelectSingleNode("ArrayOfLogEntitet/LogEntitet[Id = " + id+"]");

            ListaLogEntitet.Remove(id);

            if (xmlNode != null)
            {
                xmlNode.ParentNode.RemoveChild(xmlNode);
                doc.Save("baza.xml");
                return true;
            }

            return false;
        }

        public bool UpdateEntity(LogEntitet le)
        {
            if (ListaLogEntitet.ContainsKey(le.Id))
            {
                int rupa = NadjiRupu(ListaLogEntitet);

                ListaLogEntitet[le.Id] = le;
                
                XDocument xmlDoc = XDocument.Parse(File.ReadAllText("baza.xml"));

                var items = from item in xmlDoc.Descendants("LogEntitet")
                            where item.Element("Id").Value == le.Id
                            select item;


                foreach (XElement ielement in items)
                {
                    
                    ielement.SetElementValue("Region", le.Region.ToString());
                    ielement.SetElementValue("Grad", le.Grad.ToString());
                    ielement.SetElementValue("Year", le.Year.ToString());

                  
                    int i = 0;
                    
                    foreach(var pot in items.Descendants("Potrosnja").Descendants("float"))
                    {
                        pot.SetValue(le.Potrosnja[i]);
                        
                        i++;
                        if (i == le.Potrosnja.Count)
                            break;
                    }
                    
                }
                xmlDoc.Save("baza.xml");
                return true;
            }
            return false;
        } 

        public int NadjiRupu(Dictionary<string, LogEntitet> provera)
        {
            int a = 0;

            while(provera.ContainsKey(a.ToString()))
            { 
                a++;
            }

            return a;
        }

    }
}
