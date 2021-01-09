using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LocalDBase
{
    public class XmlHandler
    {
        public static Dictionary<string, LogEntity> ListaLogEntity = new Dictionary<string, LogEntity>();
        static readonly object pblock = new object();

        // Nemam inspiracije za comm

        static private string fileName;

        public string FileName {
            get { return fileName; }
            set { fileName = value; }
        }

        public XmlHandler() {

        }

        public XmlHandler(string fileName) {
            FileName = fileName;

            XmlWriterSettings xws = new XmlWriterSettings();
            xws.Indent = true;
            xws.NewLineOnAttributes = true;
            using (XmlWriter xmlwriter = XmlWriter.Create(fileName, xws)) {
                xmlwriter.WriteStartDocument();
                xmlwriter.WriteStartElement("ArrayOfLogEntity");
                xmlwriter.WriteEndElement();
                xmlwriter.WriteEndDocument();
                xmlwriter.Close();
            }

        }

        public List<LogEntity> ReturnList() {
            lock (pblock) {
                List<LogEntity> entries = new List<LogEntity>(ListaLogEntity.Count);
                foreach (string key in ListaLogEntity.Keys) {
                    entries.Add(ListaLogEntity[key]);
                }
                return entries;
            }
        }

        public string AddEntity(LogEntity le) {
            lock (pblock) {
                ListaLogEntity.Add(le.Id, le);


                XDocument doc = XDocument.Load(fileName);
                XElement lista = doc.Element("ArrayOfLogEntity");

                XElement LEntEle = new XElement("LogEntity");

                XElement IdEle = new XElement("Id", le.Id.ToString());
                XElement GodEle = new XElement("Godina", le.Godina.ToString());
                XElement RegEle = new XElement("Region", le.Region.ToString());
                XElement GrdEle = new XElement("Grad", le.Grad);
                XElement PotEle = new XElement("Potrosnja");

                foreach (float item in le.Potrosnja) {
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

                doc.Save(fileName);
                return le.Id;
            }
        }

        public bool IfContains(string id)
        {
            lock (pblock)
            {
                return ListaLogEntity.ContainsKey(id);
            }
        }

        public bool DeleteEntity(string id) {
            lock (pblock) {
                string xmlEntitet = "";
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogEntity));

                using (StringWriter textWriter = new StringWriter()) {
                    xmlSerializer.Serialize(textWriter, ListaLogEntity[id]);
                    xmlEntitet = textWriter.ToString();
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(File.ReadAllText(fileName));
                XmlNode xmlNode = doc.SelectSingleNode("ArrayOfLogEntity/LogEntity[Id = " + id + "]");

                ListaLogEntity.Remove(id);

                if (xmlNode != null) {
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                    doc.Save(fileName);
                    return true;
                }

                return false;
            }
        }

        public bool UpdateEntity(LogEntity le) {
            lock (pblock) {
                if (ListaLogEntity.ContainsKey(le.Id)) {
                    ListaLogEntity[le.Id] = le;

                    XDocument xmlDoc = XDocument.Parse(File.ReadAllText(fileName));

                    var items = from item in xmlDoc.Descendants("LogEntity")
                                where item.Element("Id").Value == le.Id
                                select item;


                    foreach (XElement ielement in items) {

                        ielement.SetElementValue("Region", le.Region.ToString());
                        ielement.SetElementValue("Grad", le.Grad.ToString());
                        ielement.SetElementValue("Godina", le.Godina.ToString());


                        int i = 0;

                        foreach (var pot in items.Descendants("Potrosnja").Descendants("float")) {
                            pot.SetValue(le.Potrosnja[i]);
                            i++;
                            if (i == le.Potrosnja.Count)
                                break;
                        }

                    }
                    xmlDoc.Save(fileName);
                    return true;
                }
                return false;
            }
        }
    }
}
