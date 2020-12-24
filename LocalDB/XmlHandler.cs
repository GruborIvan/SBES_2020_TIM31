using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Data.Common;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using Common;

namespace LocalDB
{
    public class XmlHandler
    {
        public static Dictionary<string, LogEntity> ListaLogEntity = new Dictionary<string, LogEntity>();
        static readonly object pblock = new object();

        public XmlHandler()
        {

            if (!File.Exists("baza.xml"))
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.Indent = true;
                xws.NewLineOnAttributes = true;
                using (XmlWriter xmlwriter = XmlWriter.Create("baza.xml", xws))
                {
                    xmlwriter.WriteStartDocument();
                    xmlwriter.WriteStartElement("ArrayOfLogEntity");
                    xmlwriter.WriteEndElement();
                    xmlwriter.WriteEndDocument();
                    xmlwriter.Close();
                }

            }
            else
            {
                if (ListaLogEntity.Count == 0)
                {
                    List<LogEntity> ArrayOfLogEntity = new List<LogEntity>();
                    string xmlstring = File.ReadAllText("baza.xml");
                    XmlSerializer ser = new XmlSerializer(typeof(List<LogEntity>), new XmlRootAttribute("ArrayOfLogEntity"));
                    StringReader sr = new StringReader(xmlstring);
                    ArrayOfLogEntity = ((List<LogEntity>)ser.Deserialize(sr));
                    UpdateDictionary(ArrayOfLogEntity);
                }
            }
        }
        public void UpdateDictionary(List<LogEntity> lista)
        {
            lock (pblock)
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>ITEMI IZ BAZE>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                foreach (var item in lista)
                {
                    Console.WriteLine("------------------------------------------------------------");
                    ListaLogEntity.Add(item.Id, item);
                    Console.WriteLine("ID: " + item.Id + " Grad: " + item.Grad + " Godina: " + item.Godina + " Region: " + item.Region);
                    Console.WriteLine("------------------------------------------------------------");
                }
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
            }
        }
        public List<LogEntity> ReturnList()
        {
            lock (pblock)
            {
                List<LogEntity> entries = new List<LogEntity>(ListaLogEntity.Count);
                foreach (string key in ListaLogEntity.Keys)
                {
                    entries.Add(ListaLogEntity[key]);
                }
                return entries;
            }
        }
        public string AddEntity(LogEntity le)
        {
            lock (pblock)
            {
                le.Id = "0";
                if (!(ListaLogEntity.Count() == 0))
                {
                    le.Id = NadjiRupu(ListaLogEntity).ToString();
                }

                ListaLogEntity.Add(le.Id, le);


                XDocument doc = XDocument.Load("baza.xml");
                XElement lista = doc.Element("ArrayOfLogEntity");

                XElement LEntEle = new XElement("LogEntity");

                XElement IdEle = new XElement("Id", le.Id.ToString());
                XElement GodEle = new XElement("Godina", le.Godina.ToString());
                XElement RegEle = new XElement("Region", le.Region.ToString());
                XElement GrdEle = new XElement("Grad", le.Grad);
                XElement PotEle = new XElement("Potrosnja");

                foreach (float item in le.Potrosnja)
                {
                    PotEle.Add(
                        new XElement("float", item.ToString())
                        );
                }

                LEntEle.Add(
                        IdEle,
                        GodEle,
                        RegEle,
                        GrdEle,
                        PotEle
                    );

                lista.Add(
                        LEntEle
                    );

                doc.Save("baza.xml");
                return le.Id;
            }
        }
        public bool DeleteEntity(string id)
        {
            lock (pblock)
            {
                string xmlEntitet = "";
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogEntity));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, ListaLogEntity[id]);
                    xmlEntitet = textWriter.ToString();
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(File.ReadAllText("baza.xml"));
                XmlNode xmlNode = doc.SelectSingleNode("ArrayOfLogEntity/LogEntity[Id = " + id + "]");

                ListaLogEntity.Remove(id);

                if (xmlNode != null)
                {
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                    doc.Save("baza.xml");
                    return true;
                }

                return false;
            }
        }
        public bool UpdateEntity(LogEntity le)
        {
            lock (pblock)
            {
                if (ListaLogEntity.ContainsKey(le.Id))
                {
                    int rupa = NadjiRupu(ListaLogEntity);

                    ListaLogEntity[le.Id] = le;

                    XDocument xmlDoc = XDocument.Parse(File.ReadAllText("baza.xml"));

                    var items = from item in xmlDoc.Descendants("LogEntity")
                                where item.Element("Id").Value == le.Id
                                select item;


                    foreach (XElement ielement in items)
                    {

                        ielement.SetElementValue("Region", le.Region.ToString());
                        ielement.SetElementValue("Grad", le.Grad.ToString());
                        ielement.SetElementValue("Godina", le.Godina.ToString());


                        int i = 0;

                        foreach (var pot in items.Descendants("Potrosnja").Descendants("float"))
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
        }
        public int NadjiRupu(Dictionary<string, LogEntity> provera)
        {
            lock (pblock)
            {
                int a = 0;

                while (provera.ContainsKey(a.ToString()))
                {
                    a++;
                }

                return a;
            }
        }

    }
}
